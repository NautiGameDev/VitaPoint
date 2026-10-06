import "./AppointmentCard.css";
import { FormatDate } from '../../Clients/TextFormatterClient';
import { CancelAppointment } from '../../Services/AppointmentService';
import { useState } from 'react';

function AppointmentCard({ data, setRefreshState }) {
    const [cardMessage, setCardMessage] = useState("Cancelling appointment...");
    const [isCancelling, setIsCancelling] = useState(false);
    const [isSendingCall, setIsSendingCall] = useState(false);

    const handleCancelAppointment = async (e) => {
        e.preventDefault();

        //Pull up confirmation window
        setIsCancelling(true);
    }

    const handleConfirmCancel = async (e) => {
        e.preventDefault();

        setCardMessage("Cancelling appointment...");
        setIsSendingCall(true);

        const response = await CancelAppointment(data.id);

        if (response.status === 200) {
            setCardMessage("Appointment cancelled successfully. Refreshing page...");

            setTimeout(() => {
                setIsCancelling(false);
                setIsSendingCall(false);
                setRefreshState(prev => prev + 1);
            }, 5000)
        }
        else {
            setCardMessage(`Error ${response.status}: ${response.message}`);

            setTimeout(() => {
                setIsCancelling(false);
                setIsSendingCall(false);
            }, 5000); 
        }

    }

  return (
      <div className="appointment-card">
          <div className="appointment-card-header no-select">
              <h4>{FormatDate(`${data.date}T${data.timeSlot}`)}</h4>
          </div>
          <div className="appointment-card-row no-select">
              <strong>{data.categoryName} with {data.doctorName}</strong>
          </div>
          <div className="appointment-card-row">
              <p>
                  {data.notes}
              </p>
          </div>
          {new Date(`${data.date}T${data.timeSlot}`) > new Date() ? (
              <div className="appointment-btn-container">
                  <button type="button" onClick={(e) => (handleCancelAppointment(e))} ><span className="no-select">Cancel Appointment</span></button>
              </div>
          ) : ("")}

          {(isCancelling && !isSendingCall) ? (
              <div className="appointment-card-popup">
                  <div className="appointment-card-row">
                      <h4>Confirm cancel appointment for {FormatDate(`${data.date}T${data.timeSlot}`)}?</h4>
                  </div>
                  <div className="appointment-btn-container">
                      <button type="button" onClick={() => (setIsCancelling(false))} >Cancel</button>
                      <button type="button" onClick={(e) => (handleConfirmCancel(e))} >Confirm</button>
                  </div>
              </div>
          ) : ("")}

          {(isSendingCall && isCancelling) ? (
              <div className="appointment-card-popup">
                  <div className="appointment-card-row">
                      <h4>
                          {cardMessage}
                      </h4>
                  </div>
              </div>
          ): ("")}
      </div>
  );
}

export default AppointmentCard;