import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import api from '../api';

export default function Events() {
  const [events, setEvents] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(false);
  const pageSize = 5;

  const fetchEvents = async () => {
    setLoading(true);
    try {
      const params = { page, pageSize };
      if (search) params.search = search;
      const { data } = await api.get('/events', { params });
      setEvents(data.items || []);
      setTotalPages(data.totalPages || 1);
    } catch {
      setEvents([]);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchEvents();
  }, [page]);

  const handleSearch = (e) => {
    e.preventDefault();
    setPage(1);
    fetchEvents();
  };

  return (
    <div>
      <div className="page-header">
        <h2>Events</h2>
      </div>

      <form onSubmit={handleSearch} className="search-bar">
        <input
          placeholder="Search events..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        <button type="submit" className="btn-primary">Search</button>
      </form>

      {loading ? (
        <p className="loading-text">Loading...</p>
      ) : events.length === 0 ? (
        <p className="empty-state">No events found.</p>
      ) : (
        <div className="event-list">
          {events.map((ev) => (
            <div key={ev.id} className="event-card">
              <div className="event-card-body">
                <div className="event-card-title">{ev.title}</div>
                <div className="event-card-meta">
                  <span>{new Date(ev.date).toLocaleDateString()}</span>
                  <span>{ev.venueName}</span>
                  <span>{ev.availableSeats} seats available</span>
                </div>
              </div>
              <div className="event-card-action">
                <Link to={`/events/${ev.id}/seats`} className="btn-primary">
                  Reserve
                </Link>
              </div>
            </div>
          ))}
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
