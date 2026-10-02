<template>
  <div class="auth-page-form forgot-password-container">
    <!-- Stepper Tracker -->
    <div class="stepper-indicator">
      <div class="step-item" :class="{ active: currentStep === 1, completed: currentStep > 1 }">
        <span class="step-num">1</span>
        <span class="step-label">Nhập Email</span>
      </div>
      <div class="step-line" :class="{ active: currentStep >= 2 }"></div>
      <div class="step-item" :class="{ active: currentStep === 2, completed: currentStep > 2 }">
        <span class="step-num">2</span>
        <span class="step-label">Xác Thực OTP</span>
      </div>
      <div class="step-line" :class="{ active: currentStep >= 3 }"></div>
      <div class="step-item" :class="{ active: currentStep === 3 }">
        <span class="step-num">3</span>
        <span class="step-label">Hoàn Tất</span>
      </div>
    </div>

    <!-- Alert Banner for Errors -->
    <transition name="fade">
      <div v-if="errorMessage" class="error-banner">
        <i class="fa-solid fa-circle-exclamation"></i>
        <span>{{ errorMessage }}</span>
      </div>
    </transition>

    <!-- Alert Banner for Info/Success -->
    <transition name="fade">
      <div v-if="infoMessage" class="info-banner">
        <i class="fa-solid fa-circle-info"></i>
        <span>{{ infoMessage }}</span>
      </div>
    </transition>

    <!-- STEP 1: NHẬP EMAIL -->
    <div v-if="currentStep === 1" class="step-content">
      <div class="form-header">
        <span class="sub-badge font-chinese">喜事・忘記密碼</span>
        <h1 class="form-title font-serif">Quên Mật Khẩu</h1>
        <p class="form-subtitle">
          Vui lòng nhập địa chỉ email đã đăng ký tài khoản. Hỷ Sự Studio sẽ gửi mã xác thực OTP gồm 6 chữ số đến hộp thư của bạn.
        </p>
      </div>

      <q-form @submit.prevent="handleSendOtp" class="auth-form">
        <div class="form-group">
          <label class="form-label" for="forgot-email">Địa chỉ Email <span class="req">*</span></label>
          <q-input
            id="forgot-email"
            v-model="email"
            type="email"
            outlined
            dense
            placeholder="Nhập email của bạn..."
            class="vintage-input"
            :rules="[
              val => !!val || 'Vui lòng nhập email',
              val => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(val) || 'Định dạng email không hợp lệ'
            ]"
            lazy-rules
          >
            <template #prepend>
              <q-icon name="fa-regular fa-envelope" size="16px" class="input-icon" />
            </template>
          </q-input>
        </div>

        <div class="form-actions">
          <q-btn
            type="submit"
            :loading="isLoading"
            class="btn-vintage full-width auth-submit-btn"
            unelevated
            no-caps
          >
            <template #loading>
              <q-spinner-dots size="20px" />
            </template>
            <span class="btn-text">Gửi Mã Xác Thực OTP</span>
          </q-btn>
        </div>
      </q-form>

      <div class="form-footer-divider">
        <span class="divider-text">Hoặc</span>
      </div>

      <div class="switch-auth">
        <router-link to="/dang-nhap" class="switch-link back-link">
          <i class="fa-solid fa-arrow-left"></i>
          <span>Quay lại Đăng nhập</span>
        </router-link>
      </div>
    </div>

    <!-- STEP 2: NHẬP OTP & ĐẶT MẬT KHẨU MỚI -->
    <div v-else-if="currentStep === 2" class="step-content">
      <div class="form-header">
        <span class="sub-badge font-chinese">喜事・重置密碼</span>
        <h1 class="form-title font-serif">Đặt Lại Mật Khẩu</h1>
        <div class="email-badge-card">
          <div class="badge-text">
            <span>Mã OTP đã gửi đến:</span>
            <strong>{{ email }}</strong>
          </div>
          <button type="button" @click="backToStep1" class="btn-change-email">
            Đổi email
          </button>
        </div>
      </div>

      <q-form @submit.prevent="handleResetPassword" class="auth-form">
        <!-- OTP Input -->
        <div class="form-group">
          <div class="label-row">
            <label class="form-label" for="otp-input">Mã xác thực OTP (6 số) <span class="req">*</span></label>
            <button
              type="button"
              @click="handleResendOtp"
              :disabled="countdown > 0 || isLoading"
              class="resend-otp-btn"
            >
              <span v-if="countdown > 0">Gửi lại sau ({{ countdown }}s)</span>
              <span v-else>Gửi lại mã OTP</span>
            </button>
          </div>
          <q-input
            id="otp-input"
            v-model="otp"
            outlined
            dense
            placeholder="Ví dụ: 123456"
            class="vintage-input otp-field"
            maxlength="6"
            :rules="[
              val => !!val || 'Vui lòng nhập mã OTP',
              val => val.length === 6 || 'Mã OTP gồm 6 chữ số'
            ]"
            lazy-rules
          >
            <template #prepend>
              <q-icon name="fa-solid fa-shield-halved" size="16px" class="input-icon" />
            </template>
          </q-input>
        </div>

        <!-- New Password Input -->
        <div class="form-group">
          <label class="form-label" for="new-password">Mật khẩu mới <span class="req">*</span></label>
          <q-input
            id="new-password"
            v-model="newPassword"
            :type="isPwdVisible ? 'text' : 'password'"
            outlined
            dense
            placeholder="Tối thiểu 6 ký tự..."
            class="vintage-input"
            :rules="[
              val => !!val || 'Vui lòng nhập mật khẩu mới',
              val => val.length >= 6 || 'Mật khẩu phải từ 6 ký tự trở lên',
              val => val.length <= 32 || 'Mật khẩu tối đa 32 ký tự'
            ]"
            lazy-rules
          >
            <template #prepend>
              <q-icon name="fa-solid fa-lock" size="16px" class="input-icon" />
            </template>
            <template #append>
              <q-icon
                :name="isPwdVisible ? 'fa-regular fa-eye-slash' : 'fa-regular fa-eye'"
                class="cursor-pointer input-icon toggle-pwd"
                size="16px"
                @click="isPwdVisible = !isPwdVisible"
              />
            </template>
          </q-input>
        </div>

        <!-- Confirm Password Input -->
        <div class="form-group">
          <label class="form-label" for="confirm-password">Xác nhận mật khẩu mới <span class="req">*</span></label>
          <q-input
            id="confirm-password"
            v-model="confirmPassword"
            :type="isConfirmPwdVisible ? 'text' : 'password'"
            outlined
            dense
            placeholder="Nhập lại mật khẩu mới..."
            class="vintage-input"
            :rules="[
              val => !!val || 'Vui lòng xác nhận mật khẩu mới',
              val => val === newPassword || 'Mật khẩu xác nhận không khớp'
            ]"
            lazy-rules
          >
            <template #prepend>
              <q-icon name="fa-solid fa-check-double" size="16px" class="input-icon" />
            </template>
            <template #append>
              <q-icon
                :name="isConfirmPwdVisible ? 'fa-regular fa-eye-slash' : 'fa-regular fa-eye'"
                class="cursor-pointer input-icon toggle-pwd"
                size="16px"
                @click="isConfirmPwdVisible = !isConfirmPwdVisible"
              />
            </template>
          </q-input>
        </div>

        <div class="form-actions">
          <q-btn
            type="submit"
            :loading="isLoading"
            class="btn-vintage full-width auth-submit-btn"
            unelevated
            no-caps
          >
            <template #loading>
              <q-spinner-dots size="20px" />
            </template>
            <span class="btn-text">Xác Nhận & Đổi Mật Khẩu</span>
          </q-btn>
        </div>
      </q-form>

      <div class="form-footer-divider">
        <span class="divider-text">✦</span>
      </div>

      <div class="switch-auth">
        <button type="button" @click="backToStep1" class="switch-link-btn">
          <i class="fa-solid fa-arrow-left"></i>
          <span>Quay lại bước trước</span>
        </button>
      </div>
    </div>

    <!-- STEP 3: THÀNH CÔNG -->
    <div v-else-if="currentStep === 3" class="step-content success-step">
      <div class="success-icon-wrapper">
        <div class="success-seal font-chinese">囍</div>
      </div>

      <div class="form-header text-center">
        <span class="sub-badge font-chinese">喜事・圓滿成功</span>
        <h1 class="form-title font-serif">Đổi Mật Khẩu Thành Công</h1>
        <p class="form-subtitle">
          Mật khẩu tài khoản của bạn đã được cập nhật thành công. Hãy đăng nhập ngay để tiếp tục trải nghiệm dịch vụ của Hỷ Sự Studio.
        </p>
      </div>

      <div class="form-actions q-mt-lg">
        <q-btn
          @click="goToLogin"
          class="btn-vintage full-width auth-submit-btn"
          unelevated
          no-caps
        >
          <span class="btn-text">Đăng Nhập Ngay</span>
          <i class="fa-solid fa-arrow-right q-ml-sm"></i>
        </q-btn>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import authService from '@/services/authService'

