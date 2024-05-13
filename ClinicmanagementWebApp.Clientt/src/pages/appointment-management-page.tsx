import { RootLayout } from '@/features/layout/userLayout';
import { AdminAppointmentSection } from '@/features/appointment/admin-appointment-section';

export const AppointmentManagementPage = () => {
  return (
    <RootLayout>
      <div className="flex items-center justify-center w-full">
        <div className=" h-full flex-1 flex-col space-y-8 p-8  max-w-[600px] md:flex  mt-8">
          <div className="flex items-center justify-between space-y-2">
            <div>
              <h2 className="text-2xl font-bold tracking-tight">
                Appointments
              </h2>
            </div>
          </div>
          <AdminAppointmentSection />
        </div>
      </div>
    </RootLayout>
  );
};
