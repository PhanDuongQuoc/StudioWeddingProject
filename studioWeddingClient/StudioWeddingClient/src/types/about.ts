export interface AboutTimeline {
  timelineId: number
  year: string
  title: string
  description?: string
  displayOrder: number
}

export interface AboutResponse {
  aboutId: number
  heroTitle: string
  heroSubtitle?: string
  heroImageUrl?: string
  storyTitle: string
  storyContent: string
  storyImageUrl?: string
  founderName?: string
  founderQuote?: string
  yearsExperience: number
  happyCouples: number
  satisfactionRate?: string
  timelines: AboutTimeline[]
  success: boolean
  message?: string
}
