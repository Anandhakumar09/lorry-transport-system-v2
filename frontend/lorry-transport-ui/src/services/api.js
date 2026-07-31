import axios from 'axios'

// Base Axios instance pointing to our ASP.NET Core API.
// Change this URL if your backend runs on a different port.
const api = axios.create({
  baseURL: 'http://localhost:5000/api'
})

export default api
