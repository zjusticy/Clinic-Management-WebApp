import { cn } from '@/lib/utils';
import { buttonVariants } from '@/features/ui/button';
import { UserRegisterForm } from '@/features/account/user-register-form';
import { Link } from 'react-router-dom';
import { RootLayout } from '@/features/layout/userLayout';

export function RegisterPage({
  role,
  adminRegister = false,
}: {
  role: string;
  adminRegister?: boolean;
}) {
  const registerContent = (
    <div className="lg:p-8">
      <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[400px]">
        <div className="flex flex-col space-y-2 text-center">
          <h1 className="text-2xl font-bold tracking-normal">
            Create an account
          </h1>
        </div>
        <UserRegisterForm role={role} adminRegister={adminRegister} />
        <p className="px-8 text-center text-sm text-muted-foreground">
          By clicking continue, you agree to our{' '}
          <Link
            to="/terms"
            className="underline underline-offset-4 hover:text-primary"
          >
            Terms of Service
          </Link>{' '}
          and{' '}
          <Link
            to="/privacy"
            className="underline underline-offset-4 hover:text-primary"
          >
            Privacy Policy
          </Link>
          .
        </p>
      </div>
    </div>
  );

  return adminRegister ? (
    <RootLayout>{registerContent}</RootLayout>
  ) : (
    <div className="container relative hidden mb-24 h-[800px] flex-col justify-center md:grid lg:max-w-[800px] lg:px-0">
      <Link
        to="/"
        className={cn(
          buttonVariants({ variant: 'ghost' }),
          'absolute right-4 top-4 md:right-8 md:top-8 text-base'
        )}
      >
        Login
      </Link>
      {registerContent}
    </div>
  );
}
