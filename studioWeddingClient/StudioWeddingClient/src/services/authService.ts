import api from '@/boot/api-gateway/axios'
import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse,
  SendForgotOtpRequest,
  ResetPasswordWithOtpRequest,
  ForgotPasswordResponse
} from '@/types/auth'

export const authService = {
  async login(payload: LoginRequest): Promise<LoginResponse> {
    const response = await api.post<LoginResponse>('/auth/login', payload)
    return response.data
  },

  async register(payload: RegisterRequest): Promise<RegisterResponse> {
    const response = await api.post<RegisterResponse>('/auth/register', payload)
    return response.data
  },

  async sendForgotOtp(payload: SendForgotOtpRequest): Promise<ForgotPasswordResponse> {
    const response = await api.post<ForgotPasswordResponse>('/auth/send-forgot-otp', payload)
    return response.data
  },

  async resetPasswordWithOtp(payload: ResetPasswordWithOtpRequest): Promise<ForgotPasswordResponse> {
    const response = await api.post<ForgotPasswordResponse>('/auth/reset-password-with-otp', payload)
    return response.data
  }
}

export default authService
