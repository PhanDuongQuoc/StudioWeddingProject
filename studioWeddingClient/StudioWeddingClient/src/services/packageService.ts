import api from '@/boot/api-gateway/axios'
import type { PackageDetailResponse } from '@/types/package'

export const getPackageDetail = async (
  slug: string,
  orderPage = 1,
  orderPageSize = 3
): Promise<PackageDetailResponse> => {
  const response = await api.get(`/Package/slug/${slug}`, {
    params: { orderPage, orderPageSize }
  })
  return response.data
}

export default {
  getPackageDetail
}
