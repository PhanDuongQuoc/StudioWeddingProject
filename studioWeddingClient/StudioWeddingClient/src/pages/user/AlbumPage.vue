<template>
  <div class="album-page">
    <!-- 1. Unified Vintage Loading State -->
    <VintageLoading v-if="isLoading" text="Đang tải danh sách album ảnh..." />

    <!-- 2. Error State -->
    <div v-else-if="error" class="album-error-state studio-container">
      <div class="error-box paper-card">
        <i class="fa-solid fa-circle-exclamation error-icon"></i>
        <h2 class="error-title font-serif">Không thể tải danh sách Album</h2>
        <p class="error-desc">{{ error }}</p>
        <button @click="fetchAlbums" class="btn-vintage font-serif">
          <i class="fa-solid fa-rotate-right q-mr-xs"></i>
          <span>Thử lại</span>
        </button>
      </div>
    </div>

    <!-- 3. Main Album Listing Content (Exact Mockup Layout) -->
    <main v-else class="album-main-content studio-container">
      <!-- Breadcrumb Bar -->
      <nav class="breadcrumb-trail font-serif" aria-label="Breadcrumb">
        <router-link to="/trang-chu">Trang chủ</router-link>
        <span class="sep">/</span>
        <span class="current-crumb">Album</span>
      </nav>

      <!-- Page Heading -->
      <div class="page-title-row">
        <h1 class="page-main-title font-serif">Album ảnh</h1>
      </div>

      <!-- Two-Column Editorial Layout: Sidebar + Gallery Grid -->
      <div class="album-layout-grid">
        <!-- Left Sidebar: Category Filters -->
        <aside class="album-sidebar">
          <div class="category-filter-box">
            <button
              class="cat-filter-item font-serif"
              :class="{ active: selectedCategory === 'all' }"
              @click="onCategoryChange('all')"
            >
              <div class="item-left">
                <i class="fa-regular fa-folder cat-icon"></i>
                <span class="cat-name">Tất cả</span>
              </div>
              <span class="cat-count font-serif">({{ totalAllAlbumsCount }})</span>
            </button>

            <button
              v-for="cat in categories"
              :key="cat.categoryId"
              class="cat-filter-item font-serif"
              :class="{ active: selectedCategory === cat.slug }"
              @click="onCategoryChange(cat.slug)"
            >
              <div class="item-left">
                <i class="fa-regular fa-folder cat-icon"></i>
                <span class="cat-name">{{ cat.name }}</span>
              </div>
              <span class="cat-count font-serif">({{ cat.albumCount }})</span>
            </button>
          </div>
        </aside>

        <!-- Right Column: Albums Grid -->
        <section class="album-gallery-col">
          <!-- Empty State -->
          <div v-if="albums.length === 0" class="empty-gallery-box">
            <i class="fa-solid fa-camera-retro empty-icon"></i>
            <h3 class="empty-title font-serif">Chưa có album nào</h3>
            <p class="empty-desc font-serif">
              Hiện chưa có album nào thuộc danh mục này. Vui lòng chọn danh mục khác.
            </p>
            <button @click="onCategoryChange('all')" class="btn-vintage font-serif">
              Xem tất cả Album
            </button>
          </div>

          <!-- 3-Column Album Cards Grid -->
          <div v-else class="albums-grid">
            <article
              v-for="album in albums"
              :key="album.albumId"
              class="album-card"
            >
              <router-link :to="`/album/${album.slug}`" class="card-inner">
                <!-- Cover Image -->
                <div class="card-image-wrap">
                  <img
                    :src="album.coverImageUrl || defaultAlbumCover"
                    :alt="album.title"
                    class="card-img film-photo"
                    loading="lazy"
                  />
                </div>

                <!-- Text Content Below Image -->
                <div class="card-info">
                  <h2 class="card-title font-serif">{{ album.title }}</h2>
                  <span class="card-count font-serif">
                    {{ album.totalPhotos || 0 }} ảnh
                  </span>
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
        </section>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAlbum } from '@/composables/useAlbum'
import VintageLoading from '@/components/common/VintageLoading.vue'

const route = useRoute()
const router = useRouter()

const {
  albums,
  categories,
  selectedCategory,
  page,
  totalItems,
  totalPages,
  isLoading,
  error,
  fetchAlbums,
  setCategory,
  setPage,
} = useAlbum()

const defaultAlbumCover = 'https://images.unsplash.com/photo-1519741497674-611481863552?auto=format&fit=crop&w=1200&q=80'

