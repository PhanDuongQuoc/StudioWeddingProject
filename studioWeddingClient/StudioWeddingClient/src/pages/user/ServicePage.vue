<template>
  <div class="service-page">
    <!-- 1. Unified Vintage Loading State -->
    <VintageLoading v-if="isLoading" text="Đang mở bảng danh mục Dịch vụ Hỷ Sự Studio..." />

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

    <!-- 3. Main Service Listing Content -->
    <main v-else class="service-main-content">
      <!-- Breadcrumb Bar -->
      <section class="service-breadcrumb-bar">
        <div class="studio-container breadcrumb-inner">
          <nav class="breadcrumb-trail" aria-label="Breadcrumb">
            <router-link to="/trang-chu">Trang chủ</router-link>
            <span class="sep">/</span>
            <span class="current-crumb">Dịch vụ</span>
            <span v-if="searchQuery" class="sep">/</span>
            <span v-if="searchQuery" class="cat-crumb">Tìm kiếm: "{{ searchQuery }}"</span>
          </nav>

          <button @click="copyShareLink" class="share-link-btn" title="Sao chép liên kết">
            <i class="fa-solid" :class="isCopied ? 'fa-check' : 'fa-arrow-up-right-from-square'"></i>
            <span>{{ isCopied ? 'Đã sao chép link' : 'Chia sẻ' }}</span>
          </button>
        </div>
      </section>

      <!-- Section 1: Editorial Masthead Hero Header -->
      <section class="service-hero-section">
        <div class="studio-container">
          <div class="hero-masthead paper-card">
            <!-- Film Timecode Strip -->
            <div class="film-timecode-bar">
              <div class="timecode-left">
                <span class="rec-dot"></span>
                <span class="rec-label">REC [●] 1998 — 2026</span>
                <span class="film-sep">·</span>
                <span class="film-stock font-chinese">喜事・特選服務</span>
              </div>
              <div class="timecode-right">
                <span class="archive-tag font-serif">HỶ SỰ SERVICE CATALOG</span>
              </div>
            </div>

            <!-- Masthead Content -->
            <div class="masthead-content">
              <div class="masthead-top-seal">
                <span class="chinese-seal font-chinese">囍</span>
                <span class="seal-sub font-serif">DANH MỤC DỊCH VỤ CƯỚI NGHỆ THUẬT</span>
              </div>

              <h1 class="masthead-title font-serif">
                Nghệ Thuật Ghi Dấu Khoảnh Khắc Bất Tận
              </h1>

              <div class="masthead-divider-line">
                <span class="line-ornament font-chinese">百年好合・永結同心</span>
              </div>

              <p class="masthead-desc font-serif">
                Tại Hỷ Sự Studio, mỗi dịch vụ đều được đầu tư tỉ mỉ từ ánh sáng, góc máy, phục trang cưới cổ điển đến kỹ thuật tinh chỉnh màu phim analog nguyên bản. Chúng tôi trân trọng từng phút giây hạnh phúc của bạn.
              </p>

              <!-- Service Highlights Row -->
              <div class="masthead-stats font-serif">
                <div class="stat-pill">
                  <i class="fa-solid fa-tags"></i>
                  <span>Bảng giá niêm yết rõ ràng</span>
                </div>
                <div class="stat-pill">
                  <i class="fa-solid fa-shirt"></i>
                  <span>Phục trang cưới Vintage 90s độc quyền</span>
                </div>
                <div class="stat-pill">
                  <i class="fa-solid fa-wand-magic-sparkles"></i>
                  <span>Makeup Artist chuyên nghiệp tận tâm</span>
                </div>
                <div class="stat-pill">
                  <i class="fa-solid fa-shield-halved"></i>
                  <span>Bảo hành photobook trọn đời</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 2: Search Controls Bar -->
      <section class="service-filter-section">
        <div class="studio-container">
          <div class="filter-controls-bar paper-card">
            <div class="filter-title-wrap">
              <span class="filter-heading font-serif">
                Tất cả <strong>{{ totalItems }}</strong> dịch vụ cưới chuyên nghiệp
              </span>
              <span class="filter-sub font-serif">Lựa chọn dịch vụ phù hợp nhất cho ngày trọng đại của hai bạn</span>
            </div>

            <!-- Search Input -->
            <div class="filter-search-wrap">
              <div class="vintage-search-box">
                <i class="fa-solid fa-magnifying-glass search-icon"></i>
                <input
                  type="text"
                  v-model="searchInput"
                  placeholder="Tìm kiếm tên dịch vụ, gói chụp..."
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

      <!-- Section 3: Service Cards Catalog Grid -->
      <section class="service-grid-section">
        <div class="studio-container">
          <!-- Empty State -->
          <div v-if="services.length === 0" class="empty-service-state paper-card">
            <div class="empty-film-box">
              <i class="fa-solid fa-list-check empty-icon"></i>
              <h3 class="empty-title font-serif">Không tìm thấy Dịch vụ phù hợp</h3>
              <p class="empty-desc font-serif">
                Không tìm thấy dịch vụ nào khớp với từ khóa "{{ searchQuery }}". Vui lòng thử lại với từ khóa khác.
              </p>
              <button @click="resetSearch" class="btn-vintage font-serif">
                <i class="fa-solid fa-rotate-left q-mr-xs"></i>
                <span>Xem tất cả Dịch vụ</span>
              </button>
            </div>
          </div>

          <!-- Services Grid (3 Columns Desktop, 2 Tablet, 1 Mobile) -->
          <div v-else class="services-grid">
            <article
              v-for="(service, index) in services"
              :key="service.serviceId"
              class="service-card paper-card"
            >
              <!-- Card Film Header -->
              <div class="card-film-header">
                <span class="card-film-idx font-mono">HK · 90s EXP-{{ String((page - 1) * pageSize + index + 1).padStart(2, '0') }}</span>
                <span class="card-film-seal font-chinese">喜事・服務</span>
              </div>

              <!-- Cover Image & Link -->
              <router-link :to="`/dich-vu/${service.slug}`" class="card-img-wrap">
                <img
                  :src="service.imageUrl || defaultServiceCover"
                  :alt="service.name"
                  class="card-img film-photo"
                  loading="lazy"
                />

                <div class="card-overlay">
                  <span class="card-view-btn font-serif">
                    <span>Xem chi tiết dịch vụ</span>
                    <i class="fa-solid fa-arrow-right"></i>
                  </span>
                </div>

                <!-- Price Tag on Image -->
                <div class="card-price-badge font-serif">
                  <span class="price-val">{{ formatCurrency(service.price) }}</span>
                </div>
              </router-link>

              <!-- Card Body Content -->
              <div class="card-body">
                <!-- Meta Info Strip (Duration + Booking Count) -->
                <div class="card-meta-strip font-mono">
                  <span class="meta-item duration" v-if="service.durationMinutes">
                    <i class="fa-regular fa-clock"></i>
                    <span>{{ formatDuration(service.durationMinutes) }}</span>
                  </span>
                  <span class="meta-dot" v-if="service.durationMinutes">·</span>
                  <span class="meta-item bookings">
                    <i class="fa-solid fa-heart"></i>
                    <span>{{ service.countBooking > 0 ? `${service.countBooking}+ tin chọn` : 'Dịch vụ nổi bật' }}</span>
                  </span>
                </div>

                <!-- Title -->
                <h2 class="card-title font-serif">
                  <router-link :to="`/dich-vu/${service.slug}`">
                    {{ service.name }}
                  </router-link>
                </h2>

                <!-- Description excerpt -->
                <p class="card-desc font-serif" v-if="service.description">
                  {{ truncateText(service.description, 110) }}
                </p>

                <!-- Footer Action Buttons -->
                <div class="card-footer-action">
                  <router-link :to="`/dich-vu/${service.slug}`" class="card-link font-serif">
                    <span>Khám phá chi tiết Concept</span>
                    <i class="fa-solid fa-arrow-right"></i>
                  </router-link>

                  <router-link :to="`/trang-chu#lien-he`" class="card-quick-book-btn font-serif" title="Tư vấn dịch vụ này">
                    <span>Tư vấn</span>
                  </router-link>
                </div>
              </div>
            </article>
          </div>

          <!-- Section 4: Vintage Editorial Pagination -->
          <div class="service-pagination-wrap" v-if="totalPages > 1">
            <div class="pagination-bar paper-card">
              <button
                class="page-nav-btn font-serif"
                :disabled="page <= 1"
                @click="setPage(page - 1)"
                title="Trang trước"
              >
                <i class="fa-solid fa-chevron-left"></i>
                <span class="desktop-only">Trang trước</span>
              </button>

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

      <!-- Section 5: Studio Workflow Timeline Section (Quy trình dịch vụ chuyên nghiệp) -->
      <section class="service-workflow-section">
        <div class="studio-container">
          <div class="workflow-box paper-card">
            <div class="workflow-header text-center">
              <span class="workflow-sub font-chinese">喜事・服務流程</span>
              <h2 class="workflow-title font-serif">Quy Trình Phục Vụ Chuẩn Mực Tại Hỷ Sự</h2>
              <div class="workflow-divider"></div>
              <p class="workflow-desc font-serif">
                Chúng tôi mang đến trải nghiệm làm việc chu đáo, rõ ràng và trọn vẹn cảm xúc qua 4 bước đồng hành cùng các cặp đôi.
              </p>
            </div>

            <div class="workflow-steps-grid">
              <div class="workflow-step-item">
                <div class="step-num font-mono">01</div>
                <div class="step-content">
                  <h3 class="step-title font-serif">Lắng nghe & Tư vấn Concept</h3>
                  <p class="step-desc font-serif">
                    Gặp gỡ trực tiếp hoặc online để lắng nghe câu chuyện tình yêu, sở thích trang phục và cùng thống nhất địa điểm chụp ảnh hoàn hảo.
                  </p>
                </div>
              </div>

              <div class="workflow-step-item">
                <div class="step-num font-mono">02</div>
                <div class="step-content">
                  <h3 class="step-title font-serif">Thử Trang Phục & Định Hình Phong Cách</h3>
                  <p class="step-desc font-serif">
                    Trải nghiệm bộ sưu tập áo cưới vintage, áo dài nhung, âu phục thập niên 90 và tư vấn layout makeup phù hợp với từng gương mặt.
                  </p>
                </div>
              </div>

              <div class="workflow-step-item">
                <div class="step-num font-mono">03</div>
                <div class="step-content">
                  <h3 class="step-title font-serif">Ngày Chụp & Bắt Trọn Cảm Xúc</h3>
                  <p class="step-desc font-serif">
                    Ekip nhiếp ảnh gia giàu kinh nghiệm hướng dẫn tạo dáng tự nhiên, bắt trọn từng ánh nhìn và nụ cười chân thật của hai bạn.
                  </p>
                </div>
              </div>

              <div class="workflow-step-item">
                <div class="step-num font-mono">04</div>
                <div class="step-content">
                  <h3 class="step-title font-serif">Tinh Chỉnh Màu Phim & Bàn Giao</h3>
                  <p class="step-desc font-serif">
                    Chỉnh sửa màu film analog theo tone cảm xúc, in ấn photobook chất lượng bảo tàng và gửi toàn bộ file gốc độ phân giải cao.
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 6: Consultation Inquiry & Packages Recommendation Banner -->
      <section class="service-cta-banner-section">
        <div class="studio-container">
          <div class="consult-envelope-box paper-card">
            <div class="envelope-top-bar">
              <div class="stamp-group">
                <span class="post-stamp font-chinese">百年好合</span>
                <span class="post-code font-mono">HỶ SỰ · 1998 STUDIO SERVICE</span>
              </div>
              <span class="post-slogan font-serif">CAM KẾT CHẤT LƯỢNG & TẬN TÂM</span>
            </div>

            <div class="envelope-content-grid">
              <div class="envelope-info">
                <span class="info-tag font-chinese">喜事・套餐特惠</span>
                <h3 class="info-title font-serif">Bạn cần gói dịch vụ trọn gói tiết kiệm hơn?</h3>
                <p class="info-desc font-serif">
                  Bên cạnh các dịch vụ lẻ, Hỷ Sự Studio có các Gói Cưới Trọn Gói (Standard, Premium, VIP) kết hợp đầy đủ Chụp ảnh cưới, Trang phục, Trang điểm và Album photobook cao cấp với mức giá ưu đãi đặc biệt.
                </p>
                <div class="info-benefits-list font-serif">
                  <div class="benefit-item">
                    <i class="fa-solid fa-check"></i>
                    <span>Tặng kèm ảnh phóng lớn tráng gương cao cấp</span>
                  </div>
                  <div class="benefit-item">
                    <i class="fa-solid fa-check"></i>
                    <span>Miễn phí mượn thêm trang phục cưới trong ngày tiệc</span>
                  </div>
                </div>
              </div>

              <div class="envelope-cta-action">
                <router-link to="/trang-chu#goi-cuoi" class="btn-vintage font-serif cta-big-btn">
                  <i class="fa-solid fa-box-archive q-mr-xs"></i>
                  <span>Khám phá các Gói Cưới Combo</span>
                </router-link>
                <router-link to="/trang-chu#lien-he" class="btn-vintage-outline font-serif cta-outline-btn q-mt-sm">
                  <i class="fa-regular fa-paper-plane q-mr-xs"></i>
                  <span>Đăng ký tư vấn miễn phí</span>
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
import { useService } from '@/composables/useService'
import VintageLoading from '@/components/common/VintageLoading.vue'

