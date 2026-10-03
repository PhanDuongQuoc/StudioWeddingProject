import api from '@/boot/api-gateway/axios'
import type { ServiceDetailResponse } from '@/types/service'

export const serviceService = {
  async getServiceDetail(slug: string, otherPage = 1, otherPageSize = 3): Promise<ServiceDetailResponse> {
    const response = await api.get<ServiceDetailResponse>(`/service/slug/${slug}`, {
      params: {
        otherPage,
        otherPageSize,
      },
    })
    return response.data
  },
}

export default serviceService
