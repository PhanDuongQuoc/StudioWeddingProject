import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  timeout: 50000,
  headers: {
    'Content-Type': 'application/json',
  },
})

// Request Interceptor: Gắn token nếu có
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('access_token')
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => Promise.reject(error)
)

// Response Interceptor: Xử lý lỗi trả về
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Nếu bị 401 thì xóa token
    if (error.response && error.response.status === 401) {
      localStorage.removeItem('access_token')
      localStorage.removeItem('auth_user')
    }
    return Promise.reject(error)
  }
)

export default api
