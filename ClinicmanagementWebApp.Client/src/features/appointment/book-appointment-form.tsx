import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Calendar as CalendarIcon } from 'lucide-react';
import { format } from 'date-fns';

import { cn } from '@/lib/utils';
import { Icons } from '@/assets/icons';
import { Button } from '@/features/ui/button';
import { Label } from '@/features/ui/label';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/features/ui/select';
import { Textarea } from '@/features/ui/textarea';
import { Calendar } from '@/features/ui/calendar';
import { Input } from '@/features/ui/input';

import { Popover, PopoverContent, PopoverTrigger } from '@/features/ui/popover';

import { useDoctorList } from '@/features/account/use-doctor-list';

import { z } from 'zod';

type FormData = {
  doctorId?: string;
  reasonForVisit?: string;
  date: Date | undefined;
  time?: string;
  patientId?: string;
};

type FormErrors = {
  doctorId?: string[];
  reasonForVisit?: string[];
  date?: string[];
  time?: string[];
  patientId?: string[];
};

const patientAppointmentSchema = (adminBooking: boolean) =>
  z.object({
    doctorId: z.string().min(1, {
      message: 'Required',
    }),

    reasonForVisit: z
      .string()
      .max(50, {
        message: 'No more than 50 characters',
      })
      .optional(),

    date: z.date(),
    time: z.string().min(1, {
      message: 'Required',
    }),
    ...(adminBooking
      ? {
          patientId: z.string().min(1, {
            message: 'Required',
          }),
        }
      : {}),
  });