const totalAllAlbumsCount = computed(() => {
  return categories.value.reduce((acc, cat) => acc + (cat.albumCount || 0), 0) || totalItems.value
})

async function onCategoryChange(slug: string) {
  await setCategory(slug)
  const query: Record<string, string> = {}
  if (slug !== 'all') {
    query.category = slug
  }
  await router.replace({ query })
}

onMounted(async () => {
  if (route.query.category && typeof route.query.category === 'string') {
    selectedCategory.value = route.query.category
  }
  await fetchAlbums()
})

watch(
  () => route.query,
  async (newQuery) => {
    const cat = (newQuery.category as string) || 'all'
    if (cat !== selectedCategory.value) {
      selectedCategory.value = cat
      await fetchAlbums()
    }
  }
)
</script>

<style lang="scss" scoped>
.album-page {
  min-height: 100vh;
  background-color: var(--color-paper);
  color: var(--color-ink);
  padding: 24px 0 80px;
}

.breadcrumb-trail {
  margin-bottom: 24px;
}

.page-title-row {
  margin-bottom: 32px;

  .page-main-title {
    font-size: clamp(2rem, 3.5vw, 2.5rem);
    font-weight: 600;
    color: var(--color-ink);
    margin: 0;
    letter-spacing: -0.01em;
  }
}

// Two-Column Layout (Mockup Match)
.album-layout-grid {
  display: grid;
  grid-template-columns: 240px 1fr;
  gap: 36px;
  align-items: start;

  @media (max-width: 900px) {
    grid-template-columns: 1fr;
    gap: 24px;
  }
}

// Left Sidebar Filters
.album-sidebar {
  .category-filter-box {
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    padding: 12px;
    display: flex;
    flex-direction: column;
    gap: 4px;

    @media (max-width: 900px) {
      flex-direction: row;
      flex-wrap: wrap;
    }
  }

  .cat-filter-item {
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;
    background: transparent;
    border: 1px solid transparent;
    border-radius: var(--radius-xs);
    padding: 10px 14px;
    font-size: 0.95rem;
    color: var(--color-ink);
    cursor: pointer;
    transition: all 0.2s ease;
    text-align: left;

    .item-left {
      display: flex;
      align-items: center;
      gap: 10px;

      .cat-icon {
        font-size: 0.95rem;
        color: var(--color-ink-soft);
      }

      .cat-name {
        font-weight: 500;
      }
    }

    .cat-count {
      font-size: 0.88rem;
      color: var(--color-muted);
    }

    &:hover {
      background-color: var(--color-paper-dark);
      color: var(--color-burgundy);

      .cat-icon {
        color: var(--color-burgundy);
      }
    }

    &.active {
      background-color: var(--color-paper-dark);
      border-color: var(--color-border);
      font-weight: 600;
      color: var(--color-ink);

      .cat-icon {
        color: var(--color-burgundy);
      }

      .cat-count {
        color: var(--color-ink);
      }
    }

    @media (max-width: 900px) {
      width: auto;
      flex-grow: 1;
    }
  }
}

// Right Column: Album Grid
.album-gallery-col {
  .albums-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 24px;

    @media (max-width: 1100px) {
      grid-template-columns: repeat(2, 1fr);
      gap: 20px;
    }

    @media (max-width: 600px) {
      grid-template-columns: 1fr;
      gap: 20px;
    }
  }

  .album-card {
    .card-inner {
      display: flex;
      flex-direction: column;
      gap: 10px;
      text-decoration: none;
    }

    .card-image-wrap {
      width: 100%;
      aspect-ratio: 4 / 3;
      overflow: hidden;
      border-radius: var(--radius-xs);
      background-color: var(--color-paper-dark);
      border: 1px solid var(--color-border);

      .card-img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        display: block;
        transition: transform 0.4s ease;
      }
    }

    .card-info {
      display: flex;
      flex-direction: column;
      gap: 3px;

      .card-title {
        font-size: 1.15rem;
        font-weight: 600;
        color: var(--color-ink);
        margin: 0;
        transition: color 0.2s;
        line-height: 1.35;
      }

      .card-count {
        font-size: 0.85rem;
        color: var(--color-muted);
      }
    }

    &:hover {
      .card-image-wrap .card-img {
        transform: scale(1.035);
      }

      .card-title {
        color: var(--color-burgundy);
      }
    }
  }

  .empty-gallery-box {
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
      margin: 0 0 20px;
      font-size: 0.95rem;
    }
  }

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
}

// Error State
.album-error-state {
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
