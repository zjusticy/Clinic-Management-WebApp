import useSWR from 'swr';
import { UserBrief } from './types';

export function useDoctorList() {
  const {
    data: doctorList,
    isLoading,
    error,
    mutate,
  } = useSWR<UserBrief[]>('/api/users/doctors', async (url: string) => {
    const res = await fetch(url, {
      method: 'GET',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    return res.json();
  });

  return {
    doctorList,
    error,
    mutate,
    isLoading,
  };
}
