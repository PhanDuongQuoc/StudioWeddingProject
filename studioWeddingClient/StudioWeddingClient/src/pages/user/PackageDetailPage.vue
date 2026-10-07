<template>
  <div class="package-detail-page">
    <!-- 1. Unified Loading State -->
    <VintageLoading v-if="isLoading" text="Đang tải thông tin gói cưới..." />

    <!-- 2. Error / Not Found State -->
    <div v-else-if="error || !pkg" class="package-error-state studio-container">
      <div class="error-box paper-card">
        <i class="fa-solid fa-circle-exclamation error-icon"></i>
        <h2 class="error-title font-serif">Không tìm thấy Gói Cưới</h2>
        <p class="error-desc">{{ error || 'Gói cưới bạn đang tìm kiếm không tồn tại hoặc đã ngừng áp dụng.' }}</p>
        <router-link to="/goi-cuoi" class="btn-vintage back-btn font-serif">
          <i class="fa-solid fa-arrow-left"></i>
          <span>Quay lại trang Gói cưới</span>
        </router-link>
      </div>
    </div>

    <!-- 3. Main Package Content State -->
    <main v-else class="package-main-content">
      <!-- Breadcrumb Bar -->
      <section class="package-breadcrumb-bar">
        <div class="studio-container breadcrumb-inner">
          <nav class="breadcrumb-trail" aria-label="Breadcrumb">
            <router-link to="/trang-chu">Trang chủ</router-link>
            <span class="sep">/</span>
            <router-link to="/goi-cuoi">Gói cưới</router-link>
            <span class="sep">/</span>
            <span class="current-crumb">{{ pkg.name }}</span>
          </nav>

          <button @click="copyShareLink" class="share-link-btn" title="Sao chép liên kết">
            <i class="fa-solid" :class="isCopied ? 'fa-check' : 'fa-arrow-up-right-from-square'"></i>
            <span>{{ isCopied ? 'Đã sao chép link' : 'Chia sẻ' }}</span>
          </button>
        </div>
      </section>

      <!-- Hero Showcase Section (Editorial Split) -->
      <section class="package-hero-section">
        <div class="studio-container">
          <div class="package-hero-grid">
            <!-- Left: Visual Poster with Film Borders -->
            <div class="hero-poster-wrapper">
              <div class="poster-frame paper-card">
                <div class="film-stamp-badge">
                  <span class="stamp-chinese font-chinese">喜事・套餐特選</span>
                  <span class="stamp-meta">EST. 1998</span>
                </div>
                <div class="poster-image-box">
                  <img
                    :src="pkg.imageUrl || defaultPkgImg"
                    :alt="pkg.name"
                    class="poster-img film-photo"
                  />
                  <!-- Floating Popular Badge if high booking -->
                  <div class="poster-badge font-serif" v-if="pkg.countBooking >= 10 || pkg.slug.includes('cao-cap') || pkg.slug.includes('vip')">
                    <span class="star">★</span> GÓI ĐƯỢC YÊU THÍCH NHẤT
                  </div>
                </div>
                <div class="poster-footer-caption font-serif">
                  <span>HỶ SỰ WEDDING ARCHIVE</span>
                  <span class="dot">·</span>
                  <span>FULL PACKAGE COMBO</span>
                </div>
              </div>
            </div>

            <!-- Right: Package Details & Meta Strip -->
            <div class="hero-info-wrapper">
              <div class="package-top-badge">
                <span class="chinese-seal font-chinese">囍</span>
                <span class="badge-text font-serif">GÓI DỊCH VỤ CƯỚI TRỌN GÓI</span>
              </div>

              <h1 class="package-main-title font-serif">{{ pkg.name }}</h1>

              <!-- Meta Pricing & Savings Bar -->
              <div class="package-meta-strip paper-card">
                <div class="meta-strip-item">
                  <span class="meta-strip-label">Giá trọn gói ưu đãi</span>
                  <div class="price-wrap">
                    <span class="meta-strip-value price font-serif">{{ formatCurrency(pkg.price) }}</span>
                    <span
                      class="original-price font-serif"
                      v-if="pkg.originalTotalPrice && pkg.originalTotalPrice > pkg.price"
                    >
                      {{ formatCurrency(pkg.originalTotalPrice) }}
                    </span>
                  </div>
                </div>

                <div class="meta-strip-divider" v-if="savingsPercent > 0"></div>
                <div class="meta-strip-item" v-if="savingsPercent > 0">
                  <span class="meta-strip-label">Mức độ ưu đãi</span>
                  <span class="meta-strip-value savings-highlight font-serif">
                    <i class="fa-solid fa-tags q-mr-xs"></i>
                    Tiết kiệm {{ savingsPercent }}%
                  </span>
                </div>

                <div class="meta-strip-divider"></div>
                <div class="meta-strip-item">
                  <span class="meta-strip-label">Mức độ tin chọn</span>
                  <span class="meta-strip-value booking-highlight">
                    <i class="fa-solid fa-heart q-mr-xs"></i>
                    {{ pkg.countBooking > 0 ? `${pkg.countBooking}+ cặp đôi` : 'Được tin chọn nhiều' }}
                  </span>
                </div>
              </div>

              <!-- Package Description -->
              <div class="package-desc-box">
                <div class="quote-mark font-serif">“</div>
                <p class="package-desc-text font-serif">
                  {{ pkg.description || 'Gói dịch vụ cưới trọn gói cao cấp tại Hỷ Sự Studio mang đến giải pháp toàn diện từ trang phục, trang điểm cô dâu, chụp ảnh ngoại cảnh/phim trường đến toàn bộ album photobook in ấn mỹ thuật lưu giữ trọn đời.' }}
                </p>
              </div>

              <!-- Action Buttons -->
              <div class="hero-actions-row">
                <q-btn
                  @click="openBookingModal"
                  class="btn-vintage btn-booking-cta font-serif"
                  unelevated
                  no-caps
                >
                  <i class="fa-regular fa-calendar-check q-mr-sm"></i>
                  <span>Đặt Lịch Tư Vấn Gói Này</span>
                </q-btn>

                <a
                  href="tel:0901234567"
                  class="btn-vintage-outline btn-contact-cta font-serif"
                >
                  <i class="fa-solid fa-phone q-mr-sm"></i>
                  <span>Hotline: 0901 234 567</span>
                </a>
              </div>

              <div class="consultation-note font-serif">
                <span class="diamond-icon">✦</span>
                <span>Cam kết không phát sinh chi phí. Tặng kèm gói tư vấn concept và thử trang phục miễn phí tại Studio.</span>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- 4. Included Services Section (Đặc quyền dịch vụ có trong gói) -->
      <section class="included-services-section">
        <div class="studio-container">
          <div class="section-title-wrap">
            <span class="sub-title font-chinese">喜事・套餐權益</span>
            <h2 class="main-title font-serif">Đặc Quyền & Dịch Vụ Bao Gồm Trong Gói</h2>
            <p class="section-sub-desc font-serif">
              Trọn bộ {{ pkg.includedServices?.length || 0 }} dịch vụ cao cấp được thiết kế đồng bộ cho ngày cưới hoàn hảo
            </p>
            <div class="title-divider"><span class="diamond">✦</span></div>
          </div>

          <!-- Services Grid List -->
          <div class="included-services-grid" v-if="pkg.includedServices && pkg.includedServices.length > 0">
            <div
              v-for="(service, idx) in pkg.includedServices"
              :key="service.serviceId || idx"
              class="included-service-card paper-card animate-fade-in-up"
              :class="`stagger-${(idx % 4) + 1}`"
            >
              <!-- Card Image with Quantity Badge -->
              <div class="service-img-wrapper">
                <img
                  :src="service.imageUrl || defaultServiceImg"
                  :alt="service.name"
                  class="service-thumb film-photo"
                  loading="lazy"
                />
                <div class="quantity-badge font-serif">
                  <span class="times">x</span>{{ service.quantity }}
                </div>
              </div>

              <!-- Card Content -->
              <div class="service-content">
                <div class="service-header-row">
                  <h3 class="service-title font-serif">{{ service.name }}</h3>
                  <span class="service-single-price font-serif" v-if="service.unitPrice">
                    Trị giá: {{ formatCurrency(service.unitPrice * service.quantity) }}
                  </span>
                </div>

                <p class="service-desc font-serif">
                  {{ service.description || 'Dịch vụ được thực hiện bởi đội ngũ chuyên viên nhiều năm kinh nghiệm tại Hỷ Sự Studio.' }}
                </p>

                <div class="service-footer-row">
                  <span class="service-duration" v-if="service.durationMinutes">
                    <i class="fa-regular fa-clock q-mr-xs"></i>
                    {{ formatDuration(service.durationMinutes) }}
                  </span>
                  <router-link :to="`/dich-vu/${service.slug}`" class="service-detail-link font-serif">
                    <span>Xem chi tiết dịch vụ</span>
                    <i class="fa-solid fa-arrow-right"></i>
                  </router-link>
                </div>
              </div>
            </div>
          </div>

          <!-- Empty fallback -->
          <div v-else class="empty-services-box paper-card">
            <i class="fa-solid fa-box-open empty-icon"></i>
            <p class="font-serif">Các dịch vụ chi tiết trong gói đang được cập nhật thêm.</p>
          </div>
        </div>
      </section>

      <!-- 5. Workflow / Wedding Journey Steps (Quy trình trải nghiệm 4 bước) -->
      <section class="package-workflow-section">
        <div class="studio-container">
          <div class="section-title-wrap">
            <span class="sub-title font-chinese">喜事・服務流程</span>
            <h2 class="main-title font-serif">Quy Trình Trải Nghiệm Gói Cưới</h2>
            <div class="title-divider"><span class="diamond">✦</span></div>
          </div>

          <div class="timeline-steps-grid">
            <div class="timeline-step-item animate-fade-in-up stagger-1">
              <div class="step-badge font-serif">01</div>
              <h4 class="step-title font-serif">Tư Vấn & Thử Đồ</h4>
              <p class="step-desc">
                Cùng đạo diễn hình ảnh định hình concept riêng, chọn địa điểm chụp và thử váy cưới/vest trực tiếp tại studio.
              </p>
            </div>

            <div class="timeline-step-item animate-fade-in-up stagger-2">
              <div class="step-badge font-serif">02</div>
              <h4 class="step-title font-serif">Ngày Chụp Nghệ Thuật</h4>
              <p class="step-desc">
                Ekip nhiếp ảnh gia và stylist theo sát chăm chút từng khung hình, hướng dẫn tạo dáng tự nhiên, cảm xúc.
              </p>
            </div>

            <div class="timeline-step-item animate-fade-in-up stagger-3">
              <div class="step-badge font-serif">03</div>
              <h4 class="step-title font-serif">Duyệt & Hậu Kỳ Màu Film</h4>
              <p class="step-desc">
                Hai bạn tự tay chọn những tấm ảnh yêu thích nhất để chuyên viên chỉnh màu tone film Hong Kong 90s độc quyền.
              </p>
            </div>

            <div class="timeline-step-item animate-fade-in-up stagger-4">
              <div class="step-badge font-serif">04</div>
              <h4 class="step-title font-serif">Bàn Giao Album & Quà Tặng</h4>
              <p class="step-desc">
                Đóng gói trang trọng album photobook cao cấp, ảnh cổng ép gỗ và bàn giao 100% file gốc chất lượng cao.
              </p>
            </div>
          </div>
        </div>
      </section>

      <!-- 6. Related / Other Packages Section with Mini Pagination -->
      <section class="other-packages-section" v-if="pkg.otherPackages && pkg.otherPackages.length > 0">
        <div class="studio-container">
          <div class="section-header-row">
            <div>
              <span class="sub-title font-chinese">喜事・其他套餐</span>
              <h2 class="main-title font-serif">Các Gói Cưới Gợi Ý Khác</h2>
            </div>

            <!-- Mini Pagination Controls -->
            <div class="other-pagination-controls" v-if="totalOtherPages > 1">
              <button
                @click="changeOtherPage(pkg.otherPage - 1)"
                :disabled="pkg.otherPage <= 1 || isOtherLoading"
                class="btn-nav-arrow"
                title="Trang trước"
              >
                <i class="fa-solid fa-chevron-left"></i>
              </button>
              <span class="pagination-indicator font-serif">
                Trang {{ pkg.otherPage }} / {{ totalOtherPages }}
              </span>
              <button
                @click="changeOtherPage(pkg.otherPage + 1)"
                :disabled="pkg.otherPage >= totalOtherPages || isOtherLoading"
                class="btn-nav-arrow"
                title="Trang sau"
              >
                <i class="fa-solid fa-chevron-right"></i>
              </button>
            </div>
          </div>

          <div class="other-packages-grid" :class="{ 'loading-dim': isOtherLoading }">
            <div
              v-for="(other, oIdx) in pkg.otherPackages"
              :key="other.packageId"
              @click="navigateToPackage(other.slug)"
              class="other-package-card paper-card animate-fade-in-up"
              :class="`stagger-${(oIdx % 3) + 1}`"
            >
              <div class="other-img-box">
                <img
                  :src="other.imageUrl || defaultPkgImg"
                  :alt="other.name"
                  class="other-img film-photo"
                  loading="lazy"
                />
              </div>
              <div class="other-card-body">
                <h3 class="other-title font-serif">{{ other.name }}</h3>
                <div class="other-footer">
                  <span class="other-price font-serif">{{ formatCurrency(other.price) }}</span>
                  <span class="other-link">
                    Xem chi tiết <i class="fa-solid fa-arrow-right"></i>
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
    </main>

    <!-- 7. Quick Consultation Booking Modal -->
    <q-dialog v-model="isBookingModalOpen">
      <q-card class="booking-dialog-card paper-card">
        <div class="dialog-header">
          <div class="dialog-title-wrap">
            <span class="sub-badge font-chinese">喜事・預約諮詢</span>
            <h3 class="dialog-title font-serif">Đăng Ký Tư Vấn Gói Cưới</h3>
            <p class="dialog-subtitle font-serif">
              {{ pkg?.name }} — {{ formatCurrency(pkg?.price) }}
            </p>
          </div>
          <q-btn icon="fa-solid fa-xmark" flat round dense v-close-popup class="close-btn" />
        </div>

        <q-form @submit.prevent="handleBookingSubmit" class="booking-form">
          <div class="form-group">
            <label class="form-label" for="pkg-book-name">Họ và tên quý khách <span class="req">*</span></label>
            <q-input
              id="pkg-book-name"
              v-model="bookingForm.fullName"
              outlined
              dense
              placeholder="Ví dụ: Nguyễn Văn A & Trần Thị B"
              class="vintage-input"
              :rules="[val => !!val || 'Vui lòng nhập họ và tên']"
              lazy-rules
            >
              <template #prepend>
                <q-icon name="fa-regular fa-user" size="16px" class="input-icon" />
              </template>
            </q-input>
          </div>

          <div class="form-row-grid">
            <div class="form-group">
              <label class="form-label" for="pkg-book-phone">Số điện thoại / Zalo <span class="req">*</span></label>
              <q-input
                id="pkg-book-phone"
                v-model="bookingForm.phone"
                type="tel"
                outlined
                dense
                placeholder="0901234567"
                class="vintage-input"
                :rules="[val => !!val || 'Vui lòng nhập số điện thoại']"
                lazy-rules
              >
                <template #prepend>
                  <q-icon name="fa-solid fa-phone" size="16px" class="input-icon" />
                </template>
              </q-input>
            </div>

            <div class="form-group">
              <label class="form-label" for="pkg-book-date">Ngày dự kiến cưới</label>
              <q-input
                id="pkg-book-date"
                v-model="bookingForm.weddingDate"
                type="date"
                outlined
                dense
                class="vintage-input"
              >
                <template #prepend>
                  <q-icon name="fa-regular fa-calendar" size="16px" class="input-icon" />
                </template>
              </q-input>
            </div>
          </div>

          <div class="form-group">
            <label class="form-label" for="pkg-book-email">Địa chỉ Email</label>
            <q-input
              id="pkg-book-email"
              v-model="bookingForm.email"
              type="email"
              outlined
              dense
              placeholder="contact@example.com"
              class="vintage-input"
            >
              <template #prepend>
                <q-icon name="fa-regular fa-envelope" size="16px" class="input-icon" />
              </template>
            </q-input>
          </div>

          <div class="form-group">
            <label class="form-label" for="pkg-book-note">Ghi chú hoặc mong muốn riêng</label>
            <q-input
              id="pkg-book-note"
              v-model="bookingForm.note"
              type="textarea"
              outlined
              rows="3"
              placeholder="Ví dụ: Muốn chụp ngoại cảnh tại Đà Lạt hoặc TP.HCM, cần tư vấn thêm váy cưới..."
              class="vintage-input textarea-input"
            />
          </div>

          <div class="dialog-actions">
            <q-btn
              type="submit"
              :loading="isSubmittingBooking"
              class="btn-vintage full-width font-serif btn-dialog-submit"
              unelevated
              no-caps
            >
              <template #loading>
                <q-spinner-dots size="20px" />
              </template>
              <span>Gửi Yêu Cầu Tư Vấn Gói Cưới</span>
            </q-btn>
          </div>
        </q-form>
      </q-card>
    </q-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import VintageLoading from '@/components/common/VintageLoading.vue'
