import { BookAppoitmentForm } from '@/features/appointment/book-appointment-form';
import { RootLayout } from '@/features/layout/userLayout';

export function BookAppoitmentPage({
  adminBooking = false,
}: {
  adminBooking?: boolean;
}) {
  return (
    <RootLayout>
      <div className="lg:p-8">
        <div className="mx-auto flex w-full flex-col justify-center space-y-8 sm:w-[400px]">
          <div className="flex flex-col space-y-4 text-center">
            <h1 className="text-2xl font-bold tracking-normal">
              Book an appointment
            </h1>
          </div>
          <BookAppoitmentForm adminBooking={adminBooking} />
        </div>
      </div>
    </RootLayout>
  );
}
