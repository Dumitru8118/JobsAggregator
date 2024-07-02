import React, { useState } from 'react';
import axios from 'axios';
import './Signup.css'

const SignUp = () => {
    const [email, setEmail] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Check if passwords match
        if (password !== confirmPassword) {
            setError('Passwords do not match');
            return;
        }

        try {
            // Send POST request to register endpoint
            const response = await axios.post('https://localhost:7247/Auth/Register', {
                email,
                firstName,
                lastName,
                password,
                confirmedPassword: confirmPassword  // Adjust if backend expects this field
            });

            console.log('Registration successful:', response.data);
            // Optionally handle successful registration, e.g., show success message, redirect to login page

            // Clear form fields
            setEmail('');
            setFirstName('');
            setLastName('');
            setPassword('');
            setConfirmPassword('');
            setError(null);
        } catch (error) {
            console.error('Registration error:', error);
            setError('Registration failed');
        }
    };

    return (

        <div className="auth-wrapper">
            <div className="auth-inner">
                <form onSubmit={handleSubmit}>
                    <h3>Sign Up</h3>

                    <div className="mb-3">
                        <label>Email address</label>
                        <input
                            type="email"
                            className="form-control"
                            placeholder="Enter email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                    </div>

                    <div className="mb-3">
                        <label>First name</label>
                        <input
                            type="text"
                            className="form-control"
                            placeholder="First name"
                            value={firstName}
                            onChange={(e) => setFirstName(e.target.value)}
                            required
                        />
                    </div>

                    <div className="mb-3">
                        <label>Last name</label>
                        <input
                            type="text"
                            className="form-control"
                            placeholder="Last name"
                            value={lastName}
                            onChange={(e) => setLastName(e.target.value)}
                            required
                        />
                    </div>

                    <div className="mb-3">
                        <label>Password</label>
                        <input
                            type="password"
                            className="form-control"
                            placeholder="Enter password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>

                    <div className="mb-3">
                        <label>Confirm Password</label>
                        <input
                            type="password"
                            className="form-control"
                            placeholder="Confirm password"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            required
                        />
                    </div>

                    {error && <p className="text-danger">{error}</p>}

                    <div className="d-grid">
                        <button type="submit" className="btn btn-primary">
                            Sign Up
                        </button>
                    </div>
                    <p className="forgot-password text-right">
                        Already registered? <a href="/sign-in">Sign in</a>
                    </p>
                </form>
            </div>
        </div>
    );
};

export default SignUp;
