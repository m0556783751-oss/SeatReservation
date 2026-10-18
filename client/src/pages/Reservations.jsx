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
      <div className="page-header">
        <h2>My Reservations</h2>
      </div>

      {message && (
        <div className={`alert ${
          messageType === 'success' ? 'alert-success'
            : messageType === 'conflict' ? 'alert-warning'
            : 'alert-error'
        }`}>
          {message}
        </div>
      )}

      {loading ? (
        <p className="loading-text">Loading...</p>
      ) : reservations.length === 0 ? (
        <p className="empty-state">No reservations yet.</p>
      ) : (
        <div className="table-wrapper">
          <table className="res-table">
            <thead>
              <tr>
                <th>Event</th>
                <th>Seat</th>
                <th>Date</th>
                <th>Status</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {reservations.map((res) => (
                <tr key={res.id}>
                  <td>{res.eventTitle}</td>
                  <td>{res.seatLabel}</td>
                  <td>{new Date(res.reservedAt).toLocaleDateString()}</td>
                  <td>
                    <span className={`badge ${res.status === 0 ? 'badge-active' : 'badge-cancelled'}`}>
                      {res.status === 0 ? 'Active' : 'Cancelled'}
                    </span>
                  </td>
                  <td>
                    {res.status === 0 && (
                      <button className="btn-danger" onClick={() => handleCancel(res.id)}>
                        Cancel
                      </button>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      <div className="pagination">
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
