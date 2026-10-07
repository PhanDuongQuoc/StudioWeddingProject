import api from '@/boot/api-gateway/axios'
import type { AlbumDetailDto, AlbumListResponse } from '@/types/album'

export const albumService = {
  async getAlbums(params?: {
    categorySlug?: string
    search?: string
    page?: number
    pageSize?: number
  }): Promise<AlbumListResponse> {
    const response = await api.get<AlbumListResponse>('/album', { params })
    return response.data
  },

  async getAlbumDetailBySlug(slug: string): Promise<AlbumDetailDto> {
    const response = await api.get<AlbumDetailDto>(`/album/slug/${slug}`)
    return response.data
  }
}

export default albumService
