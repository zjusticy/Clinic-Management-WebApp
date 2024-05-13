export type AppointBrief = {
  id: number;
  doctor: {
    id: string;
    firstName: string;
    lastName: string;
  };
  patient: {
    id: string;
    firstName: string;
    lastName: string;
  };
  date: string;
  appointmentStatus: number;
  reasonForVisit: string;
  prescription: string;
};
