<template>
  <div class="vintage-loading-container studio-container" :style="{ minHeight }">
    <div class="vintage-spinner-wrapper">
      <div class="vintage-spinner-ring outer-ring"></div>
      <div class="vintage-spinner-ring inner-ring"></div>
      <span class="center-seal font-chinese">囍</span>
    </div>
    <p class="loading-message font-serif">{{ text }}</p>
    <span v-if="subText" class="loading-submessage font-serif">{{ subText }}</span>
  </div>
</template>

<script setup lang="ts">
withDefaults(
  defineProps<{
    text?: string
    subText?: string
    minHeight?: string
  }>(),
  {
    text: 'Đang tải dữ liệu...',
    subText: 'Hỷ Sự Wedding Studio',
    minHeight: '55vh',
  }
)
</script>

<style scoped lang="scss">
.vintage-loading-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
  padding: 40px 20px;
  width: 100%;

  .vintage-spinner-wrapper {
    position: relative;
    width: 64px;
    height: 64px;
    display: flex;
    align-items: center;
    justify-content: center;
    margin-bottom: 18px;

    .vintage-spinner-ring {
      position: absolute;
      border-radius: 50%;
      border: 2px solid transparent;

      &.outer-ring {
        inset: 0;
        border-top-color: var(--color-burgundy);
        border-bottom-color: var(--color-border);
        animation: vintageSpin 1.2s cubic-bezier(0.5, 0, 0.5, 1) infinite;
      }

      &.inner-ring {
        inset: 6px;
        border-right-color: var(--color-burgundy-soft, #A8443E);
        border-left-color: transparent;
        animation: vintageSpinReverse 0.9s linear infinite;
      }
    }

    .center-seal {
      font-size: 1.15rem;
      color: var(--color-burgundy);
      user-select: none;
      animation: sealPulse 2s ease-in-out infinite;
    }
  }

  .loading-message {
    font-size: 1.05rem;
    color: var(--color-ink);
    margin: 0 0 6px 0;
    letter-spacing: 0.5px;
    font-weight: 500;
  }

  .loading-submessage {
    font-size: 0.78rem;
    color: var(--color-muted);
    letter-spacing: 1.5px;
    text-transform: uppercase;
  }
}

@keyframes vintageSpin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

@keyframes vintageSpinReverse {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(-360deg);
  }
}

@keyframes sealPulse {
  0%, 100% {
    opacity: 0.75;
    transform: scale(0.95);
  }
  50% {
    opacity: 1;
    transform: scale(1.05);
  }
}
</style>
