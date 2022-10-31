import React,{useContext} from 'react';
import { BrowserRouter as Router, Navigate, Route, Routes} from 'react-router-dom';
import LoginPage from "../Login/Login"
import Game from '../Game/Game';
import { UserDataContext } from '../Context/UserDataContext';
import { Container } from 'react-bootstrap';
import './Navbar.css';

export default function Navbar(){    
    return(
        <div className='background'>
        <Container>
        <Router>
            <Routes>
                <Route path="/" element={<LoginPage/>}/>
                <Route path="/game" element={<Game/>}/>
            </Routes>
        </Router>
        </Container>
        </div>
    )
};