import packageService from '@/services/packageService'
import type { PackageDetailResponse } from '@/types/package'

const route = useRoute()
const router = useRouter()
const $q = useQuasar()

// State
const pkg = ref<PackageDetailResponse | null>(null)
const isLoading = ref(true)
const isOtherLoading = ref(false)
const error = ref('')
const isCopied = ref(false)

// Booking Modal State
const isBookingModalOpen = ref(false)
const isSubmittingBooking = ref(false)
const bookingForm = reactive({
  fullName: '',
  phone: '',
  email: '',
  weddingDate: '',
  note: ''
})

const defaultPkgImg = 'https://images.unsplash.com/photo-1519741497674-611481863552?q=80&w=1200&auto=format&fit=crop'
const defaultServiceImg = 'https://images.unsplash.com/photo-1583939003579-730e3918a45a?q=80&w=800&auto=format&fit=crop'

// Savings Calculation
const savingsPercent = computed(() => {
  if (!pkg.value || !pkg.value.originalTotalPrice || pkg.value.originalTotalPrice <= pkg.value.price) return 0
  const diff = pkg.value.originalTotalPrice - pkg.value.price
  return Math.round((diff / pkg.value.originalTotalPrice) * 100)
})

// Computed Pagination for Other Packages
const totalOtherPages = computed(() => {
  if (!pkg.value || !pkg.value.otherPageSize) return 1
  return Math.ceil(pkg.value.otherTotal / pkg.value.otherPageSize) || 1
})

