import React, { useState, useEffect } from 'react';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import './Game.css';
import ChatInput from './GameBoard';
import MessageBox from './MessageBox';

export default function Game() {
    const [connection, setConnection ] = useState<HubConnection>();
    const [canMakeMove, setCanMakeMove] = useState(false);
    const [gameBoard, setGameBoard ] = useState([['','',''],['','',''],['','','']]);
    const [message, setMessage] = useState('');
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
                    
                    connection.on('MakeMove', mes => {
                        setCanMakeMove(true);
                    });

                    connection.on('Lost', mes => {
                        setMessage("You lost")
                    });

                    connection.on('Win', mes => {
                        setMessage("You won")
                    });

                    connection.on('Draft', mes => {
                        setMessage("Draft")
                    });

                    connection.on('UpdateBoard', mes => {
                        console.log(mes);
                        setGameBoard(JSON.parse(mes));
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);


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
            console.log(gameBoard[x][y]);
            if(canMakeMove && (gameBoard[x][y] == null || gameBoard[x][y] == ''))
            {
                try {
                    var m = {
                        userId: +userId,
                        xPos: +x,
                        yPos: +y
                    }
                    await connection.send('MakeAMove', m);
                    setCanMakeMove(false);
                }
                catch(e) {
                    console.log(e);
                }
            }
        }
        else {
            alert('No connection to server yet.');
        }
    }

    return (
        <div>
            asd
        </div>
    )
};

