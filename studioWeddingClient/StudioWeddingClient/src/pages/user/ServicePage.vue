<template>
  <div class="service-page">
    <!-- 1. Unified Vintage Loading State -->
    <VintageLoading v-if="isLoading" text="Đang tải danh sách dịch vụ..." />

    <!-- 2. Error State -->
    <div v-else-if="error" class="service-error-state studio-container">
      <div class="error-box paper-card">
        <i class="fa-solid fa-circle-exclamation error-icon"></i>
        <h2 class="error-title font-serif">Không thể tải danh sách Dịch Vụ</h2>
        <p class="error-desc">{{ error }}</p>
        <button @click="fetchServices" class="btn-vintage font-serif">
          <i class="fa-solid fa-rotate-right q-mr-xs"></i>
          <span>Thử lại</span>
        </button>
      </div>
    </div>

    <!-- 3. Main Services Content (Exact Mockup Match) -->
    <main v-else class="service-main-content studio-container">
      <!-- Breadcrumb Bar -->
      <nav class="breadcrumb-trail font-serif" aria-label="Breadcrumb">
        <router-link to="/trang-chu">Trang chủ</router-link>
        <span class="sep">/</span>
        <span class="current-crumb">Dịch vụ</span>
      </nav>

      <!-- Page Heading -->
      <div class="page-title-row">
        <h1 class="page-main-title font-serif">Dịch vụ của chúng tôi</h1>
      </div>

      <!-- Empty State -->
      <div v-if="services.length === 0" class="empty-service-box">
        <i class="fa-solid fa-camera empty-icon"></i>
        <h3 class="empty-title font-serif">Chưa có dịch vụ nào</h3>
        <p class="empty-desc font-serif">Hiện tại chưa có dịch vụ nào được cập nhật.</p>
      </div>

      <!-- 3-Column Service Grid (Mockup Match) -->
      <div v-else class="services-catalog-grid">
        <article
          v-for="service in services"
          :key="service.serviceId"
          class="service-card-mockup"
        >
          <router-link :to="`/dich-vu/${service.slug}`" class="card-inner-link">
            <!-- Service Image Container -->
            <div class="card-image-box">
              <img
                :src="service.imageUrl || defaultServiceCover"
                :alt="service.name"
                class="service-img film-photo"
                loading="lazy"
              />
            </div>

            <!-- Service Info Below Image -->
            <div class="card-content-box">
              <div class="title-with-arrow">
                <h2 class="service-title font-serif">{{ service.name }}</h2>
                <i class="fa-solid fa-arrow-right-long arrow-icon"></i>
              </div>

              <p class="service-desc font-serif" v-if="service.description">
                {{ truncateText(service.description, 90) }}
              </p>
            </div>
          </router-link>
        </article>
      </div>

      <!-- Pagination -->
      <div class="pagination-row" v-if="totalPages > 1">
        <button
          class="page-arrow-btn"
          :disabled="page <= 1"
          @click="setPage(page - 1)"
          title="Trang trước"
        >
          <i class="fa-solid fa-chevron-left"></i>
        </button>

        <span class="page-indicator font-serif">
          {{ String(page).padStart(2, '0') }} / {{ String(totalPages).padStart(2, '0') }}
        </span>

        <button
          class="page-arrow-btn"
          :disabled="page >= totalPages"
          @click="setPage(page + 1)"
          title="Trang sau"
        >
          <i class="fa-solid fa-chevron-right"></i>
        </button>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useService } from '@/composables/useService'
import VintageLoading from '@/components/common/VintageLoading.vue'

const {
  services,
  page,
  totalPages,
  isLoading,
  error,
  fetchServices,
  setPage,
} = useService()

const defaultServiceCover = 'https://images.unsplash.com/photo-1511285560929-80b456fea0bc?auto=format&fit=crop&w=1200&q=80'

function truncateText(text?: string | null, maxLen = 90): string {
  if (!text) return ''
  if (text.length <= maxLen) return text
  return text.slice(0, maxLen).trim() + '...'
}

onMounted(async () => {
  await fetchServices()
})
</script>

