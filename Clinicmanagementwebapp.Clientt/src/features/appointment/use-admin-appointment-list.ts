import useSWR from 'swr';

import { AppointBrief } from './types';

export function useAdminAppointmentList(appointmentStatus: number) {
  const {
    data: adminAppointmentList,
    isLoading,
    error,
    mutate,
  } = useSWR<AppointBrief[]>(
    `/api/appointments?status=${appointmentStatus}`,
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
    adminAppointmentList,
    error,
    mutate,
    isLoading,
  };
}
