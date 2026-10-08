<template>
  <Teleport to="body">
    <Transition name="lightbox-fade">
      <div
        v-if="isOpen && currentPhoto"
        class="vintage-lightbox-overlay"
        tabindex="-1"
        @keydown="handleKeyDown"
      >
        <!-- 1. Top Cinema Header Bar -->
        <header class="lightbox-header">
          <div class="header-left">
            <span class="wedding-seal font-serif" title="Hỷ Sự Studio">囍</span>
            <div class="title-group">
              <h3 class="album-heading font-serif">{{ albumTitle || 'Album Cưới' }}</h3>
              <span class="counter-badge font-serif">
                {{ formatNumber(currentIndex + 1) }} / {{ formatNumber(photos.length) }}
              </span>
            </div>
          </div>

          <div class="header-toolbar">
            <!-- Zoom Controls Pill -->
            <div class="zoom-pill">
              <button
                class="tool-btn zoom-sub-btn"
                :disabled="zoom <= minZoom"
                title="Thu nhỏ (-)"
                @click="zoomOut"
              >
                <i class="fa-solid fa-minus"></i>
              </button>
              <button
                class="zoom-display-btn font-serif"
                title="Đặt lại kích thước 100% (R)"
                @click="resetTransform"
              >
                {{ Math.round(zoom * 100) }}%
              </button>
              <button
                class="tool-btn zoom-sub-btn"
                :disabled="zoom >= maxZoom"
                title="Phóng to (+)"
                @click="zoomIn"
              >
                <i class="fa-solid fa-plus"></i>
              </button>
            </div>

            <!-- Rotate Button -->
            <button
              class="tool-btn"
              title="Xoay 90 độ"
              @click="rotate"
            >
              <i class="fa-solid fa-rotate-right"></i>
            </button>

            <!-- Download Button -->
            <a
              :href="currentPhoto.imageUrl"
              :download="`photo-${currentIndex + 1}.jpg`"
              target="_blank"
              rel="noopener noreferrer"
              class="tool-btn download-btn"
              title="Tải ảnh gốc"
            >
              <i class="fa-solid fa-download"></i>
            </a>

            <!-- Fullscreen Button -->
            <button
              class="tool-btn"
              :title="isFullscreen ? 'Thoát toàn màn hình' : 'Toàn màn hình'"
              @click="toggleFullscreen"
            >
              <i :class="isFullscreen ? 'fa-solid fa-compress' : 'fa-solid fa-expand'"></i>
            </button>

            <!-- Close Button -->
            <button
              class="tool-btn close-btn"
              title="Đóng (Esc)"
              @click="closeLightbox"
            >
              <i class="fa-solid fa-xmark"></i>
            </button>
          </div>
        </header>

        <!-- 2. Interactive Stage Canvas -->
        <main
          class="lightbox-stage"
          :class="{
            'is-zoomed': zoom > 1,
            'is-dragging': isDragging
          }"
          @wheel.prevent="handleWheel"
          @mousedown="handleMouseDown"
          @mousemove="handleMouseMove"
          @mouseup="handleMouseUp"
          @mouseleave="handleMouseUp"
          @touchstart="handleTouchStart"
          @touchmove="handleTouchMove"
          @touchend="handleTouchEnd"
          @dblclick="handleDoubleClick"
        >
          <!-- Stage Navigation Prev -->
          <button
            v-if="photos.length > 1"
            class="stage-arrow prev-arrow"
            title="Ảnh trước (Phím ←)"
            @click.stop="prevPhoto"
          >
            <i class="fa-solid fa-chevron-left"></i>
          </button>

          <!-- Zoomed / Panned Image Wrapper -->
          <div
            class="image-canvas"
            :style="canvasStyle"
          >
            <img
              :src="currentPhoto.imageUrl || currentPhoto.thumbnailUrl || ''"
              :alt="currentPhoto.caption || albumTitle || 'Wedding Photo'"
              class="active-photo"
              draggable="false"
            />
          </div>

          <!-- Stage Navigation Next -->
          <button
            v-if="photos.length > 1"
            class="stage-arrow next-arrow"
            title="Ảnh tiếp theo (Phím →)"
            @click.stop="nextPhoto"
          >
            <i class="fa-solid fa-chevron-right"></i>
          </button>
        </main>

        <!-- 3. Bottom Cinema Strip & Thumbnails -->
        <footer class="lightbox-footer">
          <!-- Caption if exists -->
          <div class="caption-box font-serif" v-if="currentPhoto.caption">
            <p>{{ currentPhoto.caption }}</p>
          </div>

          <!-- Bottom Thumbnail Filmstrip -->
          <div class="filmstrip-wrapper" v-if="photos.length > 1" ref="filmstripRef">
            <div class="filmstrip-track">
              <div
                v-for="(photo, index) in photos"
                :key="photo.photoId || index"
                :ref="(el) => setThumbRef(el, index)"
                class="filmstrip-thumb"
                :class="{ active: currentIndex === index }"
                @click="selectPhoto(index)"
              >
                <img
                  :src="photo.thumbnailUrl || photo.imageUrl || ''"
                  :alt="photo.caption || `Ảnh ${index + 1}`"
                  class="thumb-img"
                  loading="lazy"
                />
                <span class="thumb-index">{{ index + 1 }}</span>
              </div>
            </div>
          </div>
        </footer>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted, onUnmounted, nextTick, type ComponentPublicInstance } from 'vue'

