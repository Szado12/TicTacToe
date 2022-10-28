import React, { createContext } from 'react';
import asd from '../Hooks/test';

const UserDataContext = createContext();

function UserDataProvider({ children }) {
	const { isLogged, setIsLogged, LogOut, userData, setUserData, LoadUserData, SaveUserData } = useUserData();
	return (
		<UserDataContext.Provider
			value={{
				isLogged,
				setIsLogged,
				LogOut,
				userData,
				setUserData,
				LoadUserData,
				SaveUserData
			}}
		>
			{children}
		</UserDataContext.Provider>
	);
}
export default { UserDataContext, UserDataProvider };
