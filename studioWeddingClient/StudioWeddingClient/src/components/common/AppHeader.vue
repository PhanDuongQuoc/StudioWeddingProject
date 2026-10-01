<template>
  <header class="app-header">
    <div class="studio-container header-inner">
      <!-- Logo -->
      <router-link to="/trang-chu" class="brand-logo">
        <svg class="flower-icon" viewBox="0 0 24 24" width="22" height="22" fill="none" stroke="currentColor">
          <circle cx="12" cy="12" r="11" fill="#8E2929" stroke="#FAF7F0" stroke-width="0.75" />
          <path d="M12 4C13 6.5 14.5 8.5 17 9C14.5 10.5 13 12.5 12 15C11 12.5 9.5 10.5 7 9C9.5 8.5 11 6.5 12 4Z"
            fill="#FAF7F0" stroke="#FAF7F0" stroke-width="0.3" />
          <path d="M20 12C17.5 13 15.5 14.5 15 17C13.5 14.5 11.5 13 9 12C11.5 11 13.5 9.5 15 7C15.5 9.5 17.5 11 20 12Z"
            fill="#FAF7F0" stroke="#FAF7F0" stroke-width="0.3" />
          <path d="M12 20C11 17.5 9.5 15.5 7 15C9.5 13.5 11 11.5 12 9C13 11.5 14.5 13.5 17 15C14.5 15.5 13 17.5 12 20Z"
            fill="#FAF7F0" stroke="#FAF7F0" stroke-width="0.3" />
          <path d="M4 12C6.5 11 8.5 9.5 9 7C10.5 9.5 12.5 11 15 12C12.5 13 10.5 14.5 9 17C8.5 14.5 6.5 13 4 12Z"
            fill="#FAF7F0" stroke="#FAF7F0" stroke-width="0.3" />
          <circle cx="12" cy="12" r="2.2" fill="#8E2929" stroke="#FAF7F0" stroke-width="0.6" />
        </svg>
        <span class="brand-name font-serif">Hỷ Sự Studio</span>
      </router-link>

      <!-- Desktop Navigation -->
      <nav class="desktop-nav">
        <router-link to="/trang-chu" class="nav-link active">Trang chủ</router-link>
        <a href="#gioi-thieu" class="nav-link">Giới thiệu</a>
        <a href="#albums" class="nav-link">Album</a>
        <a href="#dich-vu" class="nav-link">Dịch vụ</a>
        <a href="#goi-cuoi" class="nav-link">Gói cưới</a>
        <a href="#lien-he" class="nav-link">Liên hệ</a>
      </nav>

      <!-- Right Utility Actions -->
      <div class="header-right">
        <!-- Search icon (Desktop only) -->
        <button class="icon-btn desktop-only" aria-label="Tìm kiếm" title="Tìm kiếm">
          <i class="fa-solid fa-magnifying-glass"></i>
        </button>

        <!-- Language Switcher (Desktop only) -->
        <div class="lang-switcher desktop-only">
          <span class="lang-item active">VI</span>
          <span class="lang-divider">|</span>
          <span class="lang-item">EN</span>
        </div>

        <!-- Auth State Button / Profile Dropdown -->
        <div v-if="authStore.isAuthenticated" class="user-auth-menu">
          <q-btn-dropdown
            flat
            dense
            no-caps
            class="user-dropdown-btn"
            content-class="vintage-menu-popup"
          >
            <template #label>
              <div class="user-pill">
                <span class="user-avatar">{{ userInitials }}</span>
                <span class="user-name desktop-only">{{ authStore.currentUser?.fullName || authStore.currentUser?.username }}</span>
              </div>
            </template>
            <q-list class="user-menu-list">
              <q-item clickable v-close-popup v-if="authStore.userRoles.includes('Admin')" to="/quan-tri-hy-su-studio">
                <q-item-section avatar>
                  <q-icon name="fa-solid fa-gauge" size="16px" />
                </q-item-section>
                <q-item-section>Trang Quản Trị</q-item-section>
              </q-item>
              <q-item clickable v-close-popup @click="handleLogout">
                <q-item-section avatar>
                  <q-icon name="fa-solid fa-right-from-bracket" color="negative" size="16px" />
                </q-item-section>
                <q-item-section class="text-negative">Đăng xuất</q-item-section>
              </q-item>
            </q-list>
          </q-btn-dropdown>
        </div>

        <router-link v-else to="/dang-nhap" class="header-auth-btn font-serif desktop-only">
          <i class="fa-solid fa-user"></i>
          <span>Đăng nhập</span>
        </router-link>

        <!-- Mobile Menu Toggle Button -->
        <button class="mobile-menu-btn" @click="mobileMenuOpen = !mobileMenuOpen" aria-label="Menu">
          <i class="fa-solid" :class="mobileMenuOpen ? 'fa-xmark' : 'fa-bars'"></i>
        </button>
      </div>
    </div>

    <!-- Mobile Drawer Navigation (Clean, Aesthetic & Fully Responsive) -->
    <transition name="drawer-slide">
      <div v-if="mobileMenuOpen" class="mobile-nav-drawer">
        <!-- Mobile Navigation Links -->
        <nav class="mobile-links-list">
          <router-link to="/trang-chu" class="mobile-nav-link font-serif" @click="mobileMenuOpen = false">
            <span>Trang chủ</span>
            <i class="fa-solid fa-chevron-right"></i>
          </router-link>
          <a href="#gioi-thieu" class="mobile-nav-link font-serif" @click="mobileMenuOpen = false">
            <span>Giới thiệu</span>
            <i class="fa-solid fa-chevron-right"></i>
          </a>
          <a href="#albums" class="mobile-nav-link font-serif" @click="mobileMenuOpen = false">
            <span>Album ảnh</span>
            <i class="fa-solid fa-chevron-right"></i>
          </a>
          <a href="#dich-vu" class="mobile-nav-link font-serif" @click="mobileMenuOpen = false">
            <span>Dịch vụ</span>
            <i class="fa-solid fa-chevron-right"></i>
          </a>
          <a href="#goi-cuoi" class="mobile-nav-link font-serif" @click="mobileMenuOpen = false">
            <span>Gói cưới</span>
            <i class="fa-solid fa-chevron-right"></i>
          </a>
          <a href="#lien-he" class="mobile-nav-link font-serif" @click="mobileMenuOpen = false">
            <span>Liên hệ tư vấn</span>
            <i class="fa-solid fa-chevron-right"></i>
          </a>
        </nav>

        <!-- Mobile Drawer Utilities & Auth -->
        <div class="mobile-drawer-footer">
          <!-- Language Selection -->
          <div class="mobile-lang-row">
            <span class="lang-label">Ngôn ngữ:</span>
            <div class="lang-options">
              <span class="lang-opt active">Tiếng Việt (VI)</span>
              <span class="lang-sep">·</span>
              <span class="lang-opt">English (EN)</span>
            </div>
          </div>

          <!-- Auth Actions -->
          <div class="mobile-drawer-auth">
            <router-link
              v-if="!authStore.isAuthenticated"
              to="/dang-nhap"
              class="btn-vintage full-width font-serif auth-btn-mobile"
              @click="mobileMenuOpen = false"
            >
              <i class="fa-solid fa-user"></i>
              <span>Đăng nhập / Đăng ký</span>
            </router-link>

            <div v-else class="mobile-logged-box">
              <div class="logged-user-info">
                <span class="logged-avatar">{{ userInitials }}</span>
                <div class="logged-name-group">
                  <span class="logged-title">Đã đăng nhập</span>
                  <span class="logged-name">{{ authStore.currentUser?.fullName || authStore.currentUser?.username }}</span>
                </div>
              </div>

              <router-link
                v-if="authStore.userRoles.includes('Admin')"
                to="/quan-tri-hy-su-studio"
                class="btn-vintage-outline full-width font-serif q-mb-sm"
                @click="mobileMenuOpen = false"
              >
                <i class="fa-solid fa-gauge"></i>
                <span>Trang Quản Trị</span>
              </router-link>

              <button @click="handleLogout" class="btn-logout-mobile font-serif">
                <i class="fa-solid fa-right-from-bracket"></i>
                <span>Đăng xuất</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </transition>
  </header>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import { useQuasar } from 'quasar'

