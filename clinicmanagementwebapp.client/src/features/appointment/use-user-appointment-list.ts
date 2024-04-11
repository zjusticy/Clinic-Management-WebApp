import useSWR from 'swr';

import { AppointBrief } from './types';

export function useAppointmentList(userId: string, appointmentStatus: number) {
  const {
    data: patientAppointmentList,
    isLoading,
    error,
    mutate,
  } = useSWR<AppointBrief[]>(
    userId
      ? `/api/users/${userId}/appointments?status=${appointmentStatus}`
      : null,
    async (url: string) => {
      const res = await fetch(url, {
        method: 'GET',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
      });
      return res.json();
    }
  );

  return {
    patientAppointmentList,
    error,
    mutate,
    isLoading,
  };
}
