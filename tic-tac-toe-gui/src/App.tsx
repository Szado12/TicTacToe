import React from 'react';
import './App.css';
import { UserDataProvider } from "./Context/UserDataContext"
import Navbar from './Navbar/Navbar';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  return (
		<UserDataProvider>
      <Navbar/>
		</UserDataProvider>
  );
}

export default App;
