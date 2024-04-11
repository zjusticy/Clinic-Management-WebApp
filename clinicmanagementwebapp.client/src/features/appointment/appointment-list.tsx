// import { ComponentProps } from 'react';
// import formatDistanceToNow from 'date-fns/formatDistanceToNow';

import { Loader2 } from 'lucide-react';

import { cn } from '@/lib/utils';
import { ScrollArea } from '@/features/ui/scroll-area';

import { AppointmentListCard } from './appointment-list-card';
import { useUserInfo } from '@/features/account/use-user-info';
import { useAppointmentList } from './use-user-appointment-list';

export function AppointmentList({ status }: { status: number }) {
  const { userInfo } = useUserInfo();

  const { patientAppointmentList, isLoading, mutate } = useAppointmentList(
    (userInfo && userInfo.id) || '',
    status
  );

  return (
    <ScrollArea className="h-screen">
      {isLoading ? (
        <Loader2 className="h-4 w-4 animate-spin" />
      ) : (
        <div className="flex flex-col gap-4 p-4 px-2 pt-0">
          {patientAppointmentList &&
            patientAppointmentList.map((appointment) => (
              <div
                key={appointment.id}
                className={cn(
                  'flex flex-col items-start gap-2 rounded-lg border p-3 text-left text-sm transition-all hover:bg-accent'
                )}
              >
                <AppointmentListCard
                  item={appointment}
                  role={(userInfo && userInfo.role) || ''}
                  status={status}
                  mutate={mutate}
                />
              </div>
            ))}
        </div>
      )}
    </ScrollArea>
  );
}
