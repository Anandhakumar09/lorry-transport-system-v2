import { Routes, Route } from 'react-router-dom'
import Sidebar from './components/Sidebar.jsx'
import Navbar from './components/Navbar.jsx'
import DashboardPage from './pages/DashboardPage.jsx'
import LoadEntryPage from './pages/LoadEntryPage.jsx'
import ExpensePage from './pages/ExpensePage.jsx'
import DriverLedgerPage from './pages/DriverLedgerPage.jsx'

function App() {
  return (
    <div className="app-layout">
      <Sidebar />
      <div style={{ flex: 1 }}>
        <Navbar />
        <div className="main-content">
          <Routes>
            <Route path="/" element={<DashboardPage />} />
            <Route path="/load-entries" element={<LoadEntryPage />} />
            <Route path="/expenses" element={<ExpensePage />} />
            <Route path="/driver-ledger" element={<DriverLedgerPage />} />
          </Routes>
        </div>
      </div>
    </div>
  )
}

export default App
