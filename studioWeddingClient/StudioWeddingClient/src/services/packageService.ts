import api from '@/boot/api-gateway/axios'
import type { PackageDetailResponse, PackageListResponse } from '@/types/package'

export const getPackages = async (params?: {
  search?: string
  page?: number
  pageSize?: number
}): Promise<PackageListResponse> => {
  const response = await api.get<PackageListResponse>('/Package', { params })
  return response.data
}

export const getPackageDetail = async (
  slug: string,
  orderPage = 1,
  orderPageSize = 3
): Promise<PackageDetailResponse> => {
  const response = await api.get<PackageDetailResponse>(`/Package/slug/${slug}`, {
    params: { orderPage, orderPageSize }
  })
  return response.data
}

export const packageService = {
  getPackages,
  getPackageDetail
}

export default packageService
