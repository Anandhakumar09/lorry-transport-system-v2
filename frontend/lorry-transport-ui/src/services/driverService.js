import api from './api'

export const getDrivers = () => api.get('/drivers')
export const getDriverLedgers = () => api.get('/driverledger')
