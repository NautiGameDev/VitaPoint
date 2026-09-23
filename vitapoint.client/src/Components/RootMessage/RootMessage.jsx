import "./RootMessage.css";
import { useNavigate } from 'react-router-dom';
function RootMessage({ data }) {
    const navigate = useNavigate();

    const formatDate = (dateString) => {
        if (!dateString) return '';
        return new Date(dateString).toLocaleString('en-US', {
            month: 'short',
            day: 'numeric',
            year: 'numeric',
            hour: 'numeric',
            minute: '2-digit',
        });
    };

  return (
      <div className="root-message-component no-select" onClick={() => (navigate(`Thread/${data.id}`)) }>
          <div className="root-message-icon-container">
              <span className="material-icons">
                  mail
              </span>
          </div>
          <div className="root-message-data-container">
              <div className="root-message-row">
                  <h3>{data.subject}</h3>
              </div>
              <div className="root-message-row">
                  <p><strong>{formatDate(data.timeSent)}</strong></p>
              </div>
              <div className="root-message-row">
                  <p><strong>{data.senderName}</strong> to <strong>{data.receiverName}</strong></p>
              </div>
          </div>          
      </div>
  );
}

export default RootMessage;