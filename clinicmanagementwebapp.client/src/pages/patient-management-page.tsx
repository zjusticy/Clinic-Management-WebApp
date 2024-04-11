// import { CardUpdate, MemoryBoard, Intro, Auth } from "@/pages";
import { Link } from 'react-router-dom';

import { RootLayout } from '@/features/layout/userLayout';
import { useAdminPatientList } from '@/features/account/use-admin-patient-list';
import { UserTable } from '@/features/account/user-table';
import { buttonVariants } from '@/features/ui/button';

export const PatientManagementPage = () => {
  const { adminPatientList, isLoading } = useAdminPatientList();

  return (
    <RootLayout>
      <div className="flex items-center justify-center w-full">
        <div className=" h-full flex-1 flex-col space-y-4 p-8  max-w-[600px] md:flex  mt-8">
          <div className="flex items-center justify-between space-y-2">
            <div>
              <h2 className="text-2xl font-bold tracking-tight">Patients</h2>
            </div>
          </div>
          <div className="flex justify-end">
            <Link
              to="/admin/patient-register"
              className={buttonVariants({ variant: 'default' })}
            >
              Add
            </Link>
          </div>
          <UserTable
            data={adminPatientList}
            isLoading={isLoading}
            role="patient"
          />
        </div>
      </div>
    </RootLayout>
  );
};
