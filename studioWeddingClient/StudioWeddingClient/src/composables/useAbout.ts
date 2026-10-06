import { ref } from 'vue'
import { getAboutData } from '@/services/aboutService'
import type { AboutResponse } from '@/types/about'

/**
 * Composable quản lý state và gọi API cho trang Giới thiệu (About Us)
 */
export function useAbout() {
  const aboutData = ref<AboutResponse | null>(null)
  const isLoading = ref<boolean>(true)
  const error = ref<string | null>(null)

  const fetchAboutData = async (aboutId?: number) => {
    isLoading.value = true
    error.value = null
    try {
      const data = await getAboutData(aboutId)
      if (data && data.success !== false) {
        aboutData.value = data
      } else {
        error.value = data?.message || 'Không tìm thấy dữ liệu Giới thiệu'
      }
    } catch (err: unknown) {
      console.error('Lỗi khi tải dữ liệu Giới thiệu:', err)
      const errorObj = err as { response?: { data?: { message?: string } }; message?: string }
      error.value = errorObj?.response?.data?.message || errorObj?.message || 'Không thể kết nối đến máy chủ để tải dữ liệu'
    } finally {
      isLoading.value = false
    }
  }

  return {
    aboutData,
    isLoading,
    error,
    fetchAboutData
  }
}
