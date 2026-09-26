<template>
  <section class="hero-section">
    <div class="hero-container">
      <!-- Main Hero Background & Media -->
      <div class="hero-image-wrapper">
        <img
          :src="currentImageUrl"
          :alt="currentBanner?.title || 'Hỷ Sự Studio Hong Kong'"
          class="hero-image film-photo"
        />
        <div class="hero-overlay"></div>

        <!-- Vertical Chinese Characters (Hong Kong 1989 aesthetic) -->
        <div class="hero-chinese-text font-chinese">
          <span>香</span>
          <span>港</span>
          <span class="chinese-sub">一九八九</span>
        </div>

        <!-- Main Hero Content Overlay -->
        <div class="hero-content">
          <h1 class="hero-title font-serif">
            Lưu giữ<br />
            những khoảnh khắc<br />
            đẹp nhất của bạn
          </h1>
          <p class="hero-description">
            {{ currentBanner?.description || 'Hỷ Sự Studio — Nơi kể lại câu chuyện tình yêu bằng những thước phim và bức ảnh mang đậm chất hoài niệm.' }}
          </p>
          <div class="hero-action">
            <a href="#albums" class="hero-cta-btn font-serif">
              <span>Khám phá album</span>
              <span class="arrow">→</span>
            </a>
          </div>
        </div>

        <!-- Film Timestamp ('98 10 26) -->
        <div class="film-timestamp">
          '98 10 26
        </div>

        <!-- Right Side Thumbnails Strip (matching mockup) -->
        <div class="hero-thumbnails" v-if="bannerImages.length > 1">
          <div
            v-for="(img, idx) in bannerImages"
            :key="img.imageBannerId || idx"
            class="thumb-item"
            :class="{ active: activeIndex === idx }"
            @click="activeIndex = idx"
          >
            <img :src="img.imageUrl" :alt="img.altText || 'Thumbnail'" />
          </div>
        </div>

        <!-- Bottom Slider Indicators (01 - 03) -->
        <div class="hero-pagination">
          <span class="page-current">0{{ activeIndex + 1 }}</span>
          <span class="page-divider">—</span>
          <span class="page-total">0{{ Math.max(bannerImages.length, 1) }}</span>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import type { HomeBanner, HomeImageBanner } from '@/types/home'

const props = defineProps<{
  banners?: HomeBanner[]
}>()

const activeIndex = ref(0)

const currentBanner = computed<HomeBanner | null>(() => {
  if (!props.banners || props.banners.length === 0) return null
  return props.banners[0] || null
})

const bannerImages = computed<HomeImageBanner[]>(() => {
  if (!currentBanner.value?.images || currentBanner.value.images.length === 0) {
    return [
      {
        imageBannerId: 1,
        imageUrl: 'https://images.unsplash.com/photo-1583939003579-730e3918a45a?q=80&w=1600&auto=format&fit=crop',
        altText: 'Hero 1'
      },
      {
        imageBannerId: 2,
        imageUrl: 'https://images.unsplash.com/photo-1519741497674-611481863552?q=80&w=1600&auto=format&fit=crop',
        altText: 'Hero 2'
      },
      {
        imageBannerId: 3,
        imageUrl: 'https://images.unsplash.com/photo-1511285560929-80b456fea0bc?q=80&w=1600&auto=format&fit=crop',
        altText: 'Hero 3'
      }
    ]
  }
  return currentBanner.value.images
})

const currentImageUrl = computed<string>(() => {
  return bannerImages.value[activeIndex.value]?.imageUrl || bannerImages.value[0]?.imageUrl || ''
})
</script>

<style lang="scss" scoped>
.hero-section {
  position: relative;
  width: 100%;
  background-color: var(--color-paper);
  padding: 0 0 50px;
}

.hero-container {
  width: 100%;
  max-width: 100%;
  margin: 0;
  padding: 0;
}

.hero-image-wrapper {
  position: relative;
  width: 100%;
  height: clamp(520px, 78vh, 760px);
  background-color: #1a2423;
  overflow: hidden;
  border-radius: 0;
}

.hero-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center 30%;
  display: block;
  animation: kenBurnsEffect 18s ease-in-out infinite alternate;
  transition: opacity 0.5s ease-in-out;
}

.hero-overlay {
  position: absolute;
  inset: 0;
  background: linear-gradient(
    90deg,
    rgba(24, 35, 34, 0.78) 0%,
    rgba(24, 35, 34, 0.48) 45%,
    rgba(24, 35, 34, 0.18) 100%
  );
  pointer-events: none;
}

