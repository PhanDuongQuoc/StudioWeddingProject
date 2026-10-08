<template>
  <div class="album-detail-page">
    <!-- 1. Unified Loading State -->
    <VintageLoading v-if="isLoading" text="Đang tải bộ ảnh cưới..." />

    <!-- 2. Error State -->
    <div v-else-if="error || !album" class="album-error-state studio-container">
      <div class="error-box paper-card">
        <i class="fa-solid fa-circle-exclamation error-icon"></i>
        <h2 class="error-title font-serif">Không tìm thấy Album</h2>
        <p class="error-desc">{{ error || 'Album bạn đang tìm kiếm không tồn tại hoặc đã được ẩn.' }}</p>
        <router-link to="/album" class="btn-vintage back-btn font-serif">
          <i class="fa-solid fa-arrow-left"></i>
          <span>Quay lại trang Album</span>
        </router-link>
      </div>
    </div>

    <!-- 3. Main Content (Mockup Top-Right Exact Match) -->
    <main v-else class="album-detail-main studio-container">
      <!-- Breadcrumb Bar -->
      <nav class="breadcrumb-trail font-serif" aria-label="Breadcrumb">
        <router-link to="/trang-chu">Trang chủ</router-link>
        <span class="sep">/</span>
        <router-link to="/album">Album</router-link>
        <span class="sep" v-if="album.categoryName">/</span>
        <span v-if="album.categoryName" class="cat-crumb">{{ album.categoryName }}</span>
      </nav>

      <!-- Top Showcase Section: Large Main Photo + Vertical Thumbnails -->
      <section class="album-showcase-section">
        <div class="showcase-grid">
          <!-- Left: Big Main Photo -->
          <div class="main-photo-frame" @click="openLightbox(currentPhotoIndex)">
            <img
              :src="currentMainPhotoUrl"
              :alt="currentMainPhotoCaption || album.title"
              class="main-img film-photo"
            />
            <div class="photo-zoom-hint font-serif">
              <i class="fa-solid fa-magnifying-glass-plus"></i>
              <span>Nhấn để xem & phóng to</span>
            </div>
          </div>

          <!-- Right: Stack of 3 Vertical Thumbnails + Prev/Next Buttons -->
          <div class="side-thumbnails-col" v-if="allPhotos.length > 1">
            <div class="thumbnails-stack">
              <div
                v-for="(photo, idx) in visibleSideThumbnails"
                :key="photo.photoId || idx"
                class="thumb-box"
                :class="{ active: currentPhotoIndex === photo.originalIndex }"
                @click="setMainPhoto(photo.originalIndex)"
              >
                <img
                  :src="photo.thumbnailUrl || photo.imageUrl"
                  :alt="photo.caption || `${album.title} thumbnail ${idx + 1}`"
                  class="thumb-img film-photo"
                />
              </div>
            </div>

            <!-- Arrows underneath vertical thumbnails -->
            <div class="thumb-nav-arrows">
              <button
                class="thumb-arrow-btn"
                @click="prevThumbnail"
                title="Ảnh trước"
              >
                <i class="fa-solid fa-chevron-left"></i>
              </button>
              <button
                class="thumb-arrow-btn"
                @click="nextThumbnail"
                title="Ảnh tiếp theo"
              >
                <i class="fa-solid fa-chevron-right"></i>
              </button>
            </div>
          </div>
        </div>
      </section>

      <!-- Middle Info Section: Title, Description & Metadata -->
      <section class="album-info-section">
        <h1 class="album-title font-serif">{{ album.title }}</h1>

        <p class="album-description font-serif" v-if="album.description">
          {{ album.description }}
        </p>

        <!-- Meta strip: photo count, year, category -->
        <div class="album-meta-strip font-serif">
          <span class="meta-item" v-if="allPhotos.length > 0">
            <i class="fa-solid fa-camera"></i>
            <span>{{ allPhotos.length }} ảnh</span>
          </span>

          <span class="meta-dot" v-if="albumYear">·</span>
          <span class="meta-item" v-if="albumYear">
            <i class="fa-regular fa-calendar"></i>
            <span>{{ albumYear }}</span>
          </span>

          <span class="meta-dot" v-if="album.categoryName">·</span>
          <span class="meta-item" v-if="album.categoryName">
            <i class="fa-solid fa-tag"></i>
            <span>{{ album.categoryName }}</span>
          </span>
        </div>
      </section>

      <!-- Bottom Photo Gallery Grid (Contact Sheet Style) -->
      <section class="bottom-gallery-section" v-if="allPhotos.length > 0">
        <div class="bottom-photos-grid">
          <div
            v-for="(photo, index) in allPhotos"
            :key="photo.photoId || index"
            class="gallery-photo-item"
            :class="{ active: currentPhotoIndex === index }"
            @click="openLightbox(index)"
          >
            <img
              :src="photo.thumbnailUrl || photo.imageUrl"
              :alt="photo.caption || `${album.title} ${index + 1}`"
              class="gallery-thumb-img film-photo"
              loading="lazy"
            />
            <div class="photo-overlay-zoom">
              <i class="fa-solid fa-magnifying-glass-plus"></i>
            </div>
          </div>
        </div>
      </section>
    </main>

    <!-- Vintage High-End Lightbox Viewer Component -->
    <VintageLightbox
      :is-open="lightboxOpen"
      :photos="allPhotos"
      :current-index="currentPhotoIndex"
      :album-title="album?.title"
      @update:is-open="lightboxOpen = $event"
      @update:current-index="currentPhotoIndex = $event"
      @close="lightboxOpen = false"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useAlbumDetail } from '@/composables/useAlbumDetail'