export interface LightboxPhoto {
  photoId?: number | string
  imageUrl: string
  thumbnailUrl?: string | null
  caption?: string | null
}

const props = withDefaults(
  defineProps<{
    isOpen: boolean
    photos: LightboxPhoto[]
    currentIndex?: number
    albumTitle?: string
  }>(),
  {
    isOpen: false,
    currentIndex: 0,
    albumTitle: ''
  }
)

const emit = defineEmits<{
  (e: 'update:isOpen', value: boolean): void
  (e: 'update:currentIndex', value: number): void
  (e: 'close'): void
}>()

// State
const currentIndex = ref(props.currentIndex)
const zoom = ref(1)
const panX = ref(0)
const panY = ref(0)
const rotation = ref(0)
const isDragging = ref(false)
const dragStart = ref({ x: 0, y: 0 })
const initialPan = ref({ x: 0, y: 0 })
const isFullscreen = ref(false)

const minZoom = 0.5
const maxZoom = 3.5
const zoomStep = 0.25

const filmstripRef = ref<HTMLElement | null>(null)
const thumbRefs = ref<Record<number, HTMLElement>>({})

function setThumbRef(el: Element | ComponentPublicInstance | null, index: number) {
  if (el && el instanceof HTMLElement) {
    thumbRefs.value[index] = el
  }
}

// Sync currentIndex prop
watch(
  () => props.currentIndex,
  (newIdx) => {
    if (newIdx !== undefined && newIdx !== currentIndex.value) {
      currentIndex.value = newIdx
      resetTransform()
      scrollThumbIntoView()
    }
  }
)

// Active Photo
const currentPhoto = computed(() => {
  if (!props.photos || props.photos.length === 0) return null
  return props.photos[currentIndex.value] || props.photos[0]
})

// Transform Style
const canvasStyle = computed(() => {
  return {
    transform: `translate3d(${panX.value}px, ${panY.value}px, 0) scale(${zoom.value}) rotate(${rotation.value}deg)`,
    transition: isDragging.value ? 'none' : 'transform 0.2s cubic-bezier(0.2, 0.8, 0.2, 1)'
  }
})

function formatNumber(num: number): string {
  return num < 10 ? `0${num}` : `${num}`
}

function resetTransform() {
  zoom.value = 1
  panX.value = 0
  panY.value = 0
  rotation.value = 0
  isDragging.value = false
}

function zoomIn() {
  zoom.value = Math.min(maxZoom, +(zoom.value + zoomStep).toFixed(2))
}

function zoomOut() {
  zoom.value = Math.max(minZoom, +(zoom.value - zoomStep).toFixed(2))
  if (zoom.value <= 1) {
    panX.value = 0
    panY.value = 0
  }
}

function rotate() {
  rotation.value = (rotation.value + 90) % 360
}

function closeLightbox() {
  emit('update:isOpen', false)
  emit('close')
  resetTransform()
}

function selectPhoto(index: number) {
  currentIndex.value = index
  emit('update:currentIndex', index)
  resetTransform()
  scrollThumbIntoView()
}

function nextPhoto() {
  if (!props.photos || props.photos.length === 0) return
  const nextIdx = (currentIndex.value + 1) % props.photos.length
  selectPhoto(nextIdx)
}

