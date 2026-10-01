import { ref } from 'vue'
import { albumService } from '@/services/albumService'
import type { AlbumDetailDto } from '@/types/album'

export function useAlbumDetail() {
  const album = ref<AlbumDetailDto | null>(null)
  const isLoading = ref<boolean>(true)
  const error = ref<string | null>(null)

  const fetchAlbumDetail = async (slug: string) => {
    if (!slug) {
      error.value = 'Slug album không hợp lệ'
      isLoading.value = false
      return
    }

    isLoading.value = true
    error.value = null

    try {
      const data = await albumService.getAlbumDetailBySlug(slug)
      album.value = data
    } catch (err: unknown) {
      const errorObj = err as { response?: { data?: { message?: string } }; message?: string }
      error.value = errorObj?.response?.data?.message || errorObj?.message || 'Không thể tải chi tiết album'
      console.error('Lỗi khi tải chi tiết album:', err)
    } finally {
      isLoading.value = false
    }
  }

  return {
    album,
    isLoading,
    error,
    fetchAlbumDetail
  }
}
