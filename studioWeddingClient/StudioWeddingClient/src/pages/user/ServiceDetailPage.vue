<template>
  <div class="service-detail-page">
    <!-- Unified Loading State -->
    <VintageLoading v-if="isLoading" text="Đang tải thông tin dịch vụ..." />

    <!-- 2. Error / Not Found State -->
    <div v-else-if="error || !service" class="service-error-state studio-container">
      <div class="error-box paper-card">
        <i class="fa-solid fa-circle-exclamation error-icon"></i>
        <h2 class="error-title font-serif">Không tìm thấy Dịch Vụ</h2>
        <p class="error-desc">{{ error || 'Dịch vụ bạn đang tìm kiếm không tồn tại hoặc đã ngừng hoạt động.' }}</p>
        <router-link to="/trang-chu#dich-vu" class="btn-vintage back-btn font-serif">
          <i class="fa-solid fa-arrow-left"></i>
          <span>Quay lại trang chủ</span>
        </router-link>
      </div>
    </div>

    <!-- 3. Main Content State -->
    <main v-else class="service-main-content">
      <!-- Breadcrumb Bar -->
      <section class="service-breadcrumb-bar">
        <div class="studio-container breadcrumb-inner">
          <nav class="breadcrumb-trail" aria-label="Breadcrumb">
            <router-link to="/trang-chu">Trang chủ</router-link>
            <span class="sep">/</span>
            <router-link to="/trang-chu#dich-vu">Dịch vụ</router-link>
            <span class="sep">/</span>
            <span class="current-crumb">{{ service.name }}</span>
          </nav>

          <button @click="copyShareLink" class="share-link-btn" title="Sao chép liên kết">
            <i class="fa-solid" :class="isCopied ? 'fa-check' : 'fa-arrow-up-right-from-square'"></i>
            <span>{{ isCopied ? 'Đã sao chép link' : 'Chia sẻ' }}</span>
          </button>
        </div>
      </section>

      <!-- Hero Showcase Section (Editorial Split) -->
      <section class="service-hero-section">
        <div class="studio-container">
          <div class="service-hero-grid">
            <!-- Left: Visual Poster with Film Borders -->
            <div class="hero-poster-wrapper">
              <div class="poster-frame paper-card">
                <div class="film-stamp-badge">
                  <span class="stamp-chinese font-chinese">喜事・特選服務</span>
                  <span class="stamp-meta">EST. 1998</span>
                </div>
                <div class="poster-image-box">
                  <img
                    :src="service.imageUrl || defaultServiceImg"
                    :alt="service.name"
                    class="poster-img film-photo"
                  />
                </div>
                <div class="poster-footer-caption font-serif">
                  <span>HỶ SỰ WEDDING STUDIO ARCHIVE</span>
                  <span class="dot">·</span>
                  <span>ANALOG FILM CONCEPT</span>
                </div>
              </div>
            </div>

            <!-- Right: Service Details & Meta -->
            <div class="hero-info-wrapper">
              <div class="service-top-badge">
                <span class="chinese-seal font-chinese">囍</span>
                <span class="badge-text font-serif">DỊCH VỤ CƯỚI NGHỆ THUẬT</span>
              </div>

              <h1 class="service-main-title font-serif">{{ service.name }}</h1>

              <!-- Meta Pricing & Duration Bar -->
              <div class="service-meta-strip paper-card">
                <div class="meta-strip-item">
                  <span class="meta-strip-label">Mức giá niêm yết</span>
                  <span class="meta-strip-value price font-serif">{{ formatCurrency(service.price) }}</span>
                </div>
                <div class="meta-strip-divider"></div>
                <div class="meta-strip-item" v-if="service.durationMinutes">
                  <span class="meta-strip-label">Thời lượng trải nghiệm</span>
                  <span class="meta-strip-value">
                    <i class="fa-regular fa-clock q-mr-xs"></i>
                    {{ formatDuration(service.durationMinutes) }}
                  </span>
                </div>
                <div class="meta-strip-divider" v-if="service.durationMinutes"></div>
                <div class="meta-strip-item">
                  <span class="meta-strip-label">Mức độ tin chọn</span>
                  <span class="meta-strip-value booking-highlight">
                    <i class="fa-solid fa-heart q-mr-xs"></i>
                    {{ service.countBooking > 0 ? `${service.countBooking}+ cặp đôi` : 'Dịch vụ nổi bật' }}
                  </span>
                </div>
              </div>

              <!-- Service Description -->
              <div class="service-desc-box">
                <div class="quote-mark font-serif">“</div>
                <p class="service-desc-text font-serif">
                  {{ service.description || 'Gói dịch vụ được thiết kế tỉ mỉ, mang phong cách ảnh film Hong Kong thập niên 90 kết hợp nét truyền thống trang trọng, lưu giữ khoảnh khắc thanh xuân trọn vẹn nhất cho ngày vui trọng đại của đôi bạn.' }}
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
                  <span>Đặt Lịch Tư Vấn Ngay</span>
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
                <span>Tư vấn concept & thử trang phục miễn phí tại Studio trước khi ký hợp đồng.</span>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Highlights / Included Services (4 Đặc Quyền) -->
      <section class="service-highlights-section">
        <div class="studio-container">
          <div class="section-title-wrap">
            <span class="sub-title font-chinese">喜事・承諾</span>
            <h2 class="main-title font-serif">Đặc Quyền & Cam Kết Chất Lượng</h2>
            <div class="title-divider"><span class="diamond">✦</span></div>
          </div>

          <div class="highlights-grid">
            <div class="highlight-card paper-card animate-fade-in-up stagger-1">
              <div class="highlight-icon font-chinese">衣</div>
              <h3 class="highlight-title font-serif">Trang Phục Cao Cấp</h3>
              <p class="highlight-desc">
                Trọn gói áo dài cưới truyền thống, váy cưới phong cách cổ điển và vest chú rể nhập khẩu tinh tế.
              </p>
            </div>

            <div class="highlight-card paper-card animate-fade-in-up stagger-2">
              <div class="highlight-icon font-chinese">美</div>
              <h3 class="highlight-title font-serif">Stylist & Trang Điểm</h3>
              <p class="highlight-desc">
                Chuyên viên trang điểm và làm tóc đồng hành suốt buổi chụp, thay đổi phong cách linh hoạt theo từng bối cảnh.
              </p>
            </div>

            <div class="highlight-card paper-card animate-fade-in-up stagger-3">
              <div class="highlight-icon font-chinese">影</div>
              <h3 class="highlight-title font-serif">Màu Film Độc Quyền</h3>
              <p class="highlight-desc">
                Công thức hậu kỳ nước màu analog hoài niệm đậm chất điện ảnh Hong Kong, tinh chỉnh tỉ mỉ từng khung hình.
              </p>
            </div>

            <div class="highlight-card paper-card animate-fade-in-up stagger-4">
              <div class="highlight-icon font-chinese">冊</div>
              <h3 class="highlight-title font-serif">Ấn Phẩm Mỹ Thuật</h3>
              <p class="highlight-desc">
                Album photobook bìa vải/ép gỗ cao cấp kèm ảnh cổng lớn in mực sắc nét, bảo hành lưu giữ bền lâu theo năm tháng.
              </p>
            </div>
          </div>
        </div>
      </section>

      <!-- Experience Workflow / Journey (Quy Trình 4 Bước) -->
      <section class="service-workflow-section">
        <div class="studio-container">
          <div class="section-title-wrap">
            <span class="sub-title font-chinese">喜事・流程</span>
            <h2 class="main-title font-serif">Quy Trình Trải Nghiệm Dịch Vụ</h2>
            <div class="title-divider"><span class="diamond">✦</span></div>
          </div>

          <div class="timeline-steps-grid">
            <div class="timeline-step-item animate-fade-in-up stagger-1">
              <div class="step-badge font-serif">01</div>
              <h4 class="step-title font-serif">Lên Ý Tưởng & Thử Đồ</h4>
              <p class="step-desc">
                Gặp gỡ đạo diễn hình ảnh, chọn concept hoài niệm và thử trang phục phù hợp với vóc dáng của hai bạn.
              </p>
            </div>

            <div class="timeline-step-item animate-fade-in-up stagger-2">
              <div class="step-badge font-serif">02</div>
              <h4 class="step-title font-serif">Buổi Chụp Thực Tế</h4>
              <p class="step-desc">
                Ekip chuyên nghiệp hướng dẫn tạo dáng tự nhiên, bắt trọn từng khoảnh khắc cảm xúc chân thành nhất.
              </p>
            </div>

            <div class="timeline-step-item animate-fade-in-up stagger-3">
              <div class="step-badge font-serif">03</div>
              <h4 class="step-title font-serif">Chọn Ảnh & Hậu Kỳ</h4>
              <p class="step-desc">
                Khách hàng tự do duyệt chọn những tấm ảnh ưng ý nhất để chuyên viên bắt đầu xử lý màu film tỉ mỉ.
              </p>
            </div>

            <div class="timeline-step-item animate-fade-in-up stagger-4">
              <div class="step-badge font-serif">04</div>
              <h4 class="step-title font-serif">Bàn Giao Tác Phẩm</h4>
              <p class="step-desc">
                Kiểm tra sản phẩm in ấn, bàn giao toàn bộ file gốc và album đóng gói trang trọng gửi đến tận nhà.
              </p>
            </div>
          </div>
        </div>
      </section>

      <!-- Related / Other Services Section with Mini Pagination -->
      <section class="other-services-section" v-if="service.otherServices && service.otherServices.length > 0">
        <div class="studio-container">
          <div class="section-header-row">
            <div>
              <span class="sub-title font-chinese">喜事・其他服務</span>
              <h2 class="main-title font-serif">Dịch Vụ Gợi Ý Khác</h2>
            </div>

            <!-- Mini Pagination Controls -->
            <div class="other-pagination-controls" v-if="totalOtherPages > 1">
              <button
                @click="changeOtherPage(service.otherPage - 1)"
                :disabled="service.otherPage <= 1 || isOtherLoading"
                class="btn-nav-arrow"
                title="Trang trước"
              >
                <i class="fa-solid fa-chevron-left"></i>
              </button>
              <span class="pagination-indicator font-serif">
                Trang {{ service.otherPage }} / {{ totalOtherPages }}
              </span>
              <button
                @click="changeOtherPage(service.otherPage + 1)"
                :disabled="service.otherPage >= totalOtherPages || isOtherLoading"
                class="btn-nav-arrow"
                title="Trang sau"
              >
                <i class="fa-solid fa-chevron-right"></i>
              </button>
            </div>
          </div>

          <div class="other-services-grid" :class="{ 'loading-dim': isOtherLoading }">
            <div
              v-for="(other, oIdx) in service.otherServices"
              :key="other.serviceId"
              @click="navigateToService(other.slug)"
              class="other-service-card paper-card animate-fade-in-up"
              :class="`stagger-${(oIdx % 3) + 1}`"
            >
              <div class="other-img-box">
                <img
                  :src="other.imageUrl || defaultServiceImg"
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

    <!-- 4. Quick Consultation Booking Modal -->
    <q-dialog v-model="isBookingModalOpen">
      <q-card class="booking-dialog-card paper-card">
        <div class="dialog-header">
          <div class="dialog-title-wrap">
            <span class="sub-badge font-chinese">喜事・預約諮詢</span>
            <h3 class="dialog-title font-serif">Đăng Ký Tư Vấn Dịch Vụ</h3>
            <p class="dialog-subtitle font-serif">
              {{ service?.name }}
            </p>
          </div>
          <q-btn icon="fa-solid fa-xmark" flat round dense v-close-popup class="close-btn" />
        </div>

        <q-form @submit.prevent="handleBookingSubmit" class="booking-form">
          <div class="form-group">
            <label class="form-label" for="book-name">Họ và tên quý khách <span class="req">*</span></label>
            <q-input
              id="book-name"
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
              <label class="form-label" for="book-phone">Số điện thoại / Zalo <span class="req">*</span></label>
              <q-input
                id="book-phone"
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
              <label class="form-label" for="book-date">Ngày dự kiến chụp / cưới</label>
              <q-input
                id="book-date"
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
            <label class="form-label" for="book-email">Địa chỉ Email</label>
            <q-input
              id="book-email"
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
            <label class="form-label" for="book-note">Ghi chú hoặc yêu cầu đặc biệt</label>
            <q-input
              id="book-note"
              v-model="bookingForm.note"
              type="textarea"
              outlined
              rows="3"
              placeholder="Ví dụ: Muốn chụp ngoại cảnh tone màu film Hong Kong 90s, thuê thêm 1 váy dạ hội..."
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
              <span>Gửi Yêu Cầu Tư Vấn</span>
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
import serviceService from '@/services/serviceService'
import type { ServiceDetailResponse } from '@/types/service'

