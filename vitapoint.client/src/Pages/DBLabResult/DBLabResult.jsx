import "./DBLabResult.css";
import { useParams } from "react-router-dom";
import { useState, useEffect } from 'react';
import { GetLabResultById } from "../../Services/LabResultService";
import { FormatDate } from "../../Clients/TextFormatterClient";

function DBLabResult() {
    const { resultId } = useParams();
    const [data, setData] = useState([]);
    const [sysMessage, setSysMessage] = useState("Loading lab result data...");

    const flagToString = (flag) => {
        switch (flag) {
            case 0:
                return "Normal";
            case 1:
                return "High";
            case 2:
                return "Low";
        }
    }

    useEffect(() => {
        const fetchData = async () => {
            const response = await GetLabResultById(resultId);

            if (response.status === 200) {
                setData(response.data);
            }
            else {
                setSysMessage(`Error ${response.status}: ${response.message}`);
            }
        }

        fetchData();
    }, [])

  return (
      <div className="page">
          {data.length === 0 ?
              (
                  <div className="result-syscontainer">
                      <h3>
                          {sysMessage}
                      </h3>
                  </div>
              ) : (
                  <div className="result-container">
                      <div className="result-title">
                          <h2>
                              <span className="material-icons" >
                                  science
                              </span>
                              Lab Results
                          </h2>
                      </div>
                     <div className = "result-card">
                          <div className="result-card-title">
                              <h2>
                                  {data.testName}
                              </h2>
                          </div>
                          <div className="result-card-row">
                              <div className="result-card-data">
                                  Ordered by <strong>{data.orderingDoctor}</strong>
                              </div>
                              <div className="result-card-data">
                                  Completed by <strong>{data.labName}</strong>
                              </div>
                          </div>
                          <div className="result-card-row">
                              <div className="result-card-data">
                                  Collected on <strong>{FormatDate(data.collectedAt)}</strong>
                              </div>
                              <div className="result-card-data">
                                  Completed on <strong>{FormatDate(data.resultAt)}</strong>
                              </div>
                          </div>
                          <div className="result-card-row">
                              <div className="result-card-data result-card-notes">
                                  <h3>Notes:</h3>
                                  <p>{data.notes}</p>
                              </div>
                          </div>
                          <div className="result-card-items">
                              <h3>Line Items:</h3>
                              <table>
                                    <thead>
                                        <tr>
                                            <th>Marker:</th>
                                            <th>Value</th>
                                            <th>Unit</th>
                                            <th>Flag</th>
                                        </tr>
                                    </thead>
                                  {data.components.map((component, index) => (
                                      <tr key={index} className={`result-card-item-${index % 2 === 0 ? "light" : "dark"}`}>
                                          <td>
                                              {component.markerName}
                                          </td>
                                          <td>
                                              {component.value}
                                          </td>
                                          <td>
                                              {component.unit}
                                          </td>
                                          <td>
                                              {flagToString(component.flag)}
                                          </td>
                                      </tr>
                                  ))}


                              </table>
                              
                          </div>
                      </div>
                </div>
              )
}
              </div >
          
  );
}

export default DBLabResult;