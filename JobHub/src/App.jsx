import React, { useState, useEffect } from 'react';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import { BrowserRouter as Router, Routes, Route, Link, Navigate, useNavigate } from 'react-router-dom';

import Login from './Components/LoginSignup/Login';
import SignUp from './Components/LoginSignup/Signup';
import Dashboard from './Components/Home';
import Navbar from './Components/Navbar';

function App() {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [jwtToken, setJwtToken] = useState(null);


    useEffect(() => {
        const token = localStorage.getItem('jwtToken');
        if (token) {
            setJwtToken(token);
            setIsLoggedIn(true);
        }
    }, []);

    const handleLoginSuccess = (token) => {
        console.log('Login successful. Token:', token); // Debug statement
        localStorage.setItem('jwtToken', token);
        setJwtToken(token);
        setIsLoggedIn(true);
        window.location.href = '/home';
        // navigate('/home');
    };

    const handleLogout = () => {
        localStorage.removeItem('jwtToken');
        setIsLoggedIn(false);
        setJwtToken(null);
        window.location.href = '/sign-in';
    };

    console.log('App component re-rendered. isLoggedIn:', isLoggedIn); // Debug statement
    console.log(localStorage.getItem("jwtToken"))

    return (
        <Router>
            <div className="App">
                <Navbar isLoggedIn={isLoggedIn} handleLogout={handleLogout} />
                <Routes>
                    {/* <Route
                        exact
                        path="/home"
                        element={isLoggedIn ? <Navigate to="/home" /> : <Navigate to="/sign-in" />}
                    /> */}
                    <Route path="/sign-in" element={<Login handleLoginSuccess={handleLoginSuccess} />} />
                    <Route path="/sign-up" element={<SignUp />} />
                    <Route path="/" element={isLoggedIn ?   <Navigate to="/home" /> : <Navigate to="/sign-in" />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
