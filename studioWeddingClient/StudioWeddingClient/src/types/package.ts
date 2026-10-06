export interface PackageIncludedServiceDto {
  serviceId: number
  name: string
  slug: string
  description?: string | null
  unitPrice: number
  quantity: number
  durationMinutes?: number | null
  imageUrl?: string | null
}

export interface RelatedPackageResponse {
  packageId: number
  name: string
  slug: string
  price: number
  imageUrl?: string | null
}

export interface PackageDetailResponse {
  packageId: number
  name: string
  slug: string
  description?: string | null
  price: number
  imageUrl?: string | null
  countBooking: number
  originalTotalPrice?: number | null
  createdAt: string
  updatedAt: string
  includedServices: PackageIncludedServiceDto[]
  otherPackages: RelatedPackageResponse[]
  otherPage: number
  otherPageSize: number
  otherTotal: number
  success: boolean
  message: string
}
