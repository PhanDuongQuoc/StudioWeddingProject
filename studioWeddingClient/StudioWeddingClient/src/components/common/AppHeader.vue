<template>
  <header class="app-header">
    <div class="studio-container header-inner">
      <!-- Logo -->
      <router-link to="/trang-chu" class="brand-logo">
        <svg class="flower-icon" viewBox="0 0 24 24" width="24" height="24" fill="none" stroke="currentColor">
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
        <button class="icon-btn" aria-label="Tìm kiếm" title="Tìm kiếm">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
            <circle cx="11" cy="11" r="8"></circle>
            <line x1="21" y1="21" x2="16.65" y2="16.65"></line>
          </svg>
        </button>
        <div class="lang-switcher">
          <span class="lang-item active">VI</span>
          <span class="lang-divider">|</span>
          <span class="lang-item">EN</span>
        </div>

        <!-- Mobile Menu Toggle Button -->
        <button class="mobile-menu-btn" @click="mobileMenuOpen = !mobileMenuOpen" aria-label="Mở menu">
          <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <line x1="3" y1="12" x2="21" y2="12"></line>
            <line x1="3" y1="6" x2="21" y2="6"></line>
            <line x1="3" y1="18" x2="21" y2="18"></line>
          </svg>
        </button>
      </div>
    </div>

    <!-- Mobile Drawer Navigation -->
    <div v-if="mobileMenuOpen" class="mobile-nav-drawer">
      <router-link to="/trang-chu" class="mobile-nav-link" @click="mobileMenuOpen = false">Trang chủ</router-link>
      <a href="#gioi-thieu" class="mobile-nav-link" @click="mobileMenuOpen = false">Giới thiệu</a>
      <a href="#albums" class="mobile-nav-link" @click="mobileMenuOpen = false">Album</a>
      <a href="#dich-vu" class="mobile-nav-link" @click="mobileMenuOpen = false">Dịch vụ</a>
      <a href="#goi-cuoi" class="mobile-nav-link" @click="mobileMenuOpen = false">Gói cưới</a>
      <a href="#lien-he" class="mobile-nav-link" @click="mobileMenuOpen = false">Liên hệ</a>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const mobileMenuOpen = ref(false)
</script>

<style lang="scss" scoped>
.app-header {
  position: sticky;
  top: 0;
  z-index: 100;
  background-color: var(--color-paper);
  border-bottom: 1px solid var(--color-border);
  height: var(--header-height);
  display: flex;
  align-items: center;
}

.header-inner {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.brand-logo {
  display: flex;
  align-items: center;
  gap: 8px;
  color: var(--color-ink);

  .flower-icon {
    flex-shrink: 0;
  }

  .brand-name {
    font-size: 1.45rem;
    font-weight: 600;
    letter-spacing: 0.5px;
    color: var(--color-ink);
  }
}

.desktop-nav {
  display: flex;
  align-items: center;
  gap: 32px;

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
  gap: 16px;

  .icon-btn {
    background: none;
    border: none;
    cursor: pointer;
    color: var(--color-ink-soft);
    padding: 4px;
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
    font-size: 0.85rem;
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

  .mobile-menu-btn {
    display: none;
    background: none;
    border: none;
    cursor: pointer;
    color: var(--color-ink);

    @media (max-width: 900px) {
      display: flex;
    }
  }
}

.mobile-nav-drawer {
  position: absolute;
  top: var(--header-height);
  left: 0;
  width: 100%;
  background-color: var(--color-paper-light);
  border-bottom: 1px solid var(--color-border);
  padding: 16px 24px;
  display: flex;
  flex-direction: column;
  gap: 12px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);

  .mobile-nav-link {
    font-size: 1rem;
    color: var(--color-ink);
    padding: 8px 0;
    border-bottom: 1px solid var(--color-border-light);

    &:last-child {
      border-bottom: none;
    }

    &:hover {
      color: var(--color-burgundy);
    }
  }
}
</style>
