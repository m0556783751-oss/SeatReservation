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

  if (loading) return <p className="loading-text">Loading...</p>;

  return (
    <div>
      <Link to="/events" className="back-link">&larr; Back to Events</Link>

      {event && (
        <div className="event-detail">
          <h2>{event.title}</h2>
          <p>{event.description}</p>
          <p><strong>Date:</strong> {new Date(event.date).toLocaleDateString()}</p>
          <p><strong>Venue:</strong> {event.venueName}</p>
          <p><strong>Available Seats:</strong> {event.availableSeats}</p>
        </div>
      )}

      {message && (
        <div className={`alert ${
          messageType === 'success' ? 'alert-success'
            : messageType === 'conflict' ? 'alert-warning'
            : 'alert-error'
        }`}>
          {message}
        </div>
      )}

      {seats.length === 0 ? (
        <p className="empty-state">No available seats for this event.</p>
      ) : (
        <div className="seat-grid">
          {seats.map((seat) => (
            <button
              key={seat.id}
              className="seat-btn"
              onClick={() => handleReserve(seat.id, `${seat.row}${seat.number}`)}
            >
              {seat.row}{seat.number}
              <small>{seat.section}</small>
            </button>
          ))}
        </div>
      )}
    </div>
  );
}
