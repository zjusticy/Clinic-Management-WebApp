import useSWR from 'swr';

type userData = {
  id: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  dateOfBirth: Date;
  gender: number;
  role: string;
};

export function useUserInfo() {
  const {
    data: userInfo,
    error,
    mutate,
  } = useSWR<userData | null>('/api/users/me', async (url: string) => {
    const res = await fetch(url, {
      method: 'GET',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (res.ok) {
      return await res.json();
    }
    console.log(res);
    return null;
  });

  const isLoading = !userInfo && !error;

  return {
    userInfo,
    error,
    mutate,
    isLoading,
  };
}
