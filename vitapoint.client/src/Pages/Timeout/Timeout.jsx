import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import "./Timeout.css";
import { useAuth } from '../../Contexts/AuthContext';

function Timeout() {
    const navigate = useNavigate();
    const { logout } = useAuth();

    const navToLogin = () => {
        setTimeout(() => {
            logout();
            navigate("/");
        }, 3000);
    }

    useEffect(() => {
        navToLogin();
    }, []);

  return (
      <div className="page">
          <div className="timeout-container">
              <div className="login-logo">
                  <h1>
                      <span class="material-icons">
                          health_and_safety
                      </span>
                      VitaPoint
                  </h1>
              </div>
              <h2>Your session has expired. Returning to login page...</h2>
          </div>
      </div>
  );
}

export default Timeout;