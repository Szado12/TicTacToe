import React, { useState } from 'react';

export default function GameBoard(props){

    const makeMove = (x:number,y:number) => {
        props.makeMove(x,y)
    }
    return (
        <>
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