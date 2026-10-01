<template>
  <div class="review-card paper-card">
    <div class="review-stars">
      <i
        v-for="star in 5"
        :key="star"
        class="star"
        :class="star <= (review?.rating || 5) ? 'fa-solid fa-star active' : 'fa-regular fa-star'"
      ></i>
    </div>

    <h4 class="review-title font-serif" v-if="review?.title">{{ review.title }}</h4>
    <p class="review-comment">“{{ review?.comment }}”</p>

    <div class="review-footer">
      <div class="customer-name font-serif">{{ review?.customerName }}</div>
      <div class="review-date" v-if="review?.createdAt">{{ formatDate(review.createdAt) }}</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { HomeReview } from '@/types/home'

defineProps<{
  review: HomeReview
}>()

const formatDate = (dateStr?: string | null) => {
  if (!dateStr) return ''
  const date = new Date(dateStr)
  return new Intl.DateTimeFormat('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' }).format(date)
}
</script>

<style lang="scss" scoped>
.review-card {
  padding: 26px;
  display: flex;
  flex-direction: column;
  height: 100%;
  transition: transform 0.35s cubic-bezier(0.25, 1, 0.5, 1),
              box-shadow 0.35s cubic-bezier(0.25, 1, 0.5, 1),
              border-color 0.3s ease;

  &:hover {
    .review-title {
      color: var(--color-burgundy);
    }

    .star.active {
      text-shadow: 0 0 8px rgba(217, 119, 6, 0.5);
      transform: scale(1.08);
    }
  }
}

.review-stars {
  display: flex;
  gap: 4px;
  margin-bottom: 12px;

  .star {
    font-size: 1.15rem;
    color: var(--color-border);
    transition: transform 0.2s ease, text-shadow 0.2s ease;

    &.active {
      color: #D97706;
    }
  }
}

.review-title {
  font-size: 1.18rem;
  font-weight: 600;
  color: var(--color-ink);
  margin: 0 0 8px;
  transition: color 0.25s ease;
}

.review-comment {
  font-size: 0.94rem;
  line-height: 1.65;
  color: var(--color-ink-soft);
  font-style: italic;
  margin: 0 0 20px;
  flex-grow: 1;
}

.review-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-top: 1px solid var(--color-border-light);
  padding-top: 14px;
  margin-top: auto;

  .customer-name {
    font-size: 1.05rem;
    font-weight: 600;
    color: var(--color-burgundy);
  }

  .review-date {
    font-size: 0.82rem;
    color: var(--color-muted);
  }
}
</style>
