import { useState, useEffect } from 'react';
import { GetPatientData, UpdatePatientData } from "../../Services/PatientService";
import "./DBUpdateProfile.css";
import { Link, useNavigate } from 'react-router-dom';


function DBUpdateProfile() {
    const [data, setData] = useState(null);
    const [sysMessage, setSysMessage] = useState("Loading Data...");    
    const [updateError, setUpdateError] = useState([]);
    const [isChangingPass, setIsChangingPass] = useState(false);
    const [password, setPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    const [preferredName, setPreferredName] = useState("");
    const [preferredLanguage, setPreferredLanguage] = useState("");
    const [pronouns, setPronouns] = useState("");
    const [maritalStatus, setMaritalStatus] = useState("");
    const [phone, setPhone] = useState("");
    const [contactMethod, setContactMethod] = useState("");
    const [address, setAddress] = useState("");
    const [address2, setAddress2] = useState("");
    const [city, setCity] = useState("");
    const [state, setState] = useState("");
    const [zip, setZip] = useState("");

    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();

        let errorArray = []
        const hasCapital = /[A-Z]/;
        const hasLowercase = /[a-z]/;
        const hasNumber = /[0-9]/;
        const hasSymbol = /[^a-zA-Z0-9\s]/;

        if (password.trim() === "") {
            errorArray = [...errorArray, "Password required"];
        }

        //Update password changing
        if (isChangingPass && newPassword.trim() === "") {
            errorArray = [...errorArray, "New password required"];
        }
        else if (isChangingPass && confirmPassword.trim() !== newPassword.trim()) {
            errorArray = [...errorArray, "New password and confirm password must match"];
        }
        else if (isChangingPass && newPassword.trim().length < 8) {
            errorArray = [...errorArray, "New password must have at least 8 characters"];
        }
        else if (isChangingPass && !hasCapital.test(newPassword)) {
            errorArray = [...errorArray, "New password must have at least on uppercase letter"];
        }
        else if (isChangingPass && !hasLowercase.test(newPassword)) {
            errorArray = [...errorArray, "New password must have at least one lowercase letter"];
        }
        else if (isChangingPass && !hasNumber.test(newPassword)) {
            errorArray = [...errorArray, "New password must have at least one number"];            
        }
        else if (isChangingPass && !hasSymbol.test(newPassword)) {
            errorArray = [...errorArray, "Password must have at least one special character (!@#$%^&*)"];            
        }

        if (phone.trim() === "") {
            errorArray = [...errorArray, "Phone number required"];
        }

        if (address.trim() === "") {
            errorArray = [...errorArray, "Address required"];
        }

        if (city.trim() === "") {
            errorArray = [...errorArray, "City required"];
        }

        if (state.trim() === "") {
            errorArray = [...errorArray, "State Required"];
        }

        if (zip.trim() === "") {
            errorArray = [...errorArray, "Zip required"];
        }

        setUpdateError(errorArray);

        if (errorArray.length > 0) {
            return;
        }

        const body = {
            currentPassword: password,
            newPassword: newPassword,
            maritalStatus: maritalStatus,
            preferredName: preferredName,
            pronouns: pronouns,
            phoneNumber: phone,
            preferredContactMethod: contactMethod,
            address: address,
            address2: address2,
            city: city,
            state: state,
            zip: zip
        }

        const response = await UpdatePatientData(body);

        if (response.status === 200) {
            navigate("/Dashboard/Profile");
        }
        else {
            setUpdateError([`Status code ${response.status}: ${response.message} `]);
        }

    }

    useEffect(() => {
        const fetchData = async () => {
            const response = await GetPatientData();

            if (response.status === 200) {
                setData(response.data);      

                setPreferredName(response.data.preferredName);
                setPreferredLanguage(response.data.preferredLanguage);
                setPronouns(response.data.pronouns);
                setMaritalStatus(response.data.maritalStatus);
                setPhone(response.data.phoneNumber);
                setContactMethod(response.data.preferredContactMethod);
                setAddress(response.data.address);
                setAddress2(response.data.address2);
                setCity(response.data.city);
                setState(response.data.state);
                setZip(response.data.zip);
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
                  <form className="profile-container" onSubmit={(e) => (handleSubmit(e))} >
                      <div className={updateError.length > 0 ? "update-profile-error-container" : ""} >
                          {updateError.length > 0 ? (<strong>Error updating your profile</strong>) : ""}

                          <ul>
                              {updateError.map((error) => (
                                  <li>
                                      {error}
                                  </li>
                              ))}
                          </ul>
                      </div>
                      <div className="profile-table">
                          <div className="profile-table-header">
                              <h3>
                                  Account
                              </h3>
                          </div>
                          
                          <div className="profile-table-row">
                              <div className="profile-table-cell">
                                  <strong>Email:</strong> <span className="profile-table-data update-profile-disabled-field">{data.email}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label htmlFor="current-password">Password:</label> <input name="current-password" type="password" value={password} onChange={(e) => (setPassword(e.target.value))}/>
                              </div>
                          </div>
                          <div className="profile-table-row">
                              <div className="update-profile-table-cell update-pass-container">
                                  <label htmlFor="change-password">
                                      Change password:
                                  </label>
                                  <input name="change-password" type="checkbox" value={isChangingPass} onChange={(e) => (setIsChangingPass(e.target.checked))} /> 
                              </div>
                          </div>
                          <div className={isChangingPass ? "profile-table-row" : "update-profile-hidden-row"}>
                              <div className="profile-table-cell">
                                  <label htmlFor="new-password">
                                      New Password:
                                  </label>
                                  <input name="new-password" type="password" value={newPassword} onChange={(e) => (setNewPassword(e.target.value))} />
                              </div>
                              <div className="profile-table-cell">
                                  <label htmlFor="confirm-password">
                                      Confirm Password:
                                  </label>
                                  <input name="confirm-password" type="password" value={confirmPassword} onChange={(e) => (setConfirmPassword(e.target.value))} />
                              </div>
                          </div>
                      </div>

                      <div className="profile-table">
                          <div className="profile-table-header">
                              <h3>
                                  Personal Information
                              </h3>
                          </div>
                          <div className="profile-table-row">
                              <div className="profile-table-cell">
                                  <strong>Full Name:</strong> <span className="profile-table-data update-profile-disabled-field">{data.firstName} {data.middleName} {data.lastName}</span>
                              </div>
                            </div>
                            <div className="profile-table-row">
                              <div className="profile-table-cell">
                                  <label htmlFor="preferred-name">Preferred Name:</label> <input name="preferred-name" type="text" value={preferredName} onChange={(e) => (setPreferredName(e.target.value))} />
                              </div>
                              <div className="profile-table-cell">
                                  <label htmlFor="preferred-language">Preferred Language:</label> <input name="preferred-language" type="text" value={preferredLanguage} onChange={(e) => (setPreferredLanguage(e.target.value))} />
                              </div>
                          </div>
                          <div className="profile-table-row">
                              <div className="profile-table-cell">
                                  <strong>Date of Birth:</strong> <span className="profile-table-data update-profile-disabled-field">{data.dob}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <strong>Sex:</strong> <span className="profile-table-data update-profile-disabled-field">{data.sex}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <label htmlFor="pref-pronouns">Pronouns:</label>
                                  <select name="pref-pronouns" value={pronouns} onChange={(e) => (setPronouns(e.target.value))}>
                                      <option defaultValue value="Prefer Not To Say">Prefer Not To Say</option>
                                      <option value="He Him">He Him</option>
                                      <option value="She Her">She Her</option>
                                      <option value="They Them">They Them</option>
                                      <option value="Other">Other</option>
                                  </select>
                              </div>

                          </div>
                          <div className="profile-table-row">

                              <div className="profile-table-cell">
                                  <label htmlFor="marital-status">Marital Status:</label>
                                  <select name="marital-status" value={maritalStatus} onChange={(e) => (setMaritalStatus(e.target.value))} >
                                      <option value="Single" defaultValue>Single</option>
                                      <option value="Married">Married</option>
                                      <option value="Separatd">Separated</option>
                                      <option value="Divorced">Divorced</option>
                                      <option value="Widowed">Widowed</option>
                                      <option value="Prefer Not To Say">Prefer Not To Say</option>
                                      
                                  </select>
                              </div>
                              <div className="profile-table-cell">
                                  <strong>Race:</strong> <span className="profile-table-data update-profile-disabled-field">{data.race}</span>
                              </div>
                              <div className="profile-table-cell">
                                  <strong>Ethnicity:</strong> <span className="profile-table-data update-profile-disabled-field">{data.ethnicity}</span>
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
                                  <label htmlFor="phone-numb">Phone:</label> <input name="phone-numb" value={phone} onChange={(e) => (setPhone(e.target.value))} />
                              </div>
                              <div className="profile-table-cell">
                                  <label htmlFor="contact-method">Preferred Contact Method:</label> 
                                  <select name="contact-method" value={contactMethod}>
                                      <option default value="Call">Call</option>
                                      <option value="Text">Text</option>
                                      <option value="Email">Email</option>
                                  </select>
                              </div>
                          </div>
                          <div className="profile-table-row">
                              <div className="profile-table-cell">
                                  <label htmlFor="address">Address:</label> <input name="address" value={address} onChange={(e) => (setAddress(e.target.value))} />
                              </div>
                              <div className="profile-table-cell">
                                  <label htmlFor="address2">Address 2:</label> <input name="address-2" value={address2} onChange={(e) => (setAddress2(e.target.value))} />
                              </div>

                          </div>
                          <div className="profile-table-row">
                              <div className="profile-table-cell">
                                  <label htmlFor="city">City:</label> <input name="city" value={city} onChange={(e) => (setCity(e.target.value))} />
                              </div>
                              <div className="profile-table-cell">
                                  <label htmlFor="state">State:</label> <input name="state" value={state} onChange={(e) => (setState(e.target.value))} />
                              </div>
                          </div>
                          <div className="profile-table-row">
                              <div className="profile-table-cell">
                                  <label htmlFor="zip">Zip:</label> <input name="zip" value={zip} onChange={(e) => (setZip(e.target.value))} />
                              </div>
                          </div>
                      </div>

                      <div className="profile-table">
                          <div className="profile-btn-container">
                              <button className="no-select" type="submit">Save Changes</button>
                              <p><Link to="/Dashboard/Profile" className="nav-link">Cancel</Link></p>
                          </div>
                      </div>
                  </form>
              )}
      </div>
  );
}

export default DBUpdateProfile;