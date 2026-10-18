import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api';

export default function Login({ onLogin }) {
  const [isRegister, setIsRegister] = useState(false);
  const [form, setForm] = useState({ fullName: '', email: '', password: '' });
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    try {
      const endpoint = isRegister ? '/auth/register' : '/auth/login';
      const body = isRegister
        ? { fullName: form.fullName, email: form.email, password: form.password }
        : { email: form.email, password: form.password };

      const { data } = await api.post(endpoint, body);
      onLogin(data.user, data.token);
      navigate('/events');
    } catch (err) {
      const msg = err.response?.data?.message
        || (err.response ? `Server error (${err.response.status})` : 'Cannot reach server');
      setError(msg);
    }
  };

  return (
    <div className="login-wrapper">
      <div className="login-card">
        <div className="login-card-header">
          <h2>{isRegister ? 'Create Account' : 'Welcome Back'}</h2>
          <p>{isRegister ? 'Sign up to reserve your seats' : 'Sign in to your account'}</p>
        </div>
        <div className="login-card-body">
          {error && <div className="alert alert-error">{error}</div>}

          <form onSubmit={handleSubmit} className="login-form">
            {isRegister && (
              <input
                name="fullName"
                placeholder="Full Name"
                value={form.fullName}
                onChange={handleChange}
                autoComplete="name"
                required
              />
            )}
            <input
              name="email"
              type="email"
              placeholder="Email"
              value={form.email}
              onChange={handleChange}
              autoComplete="email"
              required
            />
            <input
              name="password"
              type="password"
              placeholder="Password"
              value={form.password}
              onChange={handleChange}
              autoComplete={isRegister ? 'new-password' : 'current-password'}
              required
              minLength={6}
            />
            <button type="submit" className="btn-primary">
              {isRegister ? 'Create Account' : 'Sign In'}
            </button>
          </form>

          <p className="login-toggle">
            {isRegister ? 'Already have an account?' : "Don't have an account?"}{' '}
            <button
              className="link-btn"
              onClick={() => { setIsRegister(!isRegister); setError(''); }}
            >
              {isRegister ? 'Sign in' : 'Create one'}
            </button>
          </p>
        </div>
      </div>
    </div>
  );
}