<style lang="scss" scoped>
.service-page {
  min-height: 100vh;
  background-color: var(--color-paper);
  color: var(--color-ink);
  padding: 24px 0 80px;
}

.breadcrumb-trail {
  margin-bottom: 24px;
}

.page-title-row {
  margin-bottom: 36px;

  .page-main-title {
    font-size: clamp(2rem, 3.5vw, 2.5rem);
    font-weight: 600;
    color: var(--color-ink);
    margin: 0;
    letter-spacing: -0.01em;
  }
}

// 3-Column x 2-Row Grid Layout (Mockup Match)
.services-catalog-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 28px;

  @media (max-width: 1000px) {
    grid-template-columns: repeat(2, 1fr);
    gap: 22px;
  }

  @media (max-width: 600px) {
    grid-template-columns: 1fr;
    gap: 20px;
  }
}

// Service Card (Exact Mockup Match)
.service-card-mockup {
  background-color: var(--color-paper-light);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-xs);
  overflow: hidden;
  transition: border-color 0.25s ease;

  .card-inner-link {
    display: flex;
    flex-direction: column;
    text-decoration: none;
    height: 100%;
  }

  .card-image-box {
    width: 100%;
    aspect-ratio: 16 / 10;
    overflow: hidden;
    background-color: var(--color-paper-dark);
    border-bottom: 1px solid var(--color-border);

    .service-img {
      width: 100%;
      height: 100%;
      object-fit: cover;
      display: block;
      transition: transform 0.4s ease;
    }
  }

  .card-content-box {
    padding: 18px 20px;
    display: flex;
    flex-direction: column;
    gap: 6px;
    flex-grow: 1;

    .title-with-arrow {
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 12px;

      .service-title {
        font-size: 1.22rem;
        font-weight: 600;
        color: var(--color-ink);
        margin: 0;
        line-height: 1.35;
        transition: color 0.2s;
      }

      .arrow-icon {
        font-size: 0.95rem;
        color: var(--color-ink-soft);
        transition: transform 0.25s ease, color 0.2s;
        flex-shrink: 0;
      }
    }

    .service-desc {
      font-size: 0.9rem;
      line-height: 1.55;
      color: var(--color-muted);
      margin: 0;
    }
  }

  &:hover {
    border-color: var(--color-burgundy);

    .card-image-box .service-img {
      transform: scale(1.035);
    }

    .title-with-arrow {
      .service-title {
        color: var(--color-burgundy);
      }

      .arrow-icon {
        color: var(--color-burgundy);
        transform: translateX(4px);
      }
    }
  }
}

// Empty State
.empty-service-box {
  padding: 60px 20px;
  text-align: center;
  background-color: var(--color-paper-light);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-xs);

  .empty-icon {
    font-size: 2.5rem;
    color: var(--color-muted);
    margin-bottom: 12px;
  }

  .empty-title {
    font-size: 1.35rem;
    margin: 0 0 8px;
  }

  .empty-desc {
    color: var(--color-muted);
    font-size: 0.95rem;
  }
}

// Pagination
.pagination-row {
  margin-top: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 16px;

  .page-arrow-btn {
    background: var(--color-paper-light);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    width: 36px;
    height: 36px;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    color: var(--color-ink);
    cursor: pointer;
    transition: all 0.2s;

    &:hover:not(:disabled) {
      border-color: var(--color-burgundy);
      color: var(--color-burgundy);
    }

    &:disabled {
      opacity: 0.35;
      cursor: not-allowed;
    }
  }

  .page-indicator {
    font-size: 0.92rem;
    color: var(--color-ink-soft);
    font-weight: 500;
  }
}

// Error State
.service-error-state {
  min-height: 50vh;
  display: flex;
  align-items: center;
  justify-content: center;

  .error-box {
    max-width: 480px;
    padding: 36px;
    text-align: center;
    background-color: var(--color-paper-light);

    .error-icon {
      font-size: 2.5rem;
      color: var(--color-burgundy);
      margin-bottom: 12px;
    }

    .error-title {
      font-size: 1.45rem;
      margin: 0 0 8px;
    }

    .error-desc {
      color: var(--color-muted);
      margin: 0 0 20px;
    }
  }
}
</style>
