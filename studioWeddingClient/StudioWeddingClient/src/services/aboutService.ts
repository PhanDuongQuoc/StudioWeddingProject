import api from '@/boot/api-gateway/axios'
import type { AboutResponse } from '@/types/about'

/**
 * Service lấy thông tin trang Giới thiệu (About Us) từ API Backend
 * @param aboutId (Tùy chọn) Mã ID của bản ghi giới thiệu, mặc định lấy bản ghi active
 */
export const getAboutData = async (aboutId?: number): Promise<AboutResponse> => {
  const url = aboutId ? `/About/${aboutId}` : '/About'
  const response = await api.get(url)
  return response.data
}

export default {
  getAboutData
}
