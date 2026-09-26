<template>
  <section class="reviews-section" v-if="reviews && reviews.length > 0">
    <div class="studio-container">
      <SectionTitle
        title="Khách hàng nói về chúng tôi"
        subtitle="Những tình cảm và sự tin tưởng là món quà quý giá nhất dành cho Studio"
        tag="Testimonials"
        align="center"
      />

      <div class="reviews-grid">
        <ReviewCard
          v-for="(review, idx) in reviews"
          :key="review.reviewId"
          :review="review"
          class="animate-fade-in-up"
          :class="`stagger-${(idx % 3) + 1}`"
        />
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import type { HomeReview } from '@/types/home'
import SectionTitle from '@/components/common/SectionTitle.vue'
import ReviewCard from '@/components/user/home/ReviewCard.vue'

defineProps<{
  reviews?: HomeReview[]
}>()
</script>

<style lang="scss" scoped>
.reviews-section {
  padding: 70px 0;
  background-color: var(--color-paper-light);
  border-top: 1px solid var(--color-border);
}

.reviews-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 24px;
  margin-top: 36px;

  @media (max-width: 900px) {
    grid-template-columns: repeat(2, 1fr);
  }

  @media (max-width: 600px) {
    grid-template-columns: 1fr;
  }
}
</style>
