import api from '@/boot/api-gateway/axios'

/**
 * Service lấy dữ liệu trang chủ từ API Backend
 */
export const getHomePageData = async () => {
  const response = await api.get('/Home/home-page')
  return response.data
}
