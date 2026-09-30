import { defineStore } from 'pinia'
import authService from '@/services/authService'
import type { LoginRequest, RegisterRequest, UserProfileDto } from '@/types/auth'

interface AuthState {
  token: string | null
  user: UserProfileDto | null
  roles: string[]
  permissions: string[]
  isLoading: boolean
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    token: localStorage.getItem('access_token'),
    user: (() => {
      try {
        const stored = localStorage.getItem('auth_user')
        return stored ? JSON.parse(stored) : null
      } catch {
        return null
      }
    })(),
    roles: (() => {
      try {
        const stored = localStorage.getItem('auth_roles')
        return stored ? JSON.parse(stored) : []
      } catch {
        return []
      }
    })(),
    permissions: (() => {
      try {
        const stored = localStorage.getItem('auth_permissions')
        return stored ? JSON.parse(stored) : []
      } catch {
        return []
      }
    })(),
    isLoading: false,
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
    currentUser: (state) => state.user,
    userRoles: (state) => state.roles,
    hasRole: (state) => (role: string) => state.roles.includes(role),
  },

  actions: {
    async login(credentials: LoginRequest) {
      this.isLoading = true
      try {
        const res = await authService.login(credentials)
        if (res.success && res.accessToken) {
          this.token = res.accessToken
          this.user = res.user
          this.roles = res.roles || []
          this.permissions = res.permissions || []

          // Persist in localStorage
          localStorage.setItem('access_token', res.accessToken)
          localStorage.setItem('auth_user', JSON.stringify(res.user))
          localStorage.setItem('auth_roles', JSON.stringify(res.roles || []))
          localStorage.setItem('auth_permissions', JSON.stringify(res.permissions || []))
          return res
        }
        throw new Error(res.message || 'Đăng nhập thất bại!')
      } finally {
        this.isLoading = false
      }
    },

    async register(formData: RegisterRequest) {
      this.isLoading = true
      try {
        const res = await authService.register(formData)
        return res
      } finally {
        this.isLoading = false
      }
    },

    logout() {
      this.token = null
      this.user = null
      this.roles = []
      this.permissions = []
      localStorage.removeItem('access_token')
      localStorage.removeItem('auth_user')
      localStorage.removeItem('auth_roles')
      localStorage.removeItem('auth_permissions')
    },
  },
})