const route = useRoute()
const router = useRouter()
const $q = useQuasar()

// State
const service = ref<ServiceDetailResponse | null>(null)
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

const defaultServiceImg = 'https://images.unsplash.com/photo-1583939003579-730e3918a45a?q=80&w=1200&auto=format&fit=crop'

// Computed Pagination
const totalOtherPages = computed(() => {
  if (!service.value || !service.value.otherPageSize) return 1
  return Math.ceil(service.value.otherTotal / service.value.otherPageSize) || 1
})

// Format helpers
const formatCurrency = (val: number | undefined) => {
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

// Fetch Main Service Details
const fetchService = async (slug: string, page = 1) => {
  isLoading.value = true
  error.value = ''
  try {
    const res = await serviceService.getServiceDetail(slug, page, 3)
    if (res.success) {
      service.value = res
    } else {
      error.value = res.message || 'Không tìm thấy dịch vụ.'
    }
  } catch (err: unknown) {
    const errObj = err as { response?: { data?: { message?: string } }; message?: string }
    error.value = errObj.response?.data?.message || errObj.message || 'Lỗi khi tải thông tin dịch vụ.'
  } finally {
    isLoading.value = false
  }
}

// Mini Pagination for Other Services
const changeOtherPage = async (page: number) => {
  if (!service.value || isOtherLoading.value) return
  if (page < 1 || page > totalOtherPages.value) return

  isOtherLoading.value = true
  try {
    const res = await serviceService.getServiceDetail(service.value.slug, page, 3)
    if (res.success) {
      service.value.otherServices = res.otherServices
      service.value.otherPage = res.otherPage
      service.value.otherTotal = res.otherTotal
    }
  } catch (err) {
    console.error('Lỗi khi tải dịch vụ khác:', err)
  } finally {
    isOtherLoading.value = false
  }
}

// Navigation
const navigateToService = async (targetSlug: string) => {
  await router.push(`/dich-vu/${targetSlug}`)
}

// Share Link
const copyShareLink = async () => {
  try {
    await navigator.clipboard.writeText(window.location.href)
    isCopied.value = true
    $q.notify({
      type: 'positive',
      message: 'Đã sao chép liên kết dịch vụ vào bộ nhớ tạm!',
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
      message: 'Gửi yêu cầu tư vấn thành công! Hỷ Sự Studio sẽ liên hệ quý khách trong vòng 24h.',
      position: 'top',
      timeout: 4000,
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
    void fetchService(slug)
  }
})

watch(
  () => route.params.slug,
  (newSlug) => {
    if (newSlug && typeof newSlug === 'string') {
      window.scrollTo({ top: 0, behavior: 'smooth' })
      void fetchService(newSlug)
    }
  }
)
</script>

<style scoped lang="scss">
.service-detail-page {
  min-height: 80vh;
  background-color: var(--color-paper);
  color: var(--color-ink);
  padding-bottom: 70px;
}

// 1. Loading & Error States
.service-loading-state,
.service-error-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 480px;
  text-align: center;
  padding: 60px 20px;

  .loading-text {
    font-size: 1.2rem;
    color: var(--color-ink-soft);
    margin-top: 16px;
    letter-spacing: 0.5px;
  }
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
.service-breadcrumb-bar {
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
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 0.88rem;
    color: var(--color-muted);

    a {
      color: var(--color-ink-soft);
      text-decoration: none;
      transition: color 0.2s ease;

      &:hover {
        color: var(--color-burgundy);
      }
    }

    .sep {
      color: var(--color-border);
    }

    .current-crumb {
      color: var(--color-burgundy);
      font-weight: 500;
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
.service-hero-section {
  padding: 50px 0 60px 0;
  border-bottom: 1px solid var(--color-border);

  .service-hero-grid {
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
  .service-top-badge {
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
      letter-spacing: 2px;
      color: var(--color-burgundy);
      font-weight: 600;
    }
  }

  .service-main-title {
    font-size: 2.3rem;
    font-weight: 600;
    line-height: 1.25;
    color: var(--color-ink);
    margin: 0 0 20px 0;

    @media (max-width: 600px) {
      font-size: 1.8rem;
    }
  }

  .service-meta-strip {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 16px 20px;
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    margin-bottom: 24px;
    flex-wrap: wrap;
    gap: 12px;

    .meta-strip-item {
      display: flex;
      flex-direction: column;
      gap: 3px;

      .meta-strip-label {
        font-size: 0.76rem;
        color: var(--color-muted);
        text-transform: uppercase;
        letter-spacing: 0.5px;
      }

      .meta-strip-value {
        font-size: 1.05rem;
        color: var(--color-ink);
        font-weight: 600;

        &.price {
          font-size: 1.4rem;
          color: var(--color-burgundy);
        }

        &.booking-highlight {
          color: var(--color-red-soft, #A8443E);
        }
      }
    }

    .meta-strip-divider {
      width: 1px;
      height: 36px;
      background-color: var(--color-border);

      @media (max-width: 600px) {
        display: none;
      }
    }
  }

  .service-desc-box {
    margin-bottom: 28px;
    position: relative;
    padding-left: 18px;
    border-left: 2px solid var(--color-burgundy);

    .quote-mark {
      font-size: 2.2rem;
      line-height: 1;
      color: var(--color-burgundy);
      opacity: 0.4;
      margin-bottom: -10px;
    }

    .service-desc-text {
      font-size: 1.02rem;
      line-height: 1.75;
      color: var(--color-ink-soft);
      margin: 0;
    }
  }

  .hero-actions-row {
    display: flex;
    align-items: center;
    gap: 16px;
    flex-wrap: wrap;
    margin-bottom: 18px;

    .btn-booking-cta {
      height: 48px;
      padding: 0 28px;
      font-size: 1rem;
      letter-spacing: 0.5px;
      font-weight: 500;
    }

    .btn-contact-cta {
      height: 48px;
      padding: 0 24px;
      font-size: 0.95rem;
      display: inline-flex;
      align-items: center;
      text-decoration: none;
    }
  }

  .consultation-note {
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 0.84rem;
    color: var(--color-muted);

    .diamond-icon {
      color: var(--color-burgundy);
      font-size: 0.75rem;
    }
  }
}

// 4. Section Title General
.section-title-wrap {
  text-align: center;
  margin-bottom: 38px;

  .sub-title {
    display: inline-block;
    font-size: 0.82rem;
    letter-spacing: 2px;
    color: var(--color-burgundy);
    margin-bottom: 6px;
    font-weight: 600;
  }

  .main-title {
    font-size: 1.85rem;
    color: var(--color-ink);
    margin: 0 0 10px 0;
  }

  .title-divider {
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 12px;

    &::before,
    &::after {
      content: '';
      width: 60px;
      height: 1px;
      background: linear-gradient(90deg, transparent, var(--color-border), transparent);
    }

    .diamond {
      color: var(--color-burgundy);
      font-size: 0.75rem;
    }
  }
}

// 5. Highlights Grid
.service-highlights-section {
  padding: 60px 0;
  background-color: var(--color-paper-light);
  border-bottom: 1px solid var(--color-border);

  .highlights-grid {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 20px;

    @media (max-width: 960px) {
      grid-template-columns: repeat(2, 1fr);
    }

    @media (max-width: 540px) {
      grid-template-columns: 1fr;
    }
  }

  .highlight-card {
    padding: 24px 20px;
    background-color: var(--color-paper);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    text-align: center;
    transition: transform 0.3s ease, border-color 0.3s ease;

    &:hover {
      transform: translateY(-4px);
      border-color: var(--color-burgundy);
    }

    .highlight-icon {
      display: inline-flex;
      align-items: center;
      justify-content: center;
      width: 44px;
      height: 44px;
      border-radius: 50%;
      background-color: rgba(142, 41, 41, 0.08);
      color: var(--color-burgundy);
      font-size: 1.3rem;
      margin-bottom: 14px;
      border: 1px solid rgba(142, 41, 41, 0.2);
    }

    .highlight-title {
      font-size: 1.15rem;
      font-weight: 600;
      color: var(--color-ink);
      margin: 0 0 8px 0;
    }

    .highlight-desc {
      font-size: 0.88rem;
      line-height: 1.6;
      color: var(--color-ink-soft);
      margin: 0;
    }
  }
}

// 6. Workflow Timeline
.service-workflow-section {
  padding: 60px 0;
  border-bottom: 1px solid var(--color-border);

  .timeline-steps-grid {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 24px;
    position: relative;

    @media (max-width: 960px) {
      grid-template-columns: repeat(2, 1fr);
      gap: 32px;
    }

    @media (max-width: 540px) {
      grid-template-columns: 1fr;
      gap: 24px;
    }
  }

  .timeline-step-item {
    position: relative;
    padding: 24px;
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);

    .step-badge {
      font-size: 1.8rem;
      font-weight: 600;
      color: var(--color-burgundy);
      opacity: 0.8;
      margin-bottom: 8px;
    }

    .step-title {
      font-size: 1.15rem;
      color: var(--color-ink);
      margin: 0 0 8px 0;
    }

    .step-desc {
      font-size: 0.88rem;
      line-height: 1.6;
      color: var(--color-ink-soft);
      margin: 0;
    }
  }
}

// 7. Other Services Section
.other-services-section {
  padding: 60px 0;

  .section-header-row {
    display: flex;
    justify-content: space-between;
    align-items: flex-end;
    margin-bottom: 32px;
    flex-wrap: wrap;
    gap: 16px;

    .sub-title {
      font-size: 0.82rem;
      letter-spacing: 2px;
      color: var(--color-burgundy);
      display: block;
      margin-bottom: 4px;
    }

    .main-title {
      font-size: 1.85rem;
      color: var(--color-ink);
      margin: 0;
    }
  }

  .other-pagination-controls {
    display: flex;
    align-items: center;
    gap: 10px;

    .btn-nav-arrow {
      width: 36px;
      height: 36px;
      display: inline-flex;
      align-items: center;
      justify-content: center;
      border: 1px solid var(--color-border);
      border-radius: var(--radius-xs);
      background-color: var(--color-paper-light);
      color: var(--color-ink);
      cursor: pointer;
      transition: all 0.2s ease;

      &:hover:not(:disabled) {
        border-color: var(--color-burgundy);
        color: var(--color-burgundy);
      }

      &:disabled {
        opacity: 0.4;
        cursor: not-allowed;
      }
    }

    .pagination-indicator {
      font-size: 0.88rem;
      color: var(--color-muted);
      padding: 0 6px;
    }
  }

  .other-services-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 24px;
    transition: opacity 0.25s ease;

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

  .other-service-card {
    cursor: pointer;
    overflow: hidden;
    padding: 16px;
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    transition: transform 0.3s ease, border-color 0.3s ease, box-shadow 0.3s ease;

    &:hover {
      transform: translateY(-4px);
      border-color: var(--color-burgundy);
      box-shadow: 0 8px 24px rgba(36, 36, 33, 0.08);

      .other-img {
        transform: scale(1.05);
      }

      .other-title {
        color: var(--color-burgundy);
      }

      .other-link {
        color: var(--color-burgundy);
        i {
          transform: translateX(4px);
        }
      }
    }

    .other-img-box {
      width: 100%;
      aspect-ratio: 16 / 10;
      overflow: hidden;
      background-color: var(--color-paper-dark);
      border: 1px solid var(--color-border);
      margin-bottom: 12px;

      .other-img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        display: block;
        transition: transform 0.5s ease;
      }
    }

    .other-card-body {
      display: flex;
      flex-direction: column;
      gap: 8px;

      .other-title {
        font-size: 1.15rem;
        font-weight: 600;
        color: var(--color-ink);
        margin: 0;
        transition: color 0.2s ease;
      }

      .other-footer {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-top: 4px;

        .other-price {
          font-size: 0.95rem;
          font-weight: 600;
          color: var(--color-burgundy);
        }

        .other-link {
          font-size: 0.82rem;
          color: var(--color-ink-soft);
          display: inline-flex;
          align-items: center;
          gap: 4px;
          transition: color 0.2s ease;

          i {
            transition: transform 0.2s ease;
          }
        }
      }
    }
  }
}

// 8. Booking Consultation Dialog
.booking-dialog-card {
  width: 100%;
  max-width: 520px;
  background-color: var(--color-paper-light) !important;
  border: 1px solid var(--color-border) !important;
  border-radius: var(--radius-xs) !important;
  padding: 32px 28px;

  .dialog-header {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    margin-bottom: 20px;

    .sub-badge {
      display: inline-block;
      font-size: 0.78rem;
      letter-spacing: 2px;
      color: var(--color-burgundy);
      margin-bottom: 4px;
      font-weight: 600;
    }

    .dialog-title {
      font-size: 1.6rem;
      color: var(--color-ink);
      margin: 0 0 4px 0;
    }

    .dialog-subtitle {
      font-size: 0.9rem;
      color: var(--color-burgundy);
      margin: 0;
      font-style: italic;
    }

    .close-btn {
      color: var(--color-muted);
      &:hover {
        color: var(--color-ink);
      }
    }
  }

  .booking-form {
    .form-group {
      margin-bottom: 14px;

      .form-label {
        display: block;
        font-size: 0.84rem;
        font-weight: 500;
        color: var(--color-ink);
        margin-bottom: 4px;

        .req {
          color: var(--color-burgundy);
        }
      }
    }

    .form-row-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 12px;

      @media (max-width: 500px) {
        grid-template-columns: 1fr;
      }
    }

    :deep(.vintage-input) {
      .q-field__control {
        border-radius: var(--radius-xs) !important;
        background-color: var(--color-paper) !important;
        border-color: var(--color-border) !important;
        height: 42px;
        transition: all 0.25s ease;

        &:hover {
          border-color: var(--color-border-dark) !important;
        }
      }

      &.q-field--focused .q-field__control {
        border-color: var(--color-burgundy) !important;
        box-shadow: 0 0 0 2px rgba(142, 41, 41, 0.12) !important;
      }

      .input-icon {
        color: var(--color-muted);
      }

      &.textarea-input .q-field__control {
        height: auto;
      }
    }

    .dialog-actions {
      margin-top: 20px;

      .btn-dialog-submit {
        height: 44px;
        font-size: 0.96rem;
        letter-spacing: 0.5px;
      }
    }
  }
}
</style>
