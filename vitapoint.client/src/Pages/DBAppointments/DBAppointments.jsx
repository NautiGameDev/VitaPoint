import "./DBAppointments.css";
import { GetAppointments } from "../../Services/AppointmentService";
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import AppointmentCard from '../../Components/AppointmentCard/AppointmentCard';

function DBAppointments() {
    const [sysMessage, setSysMessage] = useState("Fetching appointments");
    const [appointments, setAppointments] = useState([]);
    const [refreshState, setRefreshState] = useState(0);

    const navigate = useNavigate();

    useEffect(() => {
        const fetchAppointments = async () => {
            const response = await GetAppointments();

            if (response.status === 200) {
                setAppointments(response.data);
            }
            else {
                setSysMessage(`Error ${response.status}: ${response.message}`);
            }
        }

        fetchAppointments();

    }, [refreshState]);

  return (
      <div className="page">

          {appointments.length === 0 ? (

              <div className="appointments-syscontainer">
                  <h2>{sysMessage}</h2>
              </div>

          ): (

             <div className="appointments-container">
                      <div className="appointments-header">
                        <h2>
                            <span className="material-icons">
                                calendar_month
                            </span>
                            Appointments
                        </h2>
                      </div>
                      <div className="appointments-new-container">
                          <button type="button" onClick={() => (navigate("/Dashboard/CreateAppointment"))}>New Appointment</button>
                      </div>
                      <div className="appointments-category">
                        <h3>Scheduled</h3>
                      </div>
                      <div className="appointments-list">
                          {appointments.filter(a => new Date(`${a.date}T${a.timeSlot}`) > new Date()).map((appointment, index) => (
                              <AppointmentCard key={index} data={appointment} setRefreshState={setRefreshState} />
                          ))}
                      </div>

                      <div className="appointments-category">
                          <h3>Previous</h3>
                      </div>
                      <div className="appointments-list">
                          {appointments.filter(a => new Date(`${a.date}T${a.timeSlot}`) <= new Date()).map((appointment, index) => (
                              <AppointmentCard key={index} data={appointment} setRefreshState={setRefreshState} />
                          ))}
                      </div>
            </div>

          )}
                        
      </div>
  );
}

export default DBAppointments;