function prevPhoto() {
  if (!props.photos || props.photos.length === 0) return
  const prevIdx = (currentIndex.value - 1 + props.photos.length) % props.photos.length
  selectPhoto(prevIdx)
}

function handleWheel(e: WheelEvent) {
  if (e.deltaY < 0) {
    zoomIn()
  } else {
    zoomOut()
  }
}

function handleDoubleClick() {
  if (zoom.value > 1.2) {
    resetTransform()
  } else {
    zoom.value = 2.0
  }
}

// Mouse Drag
function handleMouseDown(e: MouseEvent) {
  if (e.button !== 0) return // Left click only
  if (zoom.value <= 1) return

  isDragging.value = true
  dragStart.value = { x: e.clientX, y: e.clientY }
  initialPan.value = { x: panX.value, y: panY.value }
}

function handleMouseMove(e: MouseEvent) {
  if (!isDragging.value) return
  const deltaX = e.clientX - dragStart.value.x
  const deltaY = e.clientY - dragStart.value.y
  panX.value = initialPan.value.x + deltaX
  panY.value = initialPan.value.y + deltaY
}

function handleMouseUp() {
  isDragging.value = false
}

// Touch Drag
let touchInitialDistance = 0
function handleTouchStart(e: TouchEvent) {
  if (e.touches.length === 1 && zoom.value > 1) {
    const t = e.touches[0]
    if (!t) return
    isDragging.value = true
    dragStart.value = { x: t.clientX, y: t.clientY }
    initialPan.value = { x: panX.value, y: panY.value }
  } else if (e.touches.length === 2) {
    const t0 = e.touches[0]
    const t1 = e.touches[1]
    if (!t0 || !t1) return
    touchInitialDistance = Math.hypot(
      t0.clientX - t1.clientX,
      t0.clientY - t1.clientY
    )
  }
}

function handleTouchMove(e: TouchEvent) {
  if (e.touches.length === 1 && isDragging.value) {
    const t = e.touches[0]
    if (!t) return
    const deltaX = t.clientX - dragStart.value.x
    const deltaY = t.clientY - dragStart.value.y
    panX.value = initialPan.value.x + deltaX
    panY.value = initialPan.value.y + deltaY
  } else if (e.touches.length === 2 && touchInitialDistance > 0) {
    const t0 = e.touches[0]
    const t1 = e.touches[1]
    if (!t0 || !t1) return
    const currentDist = Math.hypot(
      t0.clientX - t1.clientX,
      t0.clientY - t1.clientY
    )
    const factor = currentDist / touchInitialDistance
    if (factor > 1.1) {
      zoomIn()
      touchInitialDistance = currentDist
    } else if (factor < 0.9) {
      zoomOut()
      touchInitialDistance = currentDist
    }
  }
}

function handleTouchEnd() {
  isDragging.value = false
  touchInitialDistance = 0
}

// Fullscreen
function toggleFullscreen() {
  if (!document.fullscreenElement) {
    document.documentElement.requestFullscreen().then(() => {
      isFullscreen.value = true
    }).catch(() => {})
  } else {
    document.exitFullscreen().then(() => {
      isFullscreen.value = false
    }).catch(() => {})
  }
}

// Keyboard
function handleKeyDown(e: KeyboardEvent) {
  if (!props.isOpen) return
  switch (e.key) {
    case 'ArrowLeft':
      prevPhoto()
      break
    case 'ArrowRight':
      nextPhoto()
      break
    case 'Escape':
      closeLightbox()
      break
    case '+':
    case '=':
      zoomIn()
      break
    case '-':
    case '_':
      zoomOut()
      break
    case 'r':
    case 'R':
    case '0':
      resetTransform()
      break
  }
}

function scrollThumbIntoView() {
  nextTick(() => {
    const targetThumb = thumbRefs.value[currentIndex.value]
    if (targetThumb && targetThumb.scrollIntoView) {
      targetThumb.scrollIntoView({
        behavior: 'smooth',
        inline: 'center',
        block: 'nearest'
      })
    }
  })
}

// Lock Body Scroll when Open
watch(
  () => props.isOpen,
  (val) => {
    if (val) {
      document.body.style.overflow = 'hidden'
      window.addEventListener('keydown', handleKeyDown)
      scrollThumbIntoView()
    } else {
      document.body.style.overflow = ''
      window.removeEventListener('keydown', handleKeyDown)
      resetTransform()
    }
  },
  { immediate: true }
)

