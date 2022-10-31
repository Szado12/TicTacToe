import React, { useState, useEffect, useContext} from 'react';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Navigate } from "react-router-dom";
import './Game.css';
import GameBoard from './GameBoard';
import { Button} from 'react-bootstrap';
import { MakeMoveModel } from '../Models/MoveModel';
import { UserDataContext } from '../Context/UserDataContext';

export default function GameLogic() {
    const [connection, setConnection ] = useState<null | HubConnection>(null);
    const [canMakeMove, setCanMakeMove] = useState(false);
    const [gameBoard, setGameBoard ] = useState([[null,null,null],[null,null,null],[null,null,null]]);
    const [message, setMessage] = useState('');
    const [inGame, setInGame] = useState(false);
    const [inQueue, setInQueue] = useState(false);
    const {userData} = useContext(UserDataContext);

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5000/hubs/game')
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
                        setInGame(false);
                        setMessage("You lost")
                    });

                    connection.on('Win', mes => {
                        setInGame(false);
                        setMessage("You won")
                    });

                    connection.on('Draft', mes => {
                        setInGame(false);
                        setMessage("Draft")
                    });

                    connection.on('UpdateBoard', mes => {
                        setInGame(true);
                        setInQueue(false);
                        setGameBoard(JSON.parse(mes));
                    });
                    connection.on('AddedToWaitlist', mes => {
                        setInQueue(true);
                        setGameBoard(JSON.parse(mes));
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);


    const searchForGame = async() => {
        console.log(userData);
    if(connection)
        try {
            console.log(userData);
            await connection.send('AddToWaitList', userData.userId);
            console.log('AddToWaitList '+userData.userId);
        }
        catch(e) {
            console.log(e);
        }
    }

    const makeMove = async(x:number,y:number) => {
        if(connection){
            if(canMakeMove && (gameBoard[x][y] == null || gameBoard[x][y] == ''))
            {
                var makeMove:MakeMoveModel = {
                    userId: userData.userId,
                    xPos: x,
                    yPos: y
                }
                console.log(makeMove);
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
        <>
        <div>
            {!inGame? <button className='searchGameBtn' disabled={inQueue} onClick={() =>searchForGame()}>Search for game</button>:<></>}
            {inQueue? <h2>Waiting for opponent...</h2>:<></> }
            <h2>{message}</h2>
            {inGame? <GameBoard makeMove={makeMove} gameBoard={gameBoard} canMakeMove={canMakeMove}/>:<></>}
        </div>
        </>
    )
};