const router = useRouter()
const $q = useQuasar()
const authStore = useAuthStore()
const mobileMenuOpen = ref(false)

const userInitials = computed(() => {
  const name = authStore.currentUser?.fullName || authStore.currentUser?.username || 'U'
  return name.charAt(0).toUpperCase()
})

async function handleLogout() {
  authStore.logout()
  $q.notify({
    type: 'info',
    message: 'Đã đăng xuất tài khoản',
    position: 'top',
    timeout: 2000
  })
  mobileMenuOpen.value = false
  await router.push('/dang-nhap')
}
</script>

<style lang="scss" scoped>
.app-header {
  position: sticky;
  top: 0;
  z-index: 1000;
  background-color: var(--color-paper);
  border-bottom: 1px solid var(--color-border);
  height: var(--header-height);
  display: flex;
  align-items: center;
  width: 100%;
}

.header-inner {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
}

.brand-logo {
  display: flex;
  align-items: center;
  gap: 8px;
  color: var(--color-ink);
  flex-shrink: 0;

  .flower-icon {
    flex-shrink: 0;
  }

  .brand-name {
    font-size: clamp(1.2rem, 3.5vw, 1.45rem);
    font-weight: 600;
    letter-spacing: 0.5px;
    color: var(--color-ink);
    white-space: nowrap;
  }
}

