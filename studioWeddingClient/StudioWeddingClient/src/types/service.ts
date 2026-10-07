export interface RelatedServiceResponse {
  serviceId: number
  name: string
  slug: string
  price: number
  imageUrl?: string | null
}

export interface ServiceDetailResponse {
  serviceId: number
  name: string
  slug: string
  description?: string | null
  price: number
  durationMinutes?: number | null
  imageUrl?: string | null
  countBooking: number
  createdAt: string
  updatedAt: string
  otherServices: RelatedServiceResponse[]
  otherPage: number
  otherPageSize: number
  otherTotal: number
  success: boolean
  message: string
}

export interface ServiceItemDto {
  serviceId: number
  name: string
  slug: string
  description?: string | null
  price: number
  durationMinutes?: number | null
  imageUrl?: string | null
  countBooking: number
  createdAt: string
}

export interface ServiceListResponse {
  items: ServiceItemDto[]
  totalItems: number
  page: number
  pageSize: number
  totalPages: number
  success: boolean
  message: string
}
