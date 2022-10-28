import React, { useState, useContext } from 'react';
import { useHistory } from 'react-router-dom';
import axios from 'axios';

export default function LoginPage() {
	const [ password, setPassword ] = useState<string>('');
	const [ login, setLogin ] = useState<string>('');
	const [ error, setError ] = useState<string>('');

	const { SaveUserData } = useContext(UserDataContext);

	let history = useHistory();
	let forbiddenChars = /[';"]/g;

	const sendUserData = () => {
		if (login === '' || forbiddenChars.test(login)) {
			setError('Username includes special characters');
			return;
		}
		if (password === '') {
			setError('Password is empty');
			return;
		}
		const data = {
			Login: login,
			Password: password
		};
		axios
			.post('https://localhost:44325/api/users/login', data, {})
			.then((response) => {
				SaveUserData(response.data);
				history.push('/list/invoices');
			})
			.catch((error) => {
				console.log(error);
			});
	};

	return (
		<div className={'loginPage'}>
			<div className={'loginPageCard'}>
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
							value={login}
							onChange={(event) => setLogin(event.target.value)}
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
						<Button variant="primary" onClick={() => sendUserData()}>
							Login
						</Button>
					</div>
				</Form>
			</div>
		</div>
	);
}