import { useState } from 'react';

import { useNavigate } from 'react-router-dom';

import { Icons } from '@/assets/icons';
import { Button } from '@/features/ui/button';
import { Input } from '@/features/ui/input';
import { Label } from '@/features/ui/label';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/features/ui/select';

// import { useForm } from 'react-hook-form';
import { z } from 'zod';

type FormData = {
  firstName: string;
  lastName: string;
  password: string;
  gender: string;
  email: string;
  phoneNumber: string;
  year: string;
  month: string;
  day: string;
};

type FormErrors = {
  firstName?: string[];
  lastName?: string[];
  password?: string[];
  gender?: string[];
  email?: string[];
  phoneNumber?: string[];
  year?: string[];
  month?: string[];
  day?: string[];
};

const signUpSchema = (role: string) =>
  z.object({
    firstName: z.string().min(1, {
      message: 'Required',
    }),
    lastName: z.string().min(1, {
      message: 'Required',
    }),
    password: z
      .string()
      .min(8, {
        message: 'At least 8 characters.',
      })
      .regex(/[a-zA-Z]/, {
        message: 'Password must contain at least one letter',
      })
      .regex(/[0-9]/, { message: 'Password must contain at least one digit' }),
    gender: z.string().min(1, {
      message: 'Required',
    }),
    email: z.string().email(),
    phoneNumber: z.string().min(1, {
      message: 'Required',
    }),
    ...(role === 'patient'
      ? {
          year: z.string().length(4, {
            message: 'Invalid',
          }),
          month: z.number({ coerce: true }).min(1).max(12),
          day: z
            .string()
            .length(2)
            .refine(
              (val: string) => {
                const numVal = Number(val);
                return !isNaN(numVal) && numVal > 0 && numVal < 32;
              },
              { message: 'Invalid' }
            ),
        }
      : {}),
  });

