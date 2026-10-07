import { ref } from 'vue'
import { serviceService } from '@/services/serviceService'
import type { ServiceItemDto } from '@/types/service'

export function useService() {
  const services = ref<ServiceItemDto[]>([])
  const searchQuery = ref<string>('')
  const page = ref<number>(1)
  const pageSize = ref<number>(12)
  const totalItems = ref<number>(0)
  const totalPages = ref<number>(1)
  const isLoading = ref<boolean>(true)
  const error = ref<string | null>(null)

  const fetchServices = async () => {
    isLoading.value = true
    error.value = null
    try {
      const response = await serviceService.getServices({
        search: searchQuery.value.trim() ? searchQuery.value.trim() : undefined,
        page: page.value,
        pageSize: pageSize.value,
      })

      if (response && response.success !== false) {
        services.value = response.items || []
        totalItems.value = response.totalItems || 0
        totalPages.value = response.totalPages || 1
        page.value = response.page || 1
      } else {
        error.value = response?.message || 'Không thể tải danh sách dịch vụ'
      }
    } catch (err: unknown) {
      console.error('Lỗi khi tải danh sách dịch vụ:', err)
      const errorObj = err as { response?: { data?: { message?: string } }; message?: string }
      error.value = errorObj?.response?.data?.message || errorObj?.message || 'Không thể kết nối đến máy chủ để tải dịch vụ'
    } finally {
      isLoading.value = false
    }
  }

  const setPage = async (newPage: number) => {
    if (newPage >= 1 && newPage <= totalPages.value) {
      page.value = newPage
      await fetchServices()
      window.scrollTo({ top: 0, behavior: 'smooth' })
    }
  }

  const handleSearch = async (term: string) => {
    searchQuery.value = term
    page.value = 1
    await fetchServices()
  }

  return {
    services,
    searchQuery,
    page,
    pageSize,
    totalItems,
    totalPages,
    isLoading,
    error,
    fetchServices,
    setPage,
    handleSearch,
  }
}

export default useService
