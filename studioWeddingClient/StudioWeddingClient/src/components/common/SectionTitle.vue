<template>
  <div class="section-title-wrapper" :class="{ 'text-center': align === 'center' }">
    <div class="title-inner">
      <span v-if="tag" class="section-tag">{{ tag }}</span>
      <h2 class="section-title font-serif">{{ title }}</h2>
      <p v-if="subtitle" class="section-subtitle">{{ subtitle }}</p>
    </div>
    <slot name="action"></slot>
  </div>
</template>

<script setup lang="ts">
defineProps({
  title: {
    type: String,
    required: true
  },
  subtitle: {
    type: String,
    default: ''
  },
  tag: {
    type: String,
    default: ''
  },
  align: {
    type: String,
    default: 'left' // 'left' | 'center'
  }
})
</script>

<style lang="scss" scoped>
.section-title-wrapper {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  margin-bottom: 32px;
  gap: 20px;

  &.text-center {
    flex-direction: column;
    align-items: center;
    text-align: center;
  }
}

.section-tag {
  display: block;
  font-size: 0.78rem;
  letter-spacing: 2px;
  text-transform: uppercase;
  color: var(--color-burgundy);
  margin-bottom: 6px;
  font-weight: 600;
}

.section-title {
  font-size: 2.2rem;
  font-weight: 600;
  color: var(--color-ink);
  line-height: 1.2;
  margin: 0;

  @media (max-width: 768px) {
    font-size: 1.75rem;
  }
}

.section-subtitle {
  font-size: 0.95rem;
  color: var(--color-muted);
  margin-top: 6px;
  margin-bottom: 0;
}
</style>