const router = useRouter()
const $q = useQuasar()

// State
const currentStep = ref<1 | 2 | 3>(1)
const isLoading = ref(false)
const errorMessage = ref('')
const infoMessage = ref('')

// Form Fields
const email = ref('')
const otp = ref('')
const newPassword = ref('')
const confirmPassword = ref('')

// Visibility Toggles
const isPwdVisible = ref(false)
const isConfirmPwdVisible = ref(false)

// Resend OTP Countdown
const countdown = ref(0)
let timer: ReturnType<typeof setInterval> | null = null

const startCountdown = (seconds = 60) => {
  countdown.value = seconds
  if (timer) clearInterval(timer)
  timer = setInterval(() => {
    if (countdown.value > 0) {
      countdown.value--
    } else {
      if (timer) clearInterval(timer)
      timer = null
    }
  }, 1000)
}

onUnmounted(() => {
  if (timer) clearInterval(timer)
})

// Handlers
const handleSendOtp = async () => {
  errorMessage.value = ''
  infoMessage.value = ''
  isLoading.value = true

  try {
    const res = await authService.sendForgotOtp({ email: email.value.trim() })
    if (res.success) {
      $q.notify({
        type: 'positive',
        message: res.message || 'Mã xác thực OTP đã được gửi đến email của bạn!',
        position: 'top',
        timeout: 4000,
        icon: 'fa-solid fa-circle-check'
      })
      currentStep.value = 2
      startCountdown(60)
    } else {
      errorMessage.value = res.message || 'Không thể gửi mã OTP. Vui lòng kiểm tra lại email!'
    }
  } catch (err: unknown) {
    const errorObj = err as { response?: { data?: { message?: string } }; message?: string }
    const msg = errorObj.response?.data?.message || errorObj.message || 'Có lỗi xảy ra khi gửi mã OTP. Vui lòng thử lại!'
    errorMessage.value = msg
  } finally {
    isLoading.value = false
  }
}

