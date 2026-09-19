import { useState, useEffect } from 'react';
import { GetPatientData } from "../../Services/PatientService";
import "./DBProfile.css";
import { useNavigate } from "react-router-dom";


function DBProfile() {
    const [data, setData] = useState(null);
    const [sysMessage, setSysMessage] = useState("Loading Profile...");

    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            const response = await GetPatientData();

            if (response.status === 200) {
                setData(response.data);
            }
            else {                
                setSysMessage(`Error fetching patient data. Status code ${response.status}. ${response.message}`);
            }
        }

        fetchData();
    }, [])

  return (
      <div className="page">
          {data === null ?
              (
                <div className="profile-loading-container">
                      <span className="material-icons">
                          account_circle
                      </span>
                      {sysMessage}
                </div>
              )
              : (
              <div className="profile-container">
                  <div className="profile-header">
                          <h2>
                              <span className="material-icons">
                                  account_circle
                              </span>
                              Patient Profile
                          </h2>
                  </div>
                      <div className="profile-table">
                          <div className="profile-table-header">
                              <h3>
                                  Personal Information
                              </h3>
                          </div>
                          <div className="profile-table-row">
                              <div className="profile-table-cell">
                                  <label>Name:</label> <span className="profile-table-data">{data.firstName} {data.middleName} {data.lastName}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label>Pref. Name:</label> <span className="profile-table-data">{data.preferredName}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label>Language:</label> <span className="profile-table-data">{data.preferredLanguage}</span>
                              </div>
                          </div>
                          <div className="profile-table-row">
                              <div className="profile-table-cell">
                                  <label>Date of Birth:</label> <span className="profile-table-data">{data.dob}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label>Sex:</label> <span className="profile-table-data">{data.sex}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label>Pronouns:</label> <span className="profile-table-data">{data.pronouns}</span>
                              </div>
                      
                          </div>
                          <div className="profile-table-row">
                      
                              <div className="profile-table-cell">
                                  <label>Marital Status:</label> <span className="profile-table-data">{data.maritalStatus}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label>Race:</label> <span className="profile-table-data">{data.race}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label>Ethnicity:</label> <span className="profile-table-data">{data.ethnicity}</span>
                              </div>
                          </div>
                      </div>
                      <div className="profile-table">
                          <div className="profile-table-header">
                              <h3>
                                  Contact Information
                              </h3>
                          </div>
                          <div className="profile-table-row">
                              <div className="profile-table-cell">
                                  <label>Email:</label> <span className="profile-table-data">{data.email}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label>Phone:</label> <span className="profile-table-data">{data.phoneNumber}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label>Contact Method:</label> <span className="profile-table-data">{data.preferredContactMethod}</span>
                              </div>
                          </div>
                          <div className="profile-table-row">
                              <div className="profile-table-cell">
                                  <label>Address:</label> <span className="profile-table-data">{data.address}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label>Address 2:</label> <span className="profile-table-data">{data.address2}</span>
                              </div>
                      
                          </div>
                          <div className="profile-table-row">                      
                              <div className="profile-table-cell">
                                  <label>City:</label> <span className="profile-table-data">{data.city}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label>State:</label> <span className="profile-table-data">{data.state}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label>Zip:</label> <span className="profile-table-data">{data.zip}</span>
                              </div>
                          </div>
                      </div>

                      <div className="profile-table">
                          <div className="profile-btn-container">
                              <button className="no-select" onClick={() => (navigate("/Dashboard/Update-Profile"))} >Edit Profile</button>
                              <p>Last Updated: {data.lastUpdated}</p>
                          </div>
                      </div>
                  </div>
                  )}
              </div>
  );
}

export default DBProfile;