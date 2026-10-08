<template>
  <section id="goi-cuoi" class="packages-section" v-reveal>
    <div class="studio-container">
      <SectionTitle
        title="Bảng giá gói cưới"
        subtitle="Lựa chọn gói dịch vụ trọn gói tối ưu chi phí và chất lượng cho ngày trọng đại"
        tag="Packages"
        align="center"
      />

      <div class="packages-grid stagger-grid" v-reveal>
        <PackageCard
          v-for="(pkg, idx) in (packages || [])"
          :key="pkg.packageId"
          :pkg="pkg"
          :is-featured="idx === 1 || pkg.slug?.includes('cao-cap')"
        />
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import type { HomePackage } from '@/types/home'
import SectionTitle from '@/components/common/SectionTitle.vue'
import PackageCard from '@/components/user/home/PackageCard.vue'

defineProps<{
  packages?: HomePackage[]
}>()
</script>

<style lang="scss" scoped>
.packages-section {
  padding: 80px 0;
  background-color: var(--color-paper);
}

.packages-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 28px;
  align-items: stretch;
  margin-top: 40px;

  @media (max-width: 900px) {
    grid-template-columns: 1fr;
    max-width: 480px;
    margin-left: auto;
    margin-right: auto;
    gap: 32px;
  }
}
</style>
