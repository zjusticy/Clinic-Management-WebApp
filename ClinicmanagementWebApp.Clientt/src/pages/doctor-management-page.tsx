import { Link } from 'react-router-dom';

import { RootLayout } from '@/features/layout/userLayout';
import { UserTable } from '@/features/account/user-table';
import { useAdminDoctorList } from '@/features/account/use-admin-doctor-list';
import { buttonVariants } from '@/features/ui/button';

export const DoctorManagementPage = () => {
  const { adminDoctorList, isLoading } = useAdminDoctorList();

  return (
    <RootLayout>
      <div className="flex items-center justify-center w-full">
        <div className=" h-full flex-1 flex-col space-y-8 p-8  max-w-[600px] md:flex  mt-8">
          <div className="flex items-center justify-between space-y-2">
            <div>
              <h2 className="text-2xl font-bold tracking-tight">Doctors</h2>
            </div>
          </div>
          <div className="flex justify-end">
            <Link
              to="/admin/doctor-register"
              className={buttonVariants({ variant: 'default' })}
            >
              Add
            </Link>
          </div>
          <UserTable
            data={adminDoctorList}
            isLoading={isLoading}
            role="doctor"
          />
        </div>
      </div>
    </RootLayout>
  );
};
