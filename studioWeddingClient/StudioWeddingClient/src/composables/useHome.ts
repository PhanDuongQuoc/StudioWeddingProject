import { ref } from 'vue'
import { getHomePageData } from '@/services/homeService'
import type { HomeData } from '@/types/home'

/**
 * Composable quản lý state và gọi API cho trang Home
 */
export function useHome() {
  const homeData = ref<HomeData | null>(null)
  const isLoading = ref<boolean>(true)
  const error = ref<string | null>(null)

  const fetchHomeData = async () => {
    isLoading.value = true
    error.value = null
    try {
      const data = await getHomePageData()
      homeData.value = data
    } catch (err: unknown) {
      console.error('Lỗi khi tải dữ liệu trang chủ:', err)
      const errorObj = err as { response?: { data?: { message?: string } }; message?: string }
      error.value = errorObj?.response?.data?.message || errorObj?.message || 'Không thể tải dữ liệu trang chủ'
    } finally {
      isLoading.value = false
    }
  }

  return {
    homeData,
    isLoading,
    error,
    fetchHomeData
  }
}
