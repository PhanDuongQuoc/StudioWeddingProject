<template>
  <div class="package-page">
    <!-- 1. Unified Vintage Loading State -->
    <VintageLoading v-if="isLoading" text="Đang tải danh sách gói cưới..." />

    <!-- 2. Error State -->
    <div v-else-if="error" class="package-error-state studio-container">
      <div class="error-box paper-card">
        <i class="fa-solid fa-circle-exclamation error-icon"></i>
        <h2 class="error-title font-serif">Không thể tải danh sách Gói Cưới</h2>
        <p class="error-desc">{{ error }}</p>
        <button @click="fetchPackages" class="btn-vintage font-serif">
          <i class="fa-solid fa-rotate-right q-mr-xs"></i>
          <span>Thử lại</span>
        </button>
      </div>
    </div>

    <!-- 3. Main Packages Content (Exact Mockup Match) -->
    <main v-else class="package-main-content studio-container">
      <!-- Breadcrumb Bar -->
      <nav class="breadcrumb-trail font-serif" aria-label="Breadcrumb">
        <router-link to="/trang-chu">Trang chủ</router-link>
        <span class="sep">/</span>
        <span class="current-crumb">Gói cưới</span>
      </nav>

      <!-- Page Heading -->
      <div class="page-title-row">
        <h1 class="page-main-title font-serif">Gói cưới</h1>
      </div>

      <!-- Empty State -->
      <div v-if="packages.length === 0" class="empty-package-box">
        <i class="fa-solid fa-box-open empty-icon"></i>
        <h3 class="empty-title font-serif">Chưa có gói cưới nào</h3>
        <p class="empty-desc font-serif">Hiện tại chưa có gói cưới nào được cập nhật.</p>
      </div>

      <!-- 3-Column Package Cards Grid (Exact Mockup Match) -->
      <div v-else class="packages-catalog-grid">
        <article
          v-for="pkg in packages"
          :key="pkg.packageId"
          class="package-card-mockup"
          :class="{ 'is-featured': pkg.isPopular }"
        >
          <!-- Top 'Phổ biến' badge for featured package -->
          <div v-if="pkg.isPopular" class="featured-badge font-serif">
            Phổ biến
          </div>

          <!-- Top Image -->
          <router-link :to="`/goi-cuoi/${pkg.slug}`" class="card-image-wrap">
            <img
              :src="pkg.imageUrl || defaultPackageCover"
              :alt="pkg.name"
              class="pkg-img film-photo"
              loading="lazy"
            />
          </router-link>

          <!-- Card Content -->
          <div class="card-body">
            <h2 class="pkg-title font-serif">
              <router-link :to="`/goi-cuoi/${pkg.slug}`">
                {{ pkg.name }}
              </router-link>
            </h2>

            <div class="pkg-price font-serif">
              {{ formatCurrency(pkg.price) }}
            </div>

            <!-- Checklist of benefits -->
            <ul class="pkg-checklist font-serif">
              <li
                v-for="(service, sIdx) in (pkg.includedServices && pkg.includedServices.length > 0 ? pkg.includedServices : pkg.includedServiceNames)"
                :key="sIdx"
                class="checklist-item"
              >
                <i class="fa-solid fa-check check-mark"></i>
                <span>
                  {{ typeof service === 'string' ? service : service.name }}
                  <em v-if="typeof service !== 'string' && service.quantity > 1" class="qty font-mono">(x{{ service.quantity }})</em>
                </span>
              </li>

              <!-- Default Mockup items if DB list is short -->
              <template v-if="!pkg.includedServices?.length && !pkg.includedServiceNames?.length">
                <li class="checklist-item"><i class="fa-solid fa-check check-mark"></i><span>Chụp ảnh cưới nghệ thuật</span></li>
                <li class="checklist-item"><i class="fa-solid fa-check check-mark"></i><span>Makeup cô dâu & chú rể</span></li>
                <li class="checklist-item"><i class="fa-solid fa-check check-mark"></i><span>Váy cưới & Âu phục cao cấp</span></li>
                <li class="checklist-item"><i class="fa-solid fa-check check-mark"></i><span>Album photobook mở phẳng</span></li>
                <li class="checklist-item"><i class="fa-solid fa-check check-mark"></i><span>Bàn giao toàn bộ file gốc</span></li>
              </template>
            </ul>

            <!-- Button: Đặt ngay -->
            <div class="card-btn-wrap">
              <router-link
                :to="`/goi-cuoi/${pkg.slug}`"
                class="btn-order font-serif"
                :class="pkg.isPopular ? 'btn-featured' : 'btn-regular'"
              >
                Đặt ngay
              </router-link>
            </div>
          </div>
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
import { usePackage } from '@/composables/usePackage'
import VintageLoading from '@/components/common/VintageLoading.vue'

