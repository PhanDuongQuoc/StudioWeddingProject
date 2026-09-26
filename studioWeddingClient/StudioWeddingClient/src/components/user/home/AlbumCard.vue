<template>
  <div class="album-card paper-card">
    <div class="album-cover-wrapper">
      <img
        :src="album?.coverImageUrl || defaultCover"
        :alt="album?.title || 'Album'"
        class="album-cover film-photo"
        loading="lazy"
      />
      <span class="album-photo-badge" v-if="album?.totalPhotos">
        {{ album.totalPhotos }} ảnh
      </span>
    </div>
    <div class="album-info">
      <div class="album-category" v-if="album?.categoryName">
        {{ album.categoryName }}
      </div>
      <h3 class="album-title font-serif">{{ album?.title }}</h3>
      <p class="album-desc" v-if="album?.description">{{ album.description }}</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { HomeAlbum } from '@/types/home'

defineProps<{
  album: HomeAlbum
}>()

const defaultCover = 'https://images.unsplash.com/photo-1583939003579-730e3918a45a?q=80&w=800&auto=format&fit=crop'
</script>

<style lang="scss" scoped>
.album-card {
  display: flex;
  flex-direction: column;
  height: 100%;
  overflow: hidden;
  cursor: pointer;
  transition: transform 0.35s cubic-bezier(0.25, 1, 0.5, 1),
              box-shadow 0.35s cubic-bezier(0.25, 1, 0.5, 1),
              border-color 0.3s ease;

  &:hover {
    .album-cover {
      transform: scale(1.06);
      filter: saturate(0.98) contrast(1.02);
    }

    .album-title {
      color: var(--color-burgundy);
    }

    .album-photo-badge {
      background-color: var(--color-burgundy);
      color: #FFF;
      transform: scale(1.05);
    }
  }
}

.album-cover-wrapper {
  position: relative;
  width: 100%;
  aspect-ratio: 4 / 3;
  overflow: hidden;
  background-color: var(--color-paper-dark);
}

.album-cover {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
  transition: transform 0.5s cubic-bezier(0.25, 1, 0.5, 1), filter 0.4s ease;
}

.album-photo-badge {
  position: absolute;
  bottom: 10px;
  right: 10px;
  background-color: rgba(24, 35, 34, 0.78);
  color: #F8F4EA;
  font-size: 0.75rem;
  padding: 3px 9px;
  border-radius: var(--radius-xs);
  backdrop-filter: blur(4px);
  transition: all 0.3s ease;
  z-index: 2;
}

.album-info {
  padding: 18px;
  display: flex;
  flex-direction: column;
  flex-grow: 1;
}

.album-category {
  font-size: 0.78rem;
  text-transform: uppercase;
  letter-spacing: 1.2px;
  color: var(--color-burgundy);
  font-weight: 600;
  margin-bottom: 6px;
  transition: color 0.2s ease;
}

.album-title {
  font-size: 1.28rem;
  font-weight: 600;
  color: var(--color-ink);
  margin: 0 0 6px;
  line-height: 1.3;
  transition: color 0.25s ease;
}

.album-desc {
  font-size: 0.88rem;
  color: var(--color-muted);
  line-height: 1.5;
  margin: 0;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>
