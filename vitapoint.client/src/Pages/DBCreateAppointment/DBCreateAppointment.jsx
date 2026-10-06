import "./DBCreateAppointment.css";
import { useState, useEffect } from 'react';
import { GetDoctors, GetUnavailableTimes } from '../../Services/AppointmentService';
import { FormatTimeOnly } from "../../Clients/TextFormatterClient";
import { PostNewAppointment } from "../../Services/AppointmentService";
import { useNavigate } from 'react-router-dom';

function DBCreateAppointment() {
    const [sysMessage, setSysMessage] = useState("Fetching available doctors...");
    const [errorMessage, setErrorMessage] = useState("");
    const [doctorsList, setDoctorsList] = useState([]);
    const [hasFetchedTimes, setHasFetchedTimes] = useState(false);
    const [isSendingAppointment, setIsSendingAppointment] = useState(false);
    const [unavailableTimes, setUnavailableTimes] = useState([]);
    const [timesMessage, setTimesMessage] = useState("Select a doctor and date to see available times");

    const [doctor, setDoctor] = useState("");
    const [date, setDate] = useState("");
    const [time, setTime] = useState("");
    const [comments, setComments] = useState("");
    const [aptType, setAptType] = useState("");

    const navigate = useNavigate();
    const todaysDate = new Date().toLocaleDateString('en-CA');
    const timeSlots = [
        "08:00:00",
        "08:30:00",
        "09:00:00",
        "09:30:00",
        "10:00:00",
        "10:30:00",
        "11:00:00",
        "11:30:00",
        "12:00:00",
        "12:30:00",
        "13:00:00",
        "13:30:00",
        "14:00:00",
        "14:30:00",
        "15:00:00",
        "15:30:00",
        "16:00:00"
    ];

    const handleDoctorSelect = (doctorId) => {
        setDoctor(doctorId);
        getUnavailableTimes(doctorId, date);
    }

    const handleDateSelect = (date) => {
        if (new Date(date).toLocaleDateString('en-CA') < todaysDate) return;

        setDate(date);
        getUnavailableTimes(doctor, date);
    }

    const getUnavailableTimes = async (doctorId, selectedDate) => {
        setHasFetchedTimes(false);

        if (doctorId.trim() === "" || selectedDate.trim() === "") return;

        
        const response = await GetUnavailableTimes(selectedDate, doctorId);

        if (response.status === 200) {
            setUnavailableTimes(response.data);
            console.log(response.data);
            setHasFetchedTimes(true);
        }
        else {
            setTimesMessage(`Error fetching times. Status ${response.status}: ${response.message}`);
        }               
    }

    const getTimeSlotStatus = (selectedTime) => {
        if (unavailableTimes.some(t => t.unavailableTime === selectedTime)) return "unavailable";
        else if (time === selectedTime) return "selected";
        else return "available";
    }

    const handleSelectTime = (selectedTime) => {
        if (unavailableTimes.some(t => t.unavailableTime === selectedTime)) return;

        setTime(selectedTime);
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (new Date(date).toLocaleDateString('en-CA') < todaysDate) {
            setErrorMessage("Date must not be in the past");
            return;
        }
        else if (aptType === "") {
            setErrorMessage("Must select appointment type");
            return;
        }
        else if (doctor === "") {
            setErrorMessage("Must select doctor for the appointment");
            return;
        }
        else if (time === "") {
            setErrorMessage("Must select a valid time slot");
            return;
        }

        const body = {
            doctorId: doctor,
            date: date,
            timeSlot: time,
            category: aptType,
            notes: comments
        }

        setSysMessage("Sending appointment to server...");
        setIsSendingAppointment(true);        

        const response = await PostNewAppointment(body);

        if (response.status === 200) {
            setSysMessage("Appointment scheduled successfully. Navigating to appointments page...");

            setTimeout(() => {
                navigate("/Dashboard/Appointments");
            }, 5000)
        }
        else {
            setSysMessage(`Error scheduling appointment: Status ${response.status}: ${response.message}`);

            setTimeout(() => {
                setIsSendingAppointment(false);
            }, 5000)
        }
    }

    useEffect(() => {
        const fetchDoctors = async () => {
            const response = await GetDoctors();

            if (response.status === 200) {
                if (response.data.length === 0) {
                    setSysMessage("No available doctors could be found");
                }
                else {
                    setDoctorsList(response.data);
                    
                }
            }
            else {
                setSysMessage(`Error ${response.status}: ${response.message}`);
            }
        }

        fetchDoctors();
    }, []);

    return (
        <div className="page">
            {(doctorsList.length === 0 || isSendingAppointment ) ? (
                <div className="create-appointment-sysContainer">
                    <h2>
                        {sysMessage}
                    </h2>
                </div>
            ) : (
                <div className="create-appointment-container">
                    <div className="create-appointment-header">
                        <h2>
                            <span className="material-icons">
                                calendar_month
                            </span>
                            Schedule New Appointment
                        </h2>
                    </div>
                    <div className="create-appointment-card">
                            <form onSubmit={(e) => (handleSubmit(e))} >
                                <div className="create-appointment-row">
                                    <div className="create-appointment-error">
                                        {errorMessage}
                                    </div>
                                </div>
                                <div className="create-appointment-row">
                                    <div className="create-appointment-col">
                                        <label>
                                            Type:
                                        </label>
                                        <select value={aptType} onChange={(e) => (setAptType(e.target.value))}>
                                            <option value="" defaultValue disabled>--</option>
                                            <option value="general checkup">General Checkup</option>
                                            <option value="sick visit">Sick Visit</option>
                                            <option value="follow up">Follow Up</option>
                                            <option value="lab work">Lab Work</option>
                                            <option value="vaccination">Vaccination</option>
                                            <option value="telehealth">Telehealth</option>
                                            <option value="other">Other</option>
                                        </select>
                                    </div>
                                    <div className="create-appointment-col">
                                        <label>
                                            Select Doctor:
                                        </label>
                                        <select value={doctor} onChange={(e) => (handleDoctorSelect(e.target.value))} >
                                            <option defaultValue disabled value="">--</option>

                                            {doctorsList.map((doctor, index) => (
                                                <option key={index} value={doctor.sysId}>{`${doctor.title} ${doctor.firstName} ${doctor.lastName}`}</option>
                                            ))}

                                        </select>
                                    </div>
                                    <div className="create-appointment-col">
                                        <label>Date:</label>
                                        <input type="date" min={todaysDate} value={date} onChange={(e) => (handleDateSelect(e.target.value))} />
                                    </div>
                                </div>
                                <div className="create-appointment-row">
                                    {!hasFetchedTimes ? (
                                        <h3>{timesMessage}</h3>
                                    ) : (
                                        <div className="create-appointments-time-grid">
                                                    {timeSlots.map((time, index) => (
                                                        <div className={`create-appointments-time-slot-${getTimeSlotStatus(time)} no-select`} key={index} onClick={() => (handleSelectTime(time))} >
                                                            {FormatTimeOnly(time)}
                                                        </div>
                                                        ))}
                                        </div>
                                        )}
                                </div>
                                <div className="create-appointment-row">
                                    <div className="create-appointment-comments-container">
                                        <label>Comments:</label>
                                        <textarea placeholder="Enter comment about your appointment" value={comments} onChange={(e) => (setComments(e.target.value))} ></textarea>
                                    </div>
                                </div>
                                <div className="create-appointment-btn-container">
                                    <button type="submit">Submit</button>
                                </div>
                        </form>
                    </div>
               </div>
          )}
      </div>
  );
}

export default DBCreateAppointment;