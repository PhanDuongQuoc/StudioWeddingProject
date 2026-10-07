import api from '@/boot/api-gateway/axios'
import type { ServiceDetailResponse, ServiceListResponse } from '@/types/service'

export const serviceService = {
  async getServices(params?: {
    search?: string
    page?: number
    pageSize?: number
  }): Promise<ServiceListResponse> {
    const response = await api.get<ServiceListResponse>('/service', { params })
    return response.data
  },

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