export function BookAppoitmentForm({
  adminBooking,
}: {
  adminBooking: boolean;
}) {
  const [isBooking, setIsBooking] = useState<boolean>(false);

  const [formData, setFormData] = useState<FormData>({
    date: new Date(),
  });
  const [errors, setErrors] = useState<FormErrors>({});

  const { doctorList, isLoading } = useDoctorList();

  const navigate = useNavigate();

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsBooking(true);
    // Validate form data using Zod schema
    const validationResult =
      patientAppointmentSchema(adminBooking).safeParse(formData);
    if (!validationResult.success) {
      setErrors(validationResult.error.formErrors.fieldErrors);
      setIsBooking(false);
      return;
    }

    if (formData.time && formData.date) {
      const [time, minutesStr] = formData.time.split(':');

      // Parse hours and minutes as integers
      const [hours, minutes] = [parseInt(time), parseInt(minutesStr)];

      formData.date.setHours(hours);
      formData.date.setMinutes(minutes);

      // If validation succeeds, submit the form data
      const appointmentData = {
        doctorId: formData.doctorId,
        ...(formData.reasonForVisit
          ? { reasonForVisit: formData.reasonForVisit }
          : {}),
        date: formData.date,
        ...(adminBooking ? { patientId: formData.patientId } : {}),
        appointmentStatus: 0,
      };

      fetch('/api/appointments', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(appointmentData),
      })
        .then((res) => {
          if (res.ok) {
            navigate(adminBooking ? '/appointment-management' : '/home');
          } else {
            console.log(res.statusText);
          }
          setIsBooking(false);
        })
        .catch((err) => {
          console.log(err);
        });
    }
  };

  const handleSelectChange =
    (name: string) => (value: string | Date | undefined) => {
      // Update the formData state with the new value
      setFormData((prevData) => ({
        ...prevData,
        [name]: value,
      }));

      // Clear the error message for the changed field
      setErrors((prevErrors) => ({
        ...prevErrors,
        [name]: undefined,
      }));
    };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { id: name, value } = e.target;
    // Update the formData state with the new value
    setFormData({
      ...formData,
      [name]: value || '',
    });
    // Clear the error message for the changed field
    setErrors((prevErrors) => ({
      ...prevErrors,
      [name]: undefined,
    }));
  };

  return (
    <div className={cn('grid gap-6')}>
      <form onSubmit={handleSubmit}>
        <div className="grid gap-6">
          <div className="grid gap-1">
            <Label htmlFor="provider">Provider</Label>
            <Select
              value={formData.doctorId}
              onValueChange={handleSelectChange('doctorId')}
            >
              <SelectTrigger id="provider">
                <SelectValue placeholder="-- Select Provider --" />
              </SelectTrigger>
              <SelectContent>
                {isLoading && (
                  <Icons.spinner className="h-4 w-4 animate-spin" />
                )}
                {!isLoading &&
                  doctorList &&
                  doctorList.map((doctor) => {
                    return (
                      <SelectItem
                        value={doctor.id}
                      >{`${doctor.firstName} ${doctor.lastName}`}</SelectItem>
                    );
                  })}
              </SelectContent>
            </Select>
            {errors.doctorId &&
              errors.doctorId.map((message, index) => (
                <div
                  key={index}
                  className="text-[0.8rem] font-medium text-destructive"
                >
                  {message}
                </div>
              ))}
          </div>

          {adminBooking && (
            <div className="grid gap-1">
              <Label htmlFor="patientId">Patient</Label>
              <Input
                id="patientId"
                placeholder="e.g. 1234567890"
                value={formData.patientId}
                onChange={handleChange}
                disabled={isLoading}
              />
              {errors.patientId &&
                errors.patientId.map((message, index) => (
                  <div
                    key={index}
                    className="text-[0.8rem] font-medium text-destructive"
                  >
                    {message}
                  </div>
                ))}
            </div>
          )}
          <div className="grid grid-cols-2 gap-4">
            <div className="grid gap-1">
              <Label htmlFor="firstName">Date</Label>
              <Popover>
                <PopoverTrigger asChild>
                  <Button
                    variant={'outline'}
                    className={cn(
                      'justify-start text-left font-normal',
                      !formData.date && 'text-muted-foreground'
                    )}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {formData.date ? (
                      format(formData.date, 'PPP')
                    ) : (
                      <span>Pick a date</span>
                    )}
                  </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0">
                  <Calendar
                    mode="single"
                    selected={formData.date}
                    onSelect={handleSelectChange('date')}
                    initialFocus
                  />
                </PopoverContent>
              </Popover>
              {errors.date &&
                errors.date.map((message, index) => (
                  <div
                    key={index}
                    className="text-[0.8rem] font-medium text-destructive"
                  >
                    {message}
                  </div>
                ))}
            </div>
            <div className="grid gap-1">
              <Label htmlFor="firstName">Time</Label>
              <Select
                value={formData.time}
                onValueChange={handleSelectChange('time')}
              >
                <SelectTrigger id="time">
                  <SelectValue placeholder="Select a time slot" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="9:00">9:00 AM - 9:30 AM</SelectItem>
                  <SelectItem value="9:30">9:30 AM - 10:00 AM</SelectItem>
                  <SelectItem value="10:00">10:00 AM - 10:30 AM</SelectItem>
                  <SelectItem value="10:30">10:30 AM - 11:00 AM</SelectItem>
                  <SelectItem value="11:00">11:00 AM - 11:30 AM</SelectItem>
                  <SelectItem value="11:30">11:30 AM - 12:00 PM</SelectItem>
                  <SelectItem value="13:00">1:00 PM - 1:30 PM</SelectItem>
                  <SelectItem value="13:30">1:30 PM - 2:00 PM</SelectItem>
                  <SelectItem value="14:00">2:00 PM - 2:30 PM</SelectItem>
                  <SelectItem value="14:30">2:30 PM - 3:00 PM</SelectItem>
                  <SelectItem value="15:00">3:00 PM - 3:30 PM</SelectItem>
                  <SelectItem value="15:30">3:30 PM - 4:00 PM</SelectItem>
                </SelectContent>
              </Select>
              {errors.time &&
                errors.time.map((message, index) => (
                  <div
                    key={index}
                    className="text-[0.8rem] font-medium text-destructive"
                  >
                    {message}
                  </div>
                ))}
            </div>
          </div>

          <div className="grid gap-2">
            <Label>Symptom</Label>
            <Textarea
              id="reasonForVisit"
              placeholder="e.g. stomach pain, etc"
              className="resize-none"
              value={formData.reasonForVisit}
              onChange={handleChange}
            />
            {errors.reasonForVisit &&
              errors.reasonForVisit.map((message, index) => (
                <div
                  key={index}
                  className="text-[0.8rem] font-medium text-destructive"
                >
                  {message}
                </div>
              ))}
          </div>

          <Button disabled={isLoading}>
            {isBooking && (
              <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
            )}
            Book now
          </Button>
        </div>
      </form>
    </div>
  );
}
