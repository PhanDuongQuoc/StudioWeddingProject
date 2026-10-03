<template>
  <div class="album-detail-page">
    <!-- Unified Loading State -->
    <VintageLoading v-if="isLoading" text="Đang tải bộ ảnh cưới..." />

    <!-- Error / Not Found State -->
    <div v-else-if="error || !album" class="album-error-state studio-container">
      <div class="error-box paper-card">
        <i class="fa-solid fa-circle-exclamation error-icon"></i>
        <h2 class="error-title font-serif">Không tìm thấy Album</h2>
        <p class="error-desc">{{ error || 'Album bạn đang tìm kiếm không tồn tại hoặc đã được ẩn.' }}</p>
        <router-link to="/trang-chu" class="btn-vintage back-btn font-serif">
          <i class="fa-solid fa-arrow-left"></i>
          <span>Quay lại trang chủ</span>
        </router-link>
      </div>
    </div>

    <!-- Main Album Content State -->
    <main v-else class="album-content">
      <!-- 1. Breadcrumb Bar -->
      <section class="album-breadcrumb-bar">
        <div class="studio-container breadcrumb-inner">
          <nav class="breadcrumb-trail" aria-label="Breadcrumb">
            <router-link to="/trang-chu">Trang chủ</router-link>
            <span class="sep">/</span>
            <router-link to="/trang-chu#albums">Album</router-link>
            <span class="sep" v-if="album.categoryName">/</span>
            <span v-if="album.categoryName" class="cat-crumb">{{ album.categoryName }}</span>
            <span class="sep">/</span>
            <span class="current-crumb">{{ album.title }}</span>
          </nav>

          <button @click="copyShareLink" class="share-link-btn" title="Sao chép liên kết">
            <i class="fa-solid" :class="isCopied ? 'fa-check' : 'fa-arrow-up-right-from-square'"></i>
            <span>{{ isCopied ? 'Đã sao chép link' : 'Chia sẻ' }}</span>
          </button>
        </div>
      </section>

      <!-- 2. Cinematic Full-Bleed Hero Header (Cover Image from DB as Background) -->
      <header
        class="album-hero-header"
        :style="heroStyle"
      >
        <div class="hero-overlay"></div>

        <div class="studio-container hero-container">
          <!-- Category Tag -->
          <div class="hero-top-badge" v-if="album.categoryName">
            <span class="chinese-seal font-chinese">囍</span>
            <span class="category-name">{{ album.categoryName }}</span>
          </div>

          <!-- Main Title -->
          <h1 class="album-main-title font-serif">{{ album.title }}</h1>

          <!-- Meta Information -->
          <div class="album-meta-row font-serif">
            <span v-if="album.photos && album.photos.length > 0" class="meta-item">
              <i class="fa-solid fa-film"></i>
              <span>{{ album.photos.length }} thước phim lưu giữ</span>
            </span>
            <span class="meta-dot" v-if="album.createdAt">·</span>
            <span v-if="album.createdAt" class="meta-item">
              <i class="fa-regular fa-calendar"></i>
              <span>{{ formatDate(album.createdAt) }}</span>
            </span>
            <span class="meta-dot">·</span>
            <span class="meta-item studio-tag">
              <span>Hỷ Sự Wedding Archive</span>
            </span>
          </div>

          <!-- Description from DB (Only rendered when description exists in DB) -->
          <div class="album-description-box" v-if="album.description">
            <div class="quote-mark font-serif">“</div>
            <p class="description-text font-serif">
              {{ album.description }}
            </p>
          </div>

          <!-- CTA Action -->
          <div class="hero-cta-wrap">
            <a href="#dat-lich-tu-van" class="btn-vintage font-serif cta-btn">
              <span>Tư vấn concept này</span>
              <i class="fa-solid fa-arrow-down"></i>
            </a>
          </div>
        </div>
      </header>

      <!-- 3. Photo Gallery Section (Real Aesthetic Masonry Flow - No Awkward Blank Spaces) -->
      <section class="album-gallery-section">
        <div class="studio-container">
          <!-- Gallery Section Title Bar -->
          <div class="gallery-section-bar">
            <div class="bar-left">
              <h2 class="section-title font-serif">Thước phim câu chuyện</h2>
              <span class="photo-total-badge font-mono" v-if="album.photos && album.photos.length > 0">
                {{ album.photos.length }} BẢN GHI ÂM BẢN
              </span>
            </div>

            <div class="bar-right">
              <span class="ornament-seal font-chinese">喜事・記錄</span>
            </div>
          </div>

          <!-- Empty photos state -->
          <div v-if="!album.photos || album.photos.length === 0" class="empty-photos-box paper-card">
            <i class="fa-solid fa-camera-retro empty-icon"></i>
            <p class="font-serif">Bộ ảnh đang được hoàn thiện bản in và cập nhật thêm hình ảnh.</p>
          </div>

          <!-- True Editorial Masonry Flow (Images hug content tightly without any stretched voids) -->
          <div v-else class="photos-masonry-flow">
            <div
              v-for="(photo, index) in album.photos"
              :key="photo.photoId || index"
              class="photo-brick paper-card animate-film-reveal"
              :style="{ animationDelay: `${(index % 6) * 0.08}s` }"
              @click="openLightbox(index)"
            >
              <!-- Film Header Strip -->
              <div class="brick-header">
                <span class="film-idx font-mono">HK · 90s EXP-{{ String(index + 1).padStart(2, '0') }}</span>
                <span class="film-mark font-mono">KODAK 400</span>
              </div>

              <!-- Photo Image Container -->
              <div class="brick-img-wrap">
                <img
                  :src="photo.thumbnailUrl || photo.imageUrl"
                  :alt="photo.caption || `${album.title} - Thước phim ${index + 1}`"
                  class="brick-img film-photo"
                  loading="lazy"
                />

                <div class="brick-hover-overlay">
                  <span class="zoom-badge">
                    <i class="fa-solid fa-magnifying-glass-plus"></i>
                  </span>
                  <span class="frame-badge font-mono">
                    {{ String(index + 1).padStart(2, '0') }} / {{ String(album.photos.length).padStart(2, '0') }}
                  </span>
                </div>
              </div>

              <!-- Caption Bar (Only rendered when caption exists in DB) -->
              <div class="brick-caption-bar" v-if="photo.caption">
                <p class="caption-text font-serif">{{ photo.caption }}</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- 4. Grand Aesthetic Consultation Inquiry Section (Expanded & Spacious) -->
      <section id="dat-lich-tu-van" class="album-cta-section">
        <div class="studio-container">
          <div class="envelope-card paper-card">
            <!-- Header with stamps -->
            <div class="envelope-header">
              <div class="stamp-group">
                <span class="envelope-stamp font-chinese">百年好合</span>
                <span class="postmark-tag font-mono">HỶ SỰ · 1998 POST</span>
              </div>
              <span class="envelope-label font-mono">HỶ SỰ WEDDING STUDIO · TƯ VẤN & BÁO GIÁ TRỌN GÓI</span>
            </div>

            <div class="envelope-body">
              <!-- Left Information Column -->
              <div class="cta-info">
                <span class="cta-sub font-mono">CONCEPT CONSULTATION</span>
                <h3 class="cta-title font-serif">Bạn yêu thích phong cách "{{ album.title }}"?</h3>
                <p class="cta-desc font-serif">
                  Để lại thông tin bên dưới, chuyên viên tư vấn của Hỷ Sự Studio sẽ trực tiếp gửi bảng báo giá ưu đãi chi tiết, gợi ý địa điểm chụp và hỗ trợ chuẩn bị các mẫu trang phục cưới Hong Kong phù hợp nhất cho hai bạn.
                </p>

                <div class="cta-benefits-list font-serif">
                  <div class="benefit-item">
                    <i class="fa-solid fa-circle-check"></i>
                    <span>Tư vấn concept & địa điểm chụp hoàn toàn miễn phí</span>
                  </div>
                  <div class="benefit-item">
                    <i class="fa-solid fa-circle-check"></i>
                    <span>Tặng kèm photobook cao cấp & file ảnh gốc chất lượng cao</span>
                  </div>
                </div>
              </div>

              <!-- Right Form Column -->
              <form @submit.prevent="handleConsultSubmit" class="cta-form">
                <div class="form-inputs-row">
                  <div class="form-field">
                    <label class="field-label">Họ và tên của bạn *</label>
                    <input
                      v-model="consultForm.fullName"
                      type="text"
                      placeholder="vd: Nguyễn Văn A"
                      required
                      class="vintage-input"
                    />
                  </div>
                  <div class="form-field">
                    <label class="field-label">Số điện thoại liên hệ *</label>
                    <input
                      v-model="consultForm.phone"
                      type="tel"
                      placeholder="vd: 0901234567"
                      required
                      class="vintage-input"
                    />
                  </div>
                </div>

                <div class="form-field">
                  <label class="field-label">Ghi chú thêm (Ngày cưới dự kiến hoặc yêu cầu riêng)</label>
                  <input
                    v-model="consultForm.note"
                    type="text"
                    placeholder="vd: Dự kiến chụp tháng sau tại studio hoặc ngoại cảnh..."
                    class="vintage-input"
                  />
                </div>

                <button type="submit" class="btn-vintage font-serif cta-submit-btn">
                  <span>Gửi thư yêu cầu tư vấn gói cưới</span>
                  <i class="fa-solid fa-paper-plane"></i>
                </button>
              </form>
            </div>
          </div>
        </div>
      </section>
    </main>

    <!-- 5. Fullscreen Cinema Lightbox (Minimal & Focused on Photography) -->
    <transition name="lightbox-fade">
      <div
        v-if="isLightboxOpen && album && album.photos && album.photos.length > 0"
        class="cinema-lightbox"
        @click.self="closeLightbox"
      >
        <!-- Top Toolbar -->
        <div class="lightbox-toolbar">
          <div class="lightbox-counter font-mono">
            <span class="current-num">{{ String(activePhotoIndex + 1).padStart(2, '0') }}</span>
            <span class="sep">/</span>
            <span class="total-num">{{ String(album.photos.length).padStart(2, '0') }}</span>
          </div>

          <div class="lightbox-title font-serif">
            {{ album.title }}
          </div>

          <button class="lightbox-close-btn" @click="closeLightbox" title="Đóng (Esc)">
            <i class="fa-solid fa-xmark"></i>
          </button>
        </div>

        <!-- Stage Area -->
        <div class="lightbox-stage">
          <!-- Prev Button -->
          <button
            class="nav-btn prev-btn"
            @click.stop="prevPhoto"
            :disabled="album.photos.length <= 1"
            aria-label="Ảnh trước (Phím ←)"
          >
            <i class="fa-solid fa-chevron-left"></i>
          </button>

          <!-- Current Photo -->
          <div class="photo-display-box">
            <img
              :src="currentPhoto?.imageUrl"
              :alt="currentPhoto?.caption || album.title"
              class="active-lightbox-img"
            />
            <!-- Caption (if available from DB) -->
            <div class="active-caption font-serif" v-if="currentPhoto?.caption">
              <p>{{ currentPhoto.caption }}</p>
            </div>
          </div>

          <!-- Next Button -->
          <button
            class="nav-btn next-btn"
            @click.stop="nextPhoto"
            :disabled="album.photos.length <= 1"
            aria-label="Ảnh sau (Phím →)"
          >
            <i class="fa-solid fa-chevron-right"></i>
          </button>
        </div>

        <!-- Bottom Thumbnails Strip -->
        <div class="lightbox-thumbs-bar" v-if="album.photos.length > 1">
          <div class="thumbs-track">
            <div
              v-for="(photo, idx) in album.photos"
              :key="photo.photoId || idx"
              class="thumb-box"
              :class="{ active: activePhotoIndex === idx }"
              @click.stop="activePhotoIndex = idx"
            >
              <img :src="photo.thumbnailUrl || photo.imageUrl" :alt="`Thumb ${idx + 1}`" />
            </div>
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, watch, reactive } from 'vue'
import { useRoute } from 'vue-router'
import { useQuasar } from 'quasar'
import { useAlbumDetail } from '@/composables/useAlbumDetail'
import VintageLoading from '@/components/common/VintageLoading.vue'

