<template>
  <div class="package-card paper-card" :class="{ 'is-featured': isFeatured }" @click="goToDetail">
    <!-- Featured Badge -->
    <div v-if="isFeatured" class="featured-badge">
      Phổ biến
    </div>

    <!-- Top Image -->
    <div class="package-img-wrapper">
      <img
        :src="pkg?.imageUrl || defaultPkgImg"
        :alt="pkg?.name || 'Package'"
        class="package-img film-photo"
        loading="lazy"
      />
    </div>

    <div class="package-body">
      <h3 class="package-name font-serif">{{ pkg?.name }}</h3>
      <div class="package-price font-serif">{{ formatCurrency(pkg?.price) }}</div>

      <!-- Feature List -->
      <ul class="package-features">
        <li v-for="(feature, idx) in featuresList" :key="idx">
          <i class="fa-solid fa-check bullet-icon"></i>
          <span>{{ feature }}</span>
        </li>
      </ul>

      <!-- Action Button -->
      <div class="package-action">
        <button
          class="package-btn font-serif"
          :class="isFeatured ? 'btn-vintage' : 'btn-vintage-outline'"
          @click.stop="goToDetail"
        >
          <span>Xem chi tiết & Đặt ngay</span>
          <i class="fa-solid fa-arrow-right q-ml-xs"></i>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import type { HomePackage } from '@/types/home'

const props = defineProps<{
  pkg: HomePackage
  isFeatured?: boolean
}>()

const router = useRouter()
const defaultPkgImg = 'https://images.unsplash.com/photo-1519741497674-611481863552?q=80&w=800&auto=format&fit=crop'

const goToDetail = () => {
  if (props.pkg?.slug) {
    void router.push(`/goi-cuoi/${props.pkg.slug}`)
  }
}

const formatCurrency = (val: number | string | undefined) => {
  if (!val) return '0đ'
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(Number(val))
}

const featuresList = computed<string[]>(() => {
  if (props.pkg?.description) {
    return props.pkg.description
      .split('\n')
      .map((s: string) => s.replace(/^[•\-*]\s*/, '').trim())
      .filter(Boolean)
  }
  if (props.pkg?.includedServices && props.pkg.includedServices.length > 0) {
    return props.pkg.includedServices
  }
  return [
    'Chụp ảnh chuyên nghiệp',
    'Makeup & làm tóc cô dâu',
    'Trang phục váy cưới & vest cao cấp',
    'Album ảnh photobook cao cấp'
  ]
})
</script>

<style lang="scss" scoped>
.package-card {
  position: relative;
  display: flex;
  flex-direction: column;
  padding: 0;
  overflow: hidden;
  height: 100%;
  transition: transform 0.35s cubic-bezier(0.25, 1, 0.5, 1),
              box-shadow 0.35s cubic-bezier(0.25, 1, 0.5, 1),
              border-color 0.3s ease;

  &:hover {
    transform: translateY(-6px);
    box-shadow: 0 16px 32px rgba(36, 36, 33, 0.1);

    .package-img {
      transform: scale(1.05);
      filter: saturate(0.98) contrast(1.02);
    }

    .package-name {
      color: var(--color-burgundy);
    }
  }

  &.is-featured {
    border: 2px solid var(--color-burgundy);
    background-color: var(--color-paper-light);
    box-shadow: 0 8px 24px rgba(142, 41, 41, 0.12);

    @media (min-width: 900px) {
      transform: translateY(-8px);

      &:hover {
        transform: translateY(-14px);
        box-shadow: 0 20px 40px rgba(142, 41, 41, 0.2);
      }
    }
  }
}

.featured-badge {
  position: absolute;
  top: 14px;
  right: 14px;
  background-color: var(--color-burgundy);
  color: #F8F4EA;
  font-size: 0.75rem;
  letter-spacing: 1.2px;
  text-transform: uppercase;
  padding: 5px 12px;
  font-weight: 600;
  z-index: 2;
  border-radius: var(--radius-xs);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.25);
}

.package-img-wrapper {
  width: 100%;
  aspect-ratio: 16 / 10;
  overflow: hidden;
  background-color: var(--color-paper-dark);
}

.package-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
  transition: transform 0.5s cubic-bezier(0.25, 1, 0.5, 1), filter 0.4s ease;
}

.package-body {
  padding: 26px;
  display: flex;
  flex-direction: column;
  flex-grow: 1;
}

.package-name {
  font-size: 1.45rem;
  font-weight: 600;
  color: var(--color-ink);
  margin: 0 0 8px;
  transition: color 0.25s ease;
}

.package-price {
  font-size: 1.65rem;
  font-weight: 600;
  color: var(--color-burgundy);
  margin-bottom: 20px;
  letter-spacing: 0.5px;
}

.package-features {
  list-style: none;
  padding: 0;
  margin: 0 0 24px;
  display: flex;
  flex-direction: column;
  gap: 12px;

  li {
    font-size: 0.92rem;
    color: var(--color-ink-soft);
    display: flex;
    align-items: baseline;
    gap: 8px;
    line-height: 1.45;

    .bullet-icon {
      color: var(--color-burgundy);
      font-size: 0.8rem;
      margin-top: 2px;
    }
  }
}

.package-action {
  margin-top: auto;

  .package-btn {
    width: 100%;
    padding: 12px 0;
    cursor: pointer;
    font-size: 0.98rem;
    letter-spacing: 0.5px;
  }
}
</style>
