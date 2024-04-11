// import { CardUpdate, MemoryBoard, Intro, Auth } from "@/pages";
import { Link } from 'react-router-dom';

import { RootLayout } from '@/features/layout/userLayout';
import { useUserInfo } from '@/features/account/use-user-info';
import { AppointmentSection } from '@/features/appointment/appointment-section';
import { buttonVariants } from '@/features/ui/button';
import { cn } from '@/lib/utils';
import { Icons } from '@/assets/icons';

export const HomePage = () => {
  const { userInfo, isLoading } = useUserInfo();

  return (
    <RootLayout>
      <div className="flex items-center justify-center w-full">
        {isLoading ? (
          <Icons.spinner className="h-8 w-8 animate-spin" />
        ) : (
          <div className=" h-full flex-1 flex-col space-y-8 p-8  max-w-[600px] md:flex  mt-8">
            <div className="flex items-center justify-between space-y-2">
              <div>
                <h2 className="text-2xl font-bold tracking-tight">
                  {`Welcome ${userInfo?.firstName || ''}`}
                </h2>
                {userInfo?.role && userInfo?.role !== 'admin' && (
                  <p className="text-muted-foreground">
                    Here&apos;s the list of all appointment
                  </p>
                )}
              </div>
            </div>

            {userInfo?.role && userInfo?.role !== 'admin' && (
              <AppointmentSection />
            )}
            {userInfo?.role === 'admin' && (
              <div className="flex flex-col space-y-4">
                <div>
                  <Link
                    className={cn(buttonVariants({ variant: 'default' }))}
                    to="/patient-management"
                  >
                    Patient management
                  </Link>
                </div>
                <div>
                  <Link
                    className={cn(buttonVariants({ variant: 'default' }))}
                    to="/doctor-management"
                  >
                    Doctor management
                  </Link>
                </div>
                <div>
                  <Link
                    className={cn(buttonVariants({ variant: 'default' }))}
                    to="/appointment-management"
                  >
                    Appointment management
                  </Link>
                </div>
              </div>
            )}
          </div>
        )}
      </div>
    </RootLayout>
  );
};