// Format helpers
const formatCurrency = (val: number | undefined | null) => {
  if (!val) return '0 ₫'
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(val)
}

const formatDuration = (minutes: number | undefined | null) => {
  if (!minutes) return 'Khoảng 4 tiếng'
  const hours = Math.floor(minutes / 60)
  const remainingMinutes = minutes % 60
  if (hours > 0 && remainingMinutes > 0) {
    return `${hours} tiếng ${remainingMinutes} phút`
  } else if (hours > 0) {
    return `Khoảng ${hours} tiếng`
  }
  return `${minutes} phút`
}

// Fetch Main Package Details
const fetchPackage = async (slug: string, page = 1) => {
  isLoading.value = true
  error.value = ''
  try {
    const res = await packageService.getPackageDetail(slug, page, 3)
    if (res.success) {
      pkg.value = res
    } else {
      error.value = res.message || 'Không tìm thấy gói cưới này.'
    }
  } catch (err: unknown) {
    const errObj = err as { response?: { data?: { message?: string } }; message?: string }
    error.value = errObj.response?.data?.message || errObj.message || 'Lỗi khi tải thông tin gói cưới.'
  } finally {
    isLoading.value = false
  }
}

// Mini Pagination for Other Packages
const changeOtherPage = async (page: number) => {
  if (!pkg.value || isOtherLoading.value) return
  if (page < 1 || page > totalOtherPages.value) return

  isOtherLoading.value = true
  try {
    const res = await packageService.getPackageDetail(pkg.value.slug, page, 3)
    if (res.success) {
      pkg.value.otherPackages = res.otherPackages
      pkg.value.otherPage = res.otherPage
      pkg.value.otherTotal = res.otherTotal
    }
  } catch (err) {
    console.error('Lỗi khi tải gói cưới khác:', err)
  } finally {
    isOtherLoading.value = false
  }
}