.desktop-nav {
  display: flex;
  align-items: center;
  gap: 28px;

  @media (max-width: 900px) {
    display: none;
  }

  .nav-link {
    font-size: 0.95rem;
    color: var(--color-ink-soft);
    position: relative;
    padding: 6px 0;
    transition: color 0.25s ease;

    &::after {
      content: '';
      position: absolute;
      bottom: 0;
      left: 0;
      width: 100%;
      height: 1.5px;
      background-color: var(--color-burgundy);
      transform: scaleX(0);
      transform-origin: right;
      transition: transform 0.3s cubic-bezier(0.25, 1, 0.5, 1);
    }

    &:hover,
    &.active {
      color: var(--color-burgundy);
    }

    &:hover::after,
    &.active::after {
      transform: scaleX(1);
      transform-origin: left;
    }
  }
}

.header-right {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-shrink: 0;

  .desktop-only {
    @media (max-width: 900px) {
      display: none !important;
    }
  }

  .icon-btn {
    background: none;
    border: none;
    cursor: pointer;
    color: var(--color-ink-soft);
    padding: 6px;
    display: flex;
    align-items: center;

    &:hover {
      color: var(--color-burgundy);
    }
  }

  .lang-switcher {
    display: flex;
    align-items: center;
    gap: 6px;
    font-size: 0.82rem;
    color: var(--color-muted);

    .lang-item {
      cursor: pointer;
      transition: color 0.2s;

      &.active {
        color: var(--color-ink);
        font-weight: 600;
      }

      &:hover:not(.active) {
        color: var(--color-burgundy);
      }
    }

    .lang-divider {
      color: var(--color-border);
    }
  }

  .header-auth-btn {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    font-size: 0.88rem;
    font-weight: 500;
    color: var(--color-paper-light);
    background-color: var(--color-burgundy);
    padding: 6px 14px;
    border-radius: var(--radius-xs);
    border: 1px solid var(--color-burgundy-dark);
    text-decoration: none;
    transition: all 0.25s ease;

    &:hover {
      background-color: var(--color-burgundy-dark);
      transform: translateY(-1px);
    }
  }

  .user-auth-menu {
    .user-pill {
      display: inline-flex;
      align-items: center;
      gap: 8px;
      padding: 3px 8px 3px 3px;
      background-color: var(--color-paper-light);
      border: 1px solid var(--color-border);
      border-radius: 20px;
      color: var(--color-ink);

      @media (max-width: 900px) {
        padding: 2px;
        border-radius: 50%;
      }

      .user-avatar {
        width: 26px;
        height: 26px;
        background-color: var(--color-burgundy);
        color: #FAF7F0;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 0.78rem;
        font-weight: 600;
      }

      .user-name {
        font-size: 0.85rem;
        font-weight: 500;
        max-width: 110px;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
      }
    }
  }

  .mobile-menu-btn {
    display: none;
    background: none;
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    cursor: pointer;
    color: var(--color-ink);
    padding: 6px 10px;
    font-size: 1.1rem;

    @media (max-width: 900px) {
      display: inline-flex;
      align-items: center;
      justify-content: center;
    }
  }
}

