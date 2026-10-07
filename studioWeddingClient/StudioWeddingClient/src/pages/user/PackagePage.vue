<template>
  <div class="package-page">
    <!-- 1. Unified Vintage Loading State -->
    <VintageLoading v-if="isLoading" text="Đang mở bảng danh mục Gói Cưới Hỷ Sự Studio..." />

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

    <!-- 3. Main Package Content -->
    <main v-else class="package-main-content">
      <!-- Breadcrumb Bar -->
      <section class="package-breadcrumb-bar">
        <div class="studio-container breadcrumb-inner">
          <nav class="breadcrumb-trail" aria-label="Breadcrumb">
            <router-link to="/trang-chu">Trang chủ</router-link>
            <span class="sep">/</span>
            <span class="current-crumb">Gói cưới</span>
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
      <section class="package-hero-section">
        <div class="studio-container">
          <div class="hero-masthead paper-card">
            <!-- Film Timecode Strip -->
            <div class="film-timecode-bar">
              <div class="timecode-left">
                <span class="rec-dot"></span>
                <span class="rec-label">REC [●] 1998 — 2026</span>
                <span class="film-sep">·</span>
                <span class="film-stock font-chinese">喜事・婚紗套餐</span>
              </div>
              <div class="timecode-right">
                <span class="archive-tag font-serif">HỶ SỰ PACKAGE ARCHIVE</span>
              </div>
            </div>

            <!-- Masthead Content -->
            <div class="masthead-content">
              <div class="masthead-top-seal">
                <span class="chinese-seal font-chinese">囍</span>
                <span class="seal-sub font-serif">BỘ SƯU TẬP GÓI CƯỚI TRỌN GÓI</span>
              </div>

              <h1 class="masthead-title font-serif">
                Gói Cưới Nghệ Thuật / Trọn Vẹn Từng Khoảnh Khắc
              </h1>

              <div class="masthead-divider-line">
                <span class="line-ornament font-chinese">百年好合・永結同心</span>
              </div>

              <p class="masthead-desc font-serif">
                Các gói cưới combo được thiết kế tối ưu, kết hợp trọn vẹn giữa chụp ảnh cưới nghệ thuật, trang phục vintage cao cấp, makeup chuyên nghiệp và album photobook mỹ thuật — giúp hai bạn an tâm tận hưởng ngày vui với mức chi phí hợp lý nhất.
              </p>

              <!-- Package Benefits Stats -->
              <div class="masthead-stats font-serif">
                <div class="stat-pill">
                  <i class="fa-solid fa-percent"></i>
                  <span>Tiết kiệm đến 30% so với đặt dịch vụ lẻ</span>
                </div>
                <div class="stat-pill">
                  <i class="fa-solid fa-gem"></i>
                  <span>Tặng Photobook & Ảnh cổng tráng gương</span>
                </div>
                <div class="stat-pill">
                  <i class="fa-solid fa-ban"></i>
                  <span>Cam kết không phát sinh chi phí</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 2: Search Controls Bar -->
      <section class="package-filter-section">
        <div class="studio-container">
          <div class="filter-controls-bar paper-card">
            <div class="filter-title-wrap">
              <span class="filter-heading font-serif">
                Tuyển tập <strong>{{ totalItems }}</strong> gói dịch vụ cưới tối ưu
              </span>
              <span class="filter-sub font-serif">Đầy đủ quyền lợi, minh bạch về giá và chất lượng phục vụ</span>
            </div>

            <!-- Search Input -->
            <div class="filter-search-wrap">
              <div class="vintage-search-box">
                <i class="fa-solid fa-magnifying-glass search-icon"></i>
                <input
                  type="text"
                  v-model="searchInput"
                  placeholder="Tìm kiếm tên gói cưới (Tiêu chuẩn, Cao cấp, VIP)..."
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

      <!-- Section 3: Package Cards Catalog Grid -->
      <section class="package-grid-section">
        <div class="studio-container">
          <!-- Empty State -->
          <div v-if="packages.length === 0" class="empty-package-state paper-card">
            <div class="empty-film-box">
              <i class="fa-solid fa-box-open empty-icon"></i>
              <h3 class="empty-title font-serif">Không tìm thấy Gói Cưới phù hợp</h3>
              <p class="empty-desc font-serif">
                Không tìm thấy gói cưới nào khớp với từ khóa "{{ searchQuery }}". Vui lòng thử lại với từ khóa khác.
              </p>
              <button @click="resetSearch" class="btn-vintage font-serif">
                <i class="fa-solid fa-rotate-left q-mr-xs"></i>
                <span>Xem tất cả Gói Cưới</span>
              </button>
            </div>
          </div>

          <!-- Packages Grid (3 Columns Desktop, 2 Tablet, 1 Mobile) -->
          <div v-else class="packages-grid">
            <article
              v-for="(pkg, index) in packages"
              :key="pkg.packageId"
              class="package-card paper-card"
              :class="{ 'featured-package': pkg.isPopular }"
            >
              <!-- Featured Popular Ribbon -->
              <div v-if="pkg.isPopular" class="popular-ribbon font-serif">
                <i class="fa-solid fa-crown q-mr-xs"></i>
                <span>ĐẶC BIỆT TIN CHỌN</span>
              </div>

              <!-- Card Film Header -->
              <div class="card-film-header">
                <span class="card-film-idx font-mono">HK · 90s PKG-{{ String((page - 1) * pageSize + index + 1).padStart(2, '0') }}</span>
                <span class="card-film-seal font-chinese">喜事・特選</span>
              </div>

              <!-- Cover Image & Link -->
              <router-link :to="`/goi-cuoi/${pkg.slug}`" class="card-img-wrap">
                <img
                  :src="pkg.imageUrl || defaultPackageCover"
                  :alt="pkg.name"
                  class="card-img film-photo"
                  loading="lazy"
                />

                <div class="card-overlay">
                  <span class="card-view-btn font-serif">
                    <span>Xem chi tiết gói cưới</span>
                    <i class="fa-solid fa-arrow-right"></i>
                  </span>
                </div>

                <!-- Booking Count Badge -->
                <div class="card-bookings-badge font-mono">
                  <i class="fa-solid fa-heart"></i>
                  <span>{{ pkg.countBooking > 0 ? `${pkg.countBooking}+ cặp đôi chọn` : 'Gói cưới ưu tú' }}</span>
                </div>
              </router-link>

              <!-- Card Body Content -->
              <div class="card-body">
                <!-- Package Title -->
                <h2 class="package-name font-serif">
                  <router-link :to="`/goi-cuoi/${pkg.slug}`">
                    {{ pkg.name }}
                  </router-link>
                </h2>

                <!-- Price Box -->
                <div class="package-price-box">
                  <div class="price-row">
                    <span class="current-price font-serif">{{ formatCurrency(pkg.price) }}</span>
                    <span class="price-unit font-serif">/ trọn gói</span>
                  </div>

                  <!-- Original Price & Savings if applicable -->
                  <div class="savings-row font-serif" v-if="pkg.originalTotalPrice && pkg.originalTotalPrice > pkg.price">
                    <span class="original-price">Giá gốc: {{ formatCurrency(pkg.originalTotalPrice) }}</span>
                    <span class="savings-badge">Tiết kiệm {{ formatCurrency(pkg.originalTotalPrice - pkg.price) }}</span>
                  </div>
                </div>

                <!-- Description excerpt -->
                <p class="package-desc font-serif" v-if="pkg.description">
                  {{ truncateText(pkg.description, 100) }}
                </p>

                <!-- Included Benefits / Services Checklist -->
                <div class="included-services-box">
                  <span class="box-title font-serif">Đặc quyền bao gồm trong gói:</span>
                  <ul class="services-list font-serif">
                    <li
                      v-for="(service, sIdx) in (pkg.includedServices && pkg.includedServices.length > 0 ? pkg.includedServices.slice(0, 5) : pkg.includedServiceNames.slice(0, 5))"
                      :key="sIdx"
                      class="service-item"
                    >
                      <i class="fa-solid fa-circle-check check-icon"></i>
                      <span>
                        {{ typeof service === 'string' ? service : service.name }}
                        <em v-if="typeof service !== 'string' && service.quantity > 1" class="qty font-mono">(x{{ service.quantity }})</em>
                      </span>
                    </li>
                    <li v-if="(pkg.includedServices?.length || pkg.includedServiceNames?.length || 0) > 5" class="more-services font-serif">
                      <span>+ thêm {{ (pkg.includedServices?.length || pkg.includedServiceNames?.length || 0) - 5 }} quyền lợi & quà tặng khác...</span>
                    </li>
                  </ul>
                </div>

                <!-- Footer Action Buttons -->
                <div class="card-actions-row">
                  <router-link :to="`/goi-cuoi/${pkg.slug}`" class="btn-vintage full-width font-serif main-cta-btn">
                    <span>Khám phá trọn bộ quyền lợi</span>
                    <i class="fa-solid fa-arrow-right q-ml-xs"></i>
                  </router-link>

                  <router-link to="/trang-chu#lien-he" class="btn-vintage-outline full-width font-serif consult-cta-btn q-mt-sm">
                    <i class="fa-regular fa-calendar-check q-mr-xs"></i>
                    <span>Tư vấn & Giữ ưu đãi gói này</span>
                  </router-link>
                </div>
              </div>
            </article>
          </div>

          <!-- Section 4: Vintage Editorial Pagination -->
          <div class="package-pagination-wrap" v-if="totalPages > 1">
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

      <!-- Section 5: Editorial Feature Comparison Table -->
      <section class="package-comparison-section">
        <div class="studio-container">
          <div class="comparison-box paper-card">
            <div class="comp-header text-center">
              <span class="comp-sub font-chinese">喜事・套餐對比</span>
              <h2 class="comp-title font-serif">Bảng So Sánh Quyền Lợi Các Gói Cưới</h2>
              <div class="comp-divider"></div>
              <p class="comp-desc font-serif">
                So sánh chi tiết các hạng mục quyền lợi giữa các gói để dễ dàng lựa chọn gói cưới phù hợp nhất với ngân sách và mong muốn của hai bạn.
              </p>
            </div>

            <div class="table-responsive-wrapper">
              <table class="vintage-comparison-table font-serif">
                <thead>
                  <tr>
                    <th class="feature-col">Hạng mục quyền lợi</th>
                    <th class="pkg-col">Gói Tiêu Chuẩn</th>
                    <th class="pkg-col highlight-col">Gói Cao Cấp (Khuyên dùng)</th>
                    <th class="pkg-col">Gói VIP Nghệ Thuật</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td class="feature-name">Số địa điểm & concept</td>
                    <td>1 bối cảnh (Studio)</td>
                    <td class="highlight-cell">2 bối cảnh (Studio + Phim trường / Phố cổ)</td>
                    <td>3 bối cảnh (Studio + Ngoại cảnh Sài Gòn / Đà Lạt)</td>
                  </tr>
                  <tr>
                    <td class="feature-name">Thời lượng thực hiện</td>
                    <td>Nửa ngày (~ 3 - 4 giờ)</td>
                    <td class="highlight-cell">Trọn 1 ngày (~ 7 - 8 giờ)</td>
                    <td>Trọn 1 - 2 ngày (Ngoại cảnh xa)</td>
                  </tr>
                  <tr>
                    <td class="feature-name">Phục trang cô dâu chú rể</td>
                    <td>2 váy cưới vintage + 2 vest âu</td>
                    <td class="highlight-cell">3 - 4 trang phục vintage + Áo dài nhung cổ</td>
                    <td>Trang phục không giới hạn + Thiết kế riêng</td>
                  </tr>
                  <tr>
                    <td class="feature-name">Makeup & Làm tóc</td>
                    <td>1 layout makeup theo concept</td>
                    <td class="highlight-cell">2 - 3 layout makeup thay đổi linh hoạt</td>
                    <td>M.U.A theo sát dặm phấn suốt buổi chụp</td>
                  </tr>
                  <tr>
                    <td class="feature-name">Album Photobook cao cấp</td>
                    <td>Size 25x25 (20 trang)</td>
                    <td class="highlight-cell">Size 30x30 mở phẳng HD (30 trang)</td>
                    <td>Size 30x45 bìa da thủ công (40 trang)</td>
                  </tr>
                  <tr>
                    <td class="feature-name">Ảnh phóng lớn tráng gương</td>
                    <td>1 ảnh cổng 60x90cm</td>
                    <td class="highlight-cell">2 ảnh cổng 60x90cm pha lê cao cấp</td>
                    <td>2 ảnh cổng 70x110cm + Bộ 10 ảnh để bàn</td>
                  </tr>
                  <tr>
                    <td class="feature-name">Bàn giao toàn bộ file gốc</td>
                    <td><i class="fa-solid fa-check text-burgundy"></i> Có</td>
                    <td class="highlight-cell"><i class="fa-solid fa-check text-burgundy"></i> Có</td>
                    <td><i class="fa-solid fa-check text-burgundy"></i> Có</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 6: Consultation & Booking Inquiry Banner -->
      <section class="package-cta-banner-section">
        <div class="studio-container">
          <div class="consult-envelope-box paper-card">
            <div class="envelope-top-bar">
              <div class="stamp-group">
                <span class="post-stamp font-chinese">百年好合</span>
                <span class="post-code font-mono">HK-VN · WEDDING PACKAGE 1998</span>
              </div>
              <span class="post-slogan font-serif">HỶ SỰ WEDDING STUDIO ARCHIVE</span>
            </div>

            <div class="envelope-content-grid">
              <div class="envelope-info">
                <span class="info-tag font-chinese">喜事・專屬諮詢</span>
                <h3 class="info-title font-serif">Bạn cần thiết kế một gói cưới theo yêu cầu riêng?</h3>
                <p class="info-desc font-serif">
                  Nếu bạn có mong muốn chụp tại những địa điểm ngoại tỉnh đặc biệt, hoặc muốn điều chỉnh số lượng trang phục và trang điểm theo lịch trình riêng của gia đình, hãy liên hệ với chúng tôi để được tư vấn gói cá nhân hóa tốt nhất.
                </p>
                <div class="info-contacts font-serif">
                  <div class="contact-item">
                    <i class="fa-solid fa-phone"></i>
                    <span>Hotline tư vấn nhanh: <strong>+84 912 345 678</strong></span>
                  </div>
                  <div class="contact-item">
                    <i class="fa-solid fa-envelope"></i>
                    <span>Email hỗ trợ: <strong>hello@weddingstudio.vn</strong></span>
                  </div>
                </div>
              </div>

              <div class="envelope-cta-action">
                <router-link to="/trang-chu#lien-he" class="btn-vintage font-serif cta-big-btn">
                  <i class="fa-regular fa-paper-plane q-mr-xs"></i>
                  <span>Gửi yêu cầu nhận báo giá chi tiết</span>
                </router-link>
                <router-link to="/album" class="btn-vintage-outline font-serif cta-outline-btn q-mt-sm">
                  <i class="fa-solid fa-images q-mr-xs"></i>
                  <span>Xem các Album đã thực hiện</span>
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
import { usePackage } from '@/composables/usePackage'
import VintageLoading from '@/components/common/VintageLoading.vue'

