import React, { useContext } from 'react';
import { UserDataContext } from '../Context/UserDataContext';
import { Navigate } from "react-router-dom";
import GameLogic from './GameLogic';

export default function Game() {
    const { isLogged} = useContext(UserDataContext);

    if(isLogged)
        return  <div>{isLogged}</div>
    else
    return <Navigate replace to="/login" />

}