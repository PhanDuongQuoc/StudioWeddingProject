export interface LoginRequest {
  email: string
  password: string
}

export interface UserProfileDto {
  userId: string
  username: string
  email: string
  fullName: string
}

export interface LoginResponse {
  success: boolean
  message: string
  accessToken: string
  tokenType: string
  user: UserProfileDto
  roles: string[]
  permissions: string[]
}

export interface RegisterRequest {
  username: string
  email: string
  password: string
  fullName?: string
  phone?: string
}

export interface UserDto {
  userId: string
  username: string
  email: string
  fullName: string
  createdAt?: string
}

export interface RegisterResponse {
  success: boolean
  message: string
  user?: UserDto
}

export interface SendForgotOtpRequest {
  email: string
}

export interface ResetPasswordWithOtpRequest {
  email: string
  otp: string
  newPassword: string
  confirmPassword: string
}

export interface ForgotPasswordResponse {
  success: boolean
  message: string
}