const handleResendOtp = async () => {
  if (countdown.value > 0 || isLoading.value) return
  errorMessage.value = ''
  isLoading.value = true

  try {
    const res = await authService.sendForgotOtp({ email: email.value.trim() })
    if (res.success) {
      $q.notify({
        type: 'positive',
        message: 'Đã gửi lại mã OTP mới về email của bạn!',
        position: 'top',
        timeout: 4000,
        icon: 'fa-solid fa-circle-check'
      })
      startCountdown(60)
    } else {
      errorMessage.value = res.message || 'Không thể gửi lại mã OTP.'
    }
  } catch (err: unknown) {
    const errorObj = err as { response?: { data?: { message?: string } }; message?: string }
    errorMessage.value = errorObj.response?.data?.message || errorObj.message || 'Lỗi gửi lại mã OTP.'
  } finally {
    isLoading.value = false
  }
}

const handleResetPassword = async () => {
  errorMessage.value = ''
  isLoading.value = true

  try {
    const res = await authService.resetPasswordWithOtp({
      email: email.value.trim(),
      otp: otp.value.trim(),
      newPassword: newPassword.value,
      confirmPassword: confirmPassword.value
    })

    if (res.success) {
      $q.notify({
        type: 'positive',
        message: res.message || 'Đặt lại mật khẩu thành công!',
        position: 'top',
        timeout: 4000,
        icon: 'fa-solid fa-circle-check'
      })
      currentStep.value = 3
    } else {
      errorMessage.value = res.message || 'Đổi mật khẩu thất bại. Vui lòng thử lại!'
    }
  } catch (err: unknown) {
    const errorObj = err as { response?: { data?: { message?: string } }; message?: string }
    const msg = errorObj.response?.data?.message || errorObj.message || 'Có lỗi xảy ra khi đặt lại mật khẩu.'
    errorMessage.value = msg
  } finally {
    isLoading.value = false
  }
}

