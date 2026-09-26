import { ref } from 'vue'
import { getHomePageData } from '@/services/homeService'

/**
 * Composable quản lý state và gọi API cho trang Home
 */
export function useHome() {
  const homeData = ref(null)
  const isLoading = ref(true)
  const error = ref(null)

  const fetchHomeData = async () => {
    isLoading.value = true
    error.value = null
    try {
      const data = await getHomePageData()
      homeData.value = data
    } catch (err) {
      console.error('Lỗi khi tải dữ liệu trang chủ:', err)
      error.value = err?.response?.data?.message || err.message || 'Không thể tải dữ liệu trang chủ'
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
