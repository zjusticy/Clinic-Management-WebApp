// import { CardUpdate, MemoryBoard, Intro, Auth } from "@/pages";
import { useNavigate } from 'react-router-dom';

import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/features/ui/tabs';
import { AppointmentTable } from '@/features/appointment/appointment-table';
import { Separator } from '@/features/ui/separator';
import { Button } from '@/features/ui/button';

const appointmentStatusTable = {
  scheduled: 0,
  completed: 1,
  cancelled: 9,
};

export const AdminAppointmentSection = () => {
  const navigate = useNavigate();

  const onClickedHandler = () => {
    navigate('/admin/new-appointment');
  };

  return (
    <div>
      <Tabs defaultValue="scheduled">
        <div className="flex items-center px-2 py-2">
          <TabsList>
            <TabsTrigger
              value="scheduled"
              className="text-zinc-600 dark:text-zinc-200"
            >
              Scheduled
            </TabsTrigger>
            <TabsTrigger
              value="completed"
              className="text-zinc-600 dark:text-zinc-200"
            >
              Completed
            </TabsTrigger>
            <TabsTrigger
              value="cancelled"
              className="text-zinc-600 dark:text-zinc-200"
            >
              Cancelled
            </TabsTrigger>
          </TabsList>
          <Button className="ml-auto" onClick={onClickedHandler}>
            New
          </Button>
        </div>
        <Separator />

        <div className="py-4">
          <TabsContent value="scheduled" className="m-0">
            <AppointmentTable
              // items={mails}
              status={appointmentStatusTable['scheduled']}
            />
          </TabsContent>
          <TabsContent value="completed" className="m-0">
            <AppointmentTable
              // items={mails}
              status={appointmentStatusTable['completed']}
            />
          </TabsContent>
          <TabsContent value="cancelled" className="m-0">
            <AppointmentTable
              // items={mails}
              status={appointmentStatusTable['cancelled']}
            />
          </TabsContent>
        </div>
      </Tabs>
    </div>
  );
};
