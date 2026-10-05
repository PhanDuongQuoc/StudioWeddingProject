<template>
  <div class="user-layout">
    <AppHeader />
    <div class="layout-main-content">
      <router-view #default="{ Component }">
        <transition name="film-dissolve" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
    </div>
    <FloatingContactWidget />
    <AppFooter />
  </div>
</template>

<script setup lang="ts">
import AppHeader from '@/components/common/AppHeader.vue'
import AppFooter from '@/components/common/AppFooter.vue'
import FloatingContactWidget from '@/components/common/FloatingContactWidget.vue'
</script>

<style lang="scss" scoped>
.user-layout {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  background-color: var(--color-paper);
}

.layout-main-content {
  flex-grow: 1;
}
</style>