const route = useRoute()
const $q = useQuasar()
const { album, isLoading, error, fetchAlbumDetail } = useAlbumDetail()

// Lightbox State
const isLightboxOpen = ref(false)
const activePhotoIndex = ref(0)
const isCopied = ref(false)

// Consultation mini-form state
const consultForm = reactive({
  fullName: '',
  phone: '',
  note: ''
})

const currentPhoto = computed(() => {
  if (!album.value?.photos || album.value.photos.length === 0) return null
  return album.value.photos[activePhotoIndex.value] || null
})

// Cover Background Image from DB
const heroBgImage = computed(() => {
  return album.value?.coverImageUrl || album.value?.photos?.[0]?.imageUrl || ''
})

const heroStyle = computed(() => {
  if (!heroBgImage.value) return {}
  return {
    backgroundImage: `url('${heroBgImage.value}')`
  }
})

// Format Date Utility
const formatDate = (dateStr?: string | null) => {
  if (!dateStr) return ''
  try {
    const date = new Date(dateStr)
    return new Intl.DateTimeFormat('vi-VN', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit'
    }).format(date)
  } catch {
    return dateStr
  }
}

// Lightbox Controls
const openLightbox = (index: number) => {
  if (!album.value?.photos || album.value.photos.length === 0) return
  activePhotoIndex.value = Math.max(0, Math.min(index, album.value.photos.length - 1))
  isLightboxOpen.value = true
  document.body.style.overflow = 'hidden'
}

