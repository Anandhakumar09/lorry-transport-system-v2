import api from './api'

export const getLoadEntries = () => api.get('/loadentries')
export const getLoadEntry = (id) => api.get(`/loadentries/${id}`)
export const createLoadEntry = (data) => api.post('/loadentries', data)
export const updateLoadEntry = (id, data) => api.put(`/loadentries/${id}`, data)
export const deleteLoadEntry = (id) => api.delete(`/loadentries/${id}`)
export const getDashboard = () => api.get('/dashboard')
