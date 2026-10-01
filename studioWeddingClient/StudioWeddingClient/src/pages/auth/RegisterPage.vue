<template>
  <div class="auth-page-form register-form-container">
    <!-- Form Header -->
    <div class="form-header">
      <span class="sub-badge font-chinese">喜事・註冊</span>
      <h1 class="form-title font-serif">Đăng Ký Tài Khoản</h1>
      <p class="form-subtitle">
        Trở thành thành viên của Hỷ Sự Studio để nhận các ưu đãi và quản lý lịch hẹn chụp ảnh cưới.
      </p>
    </div>

    <!-- Alert Banner for Errors -->
    <transition name="fade">
      <div v-if="errorMessage" class="error-banner">
        <i class="fa-solid fa-circle-exclamation"></i>
        <span>{{ errorMessage }}</span>
      </div>
    </transition>

    <!-- Register Form -->
    <q-form @submit.prevent="handleRegister" class="auth-form">
      <div class="form-grid">
        <!-- Username Input -->
        <div class="form-group">
          <label class="form-label" for="reg-username">Tên đăng nhập <span class="req">*</span></label>
          <q-input
            id="reg-username"
            v-model="form.username"
            outlined
            dense
            placeholder="vd: nguyenvana"
            class="vintage-input"
            :rules="[
              val => !!val || 'Vui lòng nhập tên đăng nhập',
              val => val.length >= 3 || 'Tên đăng nhập phải từ 3 ký tự'
            ]"
            lazy-rules
          >
            <template #prepend>
              <q-icon name="fa-regular fa-user" size="16px" class="input-icon" />
            </template>
          </q-input>
        </div>

        <!-- Full Name Input -->
        <div class="form-group">
          <label class="form-label" for="reg-fullname">Họ và tên</label>
          <q-input
            id="reg-fullname"
            v-model="form.fullName"
            outlined
            dense
            placeholder="vd: Nguyễn Văn A"
            class="vintage-input"
          >
            <template #prepend>
              <q-icon name="fa-solid fa-id-card" size="16px" class="input-icon" />
            </template>
          </q-input>
        </div>
      </div>

      <div class="form-grid">
        <!-- Email Input -->
        <div class="form-group">
          <label class="form-label" for="reg-email">Địa chỉ Email <span class="req">*</span></label>
          <q-input
            id="reg-email"
            v-model="form.email"
            type="email"
            outlined
            dense
            placeholder="vd: contact@example.com"
            class="vintage-input"
            :rules="[
              val => !!val || 'Vui lòng nhập email',
              val => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(val) || 'Email không hợp lệ'
            ]"
            lazy-rules
          >
            <template #prepend>
              <q-icon name="fa-regular fa-envelope" size="16px" class="input-icon" />
            </template>
          </q-input>
        </div>

        <!-- Phone Input -->
        <div class="form-group">
          <label class="form-label" for="reg-phone">Số điện thoại</label>
          <q-input
            id="reg-phone"
            v-model="form.phone"
            type="tel"
            outlined
            dense
            placeholder="vd: 0901234567"
            class="vintage-input"
          >
            <template #prepend>
              <q-icon name="fa-solid fa-phone" size="16px" class="input-icon" />
            </template>
          </q-input>
        </div>
      </div>

      <div class="form-grid">
        <!-- Password Input -->
        <div class="form-group">
          <label class="form-label" for="reg-password">Mật khẩu <span class="req">*</span></label>
          <q-input
            id="reg-password"
            v-model="form.password"
            :type="isPwdVisible ? 'text' : 'password'"
            outlined
            dense
            placeholder="Tối thiểu 6 ký tự..."
            class="vintage-input"
            :rules="[
              val => !!val || 'Vui lòng nhập mật khẩu',
              val => val.length >= 6 || 'Mật khẩu phải từ 6 ký tự'
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
          <label class="form-label" for="reg-confirm-password">Xác nhận mật khẩu <span class="req">*</span></label>
          <q-input
            id="reg-confirm-password"
            v-model="form.confirmPassword"
            :type="isConfirmPwdVisible ? 'text' : 'password'"
            outlined
            dense
            placeholder="Nhập lại mật khẩu..."
            class="vintage-input"
            :rules="[
              val => !!val || 'Vui lòng xác nhận mật khẩu',
              val => val === form.password || 'Mật khẩu xác nhận không khớp'
            ]"
            lazy-rules
          >
            <template #prepend>
              <q-icon name="fa-solid fa-shield-halved" size="16px" class="input-icon" />
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
      </div>

      <!-- Terms Agreement -->
      <div class="form-options">
        <q-checkbox
          v-model="agreeTerms"
          dense
          class="vintage-checkbox"
          color="brown-8"
        >
          <span class="terms-text">
            Tôi đồng ý với <a href="#dieu-khoan" class="terms-link">Chính sách & Điều khoản</a> của Hỷ Sự Studio.
          </span>
        </q-checkbox>
      </div>

      <!-- Submit Button -->
      <div class="form-actions">
        <q-btn
          type="submit"
          :loading="authStore.isLoading"
          :disabled="!agreeTerms"
          class="btn-vintage full-width auth-submit-btn"
          unelevated
          no-caps
        >
          <template #loading>
            <q-spinner-dots size="20px" />
          </template>
          <span class="btn-text">Đăng Ký Tài Khoản</span>
        </q-btn>
      </div>
    </q-form>

    <!-- Switch to Login -->
    <div class="switch-auth">
      <span class="switch-prompt">Đã có tài khoản thành viên?</span>
      <router-link to="/dang-nhap" class="switch-link">
        Đăng nhập ngay →
      </router-link>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import { useAuthStore } from '@/stores/authStore'