// Navigation
const navigateToPackage = async (targetSlug: string) => {
  await router.push(`/goi-cuoi/${targetSlug}`)
}

// Share Link
const copyShareLink = async () => {
  try {
    await navigator.clipboard.writeText(window.location.href)
    isCopied.value = true
    $q.notify({
      type: 'positive',
      message: 'Đã sao chép liên kết gói cưới vào bộ nhớ tạm!',
      position: 'top',
      timeout: 2500,
      icon: 'fa-solid fa-check'
    })
    setTimeout(() => {
      isCopied.value = false
    }, 3000)
  } catch {
    $q.notify({
      type: 'negative',
      message: 'Không thể sao chép liên kết.',
      position: 'top',
      timeout: 2000
    })
  }
}

// Booking Modal Actions
const openBookingModal = () => {
  isBookingModalOpen.value = true
}

const handleBookingSubmit = () => {
  isSubmittingBooking.value = true
  setTimeout(() => {
    isSubmittingBooking.value = false
    isBookingModalOpen.value = false
    $q.notify({
      type: 'positive',
      message: `Cảm ơn quý khách ${bookingForm.fullName}! Hỷ Sự Studio đã nhận thông tin và sẽ liên hệ tư vấn chi tiết gói ${pkg.value?.name}.`,
      position: 'top',
      timeout: 4500,
      icon: 'fa-solid fa-circle-check'
    })
    bookingForm.fullName = ''
    bookingForm.phone = ''
    bookingForm.email = ''
    bookingForm.weddingDate = ''
    bookingForm.note = ''
  }, 900)
}

