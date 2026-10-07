import "./PrescriptionCard.css";
import { FormatDateOnly } from "../../Clients/TextFormatterClient";
import { useState } from 'react';
import { RequestRefill } from "../../Services/PrescriptionService";

function PrescriptionCard({ data, setRefreshState }) {
    const [refillRequested, setRefillRequested] = useState(false);
    const [isSendingCall, setIsSendingCall] = useState(false);
    const [sysMessage, setSysMessage] = useState("Sending message to server...");

    const confirmRequest = async (e) => {
        e.preventDefault();

        setSysMessage("Sending message to server...");
        setIsSendingCall(true);
        setRefillRequested(false);

        const response = await RequestRefill(data.id);

        if (response.status === 200) {
            setSysMessage(response.message);

            setTimeout(() => {
                setIsSendingCall(false);
                setRefreshState(prev => prev + 1);
            }, 5000);            
        }

        else {
            setSysMessage(`Error ${response.status}: ${response.message}`);

            setTimeout(() => {
                setIsSendingCall(false);
            }, 5000); 
        }
        

    }

    const canRefill = () => {
        if (data.refillsRemaining === 0 || (data.refillStatus !== "None" && data.refillStatus !== "Filled")) {
            return false;
        }

        return true;
    }

    return (
        <div className="prescription-card">
            <div className="prescription-card-title">
                <h3>{data.medicineName}</h3>
            </div>
            <div className="prescription-card-row">
                <div className="prescription-card-cell">
                    Prescribed by <strong>{data.doctorName}</strong> on <strong>{FormatDateOnly(data.prescribedAt)}</strong>
                </div>
            </div>
            <div className="prescription-card-row">
                <div className="prescription-card-cell">
                    Last refilled on <strong>{FormatDateOnly(data.lastRefilledAt)}</strong>
                </div>
                <div className="prescription-card-cell">
                    Last refill request: <strong>{FormatDateOnly(data.lastRefillRequest)}</strong>
                </div>
                <div className="prescription-card-cell">
                    Refills Remaining: <strong>{data.refillsRemaining}</strong>
                </div>
            </div>
            <div className="prescription-card-btn-container">
                {canRefill() ? (
                    <button type="button" className="enabled-btn" onClick={() => (setRefillRequested(true))} >Request Refill</button>
                ) : (
                    <button type="button" className="disabled-btn">Request Refill</button>
                )}
            </div>

            {refillRequested ? (
                <div className="prescription-card-popup">
                    <div className="prescription-card-row">
                        <h4>Confirm request refill for <strong>{data.medicineName}</strong>?</h4>
                    </div>
                    <div className="prescription-card-btn-container">
                        <button type="button" className="enabled-btn" onClick={() => (setRefillRequested(false))}>Cancel</button>
                        <button type="button" className="enabled-btn" onClick={(e) => (confirmRequest(e))}>Confirm</button>
                    </div>
                </div>
            ) : (<></>)}

            {isSendingCall ? (
                <div className="prescription-card-popup">
                    <div className="prescription-card-row">
                        <h4>{sysMessage}</h4>
                    </div>
                </div>
            ) : (<></>)}
      </div>
  );
}

export default PrescriptionCard;