import { cn } from '@/lib/utils';
import { buttonVariants } from '@/features/ui/button';
import { UserAuthForm } from '@/features/account/user-auth-form';
import { Link } from 'react-router-dom';

export function AuthPage() {
  return (
    <>
      <div className="container relative hidden h-[800px] flex-col items-center justify-center md:grid lg:max-w-[800px] lg:px-0">
        <Link
          to="/register"
          className={cn(
            buttonVariants({ variant: 'ghost' }),
            'absolute right-4 top-4 md:right-8 md:top-8 text-base'
          )}
        >
          Register
        </Link>
        <div className="lg:p-8">
          <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[400px]">
            <div className="flex flex-col space-y-2 text-center">
              <h1 className="text-2xl font-bold tracking-wide">
                Login in to your account
              </h1>
            </div>
            <UserAuthForm />
          </div>
        </div>
      </div>
    </>
  );
}
