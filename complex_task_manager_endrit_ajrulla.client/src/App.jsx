import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import LandingPage from './LandingPage';
import MainApp from './MainApp';




function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [userId, setUserId] = useState(null);
    const [isLoading, setIsLoading] = useState(true);


    useEffect(() => {
        const token = localStorage.getItem('token');
        const userId = localStorage.getItem('userId');
        setIsAuthenticated(!!token);
        if (token) {
            setUserId(userId);
        }
        setIsLoading(false);
    }, []);

    if (isLoading) {
        return <div>Loading...</div>;
    }
   

    return (
        <Router>
            <Routes>
                <Route path="/landingPage" element={<LandingPage onLoginSuccess={() => setIsAuthenticated(true)} />} />
                <Route path="/" element={isAuthenticated ? <MainApp userId={userId} /> : <Navigate to="/landingPage" replace />} />
            </Routes>
        </Router>
    );
}

export default App;
