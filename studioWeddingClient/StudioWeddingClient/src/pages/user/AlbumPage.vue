<template>
  <div class="album-page">
    <!-- 1. Unified Vintage Loading State -->
    <VintageLoading v-if="isLoading" text="Đang mở kho lưu trữ Album ảnh Hỷ Sự..." />

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

    <!-- 3. Main Album Listing Content -->
    <main v-else class="album-main-content">
      <!-- Breadcrumb Bar -->
      <section class="album-breadcrumb-bar">
        <div class="studio-container breadcrumb-inner">
          <nav class="breadcrumb-trail" aria-label="Breadcrumb">
            <router-link to="/trang-chu">Trang chủ</router-link>
            <span class="sep">/</span>
            <span class="current-crumb">Album ảnh</span>
            <span v-if="activeCategoryName && selectedCategory !== 'all'" class="sep">/</span>
            <span v-if="activeCategoryName && selectedCategory !== 'all'" class="cat-crumb">{{ activeCategoryName }}</span>
          </nav>

          <button @click="copyShareLink" class="share-link-btn" title="Sao chép liên kết">
            <i class="fa-solid" :class="isCopied ? 'fa-check' : 'fa-arrow-up-right-from-square'"></i>
            <span>{{ isCopied ? 'Đã sao chép link' : 'Chia sẻ' }}</span>
          </button>
        </div>
      </section>

      <!-- Section 1: Editorial Masthead Hero Header -->
      <section class="album-hero-section">
        <div class="studio-container">
          <div class="hero-masthead paper-card">
            <!-- Film Timecode Strip -->
            <div class="film-timecode-bar">
              <div class="timecode-left">
                <span class="rec-dot"></span>
                <span class="rec-label">REC [●] 1998 — 2026</span>
                <span class="film-sep">·</span>
                <span class="film-stock font-chinese">喜事・相冊歸檔</span>
              </div>
              <div class="timecode-right">
                <span class="archive-tag font-serif">HỶ SỰ WEDDING ARCHIVE</span>
              </div>
            </div>

            <!-- Masthead Content -->
            <div class="masthead-content">
              <div class="masthead-top-seal">
                <span class="chinese-seal font-chinese">囍</span>
                <span class="seal-sub font-serif">TUYỂN TẬP ẢNH CƯỚI ĐỘC BẢN</span>
              </div>

              <h1 class="masthead-title font-serif">
                Kho Lưu Trữ Album Cưới Nghệ Thuật
              </h1>

              <div class="masthead-divider-line">
                <span class="line-ornament font-chinese">百年好合・永結同心</span>
              </div>

              <p class="masthead-desc font-serif">
                Mỗi album là một cuốn tiểu thuyết hình ảnh chân thực, ghi dấu những rung động tình yêu nguyên bản qua lăng kính màu phim Hong Kong và nét đẹp văn hóa cưới Việt Nam thập niên 90.
              </p>

              <!-- Archive Quick Meta Bar -->
              <div class="masthead-stats font-serif">
                <div class="stat-pill">
                  <i class="fa-solid fa-film"></i>
                  <span><strong>{{ totalItems }}</strong> bộ ảnh được lưu trữ</span>
                </div>
                <div class="stat-pill">
                  <i class="fa-solid fa-camera"></i>
                  <span>Phong cách Film Analog 90s</span>
                </div>
                <div class="stat-pill">
                  <i class="fa-solid fa-location-dot"></i>
                  <span>Sài Gòn · Đà Lạt · Hong Kong</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 2: Filter & Search Controls Bar -->
      <section class="album-filter-section">
        <div class="studio-container">
          <div class="filter-controls-bar paper-card">
            <!-- Category Tabs -->
            <div class="category-tabs-wrap">
              <button
                class="cat-tab-btn font-serif"
                :class="{ active: selectedCategory === 'all' }"
                @click="onCategoryChange('all')"
              >
                <span>Tất cả</span>
                <span class="tab-count font-mono">{{ totalAllAlbumsCount }}</span>
              </button>

              <button
                v-for="cat in categories"
                :key="cat.categoryId"
                class="cat-tab-btn font-serif"
                :class="{ active: selectedCategory === cat.slug }"
                @click="onCategoryChange(cat.slug)"
              >
                <span>{{ cat.name }}</span>
                <span class="tab-count font-mono">{{ cat.albumCount }}</span>
              </button>
            </div>

            <!-- Search Input -->
            <div class="filter-search-wrap">
              <div class="vintage-search-box">
                <i class="fa-solid fa-magnifying-glass search-icon"></i>
                <input
                  type="text"
                  v-model="searchInput"
                  placeholder="Tìm kiếm theo tên album, địa điểm..."
                  class="search-input font-serif"
                  @keyup.enter="onSearchSubmit"
                />
                <button
                  v-if="searchInput"
                  @click="clearSearch"
                  class="clear-search-btn"
                  title="Xóa tìm kiếm"
                >
                  <i class="fa-solid fa-xmark"></i>
                </button>
                <button
                  @click="onSearchSubmit"
                  class="search-submit-btn font-serif"
                >
                  Tìm
                </button>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 3: Album Gallery Grid -->
      <section class="album-grid-section">
        <div class="studio-container">
          <!-- Active Filter Summary & Sort Info -->
          <div class="grid-header-meta">
            <div class="header-meta-left">
              <span class="grid-showing font-serif">
                Hiển thị <strong>{{ albums.length }}</strong> trong tổng số <strong>{{ totalItems }}</strong> bộ ảnh
                <span v-if="selectedCategory !== 'all'">— Danh mục: <em>{{ activeCategoryName }}</em></span>
                <span v-if="searchQuery">— Từ khóa: "<em>{{ searchQuery }}</em>"</span>
              </span>
            </div>
            <div class="header-meta-right">
              <span class="catalog-seal font-chinese">相冊輯錄</span>
            </div>
          </div>

          <!-- Empty Search State -->
          <div v-if="albums.length === 0" class="empty-gallery-state paper-card">
            <div class="empty-film-box">
              <i class="fa-solid fa-film empty-icon"></i>
              <h3 class="empty-title font-serif">Không tìm thấy Album phù hợp</h3>
              <p class="empty-desc font-serif">
                Không có bộ ảnh nào khớp với bộ lọc hoặc từ khóa "{{ searchQuery }}". Vui lòng thử tìm kiếm với từ khóa khác hoặc chọn danh mục khác.
              </p>
              <button @click="resetAllFilters" class="btn-vintage font-serif">
                <i class="fa-solid fa-rotate-left q-mr-xs"></i>
                <span>Xem tất cả Album</span>
              </button>
            </div>
          </div>

          <!-- Album Cards Grid (3 Columns Desktop, 2 Tablet, 1-2 Mobile) -->
          <div v-else class="albums-grid">
            <article
              v-for="(album, index) in albums"
              :key="album.albumId"
              class="album-card paper-card"
            >
              <!-- Card Header Film Strip -->
              <div class="card-film-header">
                <span class="card-film-idx font-mono">HK · 90s EXP-{{ String((page - 1) * pageSize + index + 1).padStart(2, '0') }}</span>
                <span class="card-film-brand font-mono">KODAK GOLD</span>
              </div>

              <!-- Cover Image & Link -->
              <router-link :to="`/album/${album.slug}`" class="card-cover-wrap">
                <img
                  :src="album.coverImageUrl || defaultAlbumCover"
                  :alt="album.title"
                  class="card-cover-img film-photo"
                  loading="lazy"
                />

                <div class="card-overlay">
                  <span class="card-view-btn font-serif">
                    <span>Xem trọn bộ album</span>
                    <i class="fa-solid fa-arrow-right"></i>
                  </span>
                </div>

                <!-- Featured Badge -->
                <div v-if="album.isFeatured" class="card-featured-badge font-serif">
                  <i class="fa-solid fa-star"></i>
                  <span>Nổi bật</span>
                </div>

                <!-- Photo Count Pill -->
                <div class="card-photos-pill font-mono">
                  <i class="fa-solid fa-images"></i>
                  <span>{{ album.totalPhotos || 0 }} ảnh</span>
                </div>
              </router-link>

              <!-- Card Body Content -->
              <div class="card-body">
                <!-- Category Tag -->
                <div class="card-category-row">
                  <span class="category-tag font-serif" @click.stop="onCategoryChange(album.categorySlug)">
                    {{ album.categoryName }}
                  </span>
                  <span class="card-date font-mono" v-if="album.createdAt">
                    {{ formatDate(album.createdAt) }}
                  </span>
                </div>

                <!-- Title -->
                <h2 class="card-title font-serif">
                  <router-link :to="`/album/${album.slug}`">
                    {{ album.title }}
                  </router-link>
                </h2>

                <!-- Description excerpt -->
                <p class="card-desc font-serif" v-if="album.description">
                  {{ truncateText(album.description, 100) }}
                </p>

                <!-- Footer Action Link -->
                <div class="card-footer-action">
                  <router-link :to="`/album/${album.slug}`" class="card-link font-serif">
                    <span>Khám phá câu chuyện</span>
                    <i class="fa-solid fa-arrow-right"></i>
                  </router-link>
                  <span class="card-studio-mark font-chinese">喜事・記錄</span>
                </div>
              </div>
            </article>
          </div>

          <!-- Section 4: Vintage Editorial Pagination -->
          <div class="album-pagination-wrap" v-if="totalPages > 1">
            <div class="pagination-bar paper-card">
              <!-- Previous Page -->
              <button
                class="page-nav-btn font-serif"
                :disabled="page <= 1"
                @click="setPage(page - 1)"
                title="Trang trước"
              >
                <i class="fa-solid fa-chevron-left"></i>
                <span class="desktop-only">Trang trước</span>
              </button>

              <!-- Page Numbers List -->
              <div class="page-numbers-list">
                <button
                  v-for="p in visiblePages"
                  :key="p"
                  class="page-num-btn font-mono"
                  :class="{ active: p === page }"
                  @click="setPage(p)"
                >
                  {{ String(p).padStart(2, '0') }}
                </button>
              </div>

              <!-- Next Page -->
              <button
                class="page-nav-btn font-serif"
                :disabled="page >= totalPages"
                @click="setPage(page + 1)"
                title="Trang sau"
              >
                <span class="desktop-only">Trang sau</span>
                <i class="fa-solid fa-chevron-right"></i>
              </button>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 5: Consultation Inquiry Envelope Section -->
      <section class="album-cta-banner-section">
        <div class="studio-container">
          <div class="consult-envelope-box paper-card">
            <div class="envelope-top-bar">
              <div class="stamp-group">
                <span class="post-stamp font-chinese">百年好合</span>
                <span class="post-code font-mono">HK-VN · STUDIO POST 1998</span>
              </div>
              <span class="post-slogan font-serif">HỶ SỰ WEDDING STUDIO ARCHIVE</span>
            </div>

            <div class="envelope-content-grid">
              <div class="envelope-info">
                <span class="info-tag font-chinese">喜事・婚紗概念</span>
                <h3 class="info-title font-serif">Bạn mong muốn sở hữu một bộ ảnh cưới mang màu thời gian?</h3>
                <p class="info-desc font-serif">
                  Hãy chia sẻ ý tưởng của bạn với Hỷ Sự Studio. Chúng tôi luôn sẵn sàng lắng nghe câu chuyện tình yêu của hai bạn, chuẩn bị những mẫu trang phục cưới vintage kinh điển và kiến tạo một cuốn album hoài niệm đầy cảm xúc.
                </p>
                <div class="info-contacts font-serif">
                  <div class="contact-item">
                    <i class="fa-solid fa-phone"></i>
                    <span>Hotline tư vấn: <strong>+84 912 345 678</strong></span>
                  </div>
                  <div class="contact-item">
                    <i class="fa-solid fa-location-dot"></i>
                    <span>Studio: <strong>123 Nguyễn Văn Cừ, Quận 5, TP. Hồ Chí Minh</strong></span>
                  </div>
                </div>
              </div>

              <div class="envelope-cta-action">
                <router-link to="/trang-chu#lien-he" class="btn-vintage font-serif cta-big-btn">
                  <i class="fa-regular fa-paper-plane q-mr-xs"></i>
                  <span>Gửi yêu cầu tư vấn Concept</span>
                </router-link>
                <router-link to="/dich-vu" class="btn-vintage-outline font-serif cta-outline-btn q-mt-sm">
                  <i class="fa-solid fa-list-check q-mr-xs"></i>
                  <span>Xem bảng giá & Dịch vụ</span>
                </router-link>
              </div>
            </div>
          </div>
        </div>
      </section>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import { useAlbum } from '@/composables/useAlbum'
