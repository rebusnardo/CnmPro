import React from 'react';
import './messages.css';
import debug from 'sabio-debug';
import toastr from 'toastr';
import { BsFillCaretRightFill } from 'react-icons/bs';
import { useEffect, useState, useRef } from 'react';
import { createNewMessage, deleteMessageById, getByConversation, getCurrentUser } from '../../services/messageService';
const _logger = debug.extend('Message');

function ChatBox() {
    const [conversation, setConversation] = useState([]);
    const [user, setUser] = useState({});
    const messageEndRef = useRef();
    const messageScrollBox = useRef();

    const [data, setData] = useState({
        MessageText: ' ',
        Subject: 'Message',
        RecipientId: 152, //temporary until signal-r
        DateSent: new Date(),
        DateRead: new Date(),
    });

    useEffect(() => {
        getCurrentUser().then(onGetUserSuccess).catch(onGetUserError);
        if (user.id) {
            setData((prev) => {
                let pd = { ...prev };
                pd.SenderId = user.id;
                return pd;
            });
        }
    }, [user.id]);

    const onGetUserSuccess = (response) => {
        let userData = response.item;
        _logger('User Success:', userData);
        setUser({ ...userData });
    };
    const onGetUserError = (error) => {
        toastr.error('Error has occurred', error);
    };

    const onUpdateUser = (e) => {
        const newData = { ...data };
        newData[e.target.id] = e.target.value;
        setData(newData);
        _logger(newData);
    };

    const newMessage = () => {
        _logger('scroll message box', messageScrollBox);
        createNewMessage(data).then(onNewMessageSuccess).catch(onNewMessageError);
    };

    const onNewMessageSuccess = (response) => {
        let newMessage = response.item.pagedItems;
        _logger('NewMessage Success:', newMessage);
        getByConversation(data.RecipientId, 0, 5).then(onGetConversationsSuccess).catch(onGetConversationError);
    };

    const onNewMessageError = (response) => {
        let newMessage = response.item.pagedItems;
        toastr.error('Error has occurred', newMessage);
    };

    const deleteMessage = (e) => {
        deleteMessageById(e.target.id).then(onDeleteSuccess).catch(onDeleteError);
        _logger(e.target.id);
    };

    const onDeleteError = (response) => {
        toastr.error('Error has occurred', response);
    };

    const onDeleteSuccess = (response) => {
        _logger('Message Deleted', response);
    };

    useEffect(() => {
        getByConversation(data.RecipientId, 0, 5).then(onGetConversationsSuccess).catch(onGetConversationError);
    }, []);

    const onGetConversationsSuccess = (response) => {
        let conversationMessage = response.item.pagedItems;
        _logger('Convo Success:', conversationMessage.reverse());
        setConversation(conversationMessage);
    };

    const onGetConversationError = (error) => {
        toastr.error('Error has occurred', error);
    };

    useEffect(() => {
        const keyDownHandler = (event) => {
            _logger('User pressed: ', event.key);

            if (event.key === 'Enter') {
                newMessage();
            }
        };

        document.addEventListener('keydown', keyDownHandler);

        return () => {
            document.removeEventListener('keydown', keyDownHandler);
        };
    }, [data.MessageText]);

    useEffect(() => {
        messageEndRef.current?.scrollIntoView(false);
    }, [conversation]);

    const mappedConversation = (convoData) => {
        let senderUserInfo = convoData.senderData[0];
        let receiverUserInfo = convoData.recipientData[0];

        return (
            <div key={convoData.id}>
                <div className="d-flex">
                    <div className="align-self-start">
                        <img className="messages-avatar-chatBox" src={senderUserInfo.avatarUrl} alt="404" />

                        <p>{convoData.dateSent.slice(11, 16)}</p>
                    </div>
                    <div className=" chat__item__content chatBox-chatlist__item chatBox-senderStyling-coloring">
                        <div className="d-flex align-items-end ">
                            <div>
                                <div>
                                    <p className="d-flex justify-content-start chatBox-text-chats">
                                        <strong>
                                            {senderUserInfo.firstName} {senderUserInfo.lastName.slice(0, 1)}.
                                        </strong>
                                    </p>
                                </div>
                                <div className="d-flex justify-content-start chatBox-text-chats  ">
                                    <p>{convoData.messageText}</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="d-flex flex-row-reverse ">
                    <div className="align-self-start">
                        <img className="messages-avatar-chatBox" src={receiverUserInfo.avatarUrl} alt="404" />

                        <p>{convoData.dateSent.slice(11, 16)}</p>
                    </div>
                    <div className=" chat__item__content chatBox-chatlist__item chatBox-receiverStyling-coloring">
                        <div className="d-flex align-items-end ">
                            <div>
                                <div>
                                    <p className="d-flex justify-content-end chatBox-text-chats">
                                        <strong>
                                            {receiverUserInfo.firstName} {receiverUserInfo.lastName.slice(0, 1)}.
                                        </strong>
                                    </p>
                                </div>
                                <div className="d-flex justify-content-end chatBox-text-chats ">
                                    <p>{convoData.messageText}</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <button id={convoData.id} onClick={deleteMessage} className="d-none">
                    Delete
                </button>
            </div>
        );
    };

    return (
        <div>
            <div className="row">
                <div className="messages-chatBox-scoller">
                    {conversation.map(mappedConversation)} <div ref={messageEndRef} />
                </div>
                <div className="chatBox-inputBarOutline-styling">
                    <div className="row mt-3">
                        <input
                            className="chatbox-inputBar-styling"
                            placeholder="Type Your Message Here"
                            type="text"
                            name="MessageText"
                            id="MessageText"
                            onChange={onUpdateUser}></input>
                        <button
                            className="chatBox-submitButton-styling chatBox-submitButton-boxShadow"
                            type="submit"
                            onClick={newMessage}>
                            <BsFillCaretRightFill className="chatBox-submitButton-bsFill" />
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ChatBox;
