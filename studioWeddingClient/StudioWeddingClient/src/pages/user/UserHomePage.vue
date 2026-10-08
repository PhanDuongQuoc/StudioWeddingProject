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

      <!-- Romantic Wedding Interlude Quote Strip -->
      <section class="wedding-interlude-strip" v-reveal>
        <div class="studio-container text-center">
          <div class="wedding-wax-seal q-mb-md">囍</div>
          <p class="interlude-quote font-serif">
            "Từng khuôn hình tráng bạc mang theo nhịp thở của thời gian — Chắt chiu từng khoảnh khắc son rỗi, viết nên khúc tình ca vĩnh cửu."
          </p>
          <div class="interlude-sub font-serif">
            <span>HỶ SỰ STUDIO</span>
            <span class="dot">·</span>
            <span class="font-chinese">良緣夙締 · 百年好合</span>
            <span class="dot">·</span>
            <span>EST. 1998</span>
          </div>
        </div>
      </section>

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

.wedding-interlude-strip {
  padding: 60px 0;
  background-color: var(--color-paper-light);
  border-top: 1px dashed var(--color-border);
  border-bottom: 1px dashed var(--color-border);
  position: relative;
  overflow: hidden;

  .interlude-quote {
    font-size: clamp(1.2rem, 2.5vw, 1.55rem);
    line-height: 1.6;
    color: var(--color-ink);
    max-width: 820px;
    margin: 0 auto 16px;
    font-style: italic;
    font-weight: 500;
  }

  .interlude-sub {
    font-size: 0.85rem;
    color: var(--color-burgundy);
    letter-spacing: 2px;
    display: inline-flex;
    align-items: center;
    gap: 10px;

    .dot {
      color: var(--color-border-dark);
    }
  }
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
