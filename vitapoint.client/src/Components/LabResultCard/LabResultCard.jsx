import "./LabResultCard.css";
import { FormatDate } from "../../Clients/TextFormatterClient";
import { useNavigate } from "react-router-dom";
function LabResultCard({ data }) {
    const navigate = useNavigate();

  return (
      <div className="lab-result-card" onClick={() => (navigate(`${data.id}`)) }>
          <div className="lab-result-card-row">
              <h3>{data.testName}</h3>
          </div>
          <div className="lab-result-card-row">
              <div className="lab-result-card-data">
                  Ordered by: <strong>{data.orderingDoctor}</strong>
              </div>
              <div className="lab-result-card-data">
                  Completed by: <strong>{data.labName}</strong>
              </div>
          </div>
          <div className="lab-result-card-row">
              <div className="lab-result-card-data">
                  Collected on <strong>{FormatDate(data.collectedAt)}</strong>
              </div>
              <div className="lab-result-card-data">
                  Completed on <strong>{FormatDate(data.resultAt)}</strong>
              </div>
          </div>          

      </div>
  );
}

export default LabResultCard;