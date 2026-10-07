import { ref } from 'vue'
import { albumService } from '@/services/albumService'
import type { AlbumItemDto, CategoryFilterDto } from '@/types/album'

export function useAlbum() {
  const albums = ref<AlbumItemDto[]>([])
  const categories = ref<CategoryFilterDto[]>([])
  const selectedCategory = ref<string>('all')
  const searchQuery = ref<string>('')
  const page = ref<number>(1)
  const pageSize = ref<number>(9)
  const totalItems = ref<number>(0)
  const totalPages = ref<number>(1)
  const isLoading = ref<boolean>(true)
  const error = ref<string | null>(null)

  const fetchAlbums = async () => {
    isLoading.value = true
    error.value = null
    try {
      const response = await albumService.getAlbums({
        categorySlug: selectedCategory.value === 'all' ? undefined : selectedCategory.value,
        search: searchQuery.value.trim() ? searchQuery.value.trim() : undefined,
        page: page.value,
        pageSize: pageSize.value,
      })

      if (response && response.success !== false) {
        albums.value = response.items || []
        categories.value = response.categories || []
        totalItems.value = response.totalItems || 0
        totalPages.value = response.totalPages || 1
        page.value = response.page || 1
      } else {
        error.value = response?.message || 'Không thể tải danh sách album'
      }
    } catch (err: unknown) {
      console.error('Lỗi khi tải danh sách album:', err)
      const errorObj = err as { response?: { data?: { message?: string } }; message?: string }
      error.value = errorObj?.response?.data?.message || errorObj?.message || 'Không thể kết nối đến máy chủ để tải album'
    } finally {
      isLoading.value = false
    }
  }

  const setCategory = async (categorySlug: string) => {
    selectedCategory.value = categorySlug
    page.value = 1
    await fetchAlbums()
  }

  const setPage = async (newPage: number) => {
    if (newPage >= 1 && newPage <= totalPages.value) {
      page.value = newPage
      await fetchAlbums()
      window.scrollTo({ top: 0, behavior: 'smooth' })
    }
  }

  const handleSearch = async (term: string) => {
    searchQuery.value = term
    page.value = 1
    await fetchAlbums()
  }

  return {
    albums,
    categories,
    selectedCategory,
    searchQuery,
    page,
    pageSize,
    totalItems,
    totalPages,
    isLoading,
    error,
    fetchAlbums,
    setCategory,
    setPage,
    handleSearch,
  }
}

export default useAlbum