// Lifecycle & Watchers
onMounted(() => {
  const slug = route.params.slug as string
  if (slug) {
    void fetchPackage(slug)
  }
})

watch(
  () => route.params.slug,
  (newSlug) => {
    if (newSlug && typeof newSlug === 'string') {
      window.scrollTo({ top: 0, behavior: 'smooth' })
      void fetchPackage(newSlug)
    }
  }
)
</script>

<style scoped lang="scss">
.package-detail-page {
  min-height: 80vh;
  background-color: var(--color-paper);
  color: var(--color-ink);
  padding-bottom: 70px;
}

// 1. Loading & Error States
.package-error-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 480px;
  text-align: center;
  padding: 60px 20px;
}

.error-box {
  max-width: 520px;
  padding: 44px 32px;
  text-align: center;

  .error-icon {
    font-size: 2.8rem;
    color: var(--color-burgundy);
    margin-bottom: 16px;
  }

  .error-title {
    font-size: 1.8rem;
    margin: 0 0 10px 0;
    color: var(--color-ink);
  }

  .error-desc {
    color: var(--color-muted);
    font-size: 0.95rem;
    margin-bottom: 24px;
    line-height: 1.6;
  }

  .back-btn {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    padding: 10px 22px;
    text-decoration: none;
  }
}

// 2. Breadcrumb Bar
.package-breadcrumb-bar {
  background-color: var(--color-paper-light);
  border-bottom: 1px solid var(--color-border);
  padding: 14px 0;

  .breadcrumb-inner {
    display: flex;
    align-items: center;
    justify-content: space-between;
    flex-wrap: wrap;
    gap: 12px;
  }

  .breadcrumb-trail {
    .current-crumb {
      max-width: 280px;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }
  }

  .share-link-btn {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    background: none;
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    background-color: var(--color-paper);
    color: var(--color-ink-soft);
    font-size: 0.82rem;
    padding: 6px 14px;
    cursor: pointer;
    transition: all 0.25s ease;

    &:hover {
      border-color: var(--color-burgundy);
      color: var(--color-burgundy);
    }
  }
}

