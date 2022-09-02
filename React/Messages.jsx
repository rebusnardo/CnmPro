import ChatBox from './Chatbox';
import React from 'react';
import { Card } from 'react-bootstrap';
import './messages.css';

const Messages = () => {
    return (
        <div className="container">
            <div className="row">
                <Card className="col-2 background-color-messages  "></Card>
                <div className="col-7   background-color-messages  messages-chatBox-main">
                    <ChatBox />;
                </div>
                <Card className="col-2 background-color-messages  "></Card>
            </div>
        </div>
    );
};

export default Messages;