import VintageLoading from '@/components/common/VintageLoading.vue'

const route = useRoute()
const router = useRouter()
const $q = useQuasar()

const {
  albums,
  categories,
  selectedCategory,
  searchQuery,
  page,
  pageSize,
  totalItems,
  totalPages,
  isLoading,
  error,
  fetchAlbums,
  setCategory,
  setPage,
  handleSearch,
} = useAlbum()

const searchInput = ref('')
const isCopied = ref(false)
const defaultAlbumCover = 'https://images.unsplash.com/photo-1519741497674-611481863552?auto=format&fit=crop&w=1200&q=80'

// Total album count calculated across all categories
const totalAllAlbumsCount = computed(() => {
  return categories.value.reduce((acc, cat) => acc + (cat.albumCount || 0), 0) || totalItems.value
})

// Current active category display name
const activeCategoryName = computed(() => {
  if (selectedCategory.value === 'all') return 'Tất cả'
  const found = categories.value.find(c => c.slug === selectedCategory.value)
  return found ? found.name : selectedCategory.value
})

// Compute visible pagination page numbers
const visiblePages = computed(() => {
  const current = page.value
  const total = totalPages.value
  const delta = 2
  const range: number[] = []

  for (let i = Math.max(1, current - delta); i <= Math.min(total, current + delta); i++) {
    range.push(i)
  }
  return range
})

