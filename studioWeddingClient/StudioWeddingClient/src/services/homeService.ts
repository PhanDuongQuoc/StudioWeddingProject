import api from '@/boot/api-gateway/axios'
import type { HomeData, ContactRequest, ContactResponse } from '@/types/home'

/**
 * Service lấy dữ liệu trang chủ từ API Backend
 */
export const getHomePageData = async (): Promise<HomeData> => {
  const response = await api.get('/Home/home-page')
  return response.data
}

/**
 * Service gửi email liên hệ/tư vấn từ khách hàng
 */
export const sendContactEmail = async (data: ContactRequest): Promise<ContactResponse> => {
  const response = await api.post('/Home/send-contact-email-from-customer', data)
  return response.data
}

export default {
  getHomePageData,
  sendContactEmail
}
