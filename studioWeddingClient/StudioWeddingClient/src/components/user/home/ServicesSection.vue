<template>
  <section id="dich-vu" class="services-section">
    <div class="studio-container">
      <SectionTitle
        title="Dịch vụ của chúng tôi"
        subtitle="Giải pháp toàn diện và chuyên nghiệp cho ngày trọng đại của bạn"
        tag="Services"
      />

      <div class="services-grid">
        <ServiceCard
          v-for="(service, idx) in (services || [])"
          :key="service.serviceId"
          :service="service"
          class="animate-fade-in-up"
          :class="`stagger-${(idx % 3) + 1}`"
        />
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import type { HomeService } from '@/types/home'
import SectionTitle from '@/components/common/SectionTitle.vue'
import ServiceCard from '@/components/user/home/ServiceCard.vue'

defineProps<{
  services?: HomeService[]
}>()
</script>

<style lang="scss" scoped>
.services-section {
  padding: 60px 0;
  background-color: var(--color-paper-light);
  border-top: 1px solid var(--color-border);
  border-bottom: 1px solid var(--color-border);
}

.services-grid {
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