// Formatting helpers
function formatDate(dateStr?: string): string {
  if (!dateStr) return ''
  try {
    const d = new Date(dateStr)
    return d.toLocaleDateString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit' })
  } catch {
    return dateStr
  }
}

function truncateText(text?: string | null, maxLen = 100): string {
  if (!text) return ''
  if (text.length <= maxLen) return text
  return text.slice(0, maxLen).trim() + '...'
}

// Actions
async function onCategoryChange(slug: string) {
  await setCategory(slug)
  await updateUrlParams()
}

async function onSearchSubmit() {
  await handleSearch(searchInput.value)
  await updateUrlParams()
}

async function clearSearch() {
  searchInput.value = ''
  await handleSearch('')
  await updateUrlParams()
}

async function resetAllFilters() {
  searchInput.value = ''
  selectedCategory.value = 'all'
  page.value = 1
  await handleSearch('')
  await updateUrlParams()
}

async function updateUrlParams() {
  const query: Record<string, string> = {}
  if (selectedCategory.value && selectedCategory.value !== 'all') {
    query.category = selectedCategory.value
  }
  if (searchQuery.value) {
    query.q = searchQuery.value
  }
  if (page.value > 1) {
    query.p = String(page.value)
  }
  await router.replace({ query })
}

async function copyShareLink() {
  try {
    await navigator.clipboard.writeText(window.location.href)
    isCopied.value = true
    $q.notify({
      type: 'positive',
      message: 'Đã sao chép liên kết trang Album vào bộ nhớ tạm!',
      position: 'top',
      timeout: 2000,
    })
    setTimeout(() => {
      isCopied.value = false
    }, 2500)
  } catch {
    $q.notify({
      type: 'warning',
      message: 'Không thể tự động sao chép liên kết.',
      position: 'top',
    })
  }
}

