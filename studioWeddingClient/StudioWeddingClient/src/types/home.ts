export interface HomeImageBanner {
  imageBannerId?: number
  imageUrl: string
  mobileImageUrl?: string | null
  altText?: string | null
  displayOrder?: number
}

export interface HomeBanner {
  bannerId: number
  title: string
  description?: string | null
  linkUrl?: string | null
  displayOrder?: number
  images?: HomeImageBanner[]
}

export interface HomeAlbum {
  albumId: number
  title: string
  slug: string
  description?: string | null
  coverImageUrl?: string | null
  categoryId?: number
  categoryName?: string
  categorySlug?: string | null
  totalPhotos?: number
}

export interface HomeService {
  serviceId: number
  name: string
  slug: string
  description?: string | null
  price: number
  durationMinutes?: number | null
  imageUrl?: string | null
}

export interface HomePackage {
  packageId: number
  name: string
  slug: string
  description?: string | null
  price: number
  imageUrl?: string | null
  includedServices?: string[]
}

export interface HomeReview {
  reviewId: number
  customerName: string
  rating: number
  title?: string | null
  comment?: string | null
  createdAt?: string
}

export interface HomeData {
  banners: HomeBanner[]
  featuredAlbums: HomeAlbum[]
  services: HomeService[]
  packages: HomePackage[]
  reviews: HomeReview[]
}
