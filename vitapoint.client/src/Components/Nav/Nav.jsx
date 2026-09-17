import "./Nav.css";
import { useNavigate } from "react-router-dom";
import { logout } from "../../Services/AccountService"

function Nav({ closeMenuCallback }) {
    const navigate = useNavigate();

    const handleNavigate = (page) => {
        navigate(page);
        closeMenuCallback();
    }

    const handleLogout = async () => {
        const response = await logout();

        if (response.status === 200) {
            navigate("/");
        }
        else {
            alert(`Error logging out: Status ${response.status} ${response.message}`);
        }
    }

    return (
      <div className="nav-component">          
            <ul className="nav-links" >
                <li onClick={() => (handleNavigate("Profile"))}>
                    <span className="material-icons">
                        account_circle
                    </span>
                    Profile
                </li>
                <li onClick={() => (handleNavigate("Messages"))}>
                    <span className="material-icons">
                        chat
                    </span>
                    Messages
                </li>
                <li onClick={() => (handleNavigate("Appointments"))}>
                    <span className="material-icons">
                        calendar_month
                    </span>
                    Appointments
                </li>
                <li onClick={() => (handleNavigate("Results"))}>
                    <span className="material-icons" >
                        science
                    </span>
                    Lab Results
                </li>
                <li onClick={() => (handleNavigate("Prescriptions"))}>
                    <span className="material-icons">
                        medication
                    </span>
                    Prescriptions
                </li>
                <li onClick={() => (handleLogout())}>
                    <span className="material-icons">
                        logout
                    </span>
                    Logout
                </li>
            </ul>
        </div>
  );
}

export default Nav;