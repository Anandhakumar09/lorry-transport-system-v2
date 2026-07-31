import { useEffect, useState } from 'react'
import { getDriverLedgers } from '../services/driverService'

function DriverLedgerPage() {
  const [ledgers, setLedgers] = useState([])
  const [error, setError] = useState('')

  useEffect(() => {
    getDriverLedgers()
      .then(res => setLedgers(res.data))
      .catch(() => setError('Could not load driver ledger.'))
  }, [])

  return (
    <div>
      <h2>Driver Ledger</h2>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      <div className="card">
        <table>
          <thead>
            <tr>
              <th>Driver</th><th>Advance Given</th><th>Salary</th><th>Extra Paid</th><th>Total Paid</th><th>Remaining Balance</th>
            </tr>
          </thead>
          <tbody>
            {ledgers.map(l => (
              <tr key={l.driverId}>
                <td>{l.driverName}</td>
                <td>₹{l.totalAdvanceGiven}</td>
                <td>₹{l.totalSalary}</td>
                <td>₹{l.totalExtraPaid}</td>
                <td>₹{l.totalPaid}</td>
                <td style={{ fontWeight: 600 }}>₹{l.remainingBalance}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

export default DriverLedgerPage
