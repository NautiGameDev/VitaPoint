import './App.css';
import { Routes, Route } from 'react-router-dom';
import Login from "./Pages/Login/Login";
import Register from "./Pages/Register/Register";
import Dashboard from "./Pages/Dashboard/Dashboard";
import DBProfile from "./Pages/DBProfile/DBProfile";
import DBUpdateProfile from "./Pages/DBUpdateProfile/DBUpdateProfile";
import DBMessages from "./Pages/DBMessages/DBMessages";
import DBCreateMessage from "./Pages/DBCreateMessage/DBCreateMessage";
import DBMessageThread from "./Pages/DBMessageThread/DBMessageThread";
import DBAppointments from "./Pages/DBAppointments/DBAppointments";
import DBResults from './Pages/DBResults/DBResults';
import DBLabResult from './Pages/DBLabResult/DBLabResult';
import DBPrescriptions from "./Pages/DBPrescriptions/DBPrescriptions";
import DBCreateAppointment from "./Pages/DBCreateAppointment/DBCreateAppointment";
import Timeout from "./Pages/Timeout/Timeout";
import Unauthenticated from "./Pages/Unauthenticated/Unauthenticated";


function App() {
    
    return (
        <div className="app-container">
            <Routes>
                <Route index element={<Login />} />
                <Route path="/Register" element={<Register />} />
                <Route path="/Dashboard" element={<Dashboard />}>
                    <Route index element={<DBProfile />} />
                    <Route path="Profile" element={<DBProfile />} />
                    <Route path="Update-Profile" element={<DBUpdateProfile />} />
                    <Route path="Messages" element={<DBMessages />} />
                    <Route path="Messages/Thread/:messageId" element={<DBMessageThread />} />
                    <Route path="Messages/Create" element={<DBCreateMessage />} />
                    <Route path="Appointments" element={<DBAppointments />} />
                    <Route path="CreateAppointment" element={<DBCreateAppointment />} />
                    <Route path="Results" element={<DBResults />} />
                    <Route path="Results/:resultId" element={<DBLabResult />} />
                    <Route path="Prescriptions" element={<DBPrescriptions />} />
                </Route>
                <Route path="/Timeout" element={<Timeout />} />
                <Route path="/Unauthenticated" element={<Unauthenticated />} />
            </Routes>
        </div>
    );
}

export default App;