const closeLightbox = () => {
  isLightboxOpen.value = false
  document.body.style.overflow = ''
}

const prevPhoto = () => {
  if (!album.value?.photos || album.value.photos.length <= 1) return
  if (activePhotoIndex.value > 0) {
    activePhotoIndex.value--
  } else {
    activePhotoIndex.value = album.value.photos.length - 1
  }
}

const nextPhoto = () => {
  if (!album.value?.photos || album.value.photos.length <= 1) return
  if (activePhotoIndex.value < album.value.photos.length - 1) {
    activePhotoIndex.value++
  } else {
    activePhotoIndex.value = 0
  }
}

// Keyboard Navigation
const handleKeydown = (e: KeyboardEvent) => {
  if (!isLightboxOpen.value) return
  if (e.key === 'Escape') closeLightbox()
  if (e.key === 'ArrowLeft') prevPhoto()
  if (e.key === 'ArrowRight') nextPhoto()
}

// Copy Share Link
const copyShareLink = async () => {
  try {
    await navigator.clipboard.writeText(window.location.href)
    isCopied.value = true
    $q.notify({
      type: 'positive',
      message: 'Đã sao chép liên kết bộ ảnh!',
      position: 'top',
      timeout: 2000,
      icon: 'fa-solid fa-circle-check'
    })
    setTimeout(() => {
      isCopied.value = false
    }, 3000)
  } catch {
    // Fallback
  }
}

