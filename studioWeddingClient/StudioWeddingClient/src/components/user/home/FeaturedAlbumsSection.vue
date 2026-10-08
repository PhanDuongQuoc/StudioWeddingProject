<template>
  <section id="albums" class="featured-albums-section" v-reveal>
    <div class="studio-container">
      <SectionTitle
        title="Album ảnh nổi bật"
        subtitle="Mỗi khung hình là một câu chuyện tình yêu mang dấu ấn riêng biệt"
        tag="Portfolio"
      >
        <template #action>
          <router-link to="/album" class="view-all-link font-serif">
            <span>Xem tất cả album</span>
            <i class="fa-solid fa-arrow-right arrow"></i>
          </router-link>
        </template>
      </SectionTitle>

      <!-- Albums Grid (3 columns desktop, 2 tablet, 1 mobile) -->
      <div class="albums-grid stagger-grid" v-reveal>
        <AlbumCard
          v-for="album in (albums || [])"
          :key="album.albumId"
          :album="album"
        />
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import type { HomeAlbum } from '@/types/home'
import SectionTitle from '@/components/common/SectionTitle.vue'
import AlbumCard from '@/components/user/home/AlbumCard.vue'

defineProps<{
  albums?: HomeAlbum[]
}>()
</script>

<style lang="scss" scoped>
.featured-albums-section {
  padding: 60px 0;
  background-color: var(--color-paper);
}

.view-all-link {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-size: 1.05rem;
  color: var(--color-burgundy);
  font-weight: 500;
  padding-bottom: 2px;
  border-bottom: 1px solid var(--color-burgundy);
  transition: all 0.2s ease;

  .arrow {
    transition: transform 0.2s;
  }

  &:hover {
    color: var(--color-burgundy-dark);
    border-bottom-color: var(--color-burgundy-dark);

    .arrow {
      transform: translateX(4px);
    }
  }

  @media (max-width: 600px) {
    display: none;
  }
}

.albums-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 24px;

  @media (max-width: 900px) {
    grid-template-columns: repeat(2, 1fr);
    gap: 20px;
  }

  @media (max-width: 600px) {
    grid-template-columns: 1fr;
    gap: 16px;
  }
}
</style>
