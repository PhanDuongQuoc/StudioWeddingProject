<template>
  <div class="user-home-page">
    <!-- Unified Loading State -->
    <VintageLoading v-if="isLoading" text="Đang tải dữ liệu Hỷ Sự Studio..." />

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
import VintageLoading from '@/components/common/VintageLoading.vue'
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
</style>