// Submit Consultation Request
const handleConsultSubmit = () => {
  $q.notify({
    type: 'positive',
    message: `Cảm ơn ${consultForm.fullName}! Hỷ Sự Studio đã nhận thông tin và sẽ liên hệ tư vấn sớm nhất.`,
    position: 'top',
    timeout: 3000,
    icon: 'fa-solid fa-circle-check'
  })
  consultForm.fullName = ''
  consultForm.phone = ''
  consultForm.note = ''
}

// Lifecycle Hooks & Route Watcher
const loadData = async () => {
  const slug = String(route.params.slug || '')
  if (slug) {
    await fetchAlbumDetail(slug)
  }
}

watch(
  () => route.params.slug,
  async (newSlug) => {
    if (newSlug) {
      await fetchAlbumDetail(String(newSlug))
    }
  }
)

onMounted(async () => {
  window.addEventListener('keydown', handleKeydown)
  await loadData()
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeydown)
  document.body.style.overflow = ''
})
</script>

<style lang="scss" scoped>
.album-detail-page {
  min-height: 100vh;
  background-color: var(--color-paper);
  color: var(--color-ink);
  padding-bottom: 80px;
}

.font-mono {
  font-family: monospace;
}

// Loading & Error States
.album-loading-state,
.album-error-state {
  min-height: 60vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 60px 0;
  text-align: center;
}

