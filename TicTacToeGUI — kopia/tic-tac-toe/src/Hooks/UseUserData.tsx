import { UserData } from '../Models/UserData';
import { useState } from 'react';

export default function UseUserData() {
	const LoadUserData = () => {
		if (localStorage.getItem('userData') == null) {
			return null;
		}
		let savedInvoiceModel = JSON.parse(localStorage.getItem('userData')!) as UserData;
		if (savedInvoiceModel == null) {
			return null;
		}
		setIsLogged(true);
		return savedInvoiceModel;
	};

	const SaveUserData = (userData: UserData) => {
		localStorage.setItem('userData', JSON.stringify(userData));
		setIsLogged(true);
		setUserData(userData);
	};

	const LogOut = () => {
		setIsLogged(false);
		setUserData(null);
		localStorage.removeItem('userData');
	};

	const [ isLogged, setIsLogged ] = useState<boolean>(false);
	const [ userData, setUserData ] = useState<UserData | null>(() => {
		try {
			return LoadUserData();
		} catch (error) {
			console.log(error);
			return null;
		}
	});

	return {
		isLogged,
		setIsLogged,
		LogOut,
		userData,
		setUserData,
		LoadUserData,
		SaveUserData
	};
}
