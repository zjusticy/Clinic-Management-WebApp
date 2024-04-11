import { useEffect } from 'react';
import { useNavigate, Link } from 'react-router-dom';

import { useUserInfo } from '@/features/account/use-user-info';
import { Button } from '@/features/ui/button';
import { Icons } from '@/assets/icons';
import { buttonVariants } from '@/features/ui/button';
import { cn } from '@/lib/utils';

export const RootLayout = ({ children }: { children: React.ReactNode }) => {
  const navigate = useNavigate();

  const { isLoading, userInfo, mutate } = useUserInfo();

  useEffect(() => {
    if (!isLoading && !userInfo) {
      navigate('/');
    }
  }, [isLoading, userInfo, navigate]);

  const onLogoutHandler = async () => {
    try {
      const result = await fetch('/api/users/logout', {
        method: 'POST',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({}),
      });
      if (result.ok) {
        mutate(null);
        navigate('/');
      } else {
        console.log('Logout failed');
      }
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <div className="container h-[800px] flex-col items-center justify-center  lg:max-w-[800px] lg:px-0">
      {isLoading ? (
        <Icons.spinner className="h-4 w-4 animate-spin" />
      ) : (
        <>
          <div className="flex m-2">
            <Link
              // className={cn(
              //   buttonVariants({ variant: 'ghost' }),
              //   'bg-white absolute right-4 top-4 md:right-8 md:top-8 text-base'
              // )}
              className={cn(
                buttonVariants({ variant: 'ghost' }),
                'px-0 underline text-primary text-xl'
              )}
              to="/home"
            >
              Home
            </Link>
            <Button
              // className={cn(
              //   buttonVariants({ variant: 'ghost' }),
              //   'bg-white absolute right-4 top-4 md:right-8 md:top-8 text-base'
              // )}
              className="bg-white text-base ml-auto"
              variant="ghost"
              onClick={onLogoutHandler}
            >
              Log out
            </Button>
          </div>
          {children}
        </>
      )}
    </div>
  );
};
