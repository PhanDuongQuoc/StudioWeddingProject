<template>
  <div class="floating-contact-widget">
    <!-- 1. Chatbot AI Icon -->
    <div class="floating-item chatbot-btn" title="Trợ lý AI Hỷ Sự">
      <div class="icon-circle">
        <i class="fa-solid fa-robot"></i>
        <span class="online-status-dot"></span>
      </div>
      <span class="floating-tooltip font-serif">Chatbot AI</span>
    </div>

    <!-- 2. Zalo Icon -->
    <a
      href="https://zalo.me"
      target="_blank"
      rel="noopener noreferrer"
      class="floating-item zalo-btn"
      title="Chat qua Zalo"
    >
      <div class="icon-circle">
        <svg viewBox="0 0 48 48" class="zalo-svg" width="22" height="22" fill="currentColor">
          <path
            d="M24 4C12.95 4 4 12.06 4 22c0 5.6 2.89 10.6 7.42 13.98-.38 2.83-1.68 6.42-1.74 6.57-.16.42.06.88.48 1 .15.04.3.05.45.02 3.86-.75 8.16-2.58 10.36-3.64C22.04 40.28 23.01 40.33 24 40.33c11.05 0 20-8.06 20-18S35.05 4 24 4z"
          />
        </svg>
      </div>
      <span class="floating-tooltip font-serif">Zalo Studio</span>
    </a>

    <!-- 3. Facebook Messenger / Facebook Icon -->
    <a
      href="https://facebook.com"
      target="_blank"
      rel="noopener noreferrer"
      class="floating-item facebook-btn"
      title="Facebook Hỷ Sự Studio"
    >
      <div class="icon-circle">
        <i class="fa-brands fa-facebook-f"></i>
      </div>
      <span class="floating-tooltip font-serif">Facebook</span>
    </a>

    <!-- 4. Instagram Icon -->
    <a
      href="https://instagram.com"
      target="_blank"
      rel="noopener noreferrer"
      class="floating-item instagram-btn"
      title="Instagram Hỷ Sự Studio"
    >
      <div class="icon-circle">
        <i class="fa-brands fa-instagram"></i>
      </div>
      <span class="floating-tooltip font-serif">Instagram</span>
    </a>
  </div>
</template>

<script setup lang="ts">
// Currently visual only, ready for future chat modal/integrations
</script>

<style lang="scss" scoped>
.floating-contact-widget {
  position: fixed;
  right: 24px;
  bottom: 28px;
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 12px;
  z-index: 999;
  pointer-events: auto;

  @media (max-width: 600px) {
    right: 14px;
    bottom: 20px;
    gap: 10px;
  }
}

.floating-item {
  position: relative;
  display: flex;
  align-items: center;
  text-decoration: none;
  cursor: pointer;
  outline: none;
  user-select: none;

  &:hover {
    .icon-circle {
      transform: translateY(-3px) scale(1.08);
      box-shadow: 0 8px 22px rgba(36, 36, 33, 0.22);
    }

    .floating-tooltip {
      opacity: 1;
      visibility: visible;
      transform: translateX(0);
    }
  }

  &:active .icon-circle {
    transform: translateY(0) scale(0.96);
  }
}

.icon-circle {
  position: relative;
  width: 44px;
  height: 44px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.15rem;
  transition: all 0.3s cubic-bezier(0.25, 1, 0.5, 1);
  box-shadow: 0 4px 14px rgba(36, 36, 33, 0.14);
  border: 1px solid var(--color-border);
  background-color: var(--color-paper-light);
  color: var(--color-ink);

  @media (max-width: 600px) {
    width: 40px;
    height: 40px;
    font-size: 1rem;
  }
}

// 1. Chatbot button style (Burgundy luxury highlight)
.chatbot-btn {
  .icon-circle {
    background-color: var(--color-burgundy);
    color: #FAF7F0;
    border-color: var(--color-burgundy-dark);
    box-shadow: 0 6px 18px rgba(142, 41, 41, 0.3);
  }

  .online-status-dot {
    position: absolute;
    top: 2px;
    right: 2px;
    width: 10px;
    height: 10px;
    border-radius: 50%;
    background-color: #2ECC71;
    border: 2px solid #FAF7F0;
    box-shadow: 0 0 6px rgba(46, 204, 113, 0.8);
    animation: statusPulse 2s infinite ease-in-out;
  }
}

// 2. Zalo button style
.zalo-btn {
  .icon-circle {
    background-color: #FAF7F0;
    color: #0068FF;
    border-color: #0068FF33;

    &:hover {
      background-color: #0068FF;
      color: #FFFFFF;
      border-color: #0068FF;
    }
  }

  .zalo-svg {
    fill: currentColor;
  }
}

// 3. Facebook button style
.facebook-btn {
  .icon-circle {
    background-color: #FAF7F0;
    color: #1877F2;
    border-color: #1877F233;

    &:hover {
      background-color: #1877F2;
      color: #FFFFFF;
      border-color: #1877F2;
    }
  }
}

// 4. Instagram button style
.instagram-btn {
  .icon-circle {
    background-color: #FAF7F0;
    color: #E1306C;
    border-color: #E1306C33;

    &:hover {
      background: linear-gradient(45deg, #f09433 0%, #e6683c 25%, #dc2743 50%, #cc2366 75%, #bc1888 100%);
      color: #FFFFFF;
      border-color: transparent;
    }
  }
}

// Tooltip analog label on the left of each icon
.floating-tooltip {
  position: absolute;
  right: calc(100% + 12px);
  white-space: nowrap;
  background-color: var(--color-ink);
  color: #FAF7F0;
  font-size: 0.82rem;
  letter-spacing: 0.5px;
  padding: 5px 10px;
  border-radius: var(--radius-xs);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  opacity: 0;
  visibility: hidden;
  transform: translateX(8px);
  transition: all 0.25s cubic-bezier(0.25, 1, 0.5, 1);
  pointer-events: none;

  &::after {
    content: '';
    position: absolute;
    top: 50%;
    right: -4px;
    transform: translateY(-50%);
    border-width: 4px 0 4px 4px;
    border-style: solid;
    border-color: transparent transparent transparent var(--color-ink);
  }

  @media (max-width: 600px) {
    display: none; // Hide tooltips on mobile touch screens
  }
}

@keyframes statusPulse {
  0%, 100% {
    transform: scale(1);
    opacity: 1;
  }
  50% {
    transform: scale(1.2);
    opacity: 0.8;
  }
}
</style>