// Sync URL query params on initial mount
onMounted(async () => {
  if (route.query.category && typeof route.query.category === 'string') {
    selectedCategory.value = route.query.category
  }
  if (route.query.q && typeof route.query.q === 'string') {
    searchInput.value = route.query.q
    searchQuery.value = route.query.q
  }
  if (route.query.p && typeof route.query.p === 'string') {
    const p = parseInt(route.query.p, 10)
    if (!isNaN(p) && p > 0) {
      page.value = p
    }
  }

  await fetchAlbums()
})

// Watch route changes if user navigates via browser history
watch(
  () => route.query,
  async (newQuery) => {
    const cat = (newQuery.category as string) || 'all'
    const q = (newQuery.q as string) || ''
    const p = parseInt((newQuery.p as string) || '1', 10)

    if (cat !== selectedCategory.value || q !== searchQuery.value || p !== page.value) {
      selectedCategory.value = cat
      searchQuery.value = q
      searchInput.value = q
      page.value = isNaN(p) ? 1 : p
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
  padding-bottom: 80px;
}

// Breadcrumb Bar
.album-breadcrumb-bar {
  border-bottom: 1px solid var(--color-border);
  background-color: var(--color-paper-light);
  padding: 12px 0;

  .breadcrumb-inner {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 16px;
  }

  .breadcrumb-trail {
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 0.88rem;
    color: var(--color-muted);
    flex-wrap: wrap;

    a {
      color: var(--color-ink-soft);
      transition: color 0.2s;

      &:hover {
        color: var(--color-burgundy);
      }
    }

    .sep {
      color: var(--color-border-dark);
      font-size: 0.8rem;
    }

    .cat-crumb {
      color: var(--color-ink-soft);
    }

    .current-crumb {
      color: var(--color-burgundy);
      font-weight: 600;
    }
  }

  .share-link-btn {
    background: none;
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    padding: 5px 12px;
    font-size: 0.82rem;
    font-family: var(--font-serif);
    color: var(--color-ink-soft);
    display: inline-flex;
    align-items: center;
    gap: 6px;
    cursor: pointer;
    transition: all 0.2s;

    &:hover {
      border-color: var(--color-burgundy);
      color: var(--color-burgundy);
      background-color: rgba(142, 41, 41, 0.04);
    }
  }
}

// Hero Masthead Section
.album-hero-section {
  padding: 32px 0 20px;

  .hero-masthead {
    padding: 0;
    overflow: hidden;
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
  }

  .film-timecode-bar {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px 24px;
    background-color: var(--color-film-dark);
    color: #DCD5C8;
    font-size: 0.8rem;
    letter-spacing: 0.5px;

    .timecode-left {
      display: flex;
      align-items: center;
      gap: 8px;

      .rec-dot {
        width: 8px;
        height: 8px;
        border-radius: 50%;
        background-color: #E24B4B;
        animation: rec-blink 1.5s infinite ease-in-out;
      }

      .film-sep {
        color: rgba(220, 213, 200, 0.4);
      }

      .film-stock {
        color: #E2DBD0;
        font-size: 0.88rem;
      }
    }

    .archive-tag {
      font-size: 0.75rem;
      letter-spacing: 1px;
      color: #A8A296;
    }
  }

  .masthead-content {
    padding: clamp(24px, 4vw, 48px) clamp(20px, 4vw, 56px);
    text-align: center;
    display: flex;
    flex-direction: column;
    align-items: center;
  }

  .masthead-top-seal {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    margin-bottom: 12px;

    .chinese-seal {
      display: inline-flex;
      align-items: center;
      justify-content: center;
      width: 28px;
      height: 28px;
      background-color: var(--color-burgundy);
      color: #FAF7F0;
      border: 1px solid var(--color-burgundy-dark);
      border-radius: 2px;
      font-size: 0.95rem;
      font-weight: 700;
    }

    .seal-sub {
      font-size: 0.88rem;
      letter-spacing: 1.5px;
      color: var(--color-burgundy);
      font-weight: 600;
    }
  }

  .masthead-title {
    font-size: clamp(1.8rem, 4vw, 2.8rem);
    font-weight: 600;
    color: var(--color-ink);
    line-height: 1.25;
    margin: 0 0 16px;
    letter-spacing: -0.01em;
  }

  .masthead-divider-line {
    position: relative;
    width: 100%;
    max-width: 480px;
    height: 1px;
    background-color: var(--color-border);
    margin: 8px 0 20px;
    display: flex;
    align-items: center;
    justify-content: center;

    .line-ornament {
      background-color: var(--color-paper-light);
      padding: 0 16px;
      color: var(--color-muted);
      font-size: 0.85rem;
      letter-spacing: 2px;
    }
  }

  .masthead-desc {
    max-width: 760px;
    font-size: clamp(1rem, 1.8vw, 1.12rem);
    line-height: 1.7;
    color: var(--color-ink-soft);
    margin: 0 0 24px;
  }

  .masthead-stats {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    gap: 16px;

    .stat-pill {
      display: inline-flex;
      align-items: center;
      gap: 8px;
      padding: 6px 16px;
      background-color: var(--color-paper);
      border: 1px solid var(--color-border);
      border-radius: var(--radius-xs);
      font-size: 0.88rem;
      color: var(--color-ink-soft);

      i {
        color: var(--color-burgundy);
        font-size: 0.82rem;
      }
    }
  }
}

// Filter Controls Bar
.album-filter-section {
  padding: 8px 0 24px;

  .filter-controls-bar {
    padding: 16px 20px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 20px;
    flex-wrap: wrap;
    background-color: var(--color-paper-light);
  }

  .category-tabs-wrap {
    display: flex;
    align-items: center;
    gap: 8px;
    flex-wrap: wrap;

    .cat-tab-btn {
      background: var(--color-paper);
      border: 1px solid var(--color-border);
      border-radius: var(--radius-xs);
      padding: 7px 14px;
      font-size: 0.92rem;
      color: var(--color-ink-soft);
      cursor: pointer;
      display: inline-flex;
      align-items: center;
      gap: 8px;
      transition: all 0.25s ease;

      .tab-count {
        font-size: 0.75rem;
        padding: 1px 6px;
        background-color: rgba(36, 36, 33, 0.06);
        border-radius: 2px;
        color: var(--color-muted);
      }

      &:hover {
        border-color: var(--color-burgundy);
        color: var(--color-burgundy);
      }

      &.active {
        background-color: var(--color-burgundy);
        border-color: var(--color-burgundy-dark);
        color: #FAF7F0;

        .tab-count {
          background-color: rgba(255, 255, 255, 0.22);
          color: #FAF7F0;
        }
      }
    }
  }

  .filter-search-wrap {
    flex-grow: 1;
    max-width: 380px;

    @media (max-width: 900px) {
      max-width: 100%;
      width: 100%;
    }

    .vintage-search-box {
      display: flex;
      align-items: center;
      background-color: var(--color-paper);
      border: 1px solid var(--color-border);
      border-radius: var(--radius-xs);
      padding: 2px 4px 2px 10px;
      transition: border-color 0.25s;

      &:focus-within {
        border-color: var(--color-burgundy);
      }

      .search-icon {
        color: var(--color-muted);
        font-size: 0.85rem;
        margin-right: 6px;
      }

      .search-input {
        border: none;
        background: transparent;
        font-size: 0.9rem;
        color: var(--color-ink);
        width: 100%;
        outline: none;
        padding: 6px 0;

        &::placeholder {
          color: var(--color-muted);
          font-style: italic;
        }
      }

      .clear-search-btn {
        background: none;
        border: none;
        color: var(--color-muted);
        padding: 4px;
        cursor: pointer;
        font-size: 0.85rem;

        &:hover {
          color: var(--color-burgundy);
        }
      }

      .search-submit-btn {
        background-color: var(--color-film-teal);
        color: #FAF7F0;
        border: none;
        border-radius: var(--radius-xs);
        padding: 6px 12px;
        font-size: 0.85rem;
        cursor: pointer;
        margin-left: 4px;
        transition: background-color 0.2s;

        &:hover {
          background-color: var(--color-film-dark);
        }
      }
    }
  }
}

// Album Grid Section
.album-grid-section {
  padding: 10px 0 40px;

  .grid-header-meta {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding-bottom: 16px;
    border-bottom: 1px solid var(--color-border);
    margin-bottom: 28px;

    .grid-showing {
      font-size: 0.95rem;
      color: var(--color-ink-soft);

      strong {
        color: var(--color-ink);
        font-weight: 600;
      }
    }

    .catalog-seal {
      font-size: 0.85rem;
      color: var(--color-muted);
      letter-spacing: 2px;
    }
  }

  // Albums Grid Layout
  .albums-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 32px;

    @media (max-width: 1100px) {
      grid-template-columns: repeat(2, 1fr);
      gap: 24px;
    }

    @media (max-width: 640px) {
      grid-template-columns: 1fr;
      gap: 20px;
    }
  }

  // Album Card
  .album-card {
    display: flex;
    flex-direction: column;
    overflow: hidden;
    border: 1px solid var(--color-border);
    background-color: var(--color-paper-light);
    border-radius: var(--radius-xs);

    .card-film-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 6px 14px;
      background-color: var(--color-film-dark);
      color: #DCD5C8;
      font-size: 0.72rem;
      letter-spacing: 0.5px;
    }

    .card-cover-wrap {
      position: relative;
      display: block;
      width: 100%;
      aspect-ratio: 4 / 3;
      overflow: hidden;
      background-color: var(--color-film-dark);

      .card-cover-img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        display: block;
        transition: transform 0.6s cubic-bezier(0.25, 1, 0.5, 1), filter 0.3s ease;
      }

      .card-overlay {
        position: absolute;
        inset: 0;
        background: linear-gradient(180deg, transparent 40%, rgba(24, 35, 34, 0.75) 100%);
        opacity: 0;
        display: flex;
        align-items: flex-end;
        justify-content: center;
        padding: 20px;
        transition: opacity 0.3s ease;

        .card-view-btn {
          display: inline-flex;
          align-items: center;
          gap: 6px;
          background-color: var(--color-burgundy);
          color: #FAF7F0;
          font-size: 0.88rem;
          padding: 6px 14px;
          border-radius: var(--radius-xs);
          border: 1px solid var(--color-burgundy-dark);
          transform: translateY(8px);
          transition: transform 0.3s ease;
        }
      }

      .card-featured-badge {
        position: absolute;
        top: 12px;
        left: 12px;
        background-color: var(--color-burgundy);
        color: #FAF7F0;
        font-size: 0.75rem;
        font-weight: 600;
        padding: 3px 8px;
        border-radius: 2px;
        display: inline-flex;
        align-items: center;
        gap: 4px;
        box-shadow: 0 2px 6px rgba(0, 0, 0, 0.25);
        z-index: 2;
      }

      .card-photos-pill {
        position: absolute;
        bottom: 12px;
        right: 12px;
        background-color: rgba(24, 35, 34, 0.85);
        color: #FAF7F0;
        font-size: 0.75rem;
        padding: 3px 8px;
        border-radius: 2px;
        border: 1px solid rgba(255, 255, 255, 0.2);
        display: inline-flex;
        align-items: center;
        gap: 5px;
        z-index: 2;
      }

      &:hover {
        .card-cover-img {
          transform: scale(1.04);
        }

        .card-overlay {
          opacity: 1;

          .card-view-btn {
            transform: translateY(0);
          }
        }
      }
    }

    .card-body {
      padding: 18px 20px;
      display: flex;
      flex-direction: column;
      flex-grow: 1;
    }

    .card-category-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 8px;

      .category-tag {
        font-size: 0.82rem;
        color: var(--color-burgundy);
        font-weight: 600;
        text-transform: uppercase;
        letter-spacing: 0.5px;
        cursor: pointer;

        &:hover {
          text-decoration: underline;
        }
      }

      .card-date {
        font-size: 0.78rem;
        color: var(--color-muted);
      }
    }

    .card-title {
      font-size: 1.35rem;
      font-weight: 600;
      line-height: 1.35;
      margin: 0 0 10px;
      color: var(--color-ink);

      a {
        color: var(--color-ink);
        transition: color 0.2s;

        &:hover {
          color: var(--color-burgundy);
        }
      }
    }

    .card-desc {
      font-size: 0.92rem;
      line-height: 1.6;
      color: var(--color-ink-soft);
      margin: 0 0 16px;
      flex-grow: 1;
    }

    .card-footer-action {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding-top: 12px;
      border-top: 1px solid var(--color-border-light);

      .card-link {
        font-size: 0.9rem;
        font-weight: 500;
        color: var(--color-film-teal);
        display: inline-flex;
        align-items: center;
        gap: 6px;
        transition: all 0.2s;

        i {
          transition: transform 0.2s;
        }

        &:hover {
          color: var(--color-burgundy);

          i {
            transform: translateX(4px);
          }
        }
      }

      .card-studio-mark {
        font-size: 0.8rem;
        color: var(--color-muted);
      }
    }
  }
}

