import React, { createContext } from 'react';
import useUserData from '../Hooks/useUserData'

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
export { UserDataContext, UserDataProvider };