const router = useRouter()
const $q = useQuasar()
const authStore = useAuthStore()

const isPwdVisible = ref(false)
const isConfirmPwdVisible = ref(false)
const agreeTerms = ref(true)
const errorMessage = ref('')

const form = reactive({
  username: '',
  email: '',
  password: '',
  confirmPassword: '',
  fullName: '',
  phone: ''
})

async function handleRegister() {
  if (!agreeTerms.value) {
    $q.notify({
      type: 'warning',
      message: 'Vui lòng đồng ý với điều khoản sử dụng',
      position: 'top',
      icon: 'fa-solid fa-triangle-exclamation'
    })
    return
  }

  errorMessage.value = ''
  try {
    const res = await authStore.register({
      username: form.username,
      email: form.email,
      password: form.password,
      fullName: form.fullName,
      phone: form.phone
    })

    if (res.success) {
      $q.notify({
        type: 'positive',
        message: res.message || 'Đăng ký tài khoản thành công! Vui lòng đăng nhập.',
        position: 'top',
        timeout: 3000,
        icon: 'fa-solid fa-circle-check'
      })
      await router.push('/dang-nhap')
    }
  } catch (err: unknown) {
    const errorObj = err as { response?: { data?: { message?: string } }; message?: string }
    const msg = errorObj.response?.data?.message || errorObj.message || 'Đăng ký thất bại. Vui lòng kiểm tra lại thông tin!'
    errorMessage.value = msg
    $q.notify({
      type: 'negative',
      message: msg,
      position: 'top',
      timeout: 3000,
      icon: 'fa-solid fa-circle-exclamation'
    })
  }
}
</script>

<style scoped lang="scss">
.auth-page-form {
  width: 100%;
  max-width: 480px;
  margin: 0 auto;

  .form-header {
    margin-bottom: 22px;

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
      margin: 0 0 6px 0;
    }

    .form-subtitle {
      font-size: 0.88rem;
      color: var(--color-ink-soft);
      line-height: 1.5;
      margin: 0;
    }
  }

  .form-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 12px;

    @media (max-width: 500px) {
      grid-template-columns: 1fr;
      gap: 0;
    }
  }

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

  .form-group {
    margin-bottom: 14px;

    .form-label {
      display: block;
      font-size: 0.84rem;
      font-weight: 500;
      color: var(--color-ink);
      margin-bottom: 4px;

      .req {
        color: var(--color-burgundy);
      }
    }
  }

  :deep(.vintage-input) {
    .q-field__control {
      border-radius: var(--radius-xs) !important;
      background-color: var(--color-paper) !important;
      border-color: var(--color-border) !important;
      height: 42px;
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
  }

  .form-options {
    margin: 14px 0 18px 0;

    .terms-text {
      font-size: 0.82rem;
      color: var(--color-ink-soft);
      line-height: 1.4;

      .terms-link {
        color: var(--color-burgundy);
        text-decoration: underline;

        &:hover {
          color: var(--color-burgundy-dark);
        }
      }
    }
  }

  .auth-submit-btn {
    height: 44px;
    font-size: 0.98rem;
    letter-spacing: 0.5px;
    font-weight: 500;
  }

  .switch-auth {
    text-align: center;
    font-size: 0.88rem;
    margin-top: 20px;

    .switch-prompt {
      color: var(--color-ink-soft);
      margin-right: 6px;
    }

    .switch-link {
      color: var(--color-burgundy);
      font-weight: 600;
      text-decoration: none;
      transition: all 0.2s ease;

      &:hover {
        color: var(--color-burgundy-dark);
        text-decoration: underline;
      }
    }
  }
}
</style>
