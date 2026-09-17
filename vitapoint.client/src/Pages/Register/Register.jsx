import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import "./Register.css";
import { registerAccount } from '../../Services/AccountService'

function Register() {
    const [email, setEmail] = useState("");
    const [activationCode, setActivationCode] = useState("");
    const [dob, setDob] = useState("");
    const [zip, setZip] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    const [registrationError, setRegistrationError] = useState("");

    const navigate = useNavigate();

    const handleRegistration = async (e) => {
        e.preventDefault();
        setRegistrationError("");

        const hasCapital = /[A-Z]/;
        const hasLowercase = /[a-z]/;
        const hasNumber = /[0-9]/;
        const hasSymbol = /[^a-zA-Z0-9\s]/;
        const isEmail = /^\S+@\S+\.[a-zA-Z]{2,6}$/i;

        //Email testing
        if (email.trim() === "") {
            setRegistrationError("Email address required");
            return;
        }
        else if (!isEmail.test(email)) {
            setRegistrationError("Valid email required");
            return;
        }

        //Password testing
        if (password.trim() === "") {
            setRegistrationError("Password required");
            return;
        }
        else if (confirmPassword.trim() === "") {
            setRegistrationError("Must confirm password");
            return;
        }
        else if (password !== confirmPassword) {
            setRegistrationError("Passwords must match");
            return;
        }
        else if (password.trim().length < 8) {
            setRegistrationError("Password must be at least 8 characters long.");
            return;
        }
        else if (!hasCapital.test(password)) {
            setRegistrationError("Password must have at least one uppercase letter.");
            return;
        }
        else if (!hasLowercase.test(password)) {
            setRegistrationError("Password must have at least one lowercase letter.");
            return;
        }
        else if (!hasNumber.test(password)) {
            setRegistrationError("Password must have at least one number.");
            return;
        }
        else if (!hasSymbol.test(password)) {
            setRegistrationError("Password must have at least one special character (!@#$%^&*).");
            return;
        }

        //DOB testing
        if (dob.trim() === "") {
            setRegistrationError("Must enter date of birth.");
            return;
        }
        else if (new Date(dob) > new Date()) {
            setRegistrationError("Enter a valid date of birth.");
            return;
        }

        //Zip code testing
        if (zip.trim() === "") {
            setRegistrationError("Must enter zip code.");
            return;
        }
        else if (zip.trim().length < 5) {
            setRegistrationError("Must enter a valid zip code.");
            return;
        }

        if (activationCode.trim() === "") {
            setRegistrationError("Must enter an activation code.");
            return;
        }


        const response = await registerAccount(email, password, activationCode, dob, zip);

        if (response.status === 200) {
            alert("Account created successfully!");
            navigate("/");
        }
        else {
            setRegistrationError(response.message);
        }

    }

  return (
      <div className="page">
          <div className="registration-logo">
              <h1>
                  <span class="material-icons">
                      health_and_safety
                  </span>
                  VitaPoint
              </h1>
          </div>

          <div className="registration-container">
              <div className="registration-header">
                  <h2>New Account</h2>
              </div>
              <form onSubmit={(e) => (handleRegistration(e))} >
                  <div className="registration-error-container">
                      {registrationError}
                  </div>

                  <div className="registration-row">
                      <div className="registration-input-container">
                          <label htmlFor="email">Email:</label>
                          <input type="email" id="email" placeholder="Enter your email address" value={email} onChange={(e) => (setEmail(e.target.value))}></input>
                      </div>
                      <div className="registration-input-container">
                          <label htmlFor="activation-code">Activation Code:</label>
                          <input type="text" id="activation-code" placeholder="Enter one time activation code" value={activationCode} onChange={(e) => (setActivationCode(e.target.value))}></input>
                      </div>
                  </div>

                  <div className="registration-row">
                      <div className="registration-input-container">
                          <label htmlFor="date-of-birth">Date of Birth:</label>
                          <input type="date" id="date-of-birth" value={dob} onChange={(e) => (setDob(e.target.value))} ></input>
                      </div>
                      <div className="registration-input-container">
                          <label htmlFor="zip-code">Zip Code:</label>
                          <input type="text" id="zip-code" value={zip} onChange={(e) => (setZip(e.target.value))}></input>
                      </div>
                  </div>

                  <div className="registration-row">
                      <div className="registration-input-container">
                          <label htmlFor="password">Password:</label>
                          <input type="password" id="password" value={password} onChange={(e) => (setPassword(e.target.value))}></input>
                      </div>
                      <div className="registration-input-container">
                          <label htmlFor="confirm-password">Confirm Password:</label>
                          <input type="password" id="confirm-password" value={confirmPassword} onChange={(e) => (setConfirmPassword(e.target.value))}></input>
                      </div>
                  </div>

                  <div className="registration-btn-container">
                      <button className="no-select" type="submit" formAction="submit">Register</button>
                      <Link className="nav-link" to="/">Cancel Registration</Link>
                  </div>
              </form>
          </div>
      </div>
  );
}

export default Register;