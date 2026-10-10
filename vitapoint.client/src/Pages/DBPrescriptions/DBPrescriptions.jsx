import "./DBPrescriptions.css";
import { useState, useEffect } from 'react';
import { GetPrescriptions } from '../../Services/PrescriptionService';
import PrescriptionCard from '../../Components/PrescriptionCard/PrescriptionCard';
import { useNavigate } from 'react-router-dom';

function DBPrescriptions() {
    const [data, setData] = useState([]);
    const [sysMessage, setSysMessage] = useState("Loading prescriptions...");
    const [refreshState, setRefreshState] = useState(0);

    const navigate = useNavigate();

    useEffect(() => {
        const fetchPrescriptions = async () => {
            const response = await GetPrescriptions();

            if (response.status === 200) {
                setData(response.data);
            }
            else if (response.status === 401) {
                navigate("/Timeout");
            }
            else {
                setSysMessage(`Error ${response.status}: ${response.message}`);
            }
        }

        fetchPrescriptions();
    }, [refreshState]);

  return (
      <div className="page">
          {data.length === 0 ?
              (
                  <div className="prescriptions-syscontainer">
                      <h2>{sysMessage}</h2>
                  </div>
              ) : (
                        <div className = "prescriptions-container">
                          <div className = "prescriptions-title">
                              <h2>
                                  <span className = "material-icons">
                                      medication
                                  </span>
                                Prescriptions
                              </h2 >
                          </div >
                          <div className="prescriptions-row">
                                <h3>Pending</h3>
                          </div>
                          <div className="prescriptions-row">
                              {data.filter(p => p.refillStatus === "Requested").map((prescription, k) => (
                                  <div className="prescription-holder" key={k}>
                                      <PrescriptionCard data={prescription} setRefreshState={setRefreshState} />
                                  </div>
                              ))}
                          </div>
                          <div className="prescriptions-row">
                                <h3>Approved</h3>
                          </div>
                          <div className="prescriptions-row">
                              {data.filter(p => p.refillStatus === "Approved").map((prescription, k) => (
                                  <div className="prescription-holder" key={k}>
                                      <PrescriptionCard data={prescription} setRefreshState={setRefreshState} />
                                  </div>
                              ))}
                          </div>
                          <div className="prescriptions-row">
                                <h3>Filled</h3>
                          </div>
                          <div className="prescriptions-row">
                              {data.filter(p => p.refillStatus === "None" || p.refillStatus === "Filled").map((prescription, k) => (
                                  <div className="prescription-holder" key={k}>
                                      <PrescriptionCard data={prescription} setRefreshState={setRefreshState} />
                                  </div>
                              ))}
                          </div>
                          <div className="prescriptions-row">
                              <h3>Denied</h3>
                          </div>
                          <div className="prescriptions-row">
                              {data.filter(p => p.refillStatus === "Denied").map((prescription, k) => (
                                  <div className="prescription-holder" key={k}>
                                      <PrescriptionCard data={prescription} setRefreshState={setRefreshState} />
                                  </div>
                              ))}
                          </div>
                      </div>
                  )}
          
      </div>
  );
}

export default DBPrescriptions;