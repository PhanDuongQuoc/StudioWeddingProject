export interface PhotoDto {
  photoId: number
  imageUrl: string
  thumbnailUrl?: string | null
  caption?: string | null
  displayOrder: number
  isActive: boolean
  createdAt: string
}

export interface AlbumDetailDto {
  albumId: number
  title: string
  slug: string
  description?: string | null
  coverImageUrl?: string | null
  isFeatured: boolean
  isActive: boolean
  displayOrder: number
  createdAt: string
  updatedAt: string
  categoryName: string
  categorySlug: string
  photos: PhotoDto[]
}

export interface AlbumItemDto {
  albumId: number
  title: string
  slug: string
  description?: string | null
  coverImageUrl?: string | null
  categoryId: number
  categoryName: string
  categorySlug: string
  totalPhotos: number
  isFeatured: boolean
  displayOrder: number
  createdAt: string
}

export interface CategoryFilterDto {
  categoryId: number
  name: string
  slug: string
  albumCount: number
}

export interface AlbumListResponse {
  items: AlbumItemDto[]
  categories: CategoryFilterDto[]
  totalItems: number
  page: number
  pageSize: number
  totalPages: number
  success: boolean
  message: string
}