// Vertical Chinese Text with Entrance Animation
.hero-chinese-text {
  position: absolute;
  top: clamp(24px, 4vh, 48px);
  left: clamp(24px, 4vw, 64px);
  display: flex;
  flex-direction: column;
  align-items: center;
  color: rgba(248, 244, 234, 0.85);
  font-size: 1.8rem;
  letter-spacing: 6px;
  line-height: 1.3;
  pointer-events: none;
  z-index: 2;
  animation: fadeIn 1.2s cubic-bezier(0.16, 1, 0.3, 1) forwards;

  .chinese-sub {
    font-size: 0.85rem;
    writing-mode: vertical-rl;
    letter-spacing: 4px;
    margin-top: 10px;
    color: rgba(248, 244, 234, 0.7);
  }

  @media (max-width: 600px) {
    font-size: 1.3rem;
    top: 20px;
    left: 20px;
  }
}

// Hero Content Overlay with Staggered Entrance
.hero-content {
  position: absolute;
  bottom: clamp(70px, 12vh, 100px);
  left: clamp(24px, 4vw, 64px);
  max-width: clamp(320px, 40vw, 560px);
  color: #F8F4EA;
  z-index: 2;
  animation: fadeInUp 0.9s cubic-bezier(0.16, 1, 0.3, 1) forwards;

  @media (max-width: 600px) {
    left: 20px;
    right: 20px;
    bottom: 60px;
    max-width: 100%;
  }
}

.hero-title {
  font-size: clamp(1.9rem, 3.5vw, 3.2rem);
  font-weight: 500;
  line-height: 1.15;
  color: #F8F4EA;
  margin: 0 0 16px;
  text-shadow: 0 2px 10px rgba(0,0,0,0.35);
}

.hero-description {
  font-size: clamp(0.9rem, 1.1vw, 1.05rem);
  line-height: 1.6;
  color: rgba(248, 244, 234, 0.88);
  margin-bottom: 24px;
  font-weight: 300;
}

.hero-cta-btn {
  display: inline-flex;
  align-items: center;
  gap: 12px;
  padding: 12px 28px;
  background-color: transparent;
  color: #F8F4EA;
  border: 1px solid rgba(248, 244, 234, 0.7);
  font-size: 1.05rem;
  letter-spacing: 0.5px;
  transition: all 0.3s cubic-bezier(0.25, 1, 0.5, 1);
  position: relative;
  overflow: hidden;

  .arrow {
    transition: transform 0.25s ease;
  }

  &:hover {
    background-color: var(--color-burgundy);
    border-color: var(--color-burgundy);
    color: #FFF;
    transform: translateY(-2px);
    box-shadow: 0 8px 20px rgba(142, 41, 41, 0.35);

    .arrow {
      transform: translateX(6px);
    }
  }
}

// Retro Film Timestamp with Glowing Amber Pulse
.film-timestamp {
  position: absolute;
  bottom: clamp(24px, 4vh, 40px);
  right: clamp(140px, 15vw, 220px);
  font-family: 'Courier New', Courier, monospace;
  font-size: 1.4rem;
  font-weight: 700;
  color: #E89A3C; // Amber / Orange vintage digital date
  letter-spacing: 2px;
  text-shadow: 0 0 6px rgba(232, 154, 60, 0.7);
  z-index: 2;
  animation: vintageDateGlow 2.8s ease-in-out infinite;

  @media (max-width: 900px) {
    right: 24px;
    bottom: 20px;
    font-size: 1.1rem;
  }
}

// Right Side Thumbnails Strip with Hover Transitions
.hero-thumbnails {
  position: absolute;
  top: clamp(24px, 4vh, 48px);
  right: clamp(24px, 4vw, 64px);
  display: flex;
  flex-direction: column;
  gap: 14px;
  z-index: 3;
  animation: fadeIn 1s ease forwards;

  @media (max-width: 900px) {
    display: none;
  }

  .thumb-item {
    width: clamp(80px, 7vw, 110px);
    height: clamp(110px, 10vw, 150px);
    border: 1px solid rgba(255, 255, 255, 0.4);
    cursor: pointer;
    overflow: hidden;
    opacity: 0.7;
    transition: all 0.3s cubic-bezier(0.25, 1, 0.5, 1);

    img {
      width: 100%;
      height: 100%;
      object-fit: cover;
      transition: transform 0.4s ease;
    }

    &:hover {
      opacity: 0.95;
      transform: scale(1.05) translateX(-4px);

      img {
        transform: scale(1.08);
      }
    }

    &.active {
      opacity: 1;
      border: 2px solid #F8F4EA;
      box-shadow: 0 4px 14px rgba(0, 0, 0, 0.4);
    }
  }
}

// Bottom Slider Indicators
.hero-pagination {
  position: absolute;
  bottom: clamp(20px, 3.5vh, 36px);
  left: clamp(24px, 4vw, 64px);
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 0.85rem;
  color: rgba(248, 244, 234, 0.7);
  letter-spacing: 1px;
  z-index: 2;

  .page-current {
    color: #F8F4EA;
    font-weight: 600;
  }

  @media (max-width: 600px) {
    left: 20px;
    bottom: 16px;
  }
}
</style>