.vintage-spinner {
  width: 40px;
  height: 40px;
  border: 2px solid var(--color-border);
  border-top-color: var(--color-burgundy);
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
  margin-bottom: 16px;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

.loading-text {
  font-size: 1.15rem;
  color: var(--color-muted);
}

.error-box {
  padding: 48px 36px;
  max-width: 500px;
  margin: 0 auto;
  text-align: center;

  .error-icon {
    font-size: 2.8rem;
    color: var(--color-burgundy);
    margin-bottom: 16px;
  }

  .error-title {
    font-size: 1.75rem;
    font-weight: 600;
    margin: 0 0 12px;
    color: var(--color-ink);
  }

  .error-desc {
    font-size: 0.95rem;
    color: var(--color-ink-soft);
    margin-bottom: 24px;
    line-height: 1.6;
  }

  .back-btn {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    padding: 10px 22px;
    font-size: 0.95rem;
  }
}

// 1. Breadcrumb Bar
.album-breadcrumb-bar {
  border-bottom: 1px solid var(--color-border);
  background-color: var(--color-paper-light);
  padding: 12px 0;
  font-size: 0.86rem;

  .breadcrumb-inner {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 16px;
  }

  .breadcrumb-trail {
    display: flex;
    align-items: center;
    gap: 8px;
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
      color: var(--color-border);
      font-size: 0.8rem;
    }

    .cat-crumb {
      color: var(--color-burgundy);
      font-weight: 500;
    }

    .current-crumb {
      color: var(--color-ink);
      font-weight: 500;
    }
  }

  .share-link-btn {
    background: none;
    border: none;
    color: var(--color-ink-soft);
    font-size: 0.84rem;
    display: inline-flex;
    align-items: center;
    gap: 6px;
    cursor: pointer;
    padding: 4px 8px;
    border-radius: var(--radius-xs);
    transition: color 0.2s;

    &:hover {
      color: var(--color-burgundy);
    }
  }
}

// 2. Cinematic Full-Bleed Hero Header
.album-hero-header {
  position: relative;
  min-height: 480px;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: var(--color-film-dark);
  background-position: center center;
  background-size: cover;
  background-repeat: no-repeat;
  padding: 90px 0 80px;
  text-align: center;
  color: #FAF7F0;
  margin-bottom: 48px;
  overflow: hidden;

  .hero-overlay {
    position: absolute;
    inset: 0;
    background: linear-gradient(
      180deg,
      rgba(18, 24, 23, 0.55) 0%,
      rgba(18, 24, 23, 0.76) 65%,
      rgba(18, 24, 23, 0.92) 100%
    );
    z-index: 1;
  }

  .hero-container {
    position: relative;
    z-index: 2;
    max-width: 880px;
    margin: 0 auto;
  }

  .hero-top-badge {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    margin-bottom: 16px;
    background-color: rgba(24, 35, 34, 0.75);
    padding: 4px 14px 4px 6px;
    border-radius: var(--radius-xs);
    border: 1px solid rgba(201, 192, 178, 0.3);
    backdrop-filter: blur(4px);

    .chinese-seal {
      width: 20px;
      height: 20px;
      background-color: var(--color-burgundy);
      color: #FAF7F0;
      font-size: 0.75rem;
      display: flex;
      align-items: center;
      justify-content: center;
      border-radius: var(--radius-xs);
    }

    .category-name {
      font-size: 0.82rem;
      letter-spacing: 2px;
      text-transform: uppercase;
      color: #FAF7F0;
      font-weight: 600;
    }
  }

  .album-main-title {
    font-size: clamp(2.4rem, 4.8vw, 3.8rem);
    font-weight: 600;
    line-height: 1.15;
    color: #FAF7F0;
    margin: 0 0 18px;
    letter-spacing: 0.5px;
    text-shadow: 0 2px 20px rgba(0, 0, 0, 0.7);
  }

  .album-meta-row {
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 12px;
    font-size: 0.95rem;
    color: rgba(250, 247, 240, 0.88);
    margin-bottom: 24px;
    flex-wrap: wrap;

    .meta-item {
      display: inline-flex;
      align-items: center;
      gap: 6px;
    }

    .meta-dot {
      color: rgba(250, 247, 240, 0.45);
    }

    .studio-tag {
      color: #E89A3C;
      font-style: italic;
    }
  }

  .album-description-box {
    position: relative;
    max-width: 720px;
    margin: 0 auto 34px;
    padding: 0 24px;

    .quote-mark {
      font-size: 2.4rem;
      color: rgba(250, 247, 240, 0.4);
      line-height: 1;
      margin-bottom: -12px;
    }

    .description-text {
      font-size: 1.12rem;
      line-height: 1.8;
      color: #FAF7F0;
      font-style: italic;
      margin: 0;
      text-shadow: 0 2px 10px rgba(0, 0, 0, 0.6);
    }
  }

  .hero-cta-wrap {
    display: flex;
    justify-content: center;

    .cta-btn {
      display: inline-flex;
      align-items: center;
      gap: 8px;
      padding: 13px 30px;
      font-size: 1rem;
      box-shadow: 0 4px 18px rgba(0, 0, 0, 0.45) !important;
    }
  }
}

