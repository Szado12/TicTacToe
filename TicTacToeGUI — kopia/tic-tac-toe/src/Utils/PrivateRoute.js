import React, { useContext } from 'react';
import { Redirect, Route } from 'react-router-dom';
import { UserDataContext } from '../../contexts/UserDataContext';

export function PrivateRoute({component: Component, ...rest}){
	const { isLogged } = useContext(UserDataContext);
	console.log(isLogged);
	return (
		<Route
			{...rest}
			render={(props) =>
				isLogged ? (
					<Component {...props} />
				) : (
					<Redirect to={{ pathname: '/login', state: { from: props.location } }} />
				)}
		/>
	);
};
export function DefaultRedirect(){
	const { isLogged } = useContext(UserDataContext);
	console.log(isLogged);
	return <Route exact path="/">
		{isLogged ? <Redirect to="/list/invoices" /> : <Redirect to="/login" />}
	</Route>
}