const route = useRoute()
const router = useRouter()
const $q = useQuasar()

const {
  services,
  searchQuery,
  page,
  pageSize,
  totalItems,
  totalPages,
  isLoading,
  error,
  fetchServices,
  setPage,
  handleSearch,
} = useService()

const searchInput = ref('')
const isCopied = ref(false)
const defaultServiceCover = 'https://images.unsplash.com/photo-1511285560929-80b456fea0bc?auto=format&fit=crop&w=1200&q=80'

// Visible pagination numbers
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

// Format Helpers
function formatCurrency(amount: number): string {
  if (amount == null) return '0 đ'
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount)
}

function formatDuration(minutes?: number | null): string {
  if (!minutes) return ''
  if (minutes < 60) return `${minutes} phút`
  const hours = Math.floor(minutes / 60)
  const remainingMins = minutes % 60
  if (remainingMins === 0) return `${hours} giờ chụp`
  return `${hours}h ${remainingMins}p`
}

function truncateText(text?: string | null, maxLen = 110): string {
  if (!text) return ''
  if (text.length <= maxLen) return text
  return text.slice(0, maxLen).trim() + '...'
}

// Actions
async function onSearchSubmit() {
  await handleSearch(searchInput.value)
  await updateUrlParams()
}

async function clearSearch() {
  searchInput.value = ''
  await handleSearch('')
  await updateUrlParams()
}