// Mobile Drawer Navigation (Luxury Editorial Slide Down)
.mobile-nav-drawer {
  position: absolute;
  top: var(--header-height);
  left: 0;
  width: 100%;
  background-color: var(--color-paper-light);
  border-bottom: 2px solid var(--color-border);
  padding: 20px 24px;
  display: flex;
  flex-direction: column;
  gap: 16px;
  box-shadow: 0 12px 28px rgba(36, 36, 33, 0.12);
  max-height: calc(100vh - var(--header-height));
  overflow-y: auto;

  .mobile-links-list {
    display: flex;
    flex-direction: column;
  }

  .mobile-nav-link {
    font-size: 1.15rem;
    font-weight: 500;
    color: var(--color-ink);
    padding: 12px 0;
    border-bottom: 1px solid var(--color-border-light);
    display: flex;
    justify-content: space-between;
    align-items: center;

    i {
      font-size: 0.8rem;
      color: var(--color-muted);
    }

    &:hover {
      color: var(--color-burgundy);
      i {
        color: var(--color-burgundy);
      }
    }
  }

  .mobile-drawer-footer {
    display: flex;
    flex-direction: column;
    gap: 16px;
    padding-top: 10px;

    .mobile-lang-row {
      display: flex;
      align-items: center;
      gap: 10px;
      font-size: 0.85rem;
      color: var(--color-muted);

      .lang-options {
        display: flex;
        align-items: center;
        gap: 6px;

        .lang-opt {
          cursor: pointer;
          &.active {
            color: var(--color-burgundy);
            font-weight: 600;
          }
        }
      }
    }

    .auth-btn-mobile {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 8px;
      padding: 12px;
      font-size: 1rem;
    }

    .mobile-logged-box {
      display: flex;
      flex-direction: column;
      gap: 10px;

      .logged-user-info {
        display: flex;
        align-items: center;
        gap: 10px;
        padding: 10px 12px;
        background-color: var(--color-paper);
        border: 1px solid var(--color-border);
        border-radius: var(--radius-xs);

        .logged-avatar {
          width: 32px;
          height: 32px;
          background-color: var(--color-burgundy);
          color: #FAF7F0;
          border-radius: 50%;
          display: flex;
          align-items: center;
          justify-content: center;
          font-weight: 600;
        }

        .logged-name-group {
          display: flex;
          flex-direction: column;

          .logged-title {
            font-size: 0.72rem;
            color: var(--color-muted);
          }

          .logged-name {
            font-size: 0.92rem;
            font-weight: 600;
            color: var(--color-ink);
          }
        }
      }

      .btn-logout-mobile {
        background: none;
        border: 1px solid rgba(142, 41, 41, 0.3);
        color: var(--color-burgundy);
        padding: 10px;
        border-radius: var(--radius-xs);
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 8px;
        font-size: 0.92rem;
        cursor: pointer;
        transition: all 0.2s;

        &:hover {
          background-color: rgba(142, 41, 41, 0.08);
        }
      }
    }
  }
}

// Drawer animation
.drawer-slide-enter-active,
.drawer-slide-leave-active {
  transition: all 0.3s ease;
}

.drawer-slide-enter-from,
.drawer-slide-leave-to {
  opacity: 0;
  transform: translateY(-8px);
}
</style>