// Empty State
.empty-gallery-state {
  padding: 56px 24px;
  text-align: center;
  background-color: var(--color-paper-light);

  .empty-film-box {
    max-width: 520px;
    margin: 0 auto;
    display: flex;
    flex-direction: column;
    align-items: center;
  }

  .empty-icon {
    font-size: 3rem;
    color: var(--color-muted);
    margin-bottom: 16px;
    opacity: 0.7;
  }

  .empty-title {
    font-size: 1.6rem;
    font-weight: 600;
    margin: 0 0 10px;
    color: var(--color-ink);
  }

  .empty-desc {
    font-size: 1rem;
    line-height: 1.6;
    color: var(--color-ink-soft);
    margin: 0 0 24px;
  }
}

// Pagination
.album-pagination-wrap {
  margin-top: 48px;
  display: flex;
  justify-content: center;

  .pagination-bar {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 8px 16px;
    background-color: var(--color-paper-light);
  }

  .page-nav-btn {
    background: transparent;
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    padding: 6px 14px;
    font-size: 0.88rem;
    color: var(--color-ink);
    cursor: pointer;
    display: inline-flex;
    align-items: center;
    gap: 6px;
    transition: all 0.2s;

    &:hover:not(:disabled) {
      border-color: var(--color-burgundy);
      color: var(--color-burgundy);
    }

    &:disabled {
      opacity: 0.4;
      cursor: not-allowed;
    }
  }

  .page-numbers-list {
    display: flex;
    align-items: center;
    gap: 6px;

    .page-num-btn {
      width: 36px;
      height: 36px;
      display: inline-flex;
      align-items: center;
      justify-content: center;
      background: var(--color-paper);
      border: 1px solid var(--color-border);
      border-radius: var(--radius-xs);
      font-size: 0.85rem;
      color: var(--color-ink);
      cursor: pointer;
      transition: all 0.2s;

      &:hover {
        border-color: var(--color-burgundy);
        color: var(--color-burgundy);
      }

      &.active {
        background-color: var(--color-burgundy);
        border-color: var(--color-burgundy-dark);
        color: #FAF7F0;
        font-weight: 600;
      }
    }
  }
}

