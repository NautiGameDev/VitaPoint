import "./DBCreateMessage.css";
import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { GetContacts, PostNewMessage } from "../../Services/MessageService";

function DBCreateMessage() {
    const [contacts, setContacts] = useState([]);
    const [receiver, setReceiver] = useState("");
    const [subject, setSubject] = useState("");
    const [content, setContent] = useState("");
    const [sysMessage, setSysMessage] = useState("Loading...");
    const [errorMessage, setErrorMessage] = useState("");

    const navigate = useNavigate();

    const handleSend = async (e) => {
        e.preventDefault();

        setErrorMessage("");


        if (receiver === "") {
            setErrorMessage("Must choose a receipient");
            return;
        }
        else if (subject === "") {
            setErrorMessage("Must enter a subject line");
            return;
        }
        else if (content === "") {
            setErrorMessage("Must enter a message body");
            return;
        }

        const body = {
            userIds: [receiver],
            subject: subject,
            content: content
        }

        const response = await PostNewMessage(body);

        if (response.status === 200) {
            alert("Message sent successfully");
            navigate("/Dashboard/Messages");
        }
        else {
            setErrorMessage(`Error ${response.status}: ${response.message}`);
        }
    }

    useEffect(() => {
        const fetchContacts = async () => {
            const response = await GetContacts();

            if (response.status === 200) {
                setContacts(response.data);
            }
            else {
                setSysMessage(response.message);
            }
        }

        fetchContacts();

    }, [])

    return (
        <div className="page">
        {
            contacts.length === 0 ?
                (
                    <div className="create-message-syscontainer">
                            <h4>{sysMessage}</h4>
                    </div>
                )
                :
                (
                    <div className="create-message-container">
                        <div className="create-message-row">
                            <h2>
                                <span className="material-icons">
                                    chat
                                </span>
                                Create New Message
                            </h2 >
                        </div >
                        <div className="create-message-row">                            
                                <form onSubmit={(e) => (handleSend(e))} >
                                    <div className="create-message-error-row">
                                        {errorMessage}
                                    </div>
                                    <div className="create-message-form-row">
                                        <label>To:</label>
                                        <select value={receiver} onChange={(e) => (setReceiver(e.target.value))}>
                                            <option defaultValue disabled value="">--</option>
                                            {contacts.map((contact) => (
                                                <option value={contact.sysId}>{`${contact.title} ${contact.firstName} ${contact.lastName}`}</option>
                                            ))}
                                        </select>
                                    </div>
                                    <div className="create-message-form-row">
                                        <label>Subject:</label>
                                        <div className="input-box">
                                            <input type="text" placeholder="Subject line" value={subject} onChange={(e) => (setSubject(e.target.value))} ></input>
                                        </div>
                                    </div>
                                    <div className="create-message-form-row">
                                        <label>Content:</label>
                                        <div className="input-box">
                                            <textarea placeholder="Enter your message..." value={content} onChange={(e) => (setContent(e.target.value))} ></textarea>
                                        </div>
                                    </div>
                                    <div className="create-message-btn-container">
                                        <button type="submit">Send</button>
                                        <Link className="nav-link" to="/Dashboard/Messages">Cancel</Link>
                                    </div>
                                </form>
                        </div>
                    </div >
                )

            }
        </div>      
  );
}

export default DBCreateMessage;