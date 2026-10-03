<template>
  <div class="service-card paper-card" @click="goToDetail">
    <div class="service-header">
      <h3 class="service-title font-serif">{{ service?.name }}</h3>
      <span class="service-price" v-if="service?.price">
        Từ {{ formatCurrency(service.price) }}
      </span>
    </div>

    <div class="service-image-wrapper">
      <img
        :src="service?.imageUrl || defaultServiceImg"
        :alt="service?.name || 'Service'"
        class="service-img film-photo"
        loading="lazy"
      />
    </div>

    <div class="service-footer">
      <p class="service-desc">{{ service?.description || 'Dịch vụ chuyên nghiệp tại Wedding Studio' }}</p>
      <i class="fa-solid fa-arrow-right service-arrow"></i>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import type { HomeService } from '@/types/home'

const props = defineProps<{
  service: HomeService
}>()

const router = useRouter()
const defaultServiceImg = 'https://images.unsplash.com/photo-1583939003579-730e3918a45a?q=80&w=800&auto=format&fit=crop'

const formatCurrency = (val: number | string | undefined) => {
  if (!val) return '0đ'
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(Number(val))
}

const goToDetail = () => {
  if (props.service?.slug) {
    void router.push(`/dich-vu/${props.service.slug}`)
  }
}
</script>

<style lang="scss" scoped>
.service-card {
  display: flex;
  flex-direction: column;
  padding: 22px;
  cursor: pointer;
  height: 100%;
  transition: transform 0.35s cubic-bezier(0.25, 1, 0.5, 1),
              box-shadow 0.35s cubic-bezier(0.25, 1, 0.5, 1),
              border-color 0.3s ease;

  &:hover {
    .service-img {
      transform: scale(1.06);
      filter: saturate(0.98) contrast(1.02);
    }

    .service-title {
      color: var(--color-burgundy);
    }

    .service-arrow {
      transform: translateX(6px);
      color: var(--color-burgundy);
    }
  }
}

.service-header {
  display: flex;
  align-items: baseline;
  justify-content: space-between;
  margin-bottom: 14px;
}

.service-title {
  font-size: 1.3rem;
  font-weight: 600;
  color: var(--color-ink);
  margin: 0;
  transition: color 0.25s ease;
}

.service-price {
  font-size: 0.88rem;
  color: var(--color-burgundy);
  font-weight: 600;
  letter-spacing: 0.5px;
}

.service-image-wrapper {
  width: 100%;
  aspect-ratio: 16 / 10;
  overflow: hidden;
  background-color: var(--color-paper-dark);
  margin-bottom: 14px;
}

.service-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
  transition: transform 0.5s cubic-bezier(0.25, 1, 0.5, 1), filter 0.4s ease;
}

.service-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  margin-top: auto;

  .service-desc {
    font-size: 0.88rem;
    color: var(--color-muted);
    line-height: 1.45;
    margin: 0;
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }

  .service-arrow {
    font-size: 1.2rem;
    color: var(--color-ink-soft);
    transition: transform 0.3s cubic-bezier(0.25, 1, 0.5, 1), color 0.25s ease;
    flex-shrink: 0;
  }
}
</style>
