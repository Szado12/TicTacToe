import React, { useState, useContext } from 'react';
import { Container, Form, Button, Col, Row } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { UserDataContext } from '../Context/UserDataContext';
import './Login.css';

export default function LoginPage() {
	const [ password, setPassword ] = useState<string>('');
	const [ passwordReg, setPasswordReg ] = useState<string>('');
	const [ passwordRegConf, setPasswordRegConf ] = useState<string>('');
	const [ usernameReg, setUsernameReg ] = useState<string>('');
	const [ username, setUsername ] = useState<string>('');
	const [ error, setError ] = useState<string>('');
	const [ errorReg, setErrorReg ] = useState<string>('');

	const { SaveUserData } = useContext(UserDataContext);

	let navigate = useNavigate();
	let forbiddenChars = /[';"]/g;

	const validateRegData = () => {
		setErrorReg('');
		if (usernameReg === '' || forbiddenChars.test(usernameReg)) {
			setErrorReg("Username includes special characters or it's empty");
			return false;
		}
		if (passwordReg === '') {
			setErrorReg('Password is empty');
			return false;
		}
		if (passwordReg !== passwordRegConf) {
			setErrorReg('Password is empty');
			return false;
		}
		return true;
	}

	const validateLogData = ():boolean => {
		setError('');
		if (username === '' || forbiddenChars.test(username)) {
			setError('Username includes special characters');
			return false;
		}
		if (password === '') {
			setError('Password is empty');
			return false;
		}
		return true;
	}
	const Login = () => {
		if(!validateLogData())
			return;
		const data = {
			Username: username,
			Password: password
		};
		axios
			.post('/api/user/Login', data, {})
			.then((response) => {
				SaveUserData(response.data);
				navigate('/game');
			})
			.catch((error) => {
				console.log(error);
			});
	};

	const Register = () => {
		if(!validateRegData())
			return;
		const data = {
			Username: usernameReg,
			Password: passwordReg
		};
		axios
			.post('/api/user/Register', data, {})
			.then((response) => {
				SaveUserData(response.data);
				navigate('/game');
			})
			.catch((error) => {
				console.log(error);
			});
	};

	return (
		<div className={'LoginPage'}>
			<Row>
			<Col>
				<Form>
					{error != '' && (
						<div style={{ color: 'red' }}>
							<b>{error}</b>
						</div>
					)}
					<Form.Group className="mb-3" controlId="formBasicLogin">
						<Form.Label>Username</Form.Label>
						<Form.Control
							type="login"
							value={username}
							onChange={(event) => setUsername(event.target.value)}
							placeholder="Enter username"
						/>
					</Form.Group>

					<Form.Group className="mb-3" controlId="formBasicPassword">
						<Form.Label>Password</Form.Label>
						<Form.Control
							type="password"
							value={password}
							onChange={(event) => setPassword(event.target.value)}
							placeholder="Enter password"
						/>
					</Form.Group>
					<div className={'loginButton'}>
						<Button variant="primary" onClick={() => Login()}>
							Login
						</Button>
					</div>
				</Form>
			</Col>
			<Col>
				<Form>
					{error != '' && (
						<div style={{ color: 'red' }}>
							<b>{errorReg}</b>
						</div>
					)}
					<Form.Group className="mb-3" controlId="formBasicLogin">
						<Form.Label>Username</Form.Label>
						<Form.Control
							type="login"
							value={usernameReg}
							onChange={(event) => setUsernameReg(event.target.value)}
							placeholder="Enter username"
						/>
					</Form.Group>

					<Form.Group className="mb-3" controlId="formBasicPassword">
						<Form.Label>Password</Form.Label>
						<Form.Control
							type="password"
							value={passwordReg}
							onChange={(event) => setPasswordReg(event.target.value)}
							placeholder="Enter password"
						/>
					</Form.Group>
					<Form.Group className="mb-3" controlId="formBasicPassword">
						<Form.Label>Repeat password</Form.Label>
						<Form.Control
							type="password"
							value={passwordRegConf}
							onChange={(event) => setPasswordRegConf(event.target.value)}
							placeholder="Repeat password"
						/>
					</Form.Group>
					<div className={'loginButton'}>
						<Button variant="primary" onClick={() => Register()}>
							Create Account
						</Button>
					</div>
				</Form>
			</Col>
		</Row>
	</div>
	);
}