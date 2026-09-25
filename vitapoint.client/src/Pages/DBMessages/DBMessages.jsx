import "./DBMessages.css";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { GetRootMessages } from "../../Services/MessageService";
import RootMessage from "../../Components/RootMessage/RootMessage";

function DBMessages() {
    const [messages, setMessages] = useState([]);
    const [sysMessage, setSysMessage] = useState("");

    const navigate = useNavigate();

    useEffect(() => {
        const loadMessages = async () => {
            const response = await GetRootMessages();

            if (response.status === 200) {
                setMessages(response.data);
            }
            else {
                setSysMessage(response.message);
            }
        }

        loadMessages();

    }, []);

  return (
      <div className="page">
          {messages.length === 0 ? (
                  <div className="dbmessages-container">
                  {sysMessage}
                  </div>
              ):(
                  <div className="dbmessages-container">
                      <div className="dbmessages-container-row">
                          <h2>
                              <span className="material-icons">
                                  chat
                              </span>
                              Message Center
                          </h2>
                      </div>
                      <div className="dbmessages-container-row">
                          <button type="button" onClick={() => (navigate("Create"))} >New Message</button>
                      </div>
                      

                      {messages.map((message) => (
                          <div key={message.id} className="dbmessages-container-row">
                              <RootMessage data={message} />
                          </div>
                      ))}
                      
                  </div>
              )}          
      </div>
  );
}

export default DBMessages;