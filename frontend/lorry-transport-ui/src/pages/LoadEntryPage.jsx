import { useEffect, useState } from 'react'
import { getLoadEntries, createLoadEntry, updateLoadEntry, deleteLoadEntry } from '../services/loadEntryService'

const emptyForm = {
  date: '', fromLocation: '', toLocation: '', customerId: 1, materialName: '',
  vehicleId: 1, driverId: 1, ratePerTon: 0, totalTons: 0, dieselAmount: 0,
  otherExpenses: 0, notes: '',
  // Kீழ (owner accounting) fields
  weightAmount: 0, commission: 0, loadingCharge: 0, cleanerSalary: 0,
  driverSalary: 0, advanceAmount: 0
}

const numericFields = [
  'customerId', 'vehicleId', 'driverId', 'ratePerTon', 'totalTons', 'dieselAmount',
  'otherExpenses', 'weightAmount', 'commission', 'loadingCharge', 'cleanerSalary',
  'driverSalary', 'advanceAmount'
]

function toNumberPayload(form) {
  const payload = { ...form }
  numericFields.forEach(f => { payload[f] = Number(form[f]) })
  return payload
}

function LoadEntryPage() {
  const [entries, setEntries] = useState([])
  const [form, setForm] = useState(emptyForm)
  const [showForm, setShowForm] = useState(false)
  const [editingId, setEditingId] = useState(null)
  const [error, setError] = useState('')

  const loadData = () => {
    getLoadEntries()
      .then(res => setEntries(res.data))
      .catch(() => setError('Could not load entries. Is the API running?'))
  }

  useEffect(() => { loadData() }, [])

  const handleChange = (e) => {
    const { name, value } = e.target
    setForm(prev => ({ ...prev, [name]: value }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    try {
      const payload = toNumberPayload(form)
      if (editingId) {
        await updateLoadEntry(editingId, payload)
      } else {
        await createLoadEntry(payload)
      }
      setForm(emptyForm)
      setEditingId(null)
      setShowForm(false)
      loadData()
    } catch {
      setError('Failed to save load entry. Check if the API is running and reachable.')
    }
  }

  const handleEdit = (entry) => {
    setEditingId(entry.id)
    setForm({
      date: entry.date ? entry.date.substring(0, 10) : '',
      fromLocation: entry.fromLocation || '',
      toLocation: entry.toLocation || '',
      customerId: entry.customerId ?? 1,
      materialName: entry.materialName || '',
      vehicleId: entry.vehicleId ?? 1,
      driverId: entry.driverId ?? 1,
      ratePerTon: entry.ratePerTon ?? 0,
      totalTons: entry.totalTons ?? 0,
      dieselAmount: entry.dieselAmount ?? 0,
      otherExpenses: entry.otherExpenses ?? 0,
      notes: entry.notes || '',
      weightAmount: entry.weightAmount ?? 0,
      commission: entry.commission ?? 0,
      loadingCharge: entry.loadingCharge ?? 0,
      cleanerSalary: entry.cleanerSalary ?? 0,
      driverSalary: entry.driverSalary ?? 0,
      advanceAmount: entry.advanceAmount ?? 0
    })
    setShowForm(true)
  }

  const handleCancelEdit = () => {
    setEditingId(null)
    setForm(emptyForm)
    setShowForm(false)
  }

  const handleDelete = async (id) => {
    if (!confirm('Delete this load entry?')) return
    await deleteLoadEntry(id)
    if (editingId === id) handleCancelEdit()
    loadData()
  }

  // Mela (freight) section: Total Tons x Rate Per Ton = Freight Amount
  const freightAmount = Number(form.totalTons || 0) * Number(form.ratePerTon || 0)
  // Diesel thaniya kaatrom, adhu freight la irundhu kழிக்கல, thaniya track pannurom
  const dieselAmountTop = Number(form.dieselAmount || 0)
  const otherExpensesTop = Number(form.otherExpenses || 0)
  // Venuna net (freight - diesel - other expenses) kூட kaatlam
  const netAfterDieselTop = freightAmount - dieselAmountTop - otherExpensesTop

  // Kீழ (owner accounting) box - idhu maari touch pannala, ஏற்கனவே iruntha maari than
  // Weight Amount + Commission + Loading + Cleaning + Driver Padi -- ellaam add pannurathu
  const totalAll =
    Number(form.weightAmount || 0) +
    Number(form.commission || 0) +
    Number(form.loadingCharge || 0) +
    Number(form.cleanerSalary || 0) +
    Number(form.driverSalary || 0)
  // Advance - Total: + na Labam, - na innum kammi kudukanum
  const finalMithi = Number(form.advanceAmount || 0) - totalAll

  // Row-oda ella calculations (freight, freight-diesel, kila total, final mithi, expense) - ithu function-a pannitten
  // so table rows-kum, keழе Total footer row-kum rendu edathulaiyum same calculation use pannalam
  const computeRowValues = (e) => {
    const freightAmountRow = Number(e.freightAmount || 0)
    const freightMinusDiesel = freightAmountRow - Number(e.dieselAmount || 0)
    const kilaTotal =
      Number(e.weightAmount || 0) +
      Number(e.commission || 0) +
      Number(e.loadingCharge || 0) +
      Number(e.cleanerSalary || 0) +
      Number(e.driverSalary || 0)
    const finalMithiRow = Number(e.advanceAmount || 0) - kilaTotal
    const finalMithiAbs = Math.abs(finalMithiRow)
    const expenseFromDiff = freightMinusDiesel - finalMithiAbs
    return { freightAmountRow, freightMinusDiesel, kilaTotal, finalMithiRow, finalMithiAbs, expenseFromDiff }
  }

  // Ella entries-oda values-um sேrthu, keழе Total row-ku column-vaarigа total kanakku pannurom
  const columnTotals = entries.reduce(
    (acc, e) => {
      const v = computeRowValues(e)
      acc.freightAmount += v.freightAmountRow
      acc.freightMinusDiesel += v.freightMinusDiesel
      acc.finalMithiRow += v.finalMithiRow
      acc.expenseFromDiff += v.expenseFromDiff
      return acc
    },
    { freightAmount: 0, freightMinusDiesel: 0, finalMithiRow: 0, expenseFromDiff: 0 }
  )

  return (
    <div>
      {/* scrollbar-gutter reserves space for the scrollbar always,
          so opening/closing the form (which changes page height) doesn't
          shift/shake the whole page sideways when the scrollbar appears/disappears */}
      <style>{`
        html { scrollbar-gutter: stable; }
        @keyframes formFadeIn {
          from { opacity: 0; transform: translateY(-6px); }
          to { opacity: 1; transform: translateY(0); }
        }
        .load-entry-form-enter { animation: formFadeIn 0.18s ease-out; }
      `}</style>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <h2>Load Entries</h2>
        <button
          className="btn-primary"
          onClick={() => {
            if (showForm) {
              handleCancelEdit()
            } else {
              setShowForm(true)
            }
          }}
        >
          {showForm ? 'Cancel' : '+ New Load Entry'}
        </button>
      </div>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      {showForm && (
        <form className="card load-entry-form-enter" style={{ marginBottom: 20 }} onSubmit={handleSubmit}>
          <div className="form-grid">
            <div><label>Date</label><input type="date" name="date" value={form.date} onChange={handleChange} /></div>
            <div><label>From Location</label><input name="fromLocation" value={form.fromLocation} onChange={handleChange} /></div>
            <div><label>To Location</label><input name="toLocation" value={form.toLocation} onChange={handleChange} /></div>
            <div><label>Material Name</label><input name="materialName" value={form.materialName} onChange={handleChange} /></div>
            <div><label>Customer ID</label><input type="number" name="customerId" value={form.customerId} onChange={handleChange} /></div>
            <div><label>Vehicle ID</label><input type="number" name="vehicleId" value={form.vehicleId} onChange={handleChange} /></div>
            <div><label>Driver ID</label><input type="number" name="driverId" value={form.driverId} onChange={handleChange} /></div>
            <div><label>Rate Per Ton</label><input type="number" name="ratePerTon" value={form.ratePerTon} onChange={handleChange} /></div>
            <div><label>Total Tons</label><input type="number" name="totalTons" value={form.totalTons} onChange={handleChange} /></div>
            <div><label>Diesel Amount</label><input type="number" name="dieselAmount" value={form.dieselAmount} onChange={handleChange} /></div>
            <div><label>Other Expenses</label><input type="number" name="otherExpenses" value={form.otherExpenses} onChange={handleChange} /></div>
          </div>
          <label>Notes</label>
          <textarea name="notes" value={form.notes} onChange={handleChange} rows={2} />

          <div className="card" style={{ marginTop: 12, background: '#f5f7fa' }}>
            <div>Freight Amount (Total Tons × Rate Per Ton): <b>₹{freightAmount}</b></div>
            <div>Diesel Amount: ₹{dieselAmountTop}</div>
            <div>Other Expenses: ₹{otherExpensesTop}</div>
            <div style={{ color: netAfterDieselTop >= 0 ? 'green' : 'red', fontWeight: 600 }}>
              Net (Freight − Diesel − Other Expenses): ₹{netAfterDieselTop}
            </div>
          </div>

          <hr style={{ margin: '16px 0' }} />

          <div className="form-grid">
            <div><label>Weight Amount (₹)</label><input type="number" name="weightAmount" value={form.weightAmount} onChange={handleChange} /></div>
            <div><label>Commission Amount</label><input type="number" name="commission" value={form.commission} onChange={handleChange} /></div>
            <div><label>Loading Kooli</label><input type="number" name="loadingCharge" value={form.loadingCharge} onChange={handleChange} /></div>
            <div><label>Cleaning Kooli</label><input type="number" name="cleanerSalary" value={form.cleanerSalary} onChange={handleChange} /></div>
            <div><label>Driver Padi</label><input type="number" name="driverSalary" value={form.driverSalary} onChange={handleChange} /></div>
            <div><label>Advance Amount</label><input type="number" name="advanceAmount" value={form.advanceAmount} onChange={handleChange} /></div>
          </div>

          <div className="card" style={{ marginTop: 12, background: '#f5f7fa' }}>
            <div>Total (Weight + Commission + Loading + Cleaning + Driver Padi): <b>₹{totalAll}</b></div>
            <div style={{ color: finalMithi >= 0 ? 'green' : 'red', fontWeight: 600 }}>
              Final Mithi Amount (Advance − Total, + na Labam, − na Kammi kudukanum): ₹{finalMithi}
            </div>
          </div>

          <div style={{ marginTop: 10 }}>
            <button className="btn-primary" type="submit">{editingId ? 'Update Load Entry' : 'Save Load Entry'}</button>
            {editingId && <button type="button" className="btn-danger" style={{ marginLeft: 8 }} onClick={handleCancelEdit}>Cancel Edit</button>}
          </div>
        </form>
      )}

      <div className="card" style={{ overflowX: 'auto', WebkitOverflowScrolling: 'touch' }}>
        <table style={{ minWidth: 900, width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr>
              <th style={{ whiteSpace: 'nowrap' }}>Date</th>
              <th style={{ whiteSpace: 'nowrap' }}>From</th>
              <th style={{ whiteSpace: 'nowrap' }}>To</th>
              <th style={{ whiteSpace: 'nowrap' }}>Total Ton - 1 Ton</th>
              <th style={{ whiteSpace: 'nowrap' }}>Total Ton − Diesel</th>
              <th style={{ whiteSpace: 'nowrap' }}>(Advance − Total)</th>
              <th style={{ whiteSpace: 'nowrap' }}>Profit</th>
              <th style={{ whiteSpace: 'nowrap' }}>Actions</th>
            </tr>
          </thead>
          <tbody>
            {entries.map(e => {
              const { freightMinusDiesel, kilaTotal, finalMithiRow, finalMithiAbs, expenseFromDiff } = computeRowValues(e)

              return (
                <tr key={e.id}>
                  <td style={{ whiteSpace: 'nowrap' }}>{new Date(e.date).toLocaleDateString()}</td>
                  <td style={{ whiteSpace: 'nowrap' }}>{e.fromLocation}</td>
                  <td style={{ whiteSpace: 'nowrap' }}>{e.toLocation}</td>
                  <td style={{ whiteSpace: 'nowrap' }}>{e.totalTons}*{e.ratePerTon}={e.freightAmount}</td>
                  <td style={{ whiteSpace: 'nowrap' }}>{e.freightAmount}-{e.dieselAmount}={freightMinusDiesel}</td>
                  <td style={{ whiteSpace: 'nowrap', color: finalMithiRow >= 0 ? 'green' : 'red', fontWeight: 600 }}>{e.advanceAmount}-{kilaTotal}={finalMithiRow}</td>
                  <td style={{ whiteSpace: 'nowrap' }}>{freightMinusDiesel}-{finalMithiAbs}={expenseFromDiff}</td>
                  <td style={{ whiteSpace: 'nowrap' }}>
                    <button className="btn-primary" style={{ marginRight: 8 }} onClick={() => handleEdit(e)}>Edit</button>
                    <button className="btn-danger" onClick={() => handleDelete(e.id)}>Delete</button>
                  </td>
                </tr>
              )
            })}
          </tbody>
          <tfoot>
            <tr style={{ background: '#f5f7fa', fontWeight: 700, borderTop: '2px solid #ccc' }}>
              <td style={{ whiteSpace: 'nowrap' }}>Total</td>
              <td style={{ whiteSpace: 'nowrap' }}></td>
              <td style={{ whiteSpace: 'nowrap' }}></td>
              <td style={{ whiteSpace: 'nowrap' }}>₹{columnTotals.freightAmount}</td>
              <td style={{ whiteSpace: 'nowrap' }}>₹{columnTotals.freightMinusDiesel}</td>
              <td style={{ whiteSpace: 'nowrap', color: columnTotals.finalMithiRow >= 0 ? 'green' : 'red' }}>₹{columnTotals.finalMithiRow}</td>
              <td style={{ whiteSpace: 'nowrap' }}>₹{columnTotals.expenseFromDiff}</td>
              <td style={{ whiteSpace: 'nowrap' }}></td>
            </tr>
          </tfoot>
        </table>
      </div>
    </div>
  )
}

export default LoadEntryPage