// 3. Hero Section (Editorial Split)
.package-hero-section {
  padding: 50px 0 60px 0;
  border-bottom: 1px solid var(--color-border);

  .package-hero-grid {
    display: grid;
    grid-template-columns: 1fr 1.15fr;
    gap: 48px;
    align-items: center;

    @media (max-width: 960px) {
      grid-template-columns: 1fr;
      gap: 36px;
    }
  }
}

// Left Poster
.hero-poster-wrapper {
  .poster-frame {
    padding: 16px;
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    box-shadow: 0 12px 32px rgba(36, 36, 33, 0.08);
    position: relative;

    .film-stamp-badge {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 12px;
      padding: 0 4px;

      .stamp-chinese {
        font-size: 0.85rem;
        letter-spacing: 1.5px;
        color: var(--color-burgundy);
        font-weight: 600;
      }

      .stamp-meta {
        font-size: 0.76rem;
        color: var(--color-muted);
        letter-spacing: 1px;
      }
    }

    .poster-image-box {
      position: relative;
      width: 100%;
      aspect-ratio: 4 / 3.4;
      overflow: hidden;
      background-color: var(--color-paper-dark);
      border: 1px solid var(--color-border);

      .poster-img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        display: block;
        transition: transform 0.6s cubic-bezier(0.25, 1, 0.5, 1);

        &:hover {
          transform: scale(1.04);
        }
      }

      .poster-badge {
        position: absolute;
        bottom: 14px;
        left: 14px;
        background-color: var(--color-burgundy);
        color: #FAF7F0;
        font-size: 0.78rem;
        letter-spacing: 1px;
        padding: 4px 12px;
        border-radius: var(--radius-xs);
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.35);

        .star {
          color: #E89A3C;
          margin-right: 2px;
        }
      }
    }

    .poster-footer-caption {
      display: flex;
      justify-content: center;
      align-items: center;
      gap: 8px;
      margin-top: 14px;
      font-size: 0.76rem;
      letter-spacing: 1.5px;
      color: var(--color-muted);

      .dot {
        color: var(--color-burgundy);
      }
    }
  }
}

// Right Info
.hero-info-wrapper {
  .package-top-badge {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    margin-bottom: 12px;

    .chinese-seal {
      display: inline-flex;
      align-items: center;
      justify-content: center;
      width: 22px;
      height: 22px;
      background-color: var(--color-burgundy);
      color: #FAF7F0;
      font-size: 0.8rem;
      border-radius: 2px;
    }

    .badge-text {
      font-size: 0.82rem;
      letter-spacing: 1.5px;
      color: var(--color-burgundy);
      font-weight: 600;
      text-transform: uppercase;
    }
  }

  .package-main-title {
    font-size: clamp(2rem, 3.8vw, 2.7rem);
    line-height: 1.2;
    margin: 0 0 20px 0;
    color: var(--color-ink);
    font-weight: 600;
  }

  .package-meta-strip {
    display: flex;
    align-items: center;
    padding: 16px 22px;
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    margin-bottom: 24px;
    gap: 18px;
    flex-wrap: wrap;

    .meta-strip-item {
      display: flex;
      flex-direction: column;
      gap: 4px;

      .meta-strip-label {
        font-size: 0.78rem;
        text-transform: uppercase;
        letter-spacing: 1px;
        color: var(--color-muted);
      }

      .price-wrap {
        display: flex;
        align-items: baseline;
        gap: 8px;

        .price {
          font-size: 1.55rem;
          color: var(--color-burgundy);
          font-weight: 600;
        }

        .original-price {
          font-size: 0.95rem;
          color: var(--color-muted);
          text-decoration: line-through;
        }
      }

      .savings-highlight {
        font-size: 1.15rem;
        color: #B45309;
        font-weight: 600;
      }

      .booking-highlight {
        font-size: 1.1rem;
        color: var(--color-ink);
        font-weight: 500;

        i {
          color: var(--color-burgundy);
        }
      }
    }

    .meta-strip-divider {
      width: 1px;
      height: 38px;
      background-color: var(--color-border);

      @media (max-width: 600px) {
        display: none;
      }
    }
  }

  .package-desc-box {
    position: relative;
    padding: 0 0 0 18px;
    border-left: 2px solid var(--color-burgundy);
    margin-bottom: 28px;

    .quote-mark {
      position: absolute;
      top: -12px;
      left: 6px;
      font-size: 2.2rem;
      color: rgba(142, 41, 41, 0.18);
      line-height: 1;
    }

    .package-desc-text {
      font-size: 1.05rem;
      line-height: 1.7;
      color: var(--color-ink-soft);
      margin: 0;
      font-style: italic;
    }
  }

  .hero-actions-row {
    display: flex;
    align-items: center;
    gap: 16px;
    margin-bottom: 18px;
    flex-wrap: wrap;

    .btn-booking-cta {
      padding: 13px 28px;
      font-size: 1.02rem;
      border-radius: var(--radius-xs);
    }

    .btn-contact-cta {
      padding: 12px 24px;
      font-size: 0.98rem;
      text-decoration: none;
      border-radius: var(--radius-xs);
    }
  }

  .consultation-note {
    font-size: 0.85rem;
    color: var(--color-muted);
    display: flex;
    align-items: center;
    gap: 8px;
    font-style: italic;

    .diamond-icon {
      color: var(--color-burgundy);
      font-size: 0.75rem;
    }
  }
}

