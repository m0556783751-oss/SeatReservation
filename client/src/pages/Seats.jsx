import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import api from '../api';

export default function Seats() {
  const { eventId } = useParams();
  const [event, setEvent] = useState(null);
  const [seats, setSeats] = useState([]);
  const [message, setMessage] = useState('');
  const [messageType, setMessageType] = useState('');
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
    try {
      const { data: ev } = await api.get(`/events/${eventId}`);
      setEvent(ev);

      const { data: seatList } = await api.get(
        `/venues/${ev.venueId}/seats/available`,
        { params: { eventId } }
      );
      setSeats(seatList);
    } catch {
      setMessage('Failed to load event data.');
      setMessageType('error');
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchData();
  }, [eventId]);

  const handleReserve = async (seatId, seatLabel) => {
    setMessage('');
    try {
      await api.post('/reservations', { eventId: Number(eventId), seatId });
      setMessage(`Seat ${seatLabel} reserved successfully!`);
      setMessageType('success');
      fetchData();
    } catch (err) {
      const status = err.response?.status;
      const serverMessage = err.response?.data?.message || 'An error occurred';

      if (status === 409) {
        setMessage(`Conflict: ${serverMessage}`);
        setMessageType('conflict');
      } else if (status === 401) {
        setMessage('You must be logged in to reserve a seat.');
        setMessageType('error');
      } else {
        setMessage(serverMessage);
        setMessageType('error');
      }
    }
  };

  if (loading) return <p>Loading...</p>;

  return (
    <div>
      <Link to="/events">&larr; Back to Events</Link>

      {event && (
        <div style={{ margin: '15px 0' }}>
          <h2>{event.title}</h2>
          <p>{event.description}</p>
          <p><strong>Date:</strong> {new Date(event.date).toLocaleDateString()}</p>
          <p><strong>Venue:</strong> {event.venueName}</p>
          <p><strong>Available Seats:</strong> {event.availableSeats}</p>
        </div>
      )}

      {message && (
        <div style={{
          padding: '10px 15px',
          marginBottom: '15px',
          borderRadius: '4px',
          background: messageType === 'success' ? '#d4edda'
            : messageType === 'conflict' ? '#fff3cd'
            : '#f8d7da',
          color: messageType === 'success' ? '#155724'
            : messageType === 'conflict' ? '#856404'
            : '#721c24',
          border: `1px solid ${
            messageType === 'success' ? '#c3e6cb'
              : messageType === 'conflict' ? '#ffc107'
              : '#f5c6cb'
          }`
        }}>
          {message}
        </div>
      )}

      {seats.length === 0 ? (
        <p>No available seats for this event.</p>
      ) : (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(100px, 1fr))', gap: '8px' }}>
          {seats.map((seat) => (
            <button
              key={seat.id}
              onClick={() => handleReserve(seat.id, `${seat.row}${seat.number}`)}
              style={{
                padding: '10px',
                cursor: 'pointer',
                background: '#4CAF50',
                color: '#fff',
                border: 'none',
                borderRadius: '4px'
              }}
            >
              {seat.row}{seat.number}
              <br />
              <small>{seat.section}</small>
            </button>
          ))}
        </div>
      )}
    </div>
  );
}
