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
import { UserBrief } from './types';

interface MyObject {
  [key: number]: string;
}

const genderMap: MyObject = {
  0: 'Not known',
  1: 'Male',
  2: 'Female',
  9: 'Not applicable',
};

export function UserTable({
  data,
  isLoading,
  role,
}: {
  data: UserBrief[] | undefined;
  isLoading: boolean;
  role: string;
}) {
  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Name</TableHead>
          <TableHead>Gender</TableHead>
          <TableHead>Email</TableHead>
          {role === 'patient' ? (
            <TableHead className="text-right w-[100px]">DOB</TableHead>
          ) : (
            <TableHead className="text-right w-[100px]">Status</TableHead>
          )}
        </TableRow>
      </TableHeader>
      <TableBody>
        {isLoading && <Icons.spinner className="h-4 w-4 animate-spin" />}
        {data &&
          !isLoading &&
          data.map((user) => (
            <TableRow key={user.id}>
              <TableCell className="font-medium">
                <div
                  className={cn(
                    buttonVariants({ variant: 'ghost' }),
                    'px-0 underline text-primary'
                  )}
                >{`${user.firstName} ${user.lastName}`}</div>
              </TableCell>
              <TableCell>{genderMap[user.gender] || ''}</TableCell>
              <TableCell>{user.email}</TableCell>
              <TableCell className="text-right">
                {role === 'patient'
                  ? format(user.dateOfBirth, 'yyyy-MM-dd')
                  : user.status ?? ''}
              </TableCell>
            </TableRow>
          ))}
      </TableBody>
    </Table>
  );
}