export function UserRegisterForm({
  role,
  adminRegister,
}: {
  role: string;
  adminRegister: boolean;
}) {
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const [formData, setFormData] = useState<FormData>({
    firstName: '',
    lastName: '',
    password: '',
    gender: '',
    email: '',
    phoneNumber: '',
    year: '',
    month: '',
    day: '',
  });

  const [errors, setErrors] = useState<FormErrors>({});

  const navigate = useNavigate();

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);
    // Validate form data using Zod schema
    const validationResult = signUpSchema(role).safeParse(formData);
    if (!validationResult.success) {
      // If validation fails, set the errors state with validation errors
      setErrors(validationResult.error.formErrors.fieldErrors);
      setIsLoading(false);
      return;
    }
    // If validation succeeds, submit the form data
    const registerData = {
      firstName: formData.firstName,
      lastName: formData.lastName,
      gender: parseInt(formData.gender),
      password: formData.password,
      phoneNumber: formData.phoneNumber,
      email: formData.email,
      ...(role === 'patient'
        ? {
            dateOfBirth: new Date(
              `${formData.year}-${formData.month}-${formData.day}`
            ),
          }
        : {}),
    };

    fetch(
      role === 'patient' ? '/api/users/register' : '/api/users/doctor/register',
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(registerData),
      }
    )
      .then((res) => {
        if (res.ok) {
          if (adminRegister && role === 'patient') {
            navigate('/patient-management');
            return;
          }
          if (adminRegister && role === 'doctor') {
            navigate('/doctor-management');
            return;
          }
          navigate('/');
        } else {
          console.log(res.statusText);
        }
        setIsLoading(false);
      })
      .catch((err) => {
        console.log(err);
      });
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
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

  const handleSelectChange = (name: string) => (value: string) => {
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

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <div className="grid gap-5">
          <div className="grid gap-1">
            <Label htmlFor="email">Email</Label>
            <Input
              id="email"
              onChange={handleChange}
              value={formData.email}
              placeholder="name@example.com"
              type="email"
              autoCapitalize="none"
              autoComplete="email"
              autoCorrect="off"
              disabled={isLoading}
            />
            {errors.email &&
              errors.email.map((message, index) => (
                <div
                  key={index}
                  className="text-[0.8rem] font-medium text-destructive"
                >
                  {message}
                </div>
              ))}
          </div>

          <div className="grid gap-1">
            <Label htmlFor="password">Password</Label>
            <Input
              id="password"
              placeholder="At least 8 characters with digits and letters"
              onChange={handleChange}
              value={formData.password}
              disabled={isLoading}
            />
            {errors.password &&
              errors.password.map((message, index) => (
                <div
                  key={index}
                  className="text-[0.8rem] font-medium text-destructive"
                >
                  {message}
                </div>
              ))}
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div className="gap-1">
              <Label>First Name</Label>
              <Input
                id="firstName"
                placeholder="First name"
                value={formData.firstName}
                onChange={handleChange}
                disabled={isLoading}
              />
              {errors.firstName &&
                errors.firstName.map((message, index) => (
                  <div
                    key={index}
                    className="text-[0.8rem] font-medium text-destructive"
                  >
                    {message}
                  </div>
                ))}
            </div>

            <div className="grid gap-1">
              <div className="gap-1">
                <Label>Last Name</Label>
                <Input
                  id="lastName"
                  placeholder="Last name"
                  value={formData.lastName}
                  onChange={handleChange}
                  disabled={isLoading}
                />
                {errors.lastName &&
                  errors.lastName.map((message, index) => (
                    <div
                      key={index}
                      className="text-[0.8rem] font-medium text-destructive"
                    >
                      {message}
                    </div>
                  ))}
              </div>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="grid gap-1">
              <Label htmlFor="gender">Gender</Label>
              <Select
                value={formData.gender}
                onValueChange={handleSelectChange('gender')}
              >
                <SelectTrigger id="gender">
                  <SelectValue placeholder="-- --" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="0">Not known</SelectItem>
                  <SelectItem value="1">Male</SelectItem>
                  <SelectItem value="2">Female</SelectItem>
                  <SelectItem value="9">Not applicable</SelectItem>
                </SelectContent>
              </Select>
              {errors.gender &&
                errors.gender.map((message, index) => (
                  <div
                    key={index}
                    className="text-[0.8rem] font-medium text-destructive"
                  >
                    {message}
                  </div>
                ))}
            </div>
            <div className="grid gap-1">
              <Label htmlFor="phoneNumber">Phone Number</Label>
              <Input
                id="phoneNumber"
                placeholder="e.g. 1234567890"
                value={formData.phoneNumber}
                onChange={handleChange}
                disabled={isLoading}
              />
              {errors.phoneNumber &&
                errors.phoneNumber.map((message, index) => (
                  <div
                    key={index}
                    className="text-[0.8rem] font-medium text-destructive"
                  >
                    {message}
                  </div>
                ))}
            </div>
          </div>

          {role === 'patient' && (
            <div className="grid gap-1">
              <Label>Date of Birth</Label>
              <div className="grid grid-cols-3 gap-4">
                <div className="grid gap-2">
                  <Input
                    id="year"
                    placeholder="YYYY"
                    value={formData.year}
                    onChange={handleChange}
                    disabled={isLoading}
                  />
                  {errors.year &&
                    errors.year.map((message, index) => (
                      <div
                        key={index}
                        className="text-[0.8rem] font-medium text-destructive"
                      >
                        {message}
                      </div>
                    ))}
                </div>

                <div className="grid gap-2">
                  <Select
                    value={formData.month}
                    onValueChange={handleSelectChange('month')}
                  >
                    <SelectTrigger id="month">
                      <SelectValue placeholder="Month" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="1">January</SelectItem>
                      <SelectItem value="2">February</SelectItem>
                      <SelectItem value="3">March</SelectItem>
                      <SelectItem value="4">April</SelectItem>
                      <SelectItem value="5">May</SelectItem>
                      <SelectItem value="6">June</SelectItem>
                      <SelectItem value="7">July</SelectItem>
                      <SelectItem value="8">August</SelectItem>
                      <SelectItem value="9">September</SelectItem>
                      <SelectItem value="10">October</SelectItem>
                      <SelectItem value="11">November</SelectItem>
                      <SelectItem value="12">December</SelectItem>
                    </SelectContent>
                  </Select>
                  {errors.month && (
                    <div className="text-[0.8rem] font-medium text-destructive">
                      {'Invalid'}
                    </div>
                  )}
                </div>

                <div className="grid gap-2">
                  {/* <Label htmlFor="cvc">Y</Label> */}
                  <Input
                    id="day"
                    placeholder="DD"
                    value={formData.day}
                    onChange={handleChange}
                  />
                  {errors.day && (
                    <div className="text-[0.8rem] font-medium text-destructive">
                      {'Invalid'}
                    </div>
                  )}
                </div>
              </div>
            </div>
          )}

          <Button disabled={isLoading}>
            {isLoading && (
              <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
            )}
            {role === 'patient' ? 'Patient Register' : 'Doctor Register'}
          </Button>
        </div>
      </form>
    </div>
  );
}