// 3. Photo Gallery Section (Masonry Flow)
.album-gallery-section {
  padding-bottom: 70px;

  .gallery-section-bar {
    display: flex;
    justify-content: space-between;
    align-items: flex-end;
    border-bottom: 1px solid var(--color-border);
    padding-bottom: 12px;
    margin-bottom: 28px;

    .bar-left {
      display: flex;
      align-items: baseline;
      gap: 12px;
      flex-wrap: wrap;
    }

    .section-title {
      font-size: 1.6rem;
      font-weight: 600;
      color: var(--color-ink);
      margin: 0;
    }

    .photo-total-badge {
      font-size: 0.78rem;
      letter-spacing: 1.5px;
      color: var(--color-muted);
    }

    .ornament-seal {
      font-size: 0.82rem;
      color: var(--color-burgundy);
      border: 1px solid var(--color-burgundy-soft);
      padding: 2px 8px;
      border-radius: var(--radius-xs);
      letter-spacing: 1px;
    }
  }

  .empty-photos-box {
    padding: 60px 20px;
    text-align: center;
    color: var(--color-muted);
    font-size: 1.05rem;
  }

  // Pure Multi-Column Masonry (Hugs images perfectly with ZERO empty space gaps!)
  .photos-masonry-flow {
    column-count: 3;
    column-gap: 24px;

    @media (max-width: 960px) {
      column-count: 2;
      column-gap: 18px;
    }

    @media (max-width: 580px) {
      column-count: 1;
    }
  }

  .photo-brick {
    break-inside: avoid;
    margin-bottom: 24px;
    cursor: pointer;
    display: block;
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    overflow: hidden;
    transition: transform 0.35s cubic-bezier(0.25, 1, 0.5, 1), box-shadow 0.35s ease;

    .brick-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 6px 12px;
      background-color: rgba(36, 36, 33, 0.03);
      border-bottom: 1px solid var(--color-border-light);
      font-size: 0.72rem;
      color: var(--color-muted);
      letter-spacing: 1px;
    }

    .brick-img-wrap {
      position: relative;
      width: 100%;
      overflow: hidden;
      background-color: var(--color-paper-dark);

      .brick-img {
        width: 100%;
        height: auto; // Preserves natural aspect ratio! No stretching!
        display: block;
        transition: transform 0.45s cubic-bezier(0.25, 1, 0.5, 1);
      }

      .brick-hover-overlay {
        position: absolute;
        inset: 0;
        background-color: rgba(24, 35, 34, 0.3);
        display: flex;
        align-items: center;
        justify-content: center;
        opacity: 0;
        transition: opacity 0.3s ease;

        .zoom-badge {
          width: 42px;
          height: 42px;
          background-color: var(--color-paper-light);
          color: var(--color-burgundy);
          border-radius: 50%;
          display: flex;
          align-items: center;
          justify-content: center;
          font-size: 1.1rem;
          box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
          transform: scale(0.85);
          transition: transform 0.25s ease;
        }

        .frame-badge {
          position: absolute;
          bottom: 8px;
          left: 8px;
          background-color: rgba(24, 35, 34, 0.85);
          color: #FAF7F0;
          font-size: 0.72rem;
          padding: 2px 8px;
          border-radius: var(--radius-xs);
        }
      }
    }

    &:hover {
      transform: translateY(-4px);
      box-shadow: 0 12px 28px rgba(36, 36, 33, 0.1);

      .brick-img {
        transform: scale(1.03);
      }

      .brick-hover-overlay {
        opacity: 1;

        .zoom-badge {
          transform: scale(1);
        }
      }
    }

    .brick-caption-bar {
      padding: 10px 14px;
      background-color: var(--color-paper-light);
      border-top: 1px solid var(--color-border-light);

      .caption-text {
        font-size: 0.88rem;
        color: var(--color-ink-soft);
        margin: 0;
        line-height: 1.45;
        font-style: italic;
      }
    }
  }
}

