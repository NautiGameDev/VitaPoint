import './App.css';
import { Routes, Route } from 'react-router-dom';
import Login from "./Pages/Login/Login";
import Register from "./Pages/Register/Register";
import Dashboard from "./Pages/Dashboard/Dashboard";
import DBProfile from "./Pages/DBProfile/DBProfile";
import DBUpdateProfile from "./Pages/DBUpdateProfile/DBUpdateProfile";
import DBMessages from "./Pages/DBMessages/DBMessages";
import DBAppointments from "./Pages/DBAppointments/DBAppointments";
import DBResults from './Pages/DBResults/DBResults';
import DBPrescriptions from "./Pages/DBPrescriptions/DBPrescriptions";


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
                    <Route path="Appointments" element={<DBAppointments />} />
                    <Route path="Results" element={<DBResults />} />
                    <Route path="Prescriptions" element={<DBPrescriptions />} />
                </Route>
            </Routes>
        </div>
    );
}

export default App;