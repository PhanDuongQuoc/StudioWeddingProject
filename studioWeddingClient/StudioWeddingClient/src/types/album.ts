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