// 4. Grand Aesthetic Consultation CTA Section (Spacious & Luxurious)
.album-cta-section {
  padding: 20px 0 60px;

  .envelope-card {
    width: 100%;
    padding: 56px 60px;
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    box-shadow: 0 8px 30px rgba(36, 36, 33, 0.05);

    @media (max-width: 960px) {
      padding: 40px 32px;
    }

    @media (max-width: 600px) {
      padding: 28px 20px;
    }
  }

  .envelope-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 1px dashed var(--color-border);
    padding-bottom: 16px;
    margin-bottom: 32px;
    flex-wrap: wrap;
    gap: 12px;

    .stamp-group {
      display: flex;
      align-items: center;
      gap: 12px;
    }

    .envelope-stamp {
      font-size: 0.88rem;
      letter-spacing: 2.5px;
      color: var(--color-burgundy);
      border: 1px solid var(--color-burgundy-soft);
      padding: 3px 10px;
      border-radius: var(--radius-xs);
    }

    .postmark-tag {
      font-size: 0.75rem;
      color: var(--color-muted);
      border: 1px solid var(--color-border);
      padding: 2px 8px;
      border-radius: 20px;
    }

    .envelope-label {
      font-size: 0.78rem;
      letter-spacing: 1.5px;
      color: var(--color-muted);
    }
  }

  .envelope-body {
    display: grid;
    grid-template-columns: 1.15fr 1fr;
    gap: 56px;
    align-items: flex-start;

    @media (max-width: 960px) {
      grid-template-columns: 1fr;
      gap: 32px;
    }
  }

  .cta-info {
    .cta-sub {
      font-size: 0.78rem;
      letter-spacing: 2px;
      color: var(--color-burgundy);
      display: block;
      margin-bottom: 8px;
    }

    .cta-title {
      font-size: clamp(1.8rem, 2.6vw, 2.3rem);
      font-weight: 600;
      color: var(--color-ink);
      margin: 0 0 14px;
      line-height: 1.25;
      letter-spacing: 0.3px;
    }

    .cta-desc {
      font-size: 1.02rem;
      color: var(--color-ink-soft);
      line-height: 1.7;
      margin: 0 0 24px;
    }

    .cta-benefits-list {
      display: flex;
      flex-direction: column;
      gap: 10px;
      padding-top: 16px;
      border-top: 1px solid var(--color-border-light);

      .benefit-item {
        display: flex;
        align-items: center;
        gap: 10px;
        font-size: 0.95rem;
        color: var(--color-ink-soft);

        i {
          color: var(--color-burgundy);
          font-size: 0.95rem;
        }
      }
    }
  }

  .cta-form {
    display: flex;
    flex-direction: column;
    gap: 16px;
    background-color: rgba(36, 36, 33, 0.02);
    padding: 24px;
    border: 1px solid var(--color-border-light);
    border-radius: var(--radius-xs);

    @media (max-width: 600px) {
      padding: 16px;
    }
  }

  .form-inputs-row {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 14px;

    @media (max-width: 640px) {
      grid-template-columns: 1fr;
    }
  }

  .form-field {
    display: flex;
    flex-direction: column;
    gap: 6px;

    .field-label {
      font-size: 0.85rem;
      font-weight: 500;
      color: var(--color-ink-soft);
    }
  }

  .vintage-input {
    width: 100%;
    padding: 12px 16px;
    background-color: var(--color-paper);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    font-size: 0.95rem;
    color: var(--color-ink);
    outline: none;
    transition: border-color 0.2s, box-shadow 0.2s;

    &:focus {
      border-color: var(--color-burgundy);
      box-shadow: 0 0 0 2px rgba(142, 41, 41, 0.12);
    }
  }

  .cta-submit-btn {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 10px;
    padding: 14px 24px;
    font-size: 1.02rem;
    cursor: pointer;
    margin-top: 6px;
    width: 100%;
  }
}

