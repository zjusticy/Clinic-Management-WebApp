import { useState } from 'react';
import { isYesterday, format, isTomorrow } from 'date-fns';

import { Icons } from '@/assets/icons';

import { Button } from '@/features/ui/button';
import { Textarea } from '@/features/ui/textarea';
import { Separator } from '@/features/ui/separator';

import { AppointBrief } from './types';

function formatDate(date: Date) {
  const today = new Date();
  const yesterday = new Date(today);
  const tomorrow = new Date(today);
  yesterday.setDate(yesterday.getDate() - 1);
  tomorrow.setDate(tomorrow.getDate() + 1);

  if (isYesterday(date)) {
    return 'Yesterday' + format(date, 'hh:mm a');
  } else if (isTomorrow(date)) {
    return 'Tomorrow' + format(date, 'hh:mm a');
  } else {
    return format(date, 'yyyy-MM-dd hh:mm a');
  }
}

type AppointListCardProps = {
  item: AppointBrief;
  role: string;
  status: number;
  mutate: () => void;
};

const statusTable = new Map([
  [0, 'scheduled'],
  [1, 'completed'],
  [9, 'cancelled'],
]);

export function AppointmentListCard({
  item,
  role,
  status,
  mutate,
}: AppointListCardProps) {
  const [note, setNote] = useState('');

  const [editState, setEditState] = useState(false);
  const [editCancelling, setEditCancellingState] = useState(false);

  const [isLoading, setLoadingStatus] = useState(false);

  const [isCancelling, setIsCancellingStatus] = useState(false);

  const handleAddingNote = async () => {
    setLoadingStatus(true);
    try {
      const noteData: {
        path: string;
        op: string;
        value: number | string;
      }[] = [
        {
          path: '/appointmentStatus',
          op: 'replace',
          value: 1,
        },
      ];
      if (note) {
        noteData.push({
          path: '/prescription',
          op: 'replace',
          value: note,
        });
      }
      const res = await fetch(`/api/appointments/${item.id}`, {
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
      setLoadingStatus(false);
    }
  };

  const handleCancelAppointment = async () => {
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
          value: 1,
        },
      ];
      if (note) {
        noteData.push({
          path: '/prescription',
          op: 'replace',
          value: note,
        });
      }
      const res = await fetch(`/api/appointments/${item.id}`, {
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
    <>
      <div className="flex w-full flex-col gap-1">
        <div className="flex items-center">
          <div className="flex items-center gap-2">
            <div className="font-semibold">
              {role === 'patient'
                ? `Doc. ${item.doctor.firstName} ${item.doctor.lastName}`
                : `Patient: ${item.patient.firstName} ${item.patient.lastName}`}
            </div>
          </div>
          <div className="ml-auto text-xs text-muted-foreground">
            {formatDate(new Date(item.date))}
          </div>
        </div>
        <div className="flex items-center">
          <div className="text-xs font-medium text-primary capitalize">
            {statusTable.get(item.appointmentStatus)}
          </div>
          {status === 0 && role === 'doctor' && (
            <Button
              className="ml-auto bg-white text-primary"
              variant="outline"
              onClick={() => {
                setEditState((prev) => !prev);
              }}
            >
              {editState ? 'X' : 'Add note'}
            </Button>
          )}

          {/*{status === 0 && role === 'patient' && (
            <Button
              className="ml-auto bg-white text-secondary"
              variant="outline"
              onClick={() => {
                setEditCancellingState((prev) => !prev);
              }}
            >
              {editCancelling ? 'X' : 'Cancel'}
            </Button>
          )}*/}
        </div>
        <div className="line-clamp-2 text-xs text-muted-foreground pt-2">
          <span className="font-semibold">Symptom:</span>{' '}
          {item.reasonForVisit || ''}
        </div>
        {status === 1 && (
          <div className="line-clamp-2 text-xs text-muted-foreground pt-2">
            <span className="font-semibold">Prescription:</span>{' '}
            {item.prescription || ''}
          </div>
        )}

        {editState && status === 0 && (
          <div className="mt-4 space-y-2">
            <Textarea
              placeholder="Add result, e.g. prescription"
              className="resize-none"
              value={note}
              onChange={(e) => setNote(e.target.value)}
            />
            <div className="flex justify-end">
              <Button onClick={handleAddingNote}>
                {isLoading ? (
                  <Icons.spinner className="h-4 w-4 animate-spin" />
                ) : (
                  'Add'
                )}
              </Button>
            </div>
          </div>
        )}

        {editCancelling && status === 0 && (
          <div className="mt-4">
            <Separator />
            <div className="mt-4">Do you want to cancel this appointment?</div>
            <div className="mt-4 flex justify-center">
              <Button
                className="mr-8"
                variant="outline"
                onClick={() => setEditCancellingState(false)}
              >
                No
              </Button>

              <Button onClick={handleCancelAppointment}>
                {isCancelling ? (
                  <Icons.spinner className="h-4 w-4 animate-spin" />
                ) : (
                  'Yes'
                )}
              </Button>
            </div>
          </div>
        )}
      </div>
    </>
  );
}
