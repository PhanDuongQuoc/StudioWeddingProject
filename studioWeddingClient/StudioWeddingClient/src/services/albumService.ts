import api from '@/boot/api-gateway/axios'
import type { AlbumDetailDto } from '@/types/album'

export const albumService = {
  async getAlbumDetailBySlug(slug: string): Promise<AlbumDetailDto> {
    const response = await api.get<AlbumDetailDto>(`/album/slug/${slug}`)
    return response.data
  }
}

export default albumService
