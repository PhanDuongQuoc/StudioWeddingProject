<template>
  <div class="auth-page-form">
    <!-- Form Header -->
    <div class="form-header">
      <span class="sub-badge font-chinese">喜事・登入</span>
      <h1 class="form-title font-serif">Đăng Nhập</h1>
      <p class="form-subtitle">
        Chào mừng quý khách quay trở lại với không gian lưu giữ hạnh phúc của Hỷ Sự Studio.
      </p>
    </div>

    <!-- Alert Banner for Errors -->
    <transition name="fade">
      <div v-if="errorMessage" class="error-banner">
        <i class="fa-solid fa-circle-exclamation"></i>
        <span>{{ errorMessage }}</span>
      </div>
    </transition>

    <!-- Login Form -->
    <q-form @submit.prevent="handleLogin" class="auth-form">
      <!-- Email / Username Input -->
      <div class="form-group">
        <label class="form-label" for="email-input">Email hoặc Tên đăng nhập <span class="req">*</span></label>
        <q-input
          id="email-input"
          v-model="form.email"
          outlined
          dense
          placeholder="Nhập email của bạn..."
          class="vintage-input"
          :rules="[val => !!val || 'Vui lòng nhập Email hoặc tên đăng nhập']"
          lazy-rules
        >
          <template #prepend>
            <q-icon name="fa-regular fa-envelope" size="16px" class="input-icon" />
          </template>
        </q-input>
      </div>

      <!-- Password Input -->
      <div class="form-group">
        <div class="label-row">
          <label class="form-label" for="password-input">Mật khẩu <span class="req">*</span></label>
          <router-link to="/quen-mat-khau" class="forgot-link">Quên mật khẩu?</router-link>
        </div>
        <q-input
          id="password-input"
          v-model="form.password"
          :type="isPwdVisible ? 'text' : 'password'"
          outlined
          dense
          placeholder="Nhập mật khẩu..."
          class="vintage-input"
          :rules="[val => !!val || 'Vui lòng nhập mật khẩu']"
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

      <!-- Remember Me -->
      <div class="form-options">
        <q-checkbox
          v-model="rememberMe"
          label="Ghi nhớ đăng nhập"
          dense
          class="vintage-checkbox"
          color="brown-8"
        />
      </div>

      <!-- Submit Button -->
      <div class="form-actions">
        <q-btn
          type="submit"
          :loading="authStore.isLoading"
          class="btn-vintage full-width auth-submit-btn"
          unelevated
          no-caps
        >
          <template #loading>
            <q-spinner-dots size="20px" />
          </template>
          <span class="btn-text">Đăng Nhập</span>
        </q-btn>
      </div>
    </q-form>

    <!-- Form Footer Divider -->
    <div class="form-footer-divider">
      <span class="divider-text">hoặc</span>
    </div>

    <!-- Switch to Register -->
    <div class="switch-auth">
      <span class="switch-prompt">Chưa có tài khoản thành viên?</span>
      <router-link to="/dang-ky" class="switch-link">
        Đăng ký tài khoản mới →
      </router-link>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useQuasar } from 'quasar'
import { useAuthStore } from '@/stores/authStore'

const router = useRouter()
const route = useRoute()
const $q = useQuasar()
const authStore = useAuthStore()

const isPwdVisible = ref(false)
const rememberMe = ref(true)
const errorMessage = ref('')

const form = reactive({
  email: '',
  password: '',
})

onMounted(() => {
  if (route.query.email && typeof route.query.email === 'string') {
    form.email = route.query.email
  }
})

async function handleLogin() {
  errorMessage.value = ''
  try {
    const res = await authStore.login({
      email: form.email,
      password: form.password,
    })

    if (res.success) {
      $q.notify({
        type: 'positive',
        message: res.message || 'Đăng nhập thành công!',
        position: 'top',
        timeout: 2500,
        icon: 'fa-solid fa-circle-check'
      })

      // Redirect to home or admin dashboard depending on roles
      if (authStore.userRoles.includes('Admin')) {
        await router.push('/quan-tri-hy-su-studio')
      } else {
        await router.push('/trang-chu')
      }
    }
  } catch (err: unknown) {
    const errorObj = err as { response?: { data?: { message?: string } }; message?: string }
    const msg = errorObj.response?.data?.message || errorObj.message || 'Đăng nhập không thành công. Vui lòng thử lại!'
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
  max-width: 420px;
  margin: 0 auto;

  .form-header {
    margin-bottom: 26px;

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
      font-size: 2rem;
      font-weight: 600;
      color: var(--color-ink);
      line-height: 1.2;
      margin: 0 0 8px 0;
    }

    .form-subtitle {
      font-size: 0.92rem;
      color: var(--color-ink-soft);
      line-height: 1.5;
      margin: 0;
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
    margin-bottom: 20px;
  }

  .form-group {
    margin-bottom: 18px;

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

      .forgot-link {
        font-size: 0.82rem;
        color: var(--color-ink-soft);
        text-decoration: none;
        transition: color 0.2s ease;

        &:hover {
          color: var(--color-burgundy);
          text-decoration: underline;
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
  }

  .form-options {
    margin-bottom: 20px;
    display: flex;
    align-items: center;

    :deep(.vintage-checkbox) {
      .q-checkbox__label {
        font-size: 0.86rem;
        color: var(--color-ink-soft);
      }
    }
  }

  .auth-submit-btn {
    height: 44px;
    font-size: 0.98rem;
    letter-spacing: 0.5px;
    font-weight: 500;
  }

  .form-footer-divider {
    position: relative;
    text-align: center;
    margin: 24px 0 20px 0;

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
    font-size: 0.9rem;

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