// CTA Banner Section
.album-cta-banner-section {
  padding: 40px 0 20px;

  .consult-envelope-box {
    padding: 0;
    overflow: hidden;
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
  }

  .envelope-top-bar {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px 24px;
    background-color: var(--color-film-dark);
    color: #DCD5C8;
    font-size: 0.8rem;

    .stamp-group {
      display: flex;
      align-items: center;
      gap: 10px;

      .post-stamp {
        background-color: var(--color-burgundy);
        color: #FAF7F0;
        padding: 2px 6px;
        border-radius: 2px;
        font-size: 0.85rem;
      }

      .post-code {
        color: #A8A296;
        letter-spacing: 0.5px;
      }
    }

    .post-slogan {
      letter-spacing: 1px;
      color: #A8A296;
      font-size: 0.78rem;
    }
  }

  .envelope-content-grid {
    display: grid;
    grid-template-columns: 1.5fr 1fr;
    gap: 36px;
    padding: clamp(24px, 4vw, 40px);
    align-items: center;

    @media (max-width: 900px) {
      grid-template-columns: 1fr;
      gap: 24px;
    }
  }

  .envelope-info {
    .info-tag {
      font-size: 0.85rem;
      color: var(--color-burgundy);
      letter-spacing: 1.5px;
      display: inline-block;
      margin-bottom: 8px;
    }

    .info-title {
      font-size: clamp(1.4rem, 2.5vw, 1.9rem);
      font-weight: 600;
      line-height: 1.35;
      margin: 0 0 12px;
      color: var(--color-ink);
    }

    .info-desc {
      font-size: 0.98rem;
      line-height: 1.7;
      color: var(--color-ink-soft);
      margin: 0 0 20px;
    }

    .info-contacts {
      display: flex;
      flex-direction: column;
      gap: 8px;
      font-size: 0.92rem;
      color: var(--color-ink-soft);

      .contact-item {
        display: flex;
        align-items: center;
        gap: 8px;

        i {
          color: var(--color-burgundy);
          font-size: 0.85rem;
        }

        strong {
          color: var(--color-ink);
        }
      }
    }
  }

  .envelope-cta-action {
    display: flex;
    flex-direction: column;
    gap: 12px;
    align-items: stretch;

    .cta-big-btn {
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 14px 24px;
      font-size: 1.05rem;
    }

    .cta-outline-btn {
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 12px 24px;
      font-size: 0.95rem;
    }
  }
}

// Error State
.album-error-state {
  min-height: 50vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 60px 0;

  .error-box {
    max-width: 500px;
    padding: 40px 30px;
    text-align: center;
    background-color: var(--color-paper-light);

    .error-icon {
      font-size: 2.8rem;
      color: var(--color-burgundy);
      margin-bottom: 16px;
    }

    .error-title {
      font-size: 1.6rem;
      margin: 0 0 10px;
    }

    .error-desc {
      font-size: 0.95rem;
      color: var(--color-muted);
      margin: 0 0 24px;
    }
  }
}

// Keyframe Animations
@keyframes rec-blink {
  0%, 100% { opacity: 1; }
  50% { opacity: 0.3; }
}
</style>
