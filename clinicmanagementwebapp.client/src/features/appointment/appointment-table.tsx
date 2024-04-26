import { useState } from 'react';

import { format } from 'date-fns';

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/features/ui/table';
import { Icons } from '@/assets/icons';
import { buttonVariants } from '@/features/ui/button';
import { cn } from '@/lib/utils';
// import { UserBrief } from './types';
import { useAdminAppointmentList } from './use-admin-appointment-list';

export function AppointmentTable({ status }: { status: number }) {
  const [isCancelling, setIsCancellingStatus] = useState(false);

  const { adminAppointmentList, isLoading, mutate } =
    useAdminAppointmentList(status);

  const handleCancelAppointment = async (appointmentId: number) => {
    setIsCancellingStatus(true);
    try {
      const noteData: {
        path: string;
        op: string;
        value: number | string;
      }[] = [
        {
          path: '/appointmentStatus',
          op: 'replace',
          value: 9,
        },
      ];
      const res = await fetch(`/api/appointments/${appointmentId}`, {
        method: 'PATCH',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(noteData),
      });

      if (res.ok) {
        mutate();
      } else {
        console.log(res);
      }
    } catch (error) {
      console.log(error);
    } finally {
      setIsCancellingStatus(false);
    }
  };

  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>MD</TableHead>
          <TableHead>PT</TableHead>
          <TableHead>Status</TableHead>
          <TableHead>Date</TableHead>
          <TableHead className="text-right w-[100px]">Action</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {isLoading && <Icons.spinner className="h-4 w-4 animate-spin" />}
        {adminAppointmentList &&
          !isLoading &&
          adminAppointmentList.map((appointment) => (
            <TableRow key={appointment.id}>
              <TableCell className="font-medium">
                <div
                  className={cn(
                    buttonVariants({ variant: 'ghost' }),
                    'px-0 underline text-primary'
                  )}
                >{`${appointment.doctor.firstName} ${appointment.doctor.lastName}`}</div>
              </TableCell>
              <TableCell>
                {' '}
                <div
                  className={cn(
                    buttonVariants({ variant: 'ghost' }),
                    'px-0 underline text-primary'
                  )}
                >{`${appointment.patient.firstName} ${appointment.patient.lastName}`}</div>
              </TableCell>
              <TableCell>{appointment.appointmentStatus}</TableCell>
              <TableCell className="text-right">
                {format(appointment.date, 'yyyy-MM-dd hh:mm a')}
              </TableCell>
              <TableCell className="font-medium">
                      {status === 0 && <div
                          className="px-0 underline text-secondary text-right cursor-pointer"
                          onClick={() => handleCancelAppointment(appointment.id)}
                      >
                          {isCancelling ? (
                              <Icons.spinner className="h-4 w-4 animate-spin" />
                          ) : (
                              'Cancel'
                          )}
                      </div>}
              </TableCell>
            </TableRow>
          ))}
      </TableBody>
    </Table>
  );
}
