function Navbar() {
  return (
    <header style={{
      background: 'white',
      padding: '16px 24px',
      boxShadow: '0 1px 4px rgba(0,0,0,0.06)',
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center'
    }}>
      <h1 style={{ fontSize: '18px', fontWeight: 700 }}>Transport Accounting Dashboard</h1>
      <span style={{ fontSize: '14px', color: '#6b7280' }}>Lorry Owner Panel</span>
    </header>
  )
}

export default Navbar
