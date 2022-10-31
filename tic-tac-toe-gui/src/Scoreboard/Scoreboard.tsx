import React,{useContext,useEffect,useState} from 'react';
import { Table } from 'react-bootstrap';
import axios from 'axios';
import { ScoreBoardModel } from '../Models/ScoreBoardModel';
import { LoadingSymbol } from '../Utils/LoadingSymbol';
import './Scoreboard.css'

export default function Scoreboard(){    
    const [ contractors, setContractors ] = useState<ScoreBoardModel[]>([]);
	const [ loading, setLoading ] = useState(true);

    const GetScoreBoard = () => {
		axios.get<ScoreBoardModel[]>('http://localhost:5000/scoreboard').then((response) => {
			setContractors(response.data);
			setLoading(false);
		});
	};

    useEffect(() => {
		GetScoreBoard();
	}, []);

    const GetTable = () =>{
        return contractors.map((x:ScoreBoardModel)=>{
            return (
                <tr>
                    <td>{x.userName}</td>
                    <td>{x.wins}</td>
                    <td>{x.drafts}</td>
                    <td>{x.loses}</td>
                </tr>
            )
        })
    }

     return(
         <div className='scoreboard'>
            <div><h1>Scoreboard</h1></div>
             {loading? <LoadingSymbol/> :<Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Username</th>
                        <th>Wins</th>
                        <th>Drafts</th>
                        <th>Loses</th>
                    </tr>
                </thead>
                <tbody>{GetTable()}</tbody>
                </Table>}
        </div>
    )
};