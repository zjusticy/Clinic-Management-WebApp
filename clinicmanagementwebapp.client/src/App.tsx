import { Route, Routes } from 'react-router-dom';
import { AuthPage, RegisterPage, HomePage, BookAppoitmentPage } from '@/pages';
import { PatientManagementPage } from './pages/patient-management-page';
import { DoctorManagementPage } from './pages/doctor-management-page';
import { AppointmentManagementPage } from './pages/appointment-management-page';

// interface Forecast {
//   date: string;
//   temperatureC: number;
//   temperatureF: number;
//   summary: string;
// }

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<AuthPage />} />
        <Route path="/register" element={<RegisterPage role="patient" />} />
        <Route path="/home" element={<HomePage />} />
        <Route path="/new-appointment" element={<BookAppoitmentPage />} />
        <Route path="/patient-management" element={<PatientManagementPage />} />
        <Route path="/doctor-management" element={<DoctorManagementPage />} />
        <Route
          path="/admin/patient-register"
          element={<RegisterPage role="patient" adminRegister={true} />}
        />
        <Route
          path="/admin/doctor-register"
          element={<RegisterPage role="doctor" adminRegister={true} />}
        />
        <Route
          path="/admin/new-appointment"
          element={<BookAppoitmentPage adminBooking={true} />}
        />
        <Route
          path="/appointment-management"
          element={<AppointmentManagementPage />}
        />
      </Routes>
    </>
  );
}

export default App;
