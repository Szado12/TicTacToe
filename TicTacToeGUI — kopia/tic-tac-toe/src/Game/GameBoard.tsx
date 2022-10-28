import React, { useState } from 'react';

export default function GameBoard(props){
    const [userId, setUserId] = useState(1);

    const searchForGame = () =>{
        props.searchForGame(userId);
    }

    const makeMove = (x,y) => {
        props.makeMove(userId,[x,y])
    }
    return (
        <>
        <button onClick={searchForGame}>Search For Game</button>
        <div>
            {props.canMakeMove? "Yout turn": "Enemy turn"}
        </div>
        <div>
            <input readOnly className='gameButton' type={"text"} onClick={() => makeMove(0,0)} value={props.gameBoard[0][0]??''}></input>
            <input readOnly className='gameButton' type={"text"} onClick={() => makeMove(1,0)} value={props.gameBoard[1][0]??''}></input>
            <input readOnly className='gameButton' type={"text"} onClick={() => makeMove(2,0)} value={props.gameBoard[2][0]??''}></input>
            <br/>
            <input readOnly className='gameButton' type={"text"} onClick={() => makeMove(0,1)} value={props.gameBoard[0][1]??''}></input>
            <input readOnly className='gameButton' type={"text"} onClick={() => makeMove(1,1)} value={props.gameBoard[1][1]??''}></input>
            <input readOnly className='gameButton' type={"text"} onClick={() => makeMove(2,1)} value={props.gameBoard[2][1]??''}></input>
            <br/>
            <input readOnly className='gameButton' type={"text"} onClick={() => makeMove(0,2)} value={props.gameBoard[0][2]??''}></input>
            <input readOnly className='gameButton' type={"text"} onClick={() => makeMove(1,2)} value={props.gameBoard[1][2]??''}></input>
            <input readOnly className='gameButton' type={"text"} onClick={() => makeMove(2,2)} value={props.gameBoard[2][2]??''}></input>
            <br/>
        </div>
        </>
    )
};