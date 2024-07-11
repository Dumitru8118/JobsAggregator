import React, { useState, useEffect } from "react";
import "../node_modules/bootstrap/dist/css/bootstrap.min.css";
import "./App.css";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Link,
  Navigate,
  useNavigate,
} from "react-router-dom";

import Login from "./Components/LoginSignup/Login";
import SignUp from "./Components/LoginSignup/Signup";
import Dashboard from "./Components/Home";
import Navbar from "./Components/Navbar";

import Admin from "./Admin";
import { jwtDecode } from "jwt-decode";

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("jwtToken");
    if (token) {
      setIsLoggedIn(true);
      setUser(jwtDecode(token));
    }
  }, []);

  const handleLoginSuccess = (token) => {
    // console.log("Login successful. Token:", token); // Debug statement
    localStorage.setItem("jwtToken", token);

    setIsLoggedIn(true);
    setUser(jwtDecode(token));
    navigate("/home"); //using navigate as it's RR6 way of redirecting
  };

  const handleLogout = () => {
    localStorage.removeItem("jwtToken");
    setIsLoggedIn(false);

    navigate("/sign-in"); // Use navigate for programmatic navigation
  };
  // console.log(user)
  // console.log("App component re-rendered. isLoggedIn:", isLoggedIn); // Debug statement
  // console.log(localStorage.getItem("jwtToken"));

  return (
    <div className="App">
      <Navbar isLoggedIn={isLoggedIn} handleLogout={handleLogout} user ={user}/>
      <Routes>
        <Route
          index
          element={
            isLoggedIn ? (
              <Navigate to="/home" replace />
            ) : (
              <Navigate to="/sign-in" replace />
            )
          }
        />
        <Route path="/home" element={<Dashboard />} />
        <Route
          path="/sign-in"
          element={
            isLoggedIn ? (
              <Navigate to="/home" />
            ) : (
              <Login handleLoginSuccess={handleLoginSuccess} />
            )
          }
        />
        <Route path="/admin" element={<Admin user={user} />} />
        <Route path="/sign-up" element={<SignUp />} />
      </Routes>
    </div>
  );
}

export default App;
