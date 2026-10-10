import { useState, useEffect } from 'react';
import { GetLabResults } from '../../Services/LabResultService';
import LabResultCard from '../../Components/LabResultCard/LabResultCard';
import "./DBResults.css";
import { useNavigate } from 'react-router-dom';

function DBResults() {
    const [sysMessage, setSysMessage] = useState('Loading lab results...');
    const [data, setData] = useState([]);

    const navigate = useNavigate();

    useEffect(() => {
        const fetchLabResults = async () => {
            const response = await GetLabResults();

            if (response.status === 200) {
                if (response.data.length === 0) {
                    setSysMessage("Couldn't find lab results for user");
                }
                setData(response.data);
            }
            else if (response.status === 401) {
                navigate("/Timeout");
            }
            else {
                setSysMessage(`Error ${response.status}: ${response.message}`);
            }
        }

        fetchLabResults();

    }, []);

  return (
      <div className="page">
          {data.length === 0 ? (
                <div className="lab-results-syscontainer">
                    <h3>{sysMessage}</h3>
                </div>        
          ) : (
                <div className="lab-results-container">
                      <div className="lab-results-container-row">
                          <h2>
                              <span className="material-icons" >
                                  science
                              </span>
                            Lab Results
                          </h2>
                      </div>
                      <div className="lab-results-container-row">
                          <h3>Pending</h3>
                      </div>
                      <div className="lab-results-container-row">
                          <div className="lab-results-display">

                              { data.filter(result => result.status === "Pending").map((result, index) => (
                                  <div key={index} className="lab-result-card-container">
                                      <LabResultCard data={result} />                             
                                  </div>
                              ))}

                          </div>
                      </div>
                      <div className="lab-results-container-row">
                        <h3>Completed</h3>
                      </div>
                      <div className="lab-results-container-row">
                          <div className="lab-results-display">

                              {data.filter(result => result.status === "Final").map((result, index) => (
                                  <div key={index} className="lab-result-card-container">
                                      <LabResultCard data={result} />
                                  </div>
                              ))}

                          </div>
                      </div>
                </div>
          )}

          
      </div>
  );
}

export default DBResults;