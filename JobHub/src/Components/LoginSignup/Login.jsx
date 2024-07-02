import React, { Component } from 'react';
import './Login.css'
export default class Login extends Component {
    
    constructor(props) {
        super(props);
        this.state = {
            email: '',
            password: '',
            error: null,
        };
    }
    
    handleChange = (e) => {
        const { name, value } = e.target;
        this.setState({ [name]: value });
    };

    handleSubmit = async (e) => {
        e.preventDefault();
        const { email, password } = this.state;

        try {
            const response = await fetch('https://localhost:7247/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email, password }),
            });

            if (!response.ok) {
                throw new Error('Login failed');
            }

            const data = await response.json();
            localStorage.setItem('jwtToken', data.token); // Save JWT token to localStorage
            this.props.handleLoginSuccess(data.token); // Notify parent component
        } catch (error) {
            this.setState({ error: 'Login error: ' + error.message });
        }
    };

    render() {
        const { email, password, error } = this.state;
        return (
        <div className="auth-wrapper">
            <div className="auth-inner">
                <form onSubmit={this.handleSubmit}>
                    <h3>Sign In</h3>

                    {error && <div className="alert alert-danger">{error}</div>}

                    <div className="mb-3">
                        <label>Email address</label>
                        <input
                            type="email"
                            name="email"
                            className="form-control"
                            placeholder="Enter email"
                            value={email}
                            onChange={this.handleChange}
                        />
                    </div>

                    <div className="mb-3">
                        <label>Password</label>
                        <input
                            type="password"
                            name="password"
                            className="form-control"
                            placeholder="Enter password"
                            value={password}
                            onChange={this.handleChange}
                        />
                    </div>

                    <div className="d-grid">
                        <button type="submit" className="btn btn-primary">
                            Submit
                        </button>
                    </div>
                    <p className="forgot-password text-right">
                        Not a member yet? <a href="/sign-up">Signup here</a>
                    </p>
                </form>
            </div>
        </div>
        );
    }
}
