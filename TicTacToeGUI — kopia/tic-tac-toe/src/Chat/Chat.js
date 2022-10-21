import React, { useState, useEffect, useRef } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

import ChatWindow from './ChatWindow';
import ChatInput from './ChatInput';

const Chat = () => {
    const [ connection, setConnection ] = useState(null);
    const [ chat, setChat ] = useState([]);
    const latestChat = useRef(null);

    latestChat.current = chat;

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:44386/hubs/chat')
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log(result);
                    console.log('Connected!');
    
                    connection.on('ReceiveMessage', message => {
                        const updatedChat = [...latestChat.current];
                        updatedChat.push(message);
                    
                        setChat(updatedChat);
                    });

                
                    connection.on('MakeMove', mes => {
                        console.log('MakeMove');
                    });

                    connection.on('StartGame', mes => {
                        console.log('StartGame');
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    const sendMessage = async (user, message) => {
        const chatMessage = {
            user: user,
            message: message
        };
        if (connection._connectionStarted) {
            try {
                await connection.send('SendMessage', chatMessage);
            }
            catch(e) {
                console.log(e);
            }
        }
        else {
            alert('No connection to server yet.');
        }
    }

    const searchForGame = async(userId) => {
        if (connection._connectionStarted) {
            try {
                var y: number = +userId;
                await connection.send('AddToWaitList', y);
                console.log('AddToWaitList '+userId);

            }
            catch(e) {
                console.log(e);
            }
        }
        else {
            alert('No connection to server yet.');
        }
    }

    const makeMove = async(userId, [x,y]) => {
        if (connection._connectionStarted) {
            try {
                var m = {
                    userId: userId,
                    xPos: x,
                    yPos: y
                }
                await connection.send('MakeAMove', m);
                console.log('MakeAMove '+userId);

            }
            catch(e) {
                console.log(e);
            }
        }
        else {
            alert('No connection to server yet.');
        }
    }

    return (
        <div>
            <ChatInput sendMessage={sendMessage} searchForGame = {searchForGame} makeMove={makeMove}/>
            <hr />
            <ChatWindow chat={chat}/>
        </div>
    );
};

export default Chat;