// 5. Cinema Darkroom Lightbox
.cinema-lightbox {
  position: fixed;
  inset: 0;
  z-index: 9999;
  background-color: rgba(12, 15, 15, 0.97);
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  backdrop-filter: blur(8px);
}

.lightbox-toolbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px 24px;
  color: #FAF7F0;
  border-bottom: 1px solid rgba(201, 192, 178, 0.15);

  .lightbox-counter {
    font-size: 1.1rem;
    letter-spacing: 2px;
    color: #E89A3C;

    .current-num {
      font-weight: 700;
    }

    .sep {
      margin: 0 4px;
      opacity: 0.6;
    }

    .total-num {
      opacity: 0.75;
    }
  }

  .lightbox-title {
    font-size: 1.05rem;
    color: #FAF7F0;
    max-width: 50%;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;

    @media (max-width: 600px) {
      display: none;
    }
  }

  .lightbox-close-btn {
    background: none;
    border: none;
    color: #FAF7F0;
    font-size: 1.25rem;
    cursor: pointer;
    padding: 4px 8px;
    border-radius: var(--radius-xs);
    transition: color 0.2s;

    &:hover {
      color: #E89A3C;
    }
  }
}

.lightbox-stage {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px 24px;
  position: relative;
  overflow: hidden;

  .nav-btn {
    background: rgba(24, 35, 34, 0.7);
    border: 1px solid rgba(201, 192, 178, 0.2);
    color: #FAF7F0;
    width: 48px;
    height: 48px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.2rem;
    cursor: pointer;
    transition: all 0.25s ease;
    z-index: 10;

    &:hover:not(:disabled) {
      background-color: var(--color-burgundy);
      border-color: var(--color-burgundy);
    }

    &:disabled {
      opacity: 0.3;
      cursor: not-allowed;
    }

    @media (max-width: 600px) {
      width: 38px;
      height: 38px;
      font-size: 1rem;
    }
  }

  .photo-display-box {
    max-width: 84vw;
    max-height: 74vh;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;

    .active-lightbox-img {
      max-width: 100%;
      max-height: 70vh;
      object-fit: contain;
      box-shadow: 0 12px 36px rgba(0, 0, 0, 0.6);
      border: 1px solid rgba(201, 192, 178, 0.15);
    }

    .active-caption {
      margin-top: 10px;
      color: #FAF7F0;
      font-size: 1rem;
      text-align: center;
      background-color: rgba(24, 35, 34, 0.85);
      padding: 6px 16px;
      border-radius: var(--radius-xs);
      max-width: 80%;

      p {
        margin: 0;
      }
    }
  }
}

.lightbox-thumbs-bar {
  padding: 12px 20px;
  background-color: rgba(10, 12, 12, 0.98);
  border-top: 1px solid rgba(201, 192, 178, 0.15);
  overflow-x: auto;
  display: flex;
  justify-content: center;

  .thumbs-track {
    display: flex;
    gap: 8px;
    max-width: 100%;
  }

  .thumb-box {
    width: 56px;
    height: 40px;
    border: 1px solid rgba(201, 192, 178, 0.25);
    cursor: pointer;
    overflow: hidden;
    opacity: 0.5;
    transition: all 0.2s ease;
    flex-shrink: 0;

    img {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }

    &:hover {
      opacity: 0.8;
    }

    &.active {
      opacity: 1;
      border: 2px solid var(--color-burgundy);
    }
  }
}

// Lightbox Fade Animation
.lightbox-fade-enter-active,
.lightbox-fade-leave-active {
  transition: opacity 0.25s ease;
}

.lightbox-fade-enter-from,
.lightbox-fade-leave-to {
  opacity: 0;
}
</style>
