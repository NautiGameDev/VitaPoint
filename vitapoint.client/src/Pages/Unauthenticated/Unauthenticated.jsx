import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import "./Unauthenticated.css";

function Unauthenticated() {
    const navigate = useNavigate();

    const navToLogin = () => {
        setTimeout(() => {
            navigate("/");
        }, 3000);
    }

    useEffect(() => {
        navToLogin();
    }, []);

    return (
        <div className="page">
            <div className="unauthenticated-container">
                <div className="login-logo">
                    <h1>
                        <span class="material-icons">
                            health_and_safety
                        </span>
                        VitaPoint
                    </h1>
                </div>
                <h2>Must log in to access this page. Navigating to login page...</h2>
            </div>
        </div>
    );
}

export default Unauthenticated;