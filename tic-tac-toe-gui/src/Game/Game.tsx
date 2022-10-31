import React, { useContext } from 'react';
import { UserDataContext } from '../Context/UserDataContext';
import { Navigate } from "react-router-dom";
import { Container } from 'react-bootstrap';
import GameLogic from './GameLogic';
import Scoreboard from '../Scoreboard/Scoreboard';

export default function Game() {
    const { isLogged} = useContext(UserDataContext);

    if(isLogged)
        return  (
            <Container className='gameContainer'>
                <GameLogic/>
                <Scoreboard/>
            </Container>
        );
    else
    return <Navigate replace to="/login" />

}