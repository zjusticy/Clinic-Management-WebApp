import useSWR from 'swr';
import { UserBrief } from './types';

export function useAdminDoctorList() {
  const {
    data: adminDoctorList,
    isLoading,
    error,
    mutate,
  } = useSWR<UserBrief[]>(`/api/users?userType=doctor`, async (url: string) => {
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
    adminDoctorList,
    error,
    mutate,
    isLoading,
  };
}
