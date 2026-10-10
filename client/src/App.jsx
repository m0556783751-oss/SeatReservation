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
    <nav style={{ padding: '10px 20px', background: '#333', color: '#fff', display: 'flex', gap: '15px', alignItems: 'center' }}>
      <Link to="/events" style={{ color: '#fff', textDecoration: 'none' }}>Events</Link>
      {user && (
        <>
          <Link to="/reservations" style={{ color: '#fff', textDecoration: 'none' }}>My Reservations</Link>
          <span style={{ marginLeft: 'auto' }}>{user.fullName} ({user.role === 1 ? 'Admin' : 'Customer'})</span>
          <button onClick={handleLogout} style={{ cursor: 'pointer' }}>Logout</button>
        </>
      )}
      {!user && (
        <Link to="/login" style={{ color: '#fff', textDecoration: 'none', marginLeft: 'auto' }}>Login</Link>
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
      <div style={{ padding: '20px', maxWidth: '900px', margin: '0 auto' }}>
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
