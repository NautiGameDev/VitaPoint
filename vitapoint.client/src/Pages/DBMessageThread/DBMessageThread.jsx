import "./DBMessageThread.css";
import { useParams } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { GetMessageThread, PostNewMessage } from "../../Services/MessageService";
import { FormatDate } from "../../Clients/TextFormatterClient";
import { useNavigate } from 'react-router-dom';

function DBMessageThread() {
    const { messageId } = useParams();
    const [thread, setThread] = useState([]);
    const [sysMessage, setSysMessage] = useState("Loading...");
    const [reply, setReply] = useState("");
    const [replyError, setReplyError] = useState("")

    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setReplyError("");


        if (reply.trim() === "") {
            setReplyError("Message required to reply to thread");
        }

        const body = {
            userIds: [thread[0].senderId, thread[0].receiverId],
            subject: thread[0].subject,
            content: reply.trim(),
            isReply: true,
            rootId: messageId
        }

        const response = await PostNewMessage(body);

        if (response.status === 200) {
            alert("Message sent successfully");
            navigate("/Dashboard/Messages");
        }
        else {
            setReplyError(`Error ${response.status}: ${response.message}`);
        }

    }

    useEffect(() => {
        const getThread = async () => {
            const response = await GetMessageThread(messageId);

            if (response.status === 200) {
                setThread(response.data);
            }
            else {
                setSysMessage(`Error status ${response.status}: ${response.message}`);
            }
        };

        getThread();

    }, [messageId]);

    return (
        <div className="page">
            {
                thread.length === 0 ? (
                    <div className="message-thread-sys-container">
                        <h2>{sysMessage}</h2>
                    </div>
                ) :
                    (
                        <div className="message-thread-container">
                            <div className="message-thread-title">
                                <h2>
                                    <span className="material-icons">
                                        chat
                                    </span>
                                    {thread[0].subject}
                                </h2>
                            </div>
                            {thread.map((message, id) => (
                                <div key={id} className={`message-thread-message ${id % 2 === 0 ? "message-light" : "message-dark"}`}>
                                  <div className="message-thread-row">
                                        <h3>{message.senderName}</h3>
                                  </div>
                                  <div className="message-thread-row">
                                        <strong>{FormatDate(message.timeSent)}</strong>
                                  </div>
                                  <div className="message-thread-row">
                                        <p>{message.content}</p>
                                  </div>
                              </div>
                            ))}
                            <div className="message-thread-reply-container">
                                <div className="message-thread-error-container">
                                    {replyError}
                                </div>
                                <form onSubmit={(e) => (handleSubmit(e))} >
                                    <label>Reply:</label>
                                    <textarea value={reply} onChange={(e) => (setReply(e.target.value))} />
                                    <button type="submit">Send</button>
                                </form>
                            </div>
                      </div>
                    )
          }

          
      </div>
  );
}

export default DBMessageThread;