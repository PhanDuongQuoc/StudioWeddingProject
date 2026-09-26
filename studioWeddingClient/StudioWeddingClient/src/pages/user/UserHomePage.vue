<template>
  <div class="user-home-page">
    <!-- Loading State -->
    <div v-if="isLoading" class="home-loading studio-container">
      <div class="loading-spinner"></div>
      <p class="font-serif">Đang tải dữ liệu Hỷ Sự Studio...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="home-error studio-container">
      <h3 class="font-serif">Không thể kết nối đến máy chủ</h3>
      <p>{{ error }}</p>
      <button @click="fetchHomeData" class="btn-vintage">Thử lại</button>
    </div>

    <!-- Main Content State -->
    <main v-else class="home-main">
      <!-- 1. Hero Banner Slider -->
      <HeroSection :banners="homeData?.banners || []" />

      <!-- 2. Featured Albums Section -->
      <FeaturedAlbumsSection :albums="homeData?.featuredAlbums || []" />

      <!-- 3. Services Section -->
      <ServicesSection :services="homeData?.services || []" />

      <!-- 4. Packages Section -->
      <PackagesSection :packages="homeData?.packages || []" />

      <!-- 5. Customer Testimonials / Reviews -->
      <ReviewsSection :reviews="homeData?.reviews || []" />

      <!-- 6. Contact & Consultation Form -->
      <ContactCtaSection />
    </main>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useHome } from '@/composables/useHome'

// Components
import HeroSection from '@/components/user/home/HeroSection.vue'
import FeaturedAlbumsSection from '@/components/user/home/FeaturedAlbumsSection.vue'
import ServicesSection from '@/components/user/home/ServicesSection.vue'
import PackagesSection from '@/components/user/home/PackagesSection.vue'
import ReviewsSection from '@/components/user/home/ReviewsSection.vue'
import ContactCtaSection from '@/components/user/home/ContactCtaSection.vue'

const { homeData, isLoading, error, fetchHomeData } = useHome()

onMounted(async () => {
  await fetchHomeData()
})
</script>

<style lang="scss" scoped>
.user-home-page {
  min-height: 100vh;
  background-color: var(--color-paper);
}

.home-loading,
.home-error {
  min-height: 60vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
  gap: 16px;

  p {
    font-size: 1.1rem;
    color: var(--color-muted);
  }
}

.loading-spinner {
  width: 42px;
  height: 42px;
  border: 3px solid var(--color-border);
  border-top-color: var(--color-burgundy);
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}
</style>
