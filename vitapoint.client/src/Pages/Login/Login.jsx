import './Login.css';
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { postLogin } from "../../Services/AccountService"

function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const [loginError, setLoginError] = useState("");

    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        setLoginError("");

        if (email.trim() === "") {
            setLoginError("Email required to login");
            return;
        }
        else if (password.trim() === "") {
            setLoginError("Password required to login");
            return;
        }        

        const response = await postLogin(email, password);

        if (response.status === 200) {
            alert("Login successful!");
        }
        else {
            setLoginError(`Error logging in with status code ${response.status}. ${response.message}`);
        }
    }

  return (
      <div className="page">
          <div className="login-logo">
              <h1>
                  <span class="material-icons">
                      health_and_safety
                  </span>
                  VitaPoint
              </h1>
          </div>
          
          <div className="login-container">
              <div className="login-header">
                  <h2>Sign in</h2>
              </div>
              <form onSubmit={(e) => (handleLogin(e))} >
                  <div className="login-error-container">
                      {loginError}
                  </div>
                  <div className="login-input-container">
                      <label htmlFor="email">Email:</label>
                      <input type="email" id="email" placeholder="Enter your email address" value={email} onChange={(e) => (setEmail(e.target.value)) }></input>
                  </div>
                  <div className="login-input-container">
                      <label htmlFor="password">Password:</label>
                      <input type="password" id="password" value={password} onChange={(e) => (setPassword(e.target.value)) }></input>
                  </div>
                  <div className="login-btn-container">
                    <button className="no-select" type="submit" formAction="submit">Login</button>
                  </div>
                  <div className="login-registration-container">
                      <p>Don't have an account?</p>
                      <p><Link className="nav-link" to="/Register">Create one</Link></p>
                  </div>
              </form>
          </div>
      </div>
  );
}

export default Login;