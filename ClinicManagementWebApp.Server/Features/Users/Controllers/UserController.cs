using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using ClinicManagementWebApp.Server.Features.Appointment.DTOs;
using ClinicManagementWebApp.Server.Features.Appointment.Entity;
using ClinicManagementWebApp.Server.Features.User.Services;
using ClinicManagementWebApp.Server.Features.Users.DTOs;
using ClinicManagementWebApp.Server.Features.Users.Entity;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementWebApp.Server.Features.Users.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IMapper mapper, UserManager<UserModel> userManager, IUserStore<UserModel> userStore, SignInManager<UserModel> signInManager,
         IUserServices userServices) : ControllerBase
    {
        private static readonly EmailAddressAttribute _emailAddressAttribute = new();

        [Authorize(Policy = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserBriefDTO>>> GetUsers([FromQuery] string? userType)
        {
            ICollection<UserModel> usersList;

            if (userType != null)
            {
                usersList = await userManager.GetUsersForClaimAsync(new Claim("level", userType));
            }

            else usersList = await userManager.Users.ToListAsync();

            var users = mapper.Map<IEnumerable<UserBriefDTO>>(usersList);

            return Ok(users);
        }

        [Authorize(Policy = "patient")]
        [Route("doctors")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserBriefDTO>>> GetDoctors()
        {

            var usersList = await userManager.GetUsersForClaimAsync(new Claim("level", "doctor"));

            var users = mapper.Map<IEnumerable<UserBriefDTO>>(usersList);

            return Ok(users);
        }

        [Authorize]
        [Route("me")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserBriefDTO>>> GetUserByCookie()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var levelClaim = User.FindFirstValue("level");

            if (levelClaim == null)
            {
                return NotFound();
            }

            var userRole = new UserRoleDTO()
            {
                Role = levelClaim
            };

            var userOutput = new UserBriefDTO();

            mapper.Map(user, userOutput);
            mapper.Map(userRole, userOutput);

            return Ok(userOutput);
        }

        [Authorize(Policy = "doctor")]
        [HttpGet("{userId}", Name = nameof(GetUserById))]
        public async Task<ActionResult<IEnumerable<string>>> GetUserById(Guid userId)
        {

            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        [Route("logout")]
        [HttpPost]
        public async Task<Results<Ok, ProblemHttpResult>> Logout([FromBody] object empty)
        {
            if (empty != null)
            {
                await signInManager.SignOutAsync();
                return TypedResults.Ok();
            }

            return TypedResults.Problem("Unauthorized", statusCode: StatusCodes.Status401Unauthorized);
        }

        [Route("register")]
        [HttpPost]
        public async Task<Results<Ok, ValidationProblem>> Register(
            [FromBody] PatientAddOrUpdateDTO registration)
        {

            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException($"{nameof(Register)} requires a user store with email support.");
            }

            var emailStore = (IUserEmailStore<UserModel>)userStore;
            var email = registration.Email;

            if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
            {
                return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
            }

            var userRegisterModel = mapper.Map<UserRegisterDTO>(registration);

            var user = mapper.Map<UserModel>(userRegisterModel);

            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, registration.Password);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            await userManager.AddClaimAsync(user, new Claim("level", "patient"));

            /*await SendConfirmationEmailAsync(user, userManager, context, email);*/
            return TypedResults.Ok();
        }


        [Authorize(Policy = "admin")]
        [Route("doctor/register")]
        [HttpPost]
        public async Task<Results<Ok, ValidationProblem>> DoctorRegister(
            [FromBody] UserAddOrUpdateDTO registration, [FromServices] IServiceProvider sp)
        {

            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException($"{nameof(Register)} requires a user store with email support.");
            }

            var userStore = sp.GetRequiredService<IUserStore<UserModel>>();
            var emailStore = (IUserEmailStore<UserModel>)userStore;
            var email = registration.Email;

            if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
            {
                return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
            }

            var userRegisterModel = mapper.Map<UserRegisterDTO>(registration);

            var user = mapper.Map<UserModel>(userRegisterModel);

            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, registration.Password);

            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }

            await userManager.AddClaimAsync(user, new Claim("level", "doctor"));

            /*await SendConfirmationEmailAsync(user, userManager, context, email);*/
            return TypedResults.Ok();
        }

        [Route("login")]
        [HttpPost]
        public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>>
           Login([FromBody] LoginRequest login, [FromServices] IServiceProvider sp)
        {
            bool useCookies = true;
            bool useSessionCookies = false;
            var signInManager = sp.GetRequiredService<SignInManager<UserModel>>();

            var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
            var isPersistent = (useCookies == true) && (useSessionCookies != true);
            signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

            var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, lockoutOnFailure: true);

            if (result.RequiresTwoFactor)
            {
                if (!string.IsNullOrEmpty(login.TwoFactorCode))
                {
                    result = await signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent, rememberClient: isPersistent);
                }
                else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
                {
                    result = await signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
                }
            }

            if (!result.Succeeded)
            {
                return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
            }

            // The signInManager already produced the needed response in the form of a cookie or bearer token.
            return TypedResults.Empty;
        }

        [Authorize]
        [HttpGet("{userId}/appointments", Name = nameof(GetAppointmentsForPatientByUserId))]
        public async Task<ActionResult<IEnumerable<AppointmentBriefDTO>>> GetAppointmentsForPatientByUserId(Guid userId,
            [FromQuery] int status, [FromQuery] string? role)
        {
            var levelClaim = User.FindFirstValue("level");
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (levelClaim == null)
            {
                return NotFound();
            }

            ICollection<AppointmentModel>? appointmentsEntities = null;

            if (levelClaim != "admin" && currentUserId != userId.ToString())
            {
                return Unauthorized();
            }

            if (levelClaim == "admin" && role != null)
            {
                appointmentsEntities = await userServices.GetAppointmentListForUserByUserIdAsync(userId, status, role);
            }

            if (levelClaim == "patient" || levelClaim == "doctor")
            {
                appointmentsEntities = await userServices.GetAppointmentListForUserByUserIdAsync(userId, status, levelClaim);
            }


            if (appointmentsEntities == null)
            {
                return NotFound();
            }

            var appointments = mapper.Map<IEnumerable<AppointmentListBriefDTO>>(appointmentsEntities);

            return Ok(appointments);
        }



        private static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription) =>

                    TypedResults.ValidationProblem(new Dictionary<string, string[]> {
                { errorCode, [errorDescription] }
                });

        private static ValidationProblem CreateValidationProblem(IdentityResult result)
        {
            // We expect a single error code and description in the normal case.
            // This could be golfed with GroupBy and ToDictionary, but perf! :P
            Debug.Assert(!result.Succeeded);
            var errorDictionary = new Dictionary<string, string[]>(1);

            foreach (var error in result.Errors)
            {
                string[] newDescriptions;

                if (errorDictionary.TryGetValue(error.Code, out var descriptions))
                {
                    newDescriptions = new string[descriptions.Length + 1];
                    Array.Copy(descriptions, newDescriptions, descriptions.Length);
                    newDescriptions[descriptions.Length] = error.Description;
                }
                else
                {
                    newDescriptions = [error.Description];
                }

                errorDictionary[error.Code] = newDescriptions;
            }

            return TypedResults.ValidationProblem(errorDictionary);
        }
    }
}
