import React, { useState, useEffect, useContext} from 'react';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Navigate } from "react-router-dom";
import './Game.css';
import GameBoard from './GameBoard';
import { MakeMoveModel } from '../Models/MoveModel';
import { UserDataContext } from '../Context/UserDataContext';

export default function GameLogic() {
    const [connection, setConnection ] = useState<null | HubConnection>(null);
    const [canMakeMove, setCanMakeMove] = useState(false);
    const [gameBoard, setGameBoard ] = useState([['','',''],['','',''],['','','']]);
    const [message, setMessage] = useState('');
    const { isLogged, userData} = useContext(UserDataContext);

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


    const searchForGame = async(userId: number) => {
    if(connection)
        try {
            await connection.send('AddToWaitList', userId);
            console.log('AddToWaitList '+userId);

        }
        catch(e) {
            console.log(e);
        }
    }

    const makeMove = async(makeMove: MakeMoveModel) => {
        if(connection){
            if(canMakeMove && (gameBoard[makeMove.xPos][makeMove.yPos] == null || gameBoard[makeMove.xPos][makeMove.yPos] == ''))
            {
                try {
                    await connection.send('MakeAMove', makeMove);
                    setCanMakeMove(false);
                }
                catch(e) {
                    console.log(e);
                }
            }
        }
    }

    return (
        <div>
            asd
        </div>
    )
};

