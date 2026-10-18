import { useState } from 'react';
import { BrowserRouter, Routes, Route, Link, Navigate, useNavigate } from 'react-router-dom';
import Login from './pages/Login';
import Events from './pages/Events';
import Seats from './pages/Seats';
import Reservations from './pages/Reservations';
import './App.css';

function Nav({ user, onLogout }) {
  const navigate = useNavigate();

  const handleLogout = () => {
    onLogout();
    navigate('/login');
  };

  return (
    <nav className="navbar">
      <span className="nav-brand">SeatRes</span>
      <Link to="/events" className="nav-link">Events</Link>
      {user && (
        <>
          <Link to="/reservations" className="nav-link">My Reservations</Link>
          <div className="nav-spacer" />
          <span className="nav-user">
            <strong>{user.fullName}</strong> &middot; {user.role === 1 ? 'Admin' : 'Customer'}
          </span>
          <button onClick={handleLogout} className="nav-btn-logout">Logout</button>
        </>
      )}
      {!user && (
        <>
          <div className="nav-spacer" />
          <Link to="/login" className="nav-link">Login</Link>
        </>
      )}
    </nav>
  );
}

function App() {
  const [user, setUser] = useState(() => {
    const saved = localStorage.getItem('user');
    return saved ? JSON.parse(saved) : null;
  });

  const handleLogin = (userData, token) => {
    localStorage.setItem('token', token);
    localStorage.setItem('user', JSON.stringify(userData));
    setUser(userData);
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setUser(null);
  };

  return (
    <BrowserRouter>
      <Nav user={user} onLogout={handleLogout} />
      <div className="page-container">
        <Routes>
          <Route path="/login" element={<Login onLogin={handleLogin} />} />
          <Route path="/events" element={<Events />} />
          <Route path="/events/:eventId/seats" element={user ? <Seats /> : <Navigate to="/login" />} />
          <Route path="/reservations" element={user ? <Reservations /> : <Navigate to="/login" />} />
          <Route path="*" element={<Navigate to="/events" />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;