// 4. Included Services Section
.included-services-section {
  padding: 60px 0;
  background-color: var(--color-paper-light);
  border-bottom: 1px solid var(--color-border);
}

.section-title-wrap {
  text-align: center;
  margin-bottom: 44px;

  .sub-title {
    font-size: 0.9rem;
    color: var(--color-burgundy);
    letter-spacing: 3px;
    display: block;
    margin-bottom: 6px;
  }

  .main-title {
    font-size: 2.1rem;
    color: var(--color-ink);
    margin: 0 0 8px 0;
    font-weight: 600;
  }

  .section-sub-desc {
    font-size: 0.95rem;
    color: var(--color-muted);
    margin: 0 0 14px 0;
    font-style: italic;
  }

  .title-divider {
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 12px;

    &::before,
    &::after {
      content: '';
      width: 48px;
      height: 1px;
      background-color: var(--color-border);
    }

    .diamond {
      color: var(--color-burgundy);
      font-size: 0.8rem;
    }
  }
}

.included-services-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 28px;

  @media (max-width: 860px) {
    grid-template-columns: 1fr;
  }
}

.included-service-card {
  display: flex;
  gap: 20px;
  padding: 18px;
  background-color: var(--color-paper);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-xs);

  @media (max-width: 600px) {
    flex-direction: column;
  }

  .service-img-wrapper {
    position: relative;
    width: 140px;
    height: 130px;
    flex-shrink: 0;
    overflow: hidden;
    background-color: var(--color-paper-dark);
    border: 1px solid var(--color-border);

    @media (max-width: 600px) {
      width: 100%;
      height: 180px;
    }

    .service-thumb {
      width: 100%;
      height: 100%;
      object-fit: cover;
      display: block;
      transition: transform 0.5s ease;
    }

    .quantity-badge {
      position: absolute;
      top: 8px;
      left: 8px;
      background-color: var(--color-burgundy);
      color: #FAF7F0;
      font-size: 0.85rem;
      font-weight: 600;
      padding: 2px 8px;
      border-radius: 2px;
      box-shadow: 0 2px 6px rgba(0, 0, 0, 0.3);

      .times {
        font-size: 0.72rem;
        margin-right: 1px;
      }
    }
  }

  &:hover .service-thumb {
    transform: scale(1.06);
  }

  .service-content {
    display: flex;
    flex-direction: column;
    flex-grow: 1;

    .service-header-row {
      display: flex;
      justify-content: space-between;
      align-items: baseline;
      gap: 12px;
      margin-bottom: 6px;
      flex-wrap: wrap;

      .service-title {
        font-size: 1.22rem;
        font-weight: 600;
        color: var(--color-ink);
        margin: 0;
      }

      .service-single-price {
        font-size: 0.82rem;
        color: var(--color-muted);
      }
    }

    .service-desc {
      font-size: 0.88rem;
      color: var(--color-ink-soft);
      line-height: 1.5;
      margin: 0 0 12px 0;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      overflow: hidden;
    }

    .service-footer-row {
      margin-top: auto;
      display: flex;
      justify-content: space-between;
      align-items: center;
      font-size: 0.82rem;

      .service-duration {
        color: var(--color-muted);
      }

      .service-detail-link {
        color: var(--color-burgundy);
        text-decoration: none;
        font-weight: 500;
        display: inline-flex;
        align-items: center;
        gap: 6px;
        transition: transform 0.2s ease;

        &:hover {
          transform: translateX(3px);
        }
      }
    }
  }
}

.empty-services-box {
  padding: 48px;
  text-align: center;
  max-width: 480px;
  margin: 0 auto;

  .empty-icon {
    font-size: 2.4rem;
    color: var(--color-muted);
    margin-bottom: 12px;
  }
}

// 5. Workflow Section
.package-workflow-section {
  padding: 60px 0;
  background-color: var(--color-paper);
  border-bottom: 1px solid var(--color-border);
}

.timeline-steps-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 24px;

  @media (max-width: 960px) {
    grid-template-columns: repeat(2, 1fr);
  }

  @media (max-width: 560px) {
    grid-template-columns: 1fr;
  }
}