const route = useRoute()
const router = useRouter()
const $q = useQuasar()

const {
  packages,
  searchQuery,
  page,
  pageSize,
  totalItems,
  totalPages,
  isLoading,
  error,
  fetchPackages,
  setPage,
  handleSearch,
} = usePackage()

const searchInput = ref('')
const isCopied = ref(false)
const defaultPackageCover = 'https://images.unsplash.com/photo-1519741497674-611481863552?auto=format&fit=crop&w=1200&q=80'

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
function formatCurrency(amount?: number | null): string {
  if (amount == null) return '0 đ'
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount)
}

function truncateText(text?: string | null, maxLen = 100): string {
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
      message: 'Đã sao chép liên kết trang Gói Cưới vào bộ nhớ tạm!',
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

  await fetchPackages()
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
      await fetchPackages()
    }
  }
)
</script>

<style lang="scss" scoped>
.package-page {
  min-height: 100vh;
  background-color: var(--color-paper);
  color: var(--color-ink);
  padding-bottom: 80px;
}

// Breadcrumb Bar
.package-breadcrumb-bar {
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
.package-hero-section {
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
.package-filter-section {
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

// Packages Grid
.package-grid-section {
  padding: 10px 0 40px;

  .packages-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 32px;

    @media (max-width: 1100px) {
      grid-template-columns: repeat(2, 1fr);
      gap: 24px;
    }

    @media (max-width: 680px) {
      grid-template-columns: 1fr;
      gap: 24px;
    }
  }

  // Package Card
  .package-card {
    position: relative;
    display: flex;
    flex-direction: column;
    overflow: hidden;
    border: 1px solid var(--color-border);
    background-color: var(--color-paper-light);
    border-radius: var(--radius-xs);

    &.featured-package {
      border: 2px solid var(--color-burgundy);
      box-shadow: 0 4px 16px rgba(142, 41, 41, 0.12);

      .popular-ribbon {
        display: flex;
      }
    }

    .popular-ribbon {
      display: none;
      align-items: center;
      justify-content: center;
      background-color: var(--color-burgundy);
      color: #FAF7F0;
      font-size: 0.78rem;
      font-weight: 600;
      padding: 5px 12px;
      letter-spacing: 1px;
    }

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

      .card-bookings-badge {
        position: absolute;
        top: 12px;
        right: 12px;
        background-color: rgba(24, 35, 34, 0.88);
        color: #FAF7F0;
        font-size: 0.75rem;
        padding: 4px 8px;
        border-radius: 2px;
        border: 1px solid rgba(255, 255, 255, 0.2);
        display: inline-flex;
        align-items: center;
        gap: 5px;
        z-index: 2;

        i {
          color: #E24B4B;
        }
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
      padding: 24px 20px 20px;
      display: flex;
      flex-direction: column;
      flex-grow: 1;
    }

    .package-name {
      font-size: 1.45rem;
      font-weight: 600;
      line-height: 1.3;
      margin: 0 0 12px;
      color: var(--color-ink);

      a {
        color: var(--color-ink);
        transition: color 0.2s;

        &:hover {
          color: var(--color-burgundy);
        }
      }
    }

    .package-price-box {
      padding-bottom: 14px;
      margin-bottom: 14px;
      border-bottom: 1px solid var(--color-border-light);

      .price-row {
        display: flex;
        align-items: baseline;
        gap: 6px;

        .current-price {
          font-size: 1.65rem;
          font-weight: 700;
          color: var(--color-burgundy);
          line-height: 1;
        }

        .price-unit {
          font-size: 0.88rem;
          color: var(--color-muted);
        }
      }

      .savings-row {
        display: flex;
        align-items: center;
        gap: 8px;
        margin-top: 6px;
        font-size: 0.82rem;

        .original-price {
          color: var(--color-muted);
          text-decoration: line-through;
        }

        .savings-badge {
          background-color: rgba(142, 41, 41, 0.08);
          color: var(--color-burgundy);
          padding: 1px 6px;
          border-radius: 2px;
          font-weight: 600;
        }
      }
    }

    .package-desc {
      font-size: 0.92rem;
      line-height: 1.6;
      color: var(--color-ink-soft);
      margin: 0 0 16px;
    }

    .included-services-box {
      margin-bottom: 20px;
      flex-grow: 1;

      .box-title {
        font-size: 0.88rem;
        font-weight: 600;
        color: var(--color-ink);
        display: block;
        margin-bottom: 10px;
        text-transform: uppercase;
        letter-spacing: 0.5px;
      }

      .services-list {
        list-style: none;
        padding: 0;
        margin: 0;
        display: flex;
        flex-direction: column;
        gap: 8px;

        .service-item {
          display: flex;
          align-items: flex-start;
          gap: 8px;
          font-size: 0.9rem;
          line-height: 1.45;
          color: var(--color-ink-soft);

          .check-icon {
            color: var(--color-burgundy);
            font-size: 0.85rem;
            margin-top: 3px;
            flex-shrink: 0;
          }

          .qty {
            color: var(--color-burgundy);
            font-size: 0.82rem;
            font-weight: 600;
          }
        }

        .more-services {
          font-size: 0.85rem;
          color: var(--color-muted);
          font-style: italic;
          padding-left: 22px;
        }
      }
    }

    .card-actions-row {
      display: flex;
      flex-direction: column;
      padding-top: 14px;
      border-top: 1px solid var(--color-border-light);

      .main-cta-btn {
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 10px;
        font-size: 0.95rem;
      }

      .consult-cta-btn {
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 8px;
        font-size: 0.88rem;
      }
    }
  }
}

// Comparison Section
.package-comparison-section {
  padding: 40px 0 20px;

  .comparison-box {
    padding: clamp(32px, 5vw, 56px) clamp(20px, 4vw, 48px);
    background-color: var(--color-paper-light);
  }

  .comp-header {
    max-width: 680px;
    margin: 0 auto 36px;

    .comp-sub {
      font-size: 0.88rem;
      color: var(--color-burgundy);
      letter-spacing: 2px;
      display: block;
      margin-bottom: 8px;
    }

    .comp-title {
      font-size: clamp(1.6rem, 3.2vw, 2.3rem);
      font-weight: 600;
      line-height: 1.3;
      margin: 0 0 16px;
      color: var(--color-ink);
    }

    .comp-divider {
      width: 60px;
      height: 2px;
      background-color: var(--color-burgundy);
      margin: 0 auto 16px;
    }

    .comp-desc {
      font-size: 1.02rem;
      line-height: 1.65;
      color: var(--color-ink-soft);
      margin: 0;
    }
  }

  .table-responsive-wrapper {
    overflow-x: auto;
    border: 1px solid var(--color-border);
    background-color: var(--color-paper);
  }

  .vintage-comparison-table {
    width: 100%;
    border-collapse: collapse;
    text-align: left;
    min-width: 680px;

    th, td {
      padding: 14px 18px;
      border-bottom: 1px solid var(--color-border-light);
      font-size: 0.92rem;
    }

    th {
      background-color: var(--color-paper-dark);
      color: var(--color-ink);
      font-weight: 600;
      font-size: 0.98rem;
      border-bottom: 2px solid var(--color-border);

      &.highlight-col {
        background-color: rgba(142, 41, 41, 0.08);
        color: var(--color-burgundy);
      }
    }

    tbody tr:hover {
      background-color: rgba(255, 255, 255, 0.6);
    }

    .feature-name {
      font-weight: 600;
      color: var(--color-ink);
      width: 25%;
    }

    .highlight-cell {
      background-color: rgba(142, 41, 41, 0.03);
      font-weight: 500;
    }

    .text-burgundy {
      color: var(--color-burgundy);
    }
  }
}

// CTA Banner Section
.package-cta-banner-section {
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

// Empty & Error States
.empty-package-state {
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

.package-error-state {
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

// Pagination
.package-pagination-wrap {
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

// Keyframe Animations
@keyframes rec-blink {
  0%, 100% { opacity: 1; }
  50% { opacity: 0.3; }
}
</style>
