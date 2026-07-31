import { useEffect, useState } from 'react'
import { getExpenses, createExpense, updateExpense, deleteExpense } from '../services/expenseService'

const expenseTypes = ['Diesel', 'Driver Salary', 'Cleaner Salary', 'Commission', 'Loading Charge', 'Repair', 'Tyre', 'Food', 'Toll Gate', 'Parking', 'Police', 'Other']

const emptyForm = { date: '', expenseType: 'Diesel', amount: 0, remarks: '' }

function ExpensePage() {
  const [expenses, setExpenses] = useState([])
  const [form, setForm] = useState(emptyForm)
  const [editingId, setEditingId] = useState(null)
  const [error, setError] = useState('')

  const loadData = () => {
    getExpenses().then(res => setExpenses(res.data)).catch(() => setError('Could not load expenses.'))
  }

  useEffect(() => { loadData() }, [])

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    try {
      const payload = { ...form, amount: Number(form.amount) }
      if (editingId) {
        await updateExpense(editingId, payload)
      } else {
        await createExpense(payload)
      }
      setForm(emptyForm)
      setEditingId(null)
      loadData()
    } catch {
      setError('Failed to save expense. Check if the API is running.')
    }
  }

  const handleEdit = (ex) => {
    setEditingId(ex.id)
    setForm({
      date: ex.date ? ex.date.substring(0, 10) : '',
      expenseType: ex.expenseType,
      amount: ex.amount,
      remarks: ex.remarks || ''
    })
  }

  const handleCancelEdit = () => {
    setEditingId(null)
    setForm(emptyForm)
  }

  const handleDelete = async (id) => {
    if (!confirm('Delete this expense?')) return
    await deleteExpense(id)
    if (editingId === id) handleCancelEdit()
    loadData()
  }

  return (
    <div>
      <h2>Expenses</h2>
      {error && <p style={{ color: 'red' }}>{error}</p>}

      <form className="card" style={{ marginBottom: 20 }} onSubmit={handleSubmit}>
        <div className="form-grid">
          <div><label>Date</label><input type="date" value={form.date} onChange={e => setForm({ ...form, date: e.target.value })} required /></div>
          <div>
            <label>Expense Type</label>
            <select value={form.expenseType} onChange={e => setForm({ ...form, expenseType: e.target.value })}>
              {expenseTypes.map(t => <option key={t} value={t}>{t}</option>)}
            </select>
          </div>
          <div><label>Amount</label><input type="number" value={form.amount} onChange={e => setForm({ ...form, amount: e.target.value })} required /></div>
          <div><label>Remarks</label><input value={form.remarks} onChange={e => setForm({ ...form, remarks: e.target.value })} /></div>
        </div>
        <button className="btn-primary" type="submit">{editingId ? 'Update Expense' : 'Add Expense'}</button>
        {editingId && <button type="button" className="btn-danger" style={{ marginLeft: 8 }} onClick={handleCancelEdit}>Cancel</button>}
      </form>

      <div className="card">
        <table>
          <thead><tr><th>Date</th><th>Type</th><th>Amount</th><th>Remarks</th><th>Actions</th></tr></thead>
          <tbody>
            {expenses.map(ex => (
              <tr key={ex.id}>
                <td>{new Date(ex.date).toLocaleDateString()}</td>
                <td>{ex.expenseType}</td>
                <td>₹{ex.amount}</td>
                <td>{ex.remarks}</td>
                <td>
                  <button className="btn-primary" style={{ marginRight: 8 }} onClick={() => handleEdit(ex)}>Edit</button>
                  <button className="btn-danger" onClick={() => handleDelete(ex.id)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

export default ExpensePage
