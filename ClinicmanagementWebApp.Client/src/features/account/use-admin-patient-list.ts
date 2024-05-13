import useSWR from 'swr';

import { UserBrief } from './types';

export function useAdminPatientList() {
  const {
    data: adminPatientList,
    isLoading,
    error,
    mutate,
  } = useSWR<UserBrief[]>(
    '/api/users?userType=patient',
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
    adminPatientList,
    error,
    mutate,
    isLoading,
  };
}
