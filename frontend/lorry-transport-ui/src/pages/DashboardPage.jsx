import { useEffect, useState } from 'react'
import { getDashboard } from '../services/loadEntryService'

function DashboardPage() {
  const [stats, setStats] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    getDashboard()
      .then(res => setStats(res.data))
      .catch(() => setError('Could not load dashboard. Is the API running?'))
      .finally(() => setLoading(false))
  }, [])

  if (loading) return <p>Loading dashboard...</p>
  if (error) return <p style={{ color: 'red' }}>{error}</p>

  const cards = [
    { label: "Today's Trips", value: stats.todaysTrips ?? 0 },
    { label: 'This Month Trips', value: stats.thisMonthTrips ?? 0 },
    { label: 'Total Income', value: `₹${stats.totalIncome ?? 0}` },
    { label: 'Total Expense', value: `₹${stats.totalExpense ?? 0}` },
    { label: 'Total Profit', value: `₹${stats.totalProfit ?? 0}` },
    { label: 'Diesel Expense', value: `₹${stats.totalDieselExpense ?? 0}` },
    { label: 'Driver Salary', value: `₹${stats.totalDriverSalary ?? 0}` },
    { label: 'Cleaning Salary', value: `₹${stats.totalCleaningSalary ?? 0}` },
    { label: 'Commission', value: `₹${stats.totalCommission ?? 0}` },
    { label: 'Pending Driver Balance', value: `₹${stats.pendingDriverBalance ?? 0}` },
  ]

  return (
    <div style={{ padding: '0 4px' }}>
      {/*
        Mobile (chinna screen): 1 column
        Tablet (~600px+): 2 columns
        Laptop/Desktop (~960px+): auto-fit, minimum 220px per card, columns adjust automatically
        auto-fit + minmax handles this without separate media queries.
      */}
      <div
        className="dashboard-grid"
        style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(min(100%, 220px), 1fr))',
          gap: '16px',
          width: '100%',
        }}
      >
        {cards.map((c, i) => (
          <div
            className="stat-card"
            key={i}
            style={{
              padding: '16px',
              minWidth: 0,
              boxSizing: 'border-box',
              overflow: 'hidden',
            }}
          >
            <h3 style={{ margin: '0 0 8px 0', fontSize: 'clamp(13px, 2.5vw, 15px)' }}>{c.label}</h3>
            <p
              style={{
                margin: 0,
                fontSize: 'clamp(18px, 4vw, 24px)',
                fontWeight: 700,
                wordBreak: 'break-word',
              }}
            >
              {c.value}
            </p>
          </div>
        ))}
      </div>
    </div>
  )
}

export default DashboardPage