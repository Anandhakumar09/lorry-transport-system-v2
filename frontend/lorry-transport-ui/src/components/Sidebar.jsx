import { NavLink } from 'react-router-dom'

const linkStyle = ({ isActive }) => ({
  display: 'block',
  padding: '12px 20px',
  color: isActive ? '#ffffff' : '#c9d6e3',
  background: isActive ? 'rgba(255,255,255,0.15)' : 'transparent',
  textDecoration: 'none',
  fontWeight: 600,
  fontSize: '14px',
  borderRadius: '6px',
  margin: '2px 10px'
})

function Sidebar() {
  return (
    <aside style={{ width: 230, background: '#0f2942', minHeight: '100vh', paddingTop: 20 }}>
      <h2 style={{ color: 'white', textAlign: 'center', fontSize: '16px', marginBottom: 24 }}>
        🚚 Lorry Transport
      </h2>
      <nav>
        <NavLink to="/" end style={linkStyle}>Dashboard</NavLink>
        <NavLink to="/load-entries" style={linkStyle}>Load Entries</NavLink>
        <NavLink to="/expenses" style={linkStyle}>Expenses</NavLink>
        <NavLink to="/driver-ledger" style={linkStyle}>Driver Ledger</NavLink>
      </nav>
    </aside>
  )
}

export default Sidebar