.timeline-step-item {
  padding: 24px 20px;
  background-color: var(--color-paper-light);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-xs);
  position: relative;
  transition: transform 0.3s ease, border-color 0.3s ease;

  &:hover {
    transform: translateY(-4px);
    border-color: var(--color-burgundy);
  }

  .step-badge {
    font-size: 1.8rem;
    font-weight: 700;
    color: rgba(142, 41, 41, 0.22);
    margin-bottom: 8px;
    line-height: 1;
  }

  .step-title {
    font-size: 1.15rem;
    font-weight: 600;
    color: var(--color-ink);
    margin: 0 0 8px 0;
  }

  .step-desc {
    font-size: 0.88rem;
    color: var(--color-muted);
    line-height: 1.55;
    margin: 0;
  }
}

// 6. Other Packages Section
.other-packages-section {
  padding: 60px 0 40px 0;

  .section-header-row {
    display: flex;
    justify-content: space-between;
    align-items: flex-end;
    margin-bottom: 28px;

    .sub-title {
      font-size: 0.85rem;
      color: var(--color-burgundy);
      letter-spacing: 2px;
      display: block;
      margin-bottom: 4px;
    }

    .main-title {
      font-size: 1.7rem;
      color: var(--color-ink);
      margin: 0;
      font-weight: 600;
    }
  }
}

.other-pagination-controls {
  display: flex;
  align-items: center;
  gap: 12px;

  .btn-nav-arrow {
    width: 34px;
    height: 34px;
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    cursor: pointer;
    color: var(--color-ink);
    transition: all 0.2s ease;

    &:hover:not(:disabled) {
      border-color: var(--color-burgundy);
      color: var(--color-burgundy);
      background-color: var(--color-paper);
    }

    &:disabled {
      opacity: 0.35;
      cursor: not-allowed;
    }
  }

  .pagination-indicator {
    font-size: 0.88rem;
    color: var(--color-ink-soft);
  }
}

.other-packages-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 24px;
  transition: opacity 0.3s ease;

  &.loading-dim {
    opacity: 0.5;
    pointer-events: none;
  }

  @media (max-width: 860px) {
    grid-template-columns: repeat(2, 1fr);
  }

  @media (max-width: 560px) {
    grid-template-columns: 1fr;
  }
}

.other-package-card {
  cursor: pointer;
  overflow: hidden;
  background-color: var(--color-paper-light);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-xs);
  transition: transform 0.3s ease, box-shadow 0.3s ease;

  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 10px 24px rgba(36, 36, 33, 0.08);

    .other-img {
      transform: scale(1.05);
    }

    .other-title {
      color: var(--color-burgundy);
    }

    .other-link {
      color: var(--color-burgundy);
      transform: translateX(3px);
    }
  }

  .other-img-box {
    width: 100%;
    aspect-ratio: 16 / 10;
    overflow: hidden;
    background-color: var(--color-paper-dark);

    .other-img {
      width: 100%;
      height: 100%;
      object-fit: cover;
      display: block;
      transition: transform 0.5s ease;
    }
  }

  .other-card-body {
    padding: 16px;

    .other-title {
      font-size: 1.18rem;
      font-weight: 600;
      color: var(--color-ink);
      margin: 0 0 10px 0;
      transition: color 0.2s ease;
    }

    .other-footer {
      display: flex;
      justify-content: space-between;
      align-items: center;

      .other-price {
        font-size: 1.05rem;
        color: var(--color-burgundy);
        font-weight: 600;
      }

      .other-link {
        font-size: 0.82rem;
        color: var(--color-muted);
        display: inline-flex;
        align-items: center;
        gap: 4px;
        transition: all 0.2s ease;
      }
    }
  }
}

// 7. Booking Modal Styles
.booking-dialog-card {
  width: 540px;
  max-width: 92vw;
  padding: 28px;
  background-color: var(--color-paper-light);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-xs);

  .dialog-header {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    margin-bottom: 20px;
    border-bottom: 1px solid var(--color-border);
    padding-bottom: 14px;

    .sub-badge {
      font-size: 0.8rem;
      letter-spacing: 2px;
      color: var(--color-burgundy);
      display: block;
      margin-bottom: 4px;
    }

    .dialog-title {
      font-size: 1.45rem;
      font-weight: 600;
      color: var(--color-ink);
      margin: 0 0 4px 0;
    }

    .dialog-subtitle {
      font-size: 0.9rem;
      color: var(--color-muted);
      margin: 0;
      font-style: italic;
    }

    .close-btn {
      color: var(--color-muted);
      &:hover {
        color: var(--color-burgundy);
      }
    }
  }

  .booking-form {
    display: flex;
    flex-direction: column;
    gap: 14px;

    .form-group {
      display: flex;
      flex-direction: column;
      gap: 4px;

      .form-label {
        font-size: 0.85rem;
        font-weight: 500;
        color: var(--color-ink-soft);

        .req {
          color: var(--color-burgundy);
        }
      }
    }

    .form-row-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 14px;

      @media (max-width: 500px) {
        grid-template-columns: 1fr;
      }
    }

    .dialog-actions {
      margin-top: 10px;

      .btn-dialog-submit {
        padding: 12px 0;
        font-size: 1.02rem;
      }
    }
  }
}
</style>