const backToStep1 = () => {
  errorMessage.value = ''
  infoMessage.value = ''
  currentStep.value = 1
}

const goToLogin = async () => {
  await router.push({
    path: '/dang-nhap',
    query: { email: email.value }
  })
}
</script>

<style scoped lang="scss">
.auth-page-form {
  width: 100%;
  max-width: 440px;
  margin: 0 auto;

  // Stepper Indicator
  .stepper-indicator {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 28px;
    padding: 0 8px;

    .step-item {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 4px;

      .step-num {
        width: 28px;
        height: 28px;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 0.82rem;
        font-weight: 600;
        border: 1px solid var(--color-border);
        background-color: var(--color-paper);
        color: var(--color-muted);
        transition: all 0.3s ease;
      }

      .step-label {
        font-size: 0.72rem;
        color: var(--color-muted);
        letter-spacing: 0.5px;
        transition: all 0.3s ease;
      }

      &.active {
        .step-num {
          background-color: var(--color-burgundy);
          border-color: var(--color-burgundy);
          color: #FAF7F0;
        }

        .step-label {
          color: var(--color-burgundy);
          font-weight: 600;
        }
      }

      &.completed {
        .step-num {
          background-color: var(--color-burgundy-soft, #A8443E);
          border-color: var(--color-burgundy-soft, #A8443E);
          color: #FAF7F0;
        }
      }
    }

    .step-line {
      flex: 1;
      height: 1px;
      background-color: var(--color-border);
      margin: 0 10px;
      margin-bottom: 18px;
      transition: background-color 0.3s ease;

      &.active {
        background-color: var(--color-burgundy);
      }
    }
  }

  // Header
  .form-header {
    margin-bottom: 24px;

    .sub-badge {
      display: inline-block;
      font-size: 0.8rem;
      letter-spacing: 2px;
      color: var(--color-burgundy);
      margin-bottom: 6px;
      text-transform: uppercase;
      font-weight: 600;
    }

    .form-title {
      font-size: 1.85rem;
      font-weight: 600;
      color: var(--color-ink);
      line-height: 1.2;
      margin: 0 0 8px 0;
    }

    .form-subtitle {
      font-size: 0.9rem;
      color: var(--color-ink-soft);
      line-height: 1.5;
      margin: 0;
    }
  }

  // Email Card in Step 2
  .email-badge-card {
    display: flex;
    justify-content: space-between;
    align-items: center;
    background-color: var(--color-paper);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    padding: 10px 14px;
    margin-top: 12px;

    .badge-text {
      font-size: 0.84rem;
      color: var(--color-ink-soft);

      span {
        display: block;
        font-size: 0.76rem;
        color: var(--color-muted);
      }

      strong {
        color: var(--color-burgundy);
      }
    }

    .btn-change-email {
      background: none;
      border: none;
      color: var(--color-burgundy);
      font-size: 0.8rem;
      font-weight: 600;
      cursor: pointer;
      text-decoration: underline;
      padding: 4px;

      &:hover {
        color: var(--color-burgundy-dark);
      }
    }
  }

  // Banners
  .error-banner {
    display: flex;
    align-items: center;
    gap: 8px;
    background-color: #FDF2F2;
    border: 1px solid #F8B4B4;
    color: #9B1C1C;
    padding: 10px 14px;
    border-radius: var(--radius-xs);
    font-size: 0.86rem;
    margin-bottom: 18px;
  }

  .info-banner {
    display: flex;
    align-items: center;
    gap: 8px;
    background-color: #F0FDF4;
    border: 1px solid #BBF7D0;
    color: #166534;
    padding: 10px 14px;
    border-radius: var(--radius-xs);
    font-size: 0.86rem;
    margin-bottom: 18px;
  }

  // Form Controls
  .form-group {
    margin-bottom: 16px;

    .form-label {
      display: block;
      font-size: 0.86rem;
      font-weight: 500;
      color: var(--color-ink);
      margin-bottom: 6px;

      .req {
        color: var(--color-burgundy);
      }
    }

    .label-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 6px;

      .resend-otp-btn {
        background: none;
        border: none;
        font-size: 0.8rem;
        color: var(--color-burgundy);
        cursor: pointer;
        font-weight: 500;
        padding: 0;
        transition: color 0.2s;

        &:hover:not(:disabled) {
          text-decoration: underline;
        }

        &:disabled {
          color: var(--color-muted);
          cursor: not-allowed;
        }
      }
    }
  }

  :deep(.vintage-input) {
    .q-field__control {
      border-radius: var(--radius-xs) !important;
      background-color: var(--color-paper) !important;
      border-color: var(--color-border) !important;
      height: 44px;
      transition: all 0.25s ease;

      &:hover {
        border-color: var(--color-border-dark) !important;
      }
    }

    &.q-field--focused .q-field__control {
      border-color: var(--color-burgundy) !important;
      box-shadow: 0 0 0 2px rgba(142, 41, 41, 0.12) !important;
    }

    .input-icon {
      color: var(--color-muted);
      transition: color 0.2s;
    }

    .toggle-pwd:hover {
      color: var(--color-ink);
    }

    &.otp-field {
      input {
        letter-spacing: 4px;
        font-weight: 600;
        font-size: 1.1rem;
        color: var(--color-burgundy);
      }
    }
  }

  .auth-submit-btn {
    height: 44px;
    font-size: 0.98rem;
    letter-spacing: 0.5px;
    font-weight: 500;
    margin-top: 8px;
  }

  .form-footer-divider {
    position: relative;
    text-align: center;
    margin: 22px 0 18px 0;

    &::before {
      content: '';
      position: absolute;
      top: 50%;
      left: 0;
      right: 0;
      height: 1px;
      background-color: var(--color-border-light);
    }

    .divider-text {
      position: relative;
      background-color: var(--color-paper-light);
      padding: 0 12px;
      font-size: 0.8rem;
      color: var(--color-muted);
      text-transform: uppercase;
      letter-spacing: 1px;
    }
  }

  .switch-auth {
    text-align: center;
    font-size: 0.88rem;

    .switch-link,
    .switch-link-btn {
      color: var(--color-ink-soft);
      font-weight: 500;
      text-decoration: none;
      display: inline-flex;
      align-items: center;
      gap: 6px;
      background: none;
      border: none;
      cursor: pointer;
      font-size: 0.88rem;
      transition: color 0.2s ease;

      &:hover {
        color: var(--color-burgundy);
      }
    }
  }

  // Step 3: Success Layout
  .success-step {
    text-align: center;
    padding: 10px 0;

    .success-icon-wrapper {
      margin-bottom: 20px;

      .success-seal {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        width: 68px;
        height: 68px;
        background-color: var(--color-burgundy);
        color: #FAF7F0;
        font-size: 2.2rem;
        border-radius: 4px;
        border: 2px solid #FAF7F0;
        box-shadow: 0 4px 16px rgba(142, 41, 41, 0.25);
      }
    }
  }
}
</style>