const {
  packages,
  page,
  totalPages,
  isLoading,
  error,
  fetchPackages,
  setPage,
} = usePackage()

const defaultPackageCover = 'https://images.unsplash.com/photo-1519741497674-611481863552?auto=format&fit=crop&w=1200&q=80'

function formatCurrency(amount?: number | null): string {
  if (amount == null) return '0đ'
  return new Intl.NumberFormat('vi-VN').format(amount) + 'đ'
}

onMounted(async () => {
  await fetchPackages()
})
</script>

<style lang="scss" scoped>
.package-page {
  min-height: 100vh;
  background-color: var(--color-paper);
  color: var(--color-ink);
  padding: 24px 0 80px;
}

.breadcrumb-trail {
  margin-bottom: 24px;
}

.page-title-row {
  margin-bottom: 40px;

  .page-main-title {
    font-size: clamp(2rem, 3.5vw, 2.5rem);
    font-weight: 600;
    color: var(--color-ink);
    margin: 0;
    letter-spacing: -0.01em;
  }
}

// 3-Column Package Cards Grid (Mockup Match)
.packages-catalog-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 28px;
  align-items: stretch;

  @media (max-width: 990px) {
    grid-template-columns: repeat(2, 1fr);
    gap: 24px;
  }

  @media (max-width: 640px) {
    grid-template-columns: 1fr;
    gap: 24px;
  }
}

// Package Card
.package-card-mockup {
  position: relative;
  background-color: var(--color-paper-light);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-xs);
  padding: 16px;
  display: flex;
  flex-direction: column;
  transition: transform 0.25s ease, border-color 0.25s ease;

  &.is-featured {
    border: 1.5px solid var(--color-burgundy);
    box-shadow: 0 4px 16px rgba(142, 41, 41, 0.08);

    .featured-badge {
      position: absolute;
      top: -12px;
      left: 50%;
      transform: translateX(-50%);
      background-color: var(--color-paper-light);
      border: 1px solid var(--color-burgundy);
      color: var(--color-burgundy);
      font-size: 0.78rem;
      font-weight: 600;
      padding: 2px 14px;
      border-radius: 2px;
      letter-spacing: 0.5px;
      z-index: 5;
    }
  }

  .card-image-wrap {
    width: 100%;
    aspect-ratio: 16 / 11;
    overflow: hidden;
    background-color: var(--color-paper-dark);
    border: 1px solid var(--color-border);
    display: block;
    margin-bottom: 16px;

    .pkg-img {
      width: 100%;
      height: 100%;
      object-fit: cover;
      display: block;
      transition: transform 0.4s ease;
    }
  }

  .card-body {
    display: flex;
    flex-direction: column;
    flex-grow: 1;
  }

  .pkg-title {
    font-size: 1.28rem;
    font-weight: 600;
    color: var(--color-ink);
    margin: 0 0 8px;
    line-height: 1.3;

    a {
      color: var(--color-ink);
      transition: color 0.2s;

      &:hover {
        color: var(--color-burgundy);
      }
    }
  }

  .pkg-price {
    font-size: 1.45rem;
    font-weight: 700;
    color: var(--color-ink);
    margin-bottom: 16px;
    padding-bottom: 12px;
    border-bottom: 1px solid var(--color-border-light);
  }

  .pkg-checklist {
    list-style: none;
    padding: 0;
    margin: 0 0 24px;
    display: flex;
    flex-direction: column;
    gap: 8px;
    flex-grow: 1;

    .checklist-item {
      display: flex;
      align-items: flex-start;
      gap: 8px;
      font-size: 0.92rem;
      color: var(--color-ink-soft);
      line-height: 1.45;

      .check-mark {
        font-size: 0.8rem;
        color: var(--color-ink-soft);
        margin-top: 4px;
        flex-shrink: 0;
      }

      .qty {
        color: var(--color-burgundy);
        font-weight: 600;
        font-size: 0.82rem;
      }
    }
  }

  .card-btn-wrap {
    margin-top: auto;

    .btn-order {
      display: block;
      width: 100%;
      text-align: center;
      padding: 10px;
      border-radius: 2px;
      font-size: 0.98rem;
      font-weight: 500;
      text-decoration: none;
      transition: all 0.25s ease;
      cursor: pointer;

      &.btn-regular {
        background-color: #55534B;
        color: #FAF7F0;
        border: 1px solid #44423B;

        &:hover {
          background-color: #383732;
        }
      }

      &.btn-featured {
        background-color: var(--color-burgundy);
        color: #FAF7F0;
        border: 1px solid var(--color-burgundy-dark);

        &:hover {
          background-color: var(--color-burgundy-dark);
        }
      }
    }
  }

  &:hover {
    .card-image-wrap .pkg-img {
      transform: scale(1.035);
    }
  }
}

// Empty State
.empty-package-box {
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
.package-error-state {
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