onMounted(() => {
  if (props.isOpen) {
    window.addEventListener('keydown', handleKeyDown)
  }
})

onUnmounted(() => {
  document.body.style.overflow = ''
  window.removeEventListener('keydown', handleKeyDown)
})
</script>

<style lang="scss" scoped>
.vintage-lightbox-overlay {
  position: fixed;
  inset: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(10, 13, 14, 0.97);
  backdrop-filter: blur(16px);
  -webkit-backdrop-filter: blur(16px);
  z-index: 999999;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  overflow: hidden;
  user-select: none;
  touch-action: none;
  outline: none;
}

// 1. Header Toolbar
.lightbox-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 14px 28px;
  background: linear-gradient(180deg, rgba(10, 13, 14, 0.95) 0%, rgba(10, 13, 14, 0.4) 75%, transparent 100%);
  border-bottom: 1px solid rgba(250, 247, 240, 0.08);
  z-index: 100;

  @media (max-width: 768px) {
    padding: 10px 14px;
  }

  .header-left {
    display: flex;
    align-items: center;
    gap: 12px;

    .wedding-seal {
      color: #D4A373;
      font-size: 1.4rem;
      font-weight: bold;
      line-height: 1;
      text-shadow: 0 0 10px rgba(212, 163, 115, 0.4);
    }

    .title-group {
      display: flex;
      align-items: center;
      gap: 10px;

      .album-heading {
        font-size: 1.05rem;
        color: #FAF7F0;
        margin: 0;
        font-weight: 500;
        max-width: 380px;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;

        @media (max-width: 768px) {
          max-width: 150px;
          font-size: 0.9rem;
        }
      }

      .counter-badge {
        font-size: 0.85rem;
        color: #D4A373;
        background: rgba(212, 163, 115, 0.12);
        border: 1px solid rgba(212, 163, 115, 0.25);
        padding: 2px 8px;
        border-radius: 12px;
        letter-spacing: 0.5px;
      }
    }
  }

  .header-toolbar {
    display: flex;
    align-items: center;
    gap: 8px;

    .zoom-pill {
      display: inline-flex;
      align-items: center;
      background: rgba(250, 247, 240, 0.08);
      border: 1px solid rgba(250, 247, 240, 0.15);
      border-radius: 20px;
      padding: 2px 4px;

      .zoom-sub-btn {
        width: 28px;
        height: 28px;
        border: none;
        background: transparent;
        color: #FAF7F0;
        cursor: pointer;
        font-size: 0.75rem;
        border-radius: 50%;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        transition: all 0.2s ease;

        &:hover:not(:disabled) {
          background: rgba(250, 247, 240, 0.15);
          color: #D4A373;
        }

        &:disabled {
          opacity: 0.3;
          cursor: not-allowed;
        }
      }

      .zoom-display-btn {
        background: none;
        border: none;
        color: #FAF7F0;
        font-size: 0.8rem;
        padding: 0 6px;
        min-width: 46px;
        text-align: center;
        cursor: pointer;
        transition: color 0.2s ease;

        &:hover {
          color: #D4A373;
        }
      }
    }

    .tool-btn {
      width: 36px;
      height: 36px;
      background: rgba(250, 247, 240, 0.08);
      border: 1px solid rgba(250, 247, 240, 0.15);
      color: #FAF7F0;
      border-radius: 6px;
      display: inline-flex;
      align-items: center;
      justify-content: center;
      font-size: 0.9rem;
      cursor: pointer;
      text-decoration: none;
      transition: all 0.2s ease;

      &:hover {
        background: rgba(250, 247, 240, 0.18);
        border-color: #D4A373;
        color: #D4A373;
        transform: translateY(-1px);
      }

      &.close-btn {
        background: rgba(142, 41, 41, 0.7);
        border-color: rgba(142, 41, 41, 0.9);
        color: #FAF7F0;

        &:hover {
          background: #A33333;
          border-color: #FAF7F0;
          color: #FAF7F0;
          transform: translateY(-1px) scale(1.05);
        }
      }

      @media (max-width: 768px) {
        width: 32px;
        height: 32px;
        font-size: 0.82rem;

        &.download-btn {
          display: none;
        }
      }
    }
  }
}