async function resetSearch() {
  searchInput.value = ''
  page.value = 1
  await handleSearch('')
  await updateUrlParams()
}

async function updateUrlParams() {
  const query: Record<string, string> = {}
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
      message: 'Đã sao chép liên kết trang Dịch vụ vào bộ nhớ tạm!',
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

  await fetchServices()
})

// Watch route changes
watch(
  () => route.query,
  async (newQuery) => {
    const q = (newQuery.q as string) || ''
    const p = parseInt((newQuery.p as string) || '1', 10)

    if (q !== searchQuery.value || p !== page.value) {
      searchQuery.value = q
      searchInput.value = q
      page.value = isNaN(p) ? 1 : p
      await fetchServices()
    }
  }
)
</script>

<style lang="scss" scoped>
.service-page {
  min-height: 100vh;
  background-color: var(--color-paper);
  color: var(--color-ink);
  padding-bottom: 80px;
}

// Breadcrumb Bar
.service-breadcrumb-bar {
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
.service-hero-section {
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

// Filter Section
.service-filter-section {
  padding: 8px 0 24px;

  .filter-controls-bar {
    padding: 18px 24px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 20px;
    flex-wrap: wrap;
    background-color: var(--color-paper-light);
  }

  .filter-title-wrap {
    display: flex;
    flex-direction: column;
    gap: 4px;

    .filter-heading {
      font-size: 1.15rem;
      font-weight: 600;
      color: var(--color-ink);

      strong {
        color: var(--color-burgundy);
      }
    }

    .filter-sub {
      font-size: 0.88rem;
      color: var(--color-muted);
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

// Services Grid
.service-grid-section {
  padding: 10px 0 40px;

  .services-grid {
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

  // Service Card
  .service-card {
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

    .card-img-wrap {
      position: relative;
      display: block;
      width: 100%;
      aspect-ratio: 16 / 10;
      overflow: hidden;
      background-color: var(--color-film-dark);

      .card-img {
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

      .card-price-badge {
        position: absolute;
        bottom: 12px;
        left: 12px;
        background-color: var(--color-burgundy);
        color: #FAF7F0;
        padding: 4px 10px;
        border-radius: 2px;
        border: 1px solid var(--color-burgundy-dark);
        font-size: 0.95rem;
        font-weight: 600;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.3);
        z-index: 2;
      }

      &:hover {
        .card-img {
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
      padding: 20px;
      display: flex;
      flex-direction: column;
      flex-grow: 1;
    }

    .card-meta-strip {
      display: flex;
      align-items: center;
      gap: 8px;
      font-size: 0.8rem;
      color: var(--color-muted);
      margin-bottom: 10px;

      .meta-item {
        display: inline-flex;
        align-items: center;
        gap: 5px;

        i {
          font-size: 0.75rem;
        }

        &.bookings {
          color: var(--color-burgundy);
          font-weight: 600;
        }
      }

      .meta-dot {
        color: var(--color-border-dark);
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
      margin: 0 0 18px;
      flex-grow: 1;
    }

    .card-footer-action {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding-top: 14px;
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

      .card-quick-book-btn {
        background-color: transparent;
        color: var(--color-burgundy);
        border: 1px solid var(--color-burgundy);
        border-radius: var(--radius-xs);
        padding: 4px 10px;
        font-size: 0.82rem;
        transition: all 0.2s;

        &:hover {
          background-color: var(--color-burgundy);
          color: #FAF7F0;
        }
      }
    }
  }
}

// Empty State
.empty-service-state {
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
.service-pagination-wrap {
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

// Workflow Process Section
.service-workflow-section {
  padding: 40px 0 20px;

  .workflow-box {
    padding: clamp(32px, 5vw, 56px) clamp(20px, 4vw, 48px);
    background-color: var(--color-paper-light);
  }

  .workflow-header {
    max-width: 680px;
    margin: 0 auto 40px;

    .workflow-sub {
      font-size: 0.88rem;
      color: var(--color-burgundy);
      letter-spacing: 2px;
      display: block;
      margin-bottom: 8px;
    }

    .workflow-title {
      font-size: clamp(1.6rem, 3.2vw, 2.3rem);
      font-weight: 600;
      line-height: 1.3;
      margin: 0 0 16px;
      color: var(--color-ink);
    }

    .workflow-divider {
      width: 60px;
      height: 2px;
      background-color: var(--color-burgundy);
      margin: 0 auto 16px;
    }

    .workflow-desc {
      font-size: 1.02rem;
      line-height: 1.65;
      color: var(--color-ink-soft);
      margin: 0;
    }
  }

  .workflow-steps-grid {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 28px;

    @media (max-width: 1024px) {
      grid-template-columns: repeat(2, 1fr);
      gap: 24px;
    }

    @media (max-width: 640px) {
      grid-template-columns: 1fr;
      gap: 20px;
    }
  }

  .workflow-step-item {
    background-color: var(--color-paper);
    border: 1px solid var(--color-border);
    padding: 24px 20px;
    border-radius: var(--radius-xs);
    position: relative;
    display: flex;
    flex-direction: column;

    .step-num {
      font-size: 2.2rem;
      font-weight: 700;
      color: var(--color-burgundy);
      opacity: 0.85;
      line-height: 1;
      margin-bottom: 14px;
      border-bottom: 1px solid var(--color-border-light);
      padding-bottom: 8px;
    }

    .step-title {
      font-size: 1.18rem;
      font-weight: 600;
      line-height: 1.35;
      margin: 0 0 10px;
      color: var(--color-ink);
    }

    .step-desc {
      font-size: 0.9rem;
      line-height: 1.6;
      color: var(--color-ink-soft);
      margin: 0;
    }
  }
}

// CTA Banner Section
.service-cta-banner-section {
  padding: 30px 0 20px;

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

    .info-benefits-list {
      display: flex;
      flex-direction: column;
      gap: 8px;
      font-size: 0.92rem;
      color: var(--color-ink-soft);

      .benefit-item {
        display: flex;
        align-items: center;
        gap: 8px;

        i {
          color: var(--color-burgundy);
          font-size: 0.85rem;
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
.service-error-state {
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
