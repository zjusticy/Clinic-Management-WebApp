﻿using System.Security.Claims;
using AutoMapper;
using ClinicManagementWebApp.Server.Features.Appointment.DTOs;
using ClinicManagementWebApp.Server.Features.Appointment.Entity;
using ClinicManagementWebApp.Server.Features.Appointment.Services;
using ClinicManagementWebApp.Server.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementWebApp.Server.Features.Appointment.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController(IAppointmentRepository appointmentRepository, IMapper mapper,
        IAppointmentServices appointmentServices) : ControllerBase
    {

        [Authorize(Policy = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentBriefDTO>>> GetAllAppointments()
        {
            var appointmentsEntities = await appointmentRepository.GetAllAsync();

            var appointments = mapper.Map<IEnumerable<AppointmentBriefDTO>>(appointmentsEntities);

            return Ok(appointments);
        }

        [Authorize(Policy = "patient")]
        [HttpGet("patient/{userId}", Name = nameof(GetAppointmentsForPatientByUserId))]
        public async Task<ActionResult<IEnumerable<AppointmentBriefDTO>>> GetAppointmentsForPatientByUserId(Guid userId)
        {
            var levelClaim = User.FindFirstValue("level");
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (levelClaim != "admin" && currentUserId != userId.ToString())
            {
                return Unauthorized();
            }

            var appointmentsEntities = await appointmentServices.GetAppointmentListForPatientByUserIdAsync(userId);

            if (appointmentsEntities == null)
            {
                return NotFound();
            }

            var appointments = mapper.Map<IEnumerable<AppointmentListBriefDTO>>(appointmentsEntities);

            return Ok(appointments);
        }

        [Authorize(Policy = "doctor")]
        [HttpGet("doctor/{userId}", Name = nameof(GetAppointmentsForDoctorByUserId))]
        public async Task<ActionResult<IEnumerable<AppointmentBriefDTO>>> GetAppointmentsForDoctorByUserId(Guid userId)
        {
            var levelClaim = User.FindFirstValue("level");
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (levelClaim != "admin" && currentUserId != userId.ToString())
            {
                return Unauthorized();
            }

            var appointmentsEntities = await appointmentServices.GetAppointmentListForDoctorByUserIdAsync(userId);

            if (appointmentsEntities == null)
            {
                return NotFound();
            }

            var appointments = mapper.Map<IEnumerable<AppointmentListBriefDTO>>(appointmentsEntities);

            return Ok(appointments);
        }

        [HttpGet("{id}", Name = nameof(GetAppointmentById))]
        public async Task<ActionResult<AppointmentDetailDTO>> GetAppointmentById(int id)
        {
            var appoitmentEntity = await appointmentRepository.GetByIdAsync(id);
            var appointment = mapper.Map<AppointmentDetailDTO>(appoitmentEntity);
            return Ok(appointment);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAppoinment([FromBody] AppointmentAddOrUpdateDTO appointment)
        {
            var appointmentEntity = mapper.Map<AppointmentModel>(appointment);

            var levelClaim = User.FindFirstValue("level");
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (levelClaim == "patient" && currentUserId != null)
            {
                appointmentEntity.PatientId = Guid.Parse(currentUserId);
            }

            if (levelClaim == "doctor" && currentUserId != null)
            {
                appointmentEntity.DoctorId = Guid.Parse(currentUserId);
            }

            await appointmentRepository.CreateAsync(appointmentEntity);

            var appointmentOutModel = mapper.Map<AppointmentBriefDTO>(appointmentEntity);

            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointmentEntity.Id }, appointmentOutModel);
        }

        [Authorize(Policy = "doctor")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] JsonPatchDocument<AppointmentUpdateDTO> appointmentPatchDoc)
        {
            var appointment = await appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            var levelClaim = User.FindFirstValue("level");
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (levelClaim == "doctor" && currentUserId != appointment.DoctorId.ToString())
            {
                return Unauthorized();
            }

            var appointmentDtoToPatch = mapper.Map<AppointmentUpdateDTO>(appointment);

            var allowedOperations = new[] { "replace" };
            var allowedOperationPaths = new[] { "/prescription", "/appointmentStatus" };
            if (levelClaim == "doctor")
            {

                foreach (var operation in appointmentPatchDoc.Operations)
                {
                    if (!allowedOperations.Contains(operation.op))
                    {
                        return BadRequest("Permission denied");
                    }

                    if (!allowedOperationPaths.Contains(operation.path))
                    {
                        return BadRequest("Permission denied");
                    }
                }
            }

            appointmentPatchDoc.ApplyTo(appointmentDtoToPatch);

            mapper.Map(appointmentDtoToPatch, appointment);

            await appointmentRepository.ReplaceAsync(appointment);
            return NoContent();
        }

        /*        [HttpPut("{id}")]
                public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentUpdateDTO appointment)
                {
                    if (id != appointment.Id)
                    {
                        return BadRequest();
                    }

                    var appointmentEntity = _mapper.Map<AppointmentModel>(appointment);

                    await _appointmentRepository.ReplaceAsync(appointmentEntity);
                    return NoContent();
                }*/

        /*        [HttpDelete("{id}")]
                public async Task<IActionResult> DeleteAppointment(int id)
                {
                    var appointment = await _appointmentRepository.GetByIdAsync(id);

                    if (appointment == null)
                    {
                        return NotFound();
                    }

                    await _appointmentRepository.DeleteAsync(appointment);

                    return NoContent();
                }*/
    }
}