import VintageLoading from '@/components/common/VintageLoading.vue'
import VintageLightbox from '@/components/common/VintageLightbox.vue'

const route = useRoute()
const { album, isLoading, error, fetchAlbumDetail } = useAlbumDetail()

const currentPhotoIndex = ref(0)
const thumbWindowStart = ref(0)
const lightboxOpen = ref(false)

const allPhotos = computed(() => {
  if (!album.value) return []
  if (album.value.photos && album.value.photos.length > 0) {
    return album.value.photos
  }
  if (album.value.coverImageUrl) {
    return [
      {
        photoId: 0,
        imageUrl: album.value.coverImageUrl,
        thumbnailUrl: album.value.coverImageUrl,
        caption: album.value.title,
        displayOrder: 1,
        isActive: true,
        createdAt: album.value.createdAt
      }
    ]
  }
  return []
})

const currentMainPhotoUrl = computed(() => {
  const photos = allPhotos.value
  if (photos.length === 0) return album.value?.coverImageUrl || ''
  const p = photos[currentPhotoIndex.value] || photos[0]
  return p.imageUrl || p.thumbnailUrl || ''
})

const currentMainPhotoCaption = computed(() => {
  const photos = allPhotos.value
  if (photos.length === 0) return ''
  const p = photos[currentPhotoIndex.value]
  return p?.caption || ''
})

const albumYear = computed(() => {
  if (!album.value?.createdAt) return '2024'
  try {
    return new Date(album.value.createdAt).getFullYear().toString()
  } catch {
    return '2024'
  }
})

// Show 3 vertical thumbnails window
const visibleSideThumbnails = computed(() => {
  const photos = allPhotos.value
  if (photos.length <= 1) return []
  const count = 3
  const start = thumbWindowStart.value % photos.length
  const res = []
  for (let i = 0; i < Math.min(count, photos.length); i++) {
    const idx = (start + i) % photos.length
    res.push({
      ...photos[idx],
      originalIndex: idx
    })
  }
  return res
})

function setMainPhoto(index: number) {
  currentPhotoIndex.value = index
}

function nextThumbnail() {
  const photos = allPhotos.value
  if (photos.length === 0) return
  thumbWindowStart.value = (thumbWindowStart.value + 1) % photos.length
  currentPhotoIndex.value = (currentPhotoIndex.value + 1) % photos.length
}

function prevThumbnail() {
  const photos = allPhotos.value
  if (photos.length === 0) return
  thumbWindowStart.value = (thumbWindowStart.value - 1 + photos.length) % photos.length
  currentPhotoIndex.value = (currentPhotoIndex.value - 1 + photos.length) % photos.length
}

function openLightbox(index: number) {
  currentPhotoIndex.value = index
  lightboxOpen.value = true
}

onMounted(async () => {
  const slug = route.params.slug as string
  if (slug) {
    await fetchAlbumDetail(slug)
  }
})
</script>

<style lang="scss" scoped>
.album-detail-page {
  min-height: 100vh;
  background-color: var(--color-paper);
  color: var(--color-ink);
  padding: 24px 0 80px;
}

.breadcrumb-trail {
  margin-bottom: 24px;
}

