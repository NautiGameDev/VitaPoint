import Nav from "../../Components/Nav/Nav"
import "./Dashboard.css";
import { useState, useEffect } from "react";
import { Outlet } from "react-router-dom";

function Dashboard() {

    const [navVisible, setNavVisible] = useState(false);

    const closeNavCallback = () => {
        if (window.innerWidth < 1024) {
            setNavVisible(false);
        }
    }

    useEffect(() => {
        const handleResize = () => {
            setNavVisible(window.innerWidth > 1024);
        }

        handleResize();
        window.addEventListener("resize", handleResize);
    }, [])

  return (
      <div className="page">
          <div className="dashboard-container">
              <div className="dashboard-nav">
                  <h1>
                      <span class="material-icons">
                          health_and_safety
                      </span>
                      VitaPoint
                  </h1>
                  <span className="material-icons burger-icon" onClick={() => (setNavVisible(!navVisible))} >
                    menu
                  </span>
                  <div className={`dashboard-nav-menu-holder${navVisible ? "" : "-hidden"}`}>
                      <Nav closeMenuCallback={closeNavCallback} />
                  </div>
                  
              </div>
              
              
              <div className="dashboard-shell">
                  <Outlet />
              </div>
          </div>
      </div>
  );
}

export default Dashboard;