// 2. Stage Canvas
.lightbox-stage {
  flex: 1;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
  padding: 10px 40px;
  cursor: default;

  &.is-zoomed {
    cursor: grab;
  }

  &.is-dragging {
    cursor: grabbing;
  }

  .stage-arrow {
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    width: 48px;
    height: 48px;
    background: rgba(10, 13, 14, 0.65);
    border: 1px solid rgba(250, 247, 240, 0.2);
    color: #FAF7F0;
    border-radius: 50%;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    font-size: 1.1rem;
    cursor: pointer;
    z-index: 50;
    backdrop-filter: blur(8px);
    transition: all 0.25s cubic-bezier(0.2, 0.8, 0.2, 1);

    &.prev-arrow {
      left: 24px;
    }

    &.next-arrow {
      right: 24px;
    }

    &:hover {
      background: var(--color-burgundy, #8E2929);
      border-color: #D4A373;
      color: #FAF7F0;
      transform: translateY(-50%) scale(1.1);
      box-shadow: 0 4px 18px rgba(142, 41, 41, 0.6);
    }

    @media (max-width: 768px) {
      width: 38px;
      height: 38px;
      font-size: 0.95rem;

      &.prev-arrow { left: 10px; }
      &.next-arrow { right: 10px; }
    }
  }

  .image-canvas {
    display: flex;
    align-items: center;
    justify-content: center;
    max-width: 100%;
    max-height: 100%;
    will-change: transform;

    .active-photo {
      max-width: 86vw;
      max-height: 72vh;
      object-fit: contain;
      box-shadow: 0 20px 60px rgba(0, 0, 0, 0.7);
      border: 1px solid rgba(250, 247, 240, 0.12);
      border-radius: 2px;
      pointer-events: auto;
      user-select: none;
    }
  }
}

// 3. Footer Strip
.lightbox-footer {
  padding: 10px 24px 18px;
  background: linear-gradient(0deg, rgba(10, 13, 14, 0.98) 0%, rgba(10, 13, 14, 0.6) 75%, transparent 100%);
  border-top: 1px solid rgba(250, 247, 240, 0.08);
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
  z-index: 100;

  @media (max-width: 768px) {
    padding: 8px 12px 14px;
  }

  .caption-box {
    color: #FAF7F0;
    font-size: 0.92rem;
    font-style: italic;
    text-align: center;
    max-width: 680px;
    margin: 0;
    opacity: 0.9;
    text-shadow: 0 1px 4px rgba(0, 0, 0, 0.8);
  }

  .filmstrip-wrapper {
    width: 100%;
    max-width: 840px;
    overflow-x: auto;
    padding: 4px 0;
    display: flex;
    justify-content: center;

    &::-webkit-scrollbar {
      height: 4px;
    }
    &::-webkit-scrollbar-thumb {
      background: rgba(250, 247, 240, 0.25);
      border-radius: 2px;
    }

    .filmstrip-track {
      display: flex;
      align-items: center;
      gap: 10px;
      min-width: max-content;
      padding: 2px 10px;
    }

    .filmstrip-thumb {
      width: 60px;
      height: 42px;
      position: relative;
      border: 1px solid rgba(250, 247, 240, 0.2);
      border-radius: 2px;
      overflow: hidden;
      cursor: pointer;
      opacity: 0.55;
      transition: all 0.2s cubic-bezier(0.2, 0.8, 0.2, 1);

      .thumb-img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        display: block;
      }

      .thumb-index {
        position: absolute;
        bottom: 2px;
        right: 2px;
        background: rgba(10, 13, 14, 0.75);
        color: #FAF7F0;
        font-size: 0.65rem;
        padding: 0 3px;
        border-radius: 1px;
      }

      &:hover {
        opacity: 0.9;
        border-color: #D4A373;
        transform: translateY(-2px);
      }

      &.active {
        opacity: 1;
        border: 1.5px solid #D4A373;
        box-shadow: 0 0 10px rgba(212, 163, 115, 0.6);
        transform: scale(1.08);
      }
    }
  }
}

// Fade Animation
.lightbox-fade-enter-active,
.lightbox-fade-leave-active {
  transition: opacity 0.28s ease, transform 0.28s ease;
}

.lightbox-fade-enter-from,
.lightbox-fade-leave-to {
  opacity: 0;
  transform: scale(0.98);
}
</style>
