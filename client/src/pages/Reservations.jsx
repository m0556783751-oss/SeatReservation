import { useState, useEffect } from 'react';
import api from '../api';

export default function Reservations() {
  const [reservations, setReservations] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [message, setMessage] = useState('');
  const [messageType, setMessageType] = useState('');
  const [loading, setLoading] = useState(true);
  const pageSize = 10;

  const fetchReservations = async () => {
    setLoading(true);
    try {
      const { data } = await api.get('/reservations', { params: { page, pageSize } });
      setReservations(data.items);
      setTotalPages(data.totalPages);
    } catch {
      setReservations([]);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchReservations();
  }, [page]);

  const handleCancel = async (id) => {
    setMessage('');
    try {
      await api.delete(`/reservations/${id}`);
      setMessage('Reservation cancelled successfully.');
      setMessageType('success');
      fetchReservations();
    } catch (err) {
      const status = err.response?.status;
      const serverMessage = err.response?.data?.message || 'An error occurred';

      if (status === 409) {
        setMessage(`Conflict: ${serverMessage}`);
        setMessageType('conflict');
      } else {
        setMessage(serverMessage);
        setMessageType('error');
      }
    }
  };

  return (
    <div>
      <h2>My Reservations</h2>

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

      {loading ? (
        <p>Loading...</p>
      ) : reservations.length === 0 ? (
        <p>No reservations yet.</p>
      ) : (
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ borderBottom: '2px solid #333', textAlign: 'left' }}>
              <th style={{ padding: '8px' }}>Event</th>
              <th style={{ padding: '8px' }}>Seat</th>
              <th style={{ padding: '8px' }}>Date</th>
              <th style={{ padding: '8px' }}>Status</th>
              <th style={{ padding: '8px' }}></th>
            </tr>
          </thead>
          <tbody>
            {reservations.map((res) => (
              <tr key={res.id} style={{ borderBottom: '1px solid #ddd' }}>
                <td style={{ padding: '8px' }}>{res.eventTitle}</td>
                <td style={{ padding: '8px' }}>{res.seatLabel}</td>
                <td style={{ padding: '8px' }}>{new Date(res.reservedAt).toLocaleDateString()}</td>
                <td style={{ padding: '8px' }}>
                  <span style={{
                    color: res.status === 0 ? 'green' : 'red',
                    fontWeight: 'bold'
                  }}>
                    {res.status === 0 ? 'Active' : 'Cancelled'}
                  </span>
                </td>
                <td style={{ padding: '8px' }}>
                  {res.status === 0 && (
                    <button
                      onClick={() => handleCancel(res.id)}
                      style={{ color: 'red', cursor: 'pointer' }}
                    >
                      Cancel
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      <div style={{ marginTop: '15px', display: 'flex', gap: '10px', alignItems: 'center' }}>
        <button onClick={() => setPage(page - 1)} disabled={page <= 1}>
          Previous
        </button>
        <span>Page {page} of {totalPages}</span>
        <button onClick={() => setPage(page + 1)} disabled={page >= totalPages}>
          Next
        </button>
      </div>
    </div>
  );
}
