import React, { useState } from 'react';
import Login from './Login';
import Register from './Register';

const LandingPage = ({ onLoginSuccess }) => {
    const [showLogin, setShowLogin] = useState(false);
    const [showRegister, setShowRegister] = useState(false);

    return (
        <div className="landing-page">
            <h1>Welcome to Our App</h1>
            <button type="submit" className="btn btn-primary" onClick={() => setShowLogin(true)}>Login</button>
            <button type="submit" className="btn btn-primary" onClick={() => setShowRegister(true)}>Register</button>

            {showLogin && (
                <Login
                    onLoginSuccess={onLoginSuccess}
                    onClose={() => setShowLogin(false)}
                />
            )}
            {showRegister && (
                <Register onClose={() => setShowRegister(false)} />
            )}
        </div>
    );
};

export default LandingPage;