// Top Showcase: 78% Main photo + 22% Vertical Thumbnails
.album-showcase-section {
  margin-bottom: 28px;

  .showcase-grid {
    display: grid;
    grid-template-columns: 1fr 220px;
    gap: 16px;
    align-items: stretch;

    @media (max-width: 800px) {
      grid-template-columns: 1fr;
      gap: 12px;
    }
  }

  .main-photo-frame {
    width: 100%;
    aspect-ratio: 16 / 10;
    overflow: hidden;
    background-color: var(--color-paper-dark);
    border: 1px solid var(--color-border);
    cursor: pointer;
    position: relative;

    .main-img {
      width: 100%;
      height: 100%;
      object-fit: cover;
      display: block;
      transition: transform 0.5s cubic-bezier(0.2, 0.8, 0.2, 1);
    }

    .photo-zoom-hint {
      position: absolute;
      bottom: 14px;
      right: 14px;
      background: rgba(20, 26, 26, 0.8);
      color: #FAF7F0;
      backdrop-filter: blur(6px);
      border: 1px solid rgba(250, 247, 240, 0.25);
      border-radius: var(--radius-xs, 4px);
      padding: 6px 14px;
      font-size: 0.85rem;
      display: flex;
      align-items: center;
      gap: 8px;
      opacity: 0;
      transform: translateY(6px);
      transition: all 0.3s ease;
      pointer-events: none;

      i {
        color: #D4A373;
        font-size: 0.9rem;
      }
    }

    &:hover {
      .main-img {
        transform: scale(1.03);
      }

      .photo-zoom-hint {
        opacity: 1;
        transform: translateY(0);
      }
    }
  }

  .side-thumbnails-col {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    gap: 8px;

    @media (max-width: 800px) {
      flex-direction: row;
      align-items: center;
    }

    .thumbnails-stack {
      display: flex;
      flex-direction: column;
      gap: 8px;
      flex-grow: 1;

      @media (max-width: 800px) {
        flex-direction: row;
        width: 100%;
      }

      .thumb-box {
        width: 100%;
        flex: 1;
        overflow: hidden;
        border: 1px solid var(--color-border);
        cursor: pointer;
        opacity: 0.75;
        transition: all 0.2s ease;

        @media (max-width: 800px) {
          aspect-ratio: 4 / 3;
        }

        .thumb-img {
          width: 100%;
          height: 100%;
          object-fit: cover;
          display: block;
          transition: transform 0.3s ease;
        }

        &:hover,
        &.active {
          opacity: 1;
          border: 1.5px solid var(--color-burgundy);

          .thumb-img {
            transform: scale(1.04);
          }
        }
      }
    }

    .thumb-nav-arrows {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 12px;
      padding: 4px 0;

      .thumb-arrow-btn {
        background: var(--color-paper-light);
        border: 1px solid var(--color-border);
        width: 32px;
        height: 32px;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        font-size: 0.82rem;
        color: var(--color-ink);
        cursor: pointer;
        transition: all 0.2s;

        &:hover {
          border-color: var(--color-burgundy);
          color: var(--color-burgundy);
        }
      }
    }
  }
}

// Middle Info Section
.album-info-section {
  margin-bottom: 32px;

  .album-title {
    font-size: clamp(1.8rem, 3.5vw, 2.4rem);
    font-weight: 600;
    color: var(--color-ink);
    margin: 0 0 10px;
    letter-spacing: -0.01em;
  }

  .album-description {
    font-size: 1.05rem;
    line-height: 1.65;
    color: var(--color-ink-soft);
    margin: 0 0 16px;
    max-width: 900px;
  }

  .album-meta-strip {
    display: flex;
    align-items: center;
    gap: 10px;
    font-size: 0.9rem;
    color: var(--color-muted);

    .meta-item {
      display: inline-flex;
      align-items: center;
      gap: 6px;

      i {
        font-size: 0.85rem;
        color: var(--color-burgundy);
      }
    }

    .meta-dot {
      color: var(--color-border);
    }
  }
}

// Bottom Photo Contact Sheet Gallery Grid
.bottom-gallery-section {
  .bottom-photos-grid {
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    gap: 12px;

    @media (max-width: 1000px) {
      grid-template-columns: repeat(4, 1fr);
    }

    @media (max-width: 600px) {
      grid-template-columns: repeat(2, 1fr);
    }

    .gallery-photo-item {
      width: 100%;
      aspect-ratio: 4 / 3;
      overflow: hidden;
      border: 1px solid var(--color-border);
      cursor: pointer;
      position: relative;
      opacity: 0.92;
      transition: all 0.3s cubic-bezier(0.2, 0.8, 0.2, 1);

      .gallery-thumb-img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        display: block;
        transition: transform 0.4s ease;
      }

      .photo-overlay-zoom {
        position: absolute;
        inset: 0;
        background: rgba(20, 26, 26, 0.45);
        display: flex;
        align-items: center;
        justify-content: center;
        opacity: 0;
        transition: opacity 0.25s ease;
        color: #FAF7F0;
        font-size: 1.25rem;
      }

      &:hover {
        opacity: 1;
        border-color: var(--color-burgundy);
        transform: translateY(-2px);
        box-shadow: 0 6px 16px rgba(36, 36, 33, 0.08);

        .gallery-thumb-img {
          transform: scale(1.06);
        }

        .photo-overlay-zoom {
          opacity: 1;
        }
      }

      &.active {
        opacity: 1;
        border: 2px solid var(--color-burgundy);
      }
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
