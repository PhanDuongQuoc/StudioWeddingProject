<template>
  <div class="about-page">
    <!-- 1. Unified Vintage Loading State -->
    <VintageLoading v-if="isLoading" text="Đang mở cuốn biên niên sử Hỷ Sự Studio..." />

    <!-- 2. Error State -->
    <div v-else-if="error || !aboutData" class="about-error-state studio-container">
      <div class="error-box paper-card">
        <i class="fa-solid fa-circle-exclamation error-icon"></i>
        <h2 class="error-title font-serif">Không thể tải thông tin Giới thiệu</h2>
        <p class="error-desc">{{ error || 'Đã có lỗi xảy ra trong quá trình kết nối đến máy chủ.' }}</p>
        <div class="error-actions">
          <button @click="fetchAboutData(1)" class="btn-vintage font-serif">
            <i class="fa-solid fa-rotate-right q-mr-xs"></i>
            <span>Thử lại</span>
          </button>
          <router-link to="/trang-chu" class="btn-vintage-outline font-serif">
            <i class="fa-solid fa-arrow-left q-mr-xs"></i>
            <span>Về trang chủ</span>
          </router-link>
        </div>
      </div>
    </div>

    <!-- 3. Main Content State -->
    <main v-else class="about-main-content">
      <!-- Breadcrumb Bar -->
      <section class="about-breadcrumb-bar">
        <div class="studio-container breadcrumb-inner">
          <nav class="breadcrumb-trail" aria-label="Breadcrumb">
            <router-link to="/trang-chu">Trang chủ</router-link>
            <span class="sep">/</span>
            <span class="current-crumb">Về Hỷ Sự Studio</span>
          </nav>

          <button @click="copyShareLink" class="share-link-btn" title="Sao chép liên kết">
            <i class="fa-solid" :class="isCopied ? 'fa-check' : 'fa-arrow-up-right-from-square'"></i>
            <span>{{ isCopied ? 'Đã sao chép link' : 'Chia sẻ' }}</span>
          </button>
        </div>
      </section>

      <!-- Section 1: Cinematic Full-Width Hero Banner Showcase -->
      <section class="about-hero-section">
        <div class="cinematic-hero-banner">
          <!-- Background Image with Gradient Overlay & Grain -->
          <div class="banner-bg-wrapper">
            <img
              :src="aboutData.heroImageUrl || defaultHeroImg"
              alt="Hỷ Sự Wedding Studio Heritage"
              class="banner-bg-img"
            />
            <div class="banner-overlay"></div>
            <div class="banner-grain-layer"></div>
          </div>

          <!-- Top Film Perforation Strip / Sprocket Holes & Timecode -->
          <div class="banner-film-strip-top">
            <div class="sprocket-holes-row">
              <span v-for="i in 28" :key="'sprocket-top-' + i" class="sprocket-hole"></span>
            </div>
            <div class="studio-container">
              <div class="banner-timecode-bar">
                <div class="timecode-left">
                  <span class="rec-dot"></span>
                  <span class="rec-label font-serif">REC [●] 1998 — 2026</span>
                  <span class="film-sep">·</span>
                  <span class="film-stock font-chinese">喜事 · 百年好合</span>
                </div>
                <div class="timecode-right">
                  <span class="archive-tag font-serif">HỶ SỰ HERITAGE ARCHIVE · 35MM KODAK GOLD</span>
                </div>
              </div>
            </div>
          </div>

          <!-- Central Hero Banner Content -->
          <div class="studio-container banner-content-container">
            <div class="banner-content-inner">
              <!-- Seal Badge -->
              <div class="hero-seal-badge">
                <span class="seal-icon font-chinese">囍</span>
                <span class="seal-text font-serif">KÝ ỨC ĐIỆN ẢNH HONG KONG 90S</span>
              </div>

              <!-- Main Title -->
              <h1 class="hero-main-title font-serif">
                {{ aboutData.heroTitle || 'Nơi Lưu Giữ Tình Yêu Bằng Nước Màu Ký Ức' }}
              </h1>

              <!-- Subtitle -->
              <p class="hero-subtitle font-serif">
                {{ aboutData.heroSubtitle || 'Hỷ Sự Studio — Hành trình 28 năm gìn giữ nghệ thuật ảnh cưới mang đậm chất hoài niệm điện ảnh Hong Kong thập niên 90.' }}
              </p>

              <!-- Action CTAs -->
              <div class="banner-actions">
                <button @click="scrollToSection('brand-story')" class="btn-banner-primary font-serif">
                  <i class="fa-solid fa-book-open q-mr-xs"></i>
                  <span>Khám phá câu chuyện</span>
                </button>
                <router-link to="/album" class="btn-banner-outline font-serif">
                  <i class="fa-solid fa-images q-mr-xs"></i>
                  <span>Xem album ảnh cưới</span>
                </router-link>
              </div>
            </div>
          </div>

          <!-- Bottom Film Metadata Bar & Sprockets -->
          <div class="banner-film-strip-bottom">
            <div class="studio-container">
              <div class="banner-meta-row font-serif">
                <div class="meta-item">
                  <i class="fa-solid fa-film q-mr-xs"></i>
                  <span>ISO 400 · 35MM ANALOGUE FILM</span>
                </div>
                <div class="meta-item-center font-chinese">
                  良緣夙締 · 佳偶天成
                </div>
                <div class="meta-item">
                  <i class="fa-solid fa-location-dot q-mr-xs"></i>
                  <span>SÀI GÒN 1998 — 2026</span>
                </div>
              </div>
            </div>
            <div class="sprocket-holes-row">
              <span v-for="i in 28" :key="'sprocket-bot-' + i" class="sprocket-hole"></span>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 2: Milestone Statistics Strip -->
      <section class="milestone-stats-section">
        <div class="studio-container">
          <div class="stats-grid">
            <!-- Stat 1: Years -->
            <div class="stat-card paper-card">
              <div class="stat-icon-wrap">
                <i class="fa-solid fa-clock-rotate-left"></i>
              </div>
              <div class="stat-value font-serif">{{ aboutData.yearsExperience }}+</div>
              <div class="stat-label font-serif">Năm Di Sản Nghệ Thuật</div>
              <p class="stat-desc">Tiên phong bảo tồn phong cách ảnh cưới film Hong Kong 90s</p>
            </div>

            <!-- Stat 2: Happy Couples -->
            <div class="stat-card paper-card">
              <div class="stat-icon-wrap">
                <i class="fa-solid fa-heart"></i>
              </div>
              <div class="stat-value font-serif">{{ formatNumber(aboutData.happyCouples) }}+</div>
              <div class="stat-label font-serif">Cặp Đôi Đồng Hành</div>
              <p class="stat-desc">Được các thế hệ dâu rể Sài Gòn & cả nước gửi gắm ngày trọng đại</p>
            </div>

            <!-- Stat 3: Satisfaction -->
            <div class="stat-card paper-card">
              <div class="stat-icon-wrap">
                <i class="fa-solid fa-award"></i>
              </div>
              <div class="stat-value font-serif">{{ aboutData.satisfactionRate || '99.8%' }}</div>
              <div class="stat-label font-serif">Mức Độ Hài Lòng</div>
              <p class="stat-desc">Đánh giá 5 sao về sự chỉn chu, màu ảnh hoài niệm và dịch vụ tận tâm</p>
            </div>

            <!-- Stat 4: Authentic Tone -->
            <div class="stat-card paper-card">
              <div class="stat-icon-wrap">
                <i class="fa-solid fa-camera-retro"></i>
              </div>
              <div class="stat-value font-serif">100%</div>
              <div class="stat-label font-serif">Chất Phim Độc Bản</div>
              <p class="stat-desc">Xử lý quang học & màu ảnh thủ công giữ nguyên xúc cảm tự nhiên</p>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 3: Brand Story (Editorial 2-Column Split) -->
      <section id="brand-story" class="brand-story-section">
        <div class="studio-container">
          <div class="story-grid">
            <!-- Left: Darkroom & Vintage Photo Showcase -->
            <div class="story-visual-column">
              <div class="polaroid-stack-wrapper">
                <div class="vintage-polaroid-plate paper-card">
                  <div class="polaroid-tape"></div>
                  <div class="polaroid-img-box">
                    <img
                      :src="aboutData.storyImageUrl || defaultStoryImg"
                      alt="Câu chuyện thương hiệu Hỷ Sự"
                      class="story-img"
                    />
                    <div class="red-wax-stamp font-chinese">
                      <span>囍</span>
                      <small>喜事</small>
                    </div>
                  </div>
                  <div class="polaroid-meta-caption">
                    <span class="story-label font-serif">DARKROOM ARCHIVE · SÀI GÒN 1998</span>
                    <span class="story-sub font-serif">"Từng khuôn hình tráng bạc - Một đời chung đôi"</span>
                  </div>
                </div>

                <!-- Floating Decorative Note -->
                <div class="floating-vintage-tag font-serif">
                  <i class="fa-solid fa-fingerprint q-mr-xs"></i>
                  <span>Màu Phim Hoài Niệm Bất Hủ</span>
                </div>
              </div>
            </div>

            <!-- Right: Editorial Story Content -->
            <div class="story-text-column">
              <div class="section-tag-row">
                <span class="chinese-ornament font-chinese">喜事・品牌故事</span>
                <span class="tag-title font-serif">CÂU CHUYỆN THƯƠNG HIỆU</span>
              </div>

              <h2 class="story-title font-serif">
                {{ aboutData.storyTitle || 'Từ Tiệm Chụp Ảnh Nhỏ Góc Phố Đến Di Sản Nghệ Thuật Cưới 90s' }}
              </h2>

              <div class="story-paragraphs">
                <!-- Drop Cap Editorial Story Content -->
                <div class="story-content-text" v-html="formattedStoryContent"></div>
              </div>

              <!-- Brand Values Bulleted Highlight -->
              <div class="story-highlights-grid">
                <div class="highlight-item">
                  <div class="item-dot"></div>
                  <div class="item-info">
                    <h4 class="font-serif">Tôn Trọng Sự Tự Nhiên</h4>
                    <p>Không tạo dáng công nghiệp rập khuôn, nắm bắt khoảnh khắc cảm xúc thăng hoa chân thật nhất.</p>
                  </div>
                </div>
                <div class="highlight-item">
                  <div class="item-dot"></div>
                  <div class="item-info">
                    <h4 class="font-serif">Chế Bản In Ấn Mỹ Thuật</h4>
                    <p>100% Album photobook được in trên giấy mỹ thuật nhập khẩu lưu giữ màu sắc qua nhiều thế hệ.</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 4: 4 Core Principles / Nghệ Thuật & Triết Lý Hỷ Sự -->
      <section class="core-philosophy-section">
        <div class="studio-container">
          <div class="section-header-center">
            <div class="center-seal font-chinese">囍</div>
            <h2 class="center-title font-serif">Bốn Giá Trị Cốt Lõi Tạo Nên Di Sản Hỷ Sự</h2>
            <p class="center-subtitle font-serif">
              Từng thước ảnh cưới trao tay khách hàng là kết tinh của đam mê, sự tỉ mỉ và lòng tôn kính dành cho tình yêu đôi lứa.
            </p>
          </div>

          <div class="philosophy-grid">
            <!-- Item 1 -->
            <div class="philosophy-card paper-card">
              <div class="card-num font-serif">01</div>
              <div class="card-icon"><i class="fa-solid fa-face-smile"></i></div>
              <h3 class="card-title font-serif">Chân Thực Trong Từng Cảm Xúc</h3>
              <p class="card-desc">
                Chúng tôi không tìm kiếm sự hoàn hảo không tì vết, mà trân trọng từng ánh nhìn e ấp, cái nắm tay siết nhẹ hay nụ cười rạng rỡ của cô dâu chú rể.
              </p>
              <div class="card-seal-bg font-chinese">真</div>
            </div>

            <!-- Item 2 -->
            <div class="philosophy-card paper-card">
              <div class="card-num font-serif">02</div>
              <div class="card-icon"><i class="fa-solid fa-film"></i></div>
              <h3 class="card-title font-serif">Màu Sắc Điện Ảnh Hong Kong</h3>
              <p class="card-desc">
                Gam màu ấm áp hoài niệm, độ tương phản mượt mà và hạt grain analog đặc trưng lấy cảm hứng từ các tác phẩm điện ảnh kinh điển thập niên 90.
              </p>
              <div class="card-seal-bg font-chinese">影</div>
            </div>

            <!-- Item 3 -->
            <div class="philosophy-card paper-card">
              <div class="card-num font-serif">03</div>
              <div class="card-icon"><i class="fa-solid fa-wand-magic-sparkles"></i></div>
              <h3 class="card-title font-serif">Y Phục & Tạo Hình Độc Bản</h3>
              <p class="card-desc">
                Bộ sưu tập áo khỏa thêu tay rồng phượng, sườn xám thượng hải, comple cổ điển và váy ren vintage được phục dựng theo đúng tỷ lệ mỹ thuật xưa.
              </p>
              <div class="card-seal-bg font-chinese">藝</div>
            </div>

            <!-- Item 4 -->
            <div class="philosophy-card paper-card">
              <div class="card-num font-serif">04</div>
              <div class="card-icon"><i class="fa-solid fa-book-open"></i></div>
              <h3 class="card-title font-serif">Kỷ Vật Truyền Đời Lưu Giữ</h3>
              <p class="card-desc">
                Từ bìa bọc nhung lụa đỏ đến giấy in mỹ thuật tráng phủ chống ẩm, mỗi cuốn album tại Hỷ Sự là một bảo vật gia đình sống mãi với thời gian.
              </p>
              <div class="card-seal-bg font-chinese">傳</div>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 5: Founder / Master Artist Statement -->
      <section class="founder-statement-section" v-if="aboutData.founderQuote">
        <div class="studio-container">
          <div class="founder-banner paper-card">
            <div class="banner-inner">
              <div class="quote-sign font-serif">“</div>
              <blockquote class="founder-quote-text font-serif">
                {{ aboutData.founderQuote }}
              </blockquote>
              <div class="founder-signature-box">
                <div class="founder-identity">
                  <div class="founder-name font-serif">{{ aboutData.founderName || 'Nhiếp ảnh gia Trần Hỷ' }}</div>
                  <div class="founder-role">Người Sáng Lập & Giám Đốc Nghệ Thuật Hỷ Sự Studio</div>
                </div>
                <!-- Traditional Vermilion Seal Stamp -->
                <div class="vermilion-seal font-chinese">
                  <span>喜事之印</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 6: Historical Chronological Timeline (Biên Niên Sử Hành Trình) -->
      <section class="historical-timeline-section" v-if="aboutData.timelines && aboutData.timelines.length > 0">
        <div class="studio-container">
          <div class="section-header-center">
            <div class="center-seal font-chinese">歷</div>
            <h2 class="center-title font-serif">Hành Trình 28 Năm Gìn Giữ & Sáng Tạo</h2>
            <p class="center-subtitle font-serif">
              Từng cột mốc là minh chứng cho sự kiên định với nghệ thuật nhiếp ảnh truyền thống và tình cảm yêu mến của quý khách hàng.
            </p>
          </div>

          <div class="timeline-container">
            <div class="timeline-axis-line"></div>

            <div
              v-for="(item, index) in aboutData.timelines"
              :key="item.timelineId || index"
              class="timeline-item-row"
              :class="{ 'row-reverse': index % 2 !== 0 }"
            >
              <!-- Timeline Marker Node -->
              <div class="timeline-node">
                <div class="node-dot">
                  <span class="font-chinese">囍</span>
                </div>
              </div>

              <!-- Content Card -->
              <div class="timeline-content-card paper-card">
                <div class="card-year-badge font-serif">
                  <i class="fa-solid fa-calendar-check q-mr-xs"></i>
                  <span>NĂM {{ item.year }}</span>
                </div>
                <h3 class="timeline-card-title font-serif">{{ item.title }}</h3>
                <p class="timeline-card-desc">{{ item.description }}</p>
                <div class="card-bracket-corner"></div>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Section 7: Studio Experience & Heritage Darkroom Showcase -->
      <section class="studio-space-section">
        <div class="studio-container">
          <div class="space-showcase-box paper-card">
            <div class="space-text-part">
              <span class="space-badge font-serif">KHÔNG GIAN TRẢI NGHIỆM</span>
              <h2 class="space-title font-serif">Ghé Thăm Studio Cổ Điển Góc Phố Sài Gòn</h2>
              <p class="space-desc">
                Không gian Hỷ Sự Studio được bài trí đậm chất hoài niệm với góc phòng tối tráng phim, tủ trưng bày máy ảnh cổ Hasselblad, Leica, Mamiya cùng phòng thử y phục cưới truyền thống riêng tư, sang trọng.
              </p>
              <div class="space-features-list">
                <div class="feature-line">
                  <i class="fa-solid fa-location-dot"></i>
                  <span><strong>Địa chỉ:</strong> 123 Nguyễn Văn Cừ, Phường 2, Quận 5, TP. Hồ Chí Minh</span>
                </div>
                <div class="feature-line">
                  <i class="fa-solid fa-phone"></i>
                  <span><strong>Hotline tư vấn:</strong> +84 912 345 678 (Hỗ trợ 24/7)</span>
                </div>
                <div class="feature-line">
                  <i class="fa-solid fa-door-open"></i>
                  <span><strong>Giờ đón khách:</strong> 08:30 — 21:00 (Tất cả các ngày trong tuần)</span>
                </div>
              </div>
            </div>

            <div class="space-cta-part">
              <div class="invitation-seal font-chinese">囍</div>
              <h3 class="invitation-heading font-serif">Cùng Kể Câu Chuyện Tình Của Bạn</h3>
              <p class="invitation-text">
                Hãy để tách trà ấm và những thước phim cưới tại Hỷ Sự đưa bạn về một miền ký ức ngọt ngào nhất.
              </p>
              <div class="invitation-actions">
                <router-link to="/trang-chu#lien-he" class="btn-vintage font-serif">
                  <i class="fa-solid fa-calendar-plus q-mr-xs"></i>
                  <span>Đặt lịch tư vấn & thử váy</span>
                </router-link>
                <router-link to="/trang-chu#goi-cuoi" class="btn-vintage-outline font-serif">
                  <i class="fa-solid fa-box-archive q-mr-xs"></i>
                  <span>Xem các gói cưới</span>
                </router-link>
              </div>
            </div>
          </div>
        </div>
      </section>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import { useAbout } from '@/composables/useAbout'
import VintageLoading from '@/components/common/VintageLoading.vue'

const $q = useQuasar()
const { aboutData, isLoading, error, fetchAboutData } = useAbout()

const isCopied = ref(false)

// High Quality Curated Fallback Images for Retro 90s Wedding Aesthetics
const defaultHeroImg = 'https://images.unsplash.com/photo-1583939003579-730e3918a45a?auto=format&fit=crop&w=1600&q=80'
const defaultStoryImg = 'https://images.unsplash.com/photo-1519741497674-611481863552?auto=format&fit=crop&w=1200&q=80'

// Format formatted story content with drop-cap and linebreaks if plain text
const formattedStoryContent = computed(() => {
  if (!aboutData.value?.storyContent) {
    return `<p class="story-p"><span class="drop-cap font-serif">H</span>ỷ Sự Studio ra đời từ niềm say mê sâu sắc với vẻ đẹp kinh điển của nghệ thuật nhiếp ảnh cưới phong cách Hong Kong và Sài Gòn thập niên 90. Trải qua gần ba thập kỷ bền bỉ gìn giữ từng giá trị truyền thống, chúng tôi tin rằng tình yêu đích thực luôn mang một vẻ đẹp dung dị, tự nhiên và trường tồn cùng thời gian.</p><p class="story-p">Chúng tôi không chỉ chụp ảnh, mà cùng bạn chắt chiu từng khoảnh khắc son rỗi, viết nên khúc tình ca bằng những gam màu phim hoài niệm độc bản.</p>`
  }

  const raw = aboutData.value.storyContent
  // Split paragraphs if not HTML
  if (!raw.includes('<p>')) {
    const paragraphs = raw.split('\n\n').filter(p => p.trim().length > 0)
    return paragraphs.map((p, idx) => {
      const cleanP = p.trim()
      if (idx === 0 && cleanP.length > 1) {
        const firstLetter = cleanP.charAt(0)
        const rest = cleanP.slice(1)
        return `<p class="story-p"><span class="drop-cap font-serif">${firstLetter}</span>${rest}</p>`
      }
      return `<p class="story-p">${cleanP}</p>`
    }).join('')
  }

  return raw
})

function formatNumber(num?: number): string {
  if (!num) return '0'
  return new Intl.NumberFormat('vi-VN').format(num)
}

async function copyShareLink() {
  try {
    const url = window.location.href
    await navigator.clipboard.writeText(url)
    isCopied.value = true
    $q.notify({
      type: 'positive',
      message: 'Đã sao chép liên kết trang Giới thiệu!',
      position: 'top',
      timeout: 2000
    })
    setTimeout(() => {
      isCopied.value = false
    }, 3000)
  } catch (err) {
    console.error('Không thể sao chép link:', err)
  }
}

function scrollToSection(sectionId: string) {
  const el = document.getElementById(sectionId)
  if (el) {
    el.scrollIntoView({ behavior: 'smooth', block: 'start' })
  }
}

onMounted(async () => {
  window.scrollTo({ top: 0, behavior: 'instant' })
  await fetchAboutData(1)
})
</script>

<style lang="scss" scoped>
.about-page {
  min-height: 100vh;
  background-color: var(--color-paper);
  color: var(--color-ink);
  padding-bottom: 80px;
}

// Breadcrumb Bar
.about-breadcrumb-bar {
  background-color: var(--color-paper-light);
  border-bottom: 1px solid var(--color-border);
  padding: 14px 0;
  margin-bottom: 0;

  .breadcrumb-inner {
    display: flex;
    align-items: center;
    justify-content: space-between;
    flex-wrap: wrap;
    gap: 12px;
  }

  .breadcrumb-trail {
    // Inherits standardized typography & color from global app.scss
  }

  .share-link-btn {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    background: none;
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    padding: 6px 14px;
    font-size: 0.85rem;
    color: var(--color-ink);
    cursor: pointer;
    transition: all 0.2s;

    &:hover {
      background-color: var(--color-paper);
      color: var(--color-burgundy);
      border-color: var(--color-burgundy);
    }
  }
}

// Section 1: Cinematic Full-Width Hero Banner
.about-hero-section {
  width: 100%;
  margin-bottom: 70px;

  .cinematic-hero-banner {
    position: relative;
    width: 100%;
    border-radius: 0;
    overflow: hidden;
    min-height: 560px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    padding: 18px 0;
    border-top: 1px solid var(--color-border);
    border-bottom: 1px solid var(--color-border);
    box-shadow: 0 16px 40px rgba(36, 36, 33, 0.16);
    background-color: var(--color-film-dark);

    @media (max-width: 768px) {
      min-height: 500px;
      padding: 14px 0;
    }
  }

  .banner-bg-wrapper {
    position: absolute;
    inset: 0;
    z-index: 1;
    overflow: hidden;

    .banner-bg-img {
      width: 100%;
      height: 100%;
      object-fit: cover;
      object-position: center;
      transform: scale(1.02);
      transition: transform 1.2s cubic-bezier(0.2, 0.8, 0.2, 1);
    }

    .banner-overlay {
      position: absolute;
      inset: 0;
      background: radial-gradient(circle at center, rgba(142, 41, 41, 0.22) 0%, transparent 65%),
        linear-gradient(180deg, rgba(20, 26, 26, 0.65) 0%, rgba(20, 26, 26, 0.5) 40%, rgba(20, 26, 26, 0.92) 100%);
    }

    .banner-grain-layer {
      position: absolute;
      inset: 0;
      opacity: 0.2;
      background-image: radial-gradient(#FAF7F0 0.75px, transparent 0.75px);
      background-size: 8px 8px;
      pointer-events: none;
    }
  }

  .cinematic-hero-banner:hover .banner-bg-img {
    transform: scale(1.06);
  }

  // Sprocket Holes Film Effect
  .sprocket-holes-row {
    display: flex;
    justify-content: space-between;
    gap: 8px;
    width: 100%;
    padding: 0 16px;

    .sprocket-hole {
      width: 14px;
      height: 8px;
      background: rgba(250, 247, 240, 0.25);
      border-radius: 2px;
      border: 1px solid rgba(250, 247, 240, 0.12);

      @media (max-width: 768px) {
        width: 8px;
        height: 5px;
      }
    }
  }

  .banner-film-strip-top {
    position: relative;
    z-index: 2;

    .banner-timecode-bar {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding-top: 14px;
      padding-bottom: 12px;
      border-bottom: 1px dashed rgba(250, 247, 240, 0.2);
      font-size: 0.82rem;
      color: rgba(250, 247, 240, 0.8);

      @media (max-width: 600px) {
        flex-direction: column;
        gap: 6px;
        align-items: flex-start;
      }

      .timecode-left {
        display: flex;
        align-items: center;
        gap: 8px;

        .rec-dot {
          width: 9px;
          height: 9px;
          border-radius: 50%;
          background-color: #d32f2f;
          box-shadow: 0 0 8px rgba(211, 47, 47, 0.8);
          animation: recBlink 1.4s infinite;
        }

        .rec-label {
          font-weight: 600;
          letter-spacing: 0.5px;
        }

        .film-sep {
          color: rgba(250, 247, 240, 0.4);
        }

        .film-stock {
          color: #E2B878;
          font-weight: 600;
        }
      }

      .archive-tag {
        font-size: 0.78rem;
        letter-spacing: 1px;
        color: rgba(250, 247, 240, 0.7);
      }
    }
  }

  // Central Banner Content
  .banner-content-inner {
    position: relative;
    z-index: 2;
    text-align: center;
    max-width: 860px;
    margin: 40px auto;
    padding: 0 16px;

    .hero-seal-badge {
      display: inline-flex;
      align-items: center;
      gap: 8px;
      background-color: rgba(142, 41, 41, 0.85);
      border: 1px solid rgba(226, 184, 120, 0.6);
      padding: 6px 20px;
      border-radius: 30px;
      margin-bottom: 20px;
      backdrop-filter: blur(6px);
      box-shadow: 0 4px 16px rgba(0, 0, 0, 0.35);

      .seal-icon {
        color: #FAF7F0;
        font-size: 1.15rem;
        font-weight: bold;
      }

      .seal-text {
        color: #FAF7F0;
        font-size: 0.82rem;
        font-weight: 600;
        letter-spacing: 1.5px;
      }
    }

    .hero-main-title {
      font-size: clamp(1.85rem, 4.2vw, 3rem);
      line-height: 1.28;
      color: #FAF7F0;
      margin-bottom: 18px;
      font-weight: 700;
      text-shadow: 0 3px 12px rgba(0, 0, 0, 0.7);
      letter-spacing: -0.2px;
    }

    .hero-subtitle {
      font-size: clamp(1rem, 1.8vw, 1.18rem);
      line-height: 1.7;
      color: #E2D9C8;
      font-style: italic;
      margin-bottom: 28px;
      text-shadow: 0 2px 8px rgba(0, 0, 0, 0.6);
      max-width: 720px;
      margin-left: auto;
      margin-right: auto;
    }

    .banner-actions {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 16px;
      flex-wrap: wrap;

      .btn-banner-primary {
        background-color: var(--color-burgundy);
        color: #FAF7F0;
        border: 1px solid rgba(250, 247, 240, 0.4);
        padding: 11px 26px;
        border-radius: var(--radius-xs);
        font-size: 0.95rem;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.3s ease;
        box-shadow: 0 4px 14px rgba(142, 41, 41, 0.5);

        &:hover {
          background-color: #A33333;
          border-color: #FAF7F0;
          transform: translateY(-2px);
          box-shadow: 0 6px 18px rgba(142, 41, 41, 0.7);
        }
      }

      .btn-banner-outline {
        background-color: rgba(250, 247, 240, 0.1);
        color: #FAF7F0;
        border: 1px solid rgba(250, 247, 240, 0.5);
        padding: 11px 26px;
        border-radius: var(--radius-xs);
        font-size: 0.95rem;
        text-decoration: none;
        transition: all 0.3s ease;
        backdrop-filter: blur(4px);

        &:hover {
          background-color: rgba(250, 247, 240, 0.22);
          border-color: #FAF7F0;
          transform: translateY(-2px);
        }
      }
    }
  }

  // Bottom Film Metadata Strip
  .banner-film-strip-bottom {
    position: relative;
    z-index: 2;

    .banner-meta-row {
      display: flex;
      align-items: center;
      justify-content: space-between;
      font-size: 0.82rem;
      color: rgba(250, 247, 240, 0.75);
      padding-bottom: 12px;
      border-bottom: 1px dashed rgba(250, 247, 240, 0.2);
      margin-bottom: 14px;

      @media (max-width: 600px) {
        flex-direction: column;
        gap: 6px;
        text-align: center;
      }

      .meta-item-center {
        color: #E2B878;
        font-weight: 600;
        letter-spacing: 2px;
        font-size: 0.95rem;
      }
    }
  }
}

@keyframes recBlink {
  0%, 100% { opacity: 1; transform: scale(1); }
  50% { opacity: 0.3; transform: scale(0.85); }
}

// Section 2: Milestone Statistics Strip
.milestone-stats-section {
  margin-bottom: 70px;

  .stats-grid {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 20px;

    @media (max-width: 1024px) {
      grid-template-columns: repeat(2, 1fr);
    }

    @media (max-width: 600px) {
      grid-template-columns: 1fr;
    }
  }

  .stat-card {
    padding: 28px 20px;
    text-align: center;
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    transition: all 0.3s ease;

    &:hover {
      transform: translateY(-4px);
      box-shadow: 0 10px 24px rgba(36, 36, 33, 0.08);
      border-color: var(--color-burgundy);
    }

    .stat-icon-wrap {
      width: 46px;
      height: 46px;
      border-radius: 50%;
      background-color: rgba(142, 41, 41, 0.08);
      color: var(--color-burgundy);
      display: flex;
      align-items: center;
      justify-content: center;
      margin: 0 auto 16px;
      font-size: 1.2rem;
    }

    .stat-value {
      font-size: 2.2rem;
      font-weight: 700;
      color: var(--color-burgundy);
      line-height: 1.1;
      margin-bottom: 8px;
    }

    .stat-label {
      font-size: 1.05rem;
      font-weight: 600;
      color: var(--color-ink);
      margin-bottom: 8px;
    }

    .stat-desc {
      font-size: 0.85rem;
      color: var(--color-muted);
      line-height: 1.5;
      margin: 0;
    }
  }
}

// Section 3: Brand Story (Editorial 2-Column Split)
.brand-story-section {
  margin-bottom: 80px;

  .story-grid {
    display: grid;
    grid-template-columns: 1fr 1.2fr;
    gap: 50px;
    align-items: center;

    @media (max-width: 960px) {
      grid-template-columns: 1fr;
      gap: 36px;
    }
  }

  .story-visual-column {
    position: relative;

    .polaroid-stack-wrapper {
      position: relative;
      padding-top: 15px;
    }

    .vintage-polaroid-plate {
      padding: 16px 16px 24px;
      background-color: #FCFAF5;
      border: 1px solid var(--color-border);
      box-shadow: 0 12px 32px rgba(36, 36, 33, 0.1);
      position: relative;
      transform: rotate(-1.5deg);
      transition: transform 0.4s ease;

      &:hover {
        transform: rotate(0deg) scale(1.01);
      }

      .polaroid-tape {
        position: absolute;
        top: -12px;
        left: 40%;
        width: 80px;
        height: 24px;
        background-color: rgba(229, 223, 211, 0.6);
        backdrop-filter: blur(2px);
        transform: rotate(3deg);
        border: 1px dashed rgba(142, 41, 41, 0.2);
        z-index: 2;
      }

      .polaroid-img-box {
        position: relative;
        overflow: hidden;
        border-radius: 2px;
        aspect-ratio: 4/5;

        .story-img {
          width: 100%;
          height: 100%;
          object-fit: cover;
          display: block;
        }

        .red-wax-stamp {
          position: absolute;
          bottom: 14px;
          right: 14px;
          width: 52px;
          height: 52px;
          border: 2px solid #8E2929;
          border-radius: 50%;
          display: flex;
          flex-direction: column;
          align-items: center;
          justify-content: center;
          background-color: rgba(250, 247, 240, 0.9);
          color: #8E2929;
          box-shadow: 0 4px 12px rgba(142, 41, 41, 0.2);
          transform: rotate(-12deg);

          span {
            font-size: 1.3rem;
            font-weight: bold;
            line-height: 1;
          }

          small {
            font-size: 0.6rem;
            font-weight: bold;
          }
        }
      }

      .polaroid-meta-caption {
        margin-top: 16px;
        display: flex;
        flex-direction: column;
        gap: 4px;
        text-align: center;

        .story-label {
          font-size: 0.78rem;
          color: var(--color-muted);
          letter-spacing: 1px;
        }

        .story-sub {
          font-size: 0.95rem;
          color: var(--color-burgundy);
          font-style: italic;
        }
      }
    }

    .floating-vintage-tag {
      position: absolute;
      bottom: -15px;
      right: 10px;
      background-color: var(--color-film-dark);
      color: var(--color-gold);
      padding: 8px 16px;
      font-size: 0.82rem;
      border-radius: var(--radius-xs);
      border: 1px solid var(--color-gold);
      box-shadow: 0 6px 18px rgba(0, 0, 0, 0.2);
    }
  }

  .story-text-column {
    .section-tag-row {
      display: flex;
      align-items: center;
      gap: 8px;
      margin-bottom: 12px;

      .chinese-ornament {
        color: var(--color-burgundy);
        font-size: 0.95rem;
        font-weight: 600;
      }

      .tag-title {
        font-size: 0.82rem;
        font-weight: 600;
        color: var(--color-muted);
        letter-spacing: 1.5px;
      }
    }

    .story-title {
      font-size: clamp(1.6rem, 3.2vw, 2.3rem);
      line-height: 1.35;
      color: var(--color-ink);
      margin-bottom: 24px;
    }

    .story-paragraphs {
      margin-bottom: 28px;

      :deep(.story-p) {
        font-size: 1.05rem;
        line-height: 1.8;
        color: var(--color-ink-soft);
        margin-bottom: 16px;
      }

      :deep(.drop-cap) {
        float: left;
        font-size: 3.4rem;
        line-height: 0.8;
        padding-top: 4px;
        padding-right: 12px;
        padding-bottom: 2px;
        color: var(--color-burgundy);
        font-weight: 700;
      }
    }

    .story-highlights-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 16px;
      padding-top: 16px;
      border-top: 1px dashed var(--color-border);

      @media (max-width: 600px) {
        grid-template-columns: 1fr;
      }

      .highlight-item {
        display: flex;
        gap: 12px;

        .item-dot {
          width: 8px;
          height: 8px;
          border-radius: 50%;
          background-color: var(--color-burgundy);
          margin-top: 6px;
          flex-shrink: 0;
        }

        .item-info {
          h4 {
            font-size: 0.98rem;
            font-weight: 600;
            color: var(--color-ink);
            margin: 0 0 4px;
          }

          p {
            font-size: 0.85rem;
            color: var(--color-muted);
            line-height: 1.5;
            margin: 0;
          }
        }
      }
    }
  }
}

// Section 4: 4 Core Principles / Nghệ Thuật & Triết Lý Hỷ Sự
.core-philosophy-section {
  margin-bottom: 80px;

  .section-header-center {
    text-align: center;
    max-width: 720px;
    margin: 0 auto 46px;

    .center-seal {
      width: 44px;
      height: 44px;
      border-radius: 50%;
      background-color: var(--color-burgundy);
      color: #FAF7F0;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 1.4rem;
      margin: 0 auto 16px;
      box-shadow: 0 4px 12px rgba(142, 41, 41, 0.3);
    }

    .center-title {
      font-size: clamp(1.6rem, 3.2vw, 2.2rem);
      color: var(--color-ink);
      margin-bottom: 14px;
    }

    .center-subtitle {
      font-size: 1.05rem;
      line-height: 1.65;
      color: var(--color-ink-soft);
      font-style: italic;
    }
  }

  .philosophy-grid {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 24px;

    @media (max-width: 1024px) {
      grid-template-columns: repeat(2, 1fr);
    }

    @media (max-width: 600px) {
      grid-template-columns: 1fr;
    }
  }

  .philosophy-card {
    padding: 32px 24px;
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    position: relative;
    overflow: hidden;
    transition: all 0.3s ease;

    &:hover {
      transform: translateY(-5px);
      box-shadow: 0 12px 28px rgba(36, 36, 33, 0.08);
      border-color: var(--color-burgundy);

      .card-seal-bg {
        transform: rotate(0deg) scale(1.1);
        opacity: 0.12;
      }
    }

    .card-num {
      font-size: 0.85rem;
      font-weight: 700;
      color: var(--color-burgundy);
      letter-spacing: 1px;
      margin-bottom: 12px;
    }

    .card-icon {
      font-size: 1.6rem;
      color: var(--color-burgundy);
      margin-bottom: 18px;
    }

    .card-title {
      font-size: 1.2rem;
      font-weight: 600;
      color: var(--color-ink);
      margin-bottom: 12px;
      line-height: 1.4;
    }

    .card-desc {
      font-size: 0.92rem;
      line-height: 1.65;
      color: var(--color-ink-soft);
      margin: 0;
      position: relative;
      z-index: 1;
    }

    .card-seal-bg {
      position: absolute;
      bottom: -15px;
      right: -10px;
      font-size: 6.5rem;
      color: var(--color-burgundy);
      opacity: 0.05;
      transform: rotate(-10deg);
      transition: all 0.4s ease;
      user-select: none;
      pointer-events: none;
      line-height: 1;
    }
  }
}

// Section 5: Founder / Master Artist Statement
.founder-statement-section {
  margin-bottom: 80px;

  .founder-banner {
    background: linear-gradient(135deg, #FCFAF5 0%, #F5EFEB 100%);
    border: 1px solid var(--color-border);
    border-left: 5px solid var(--color-burgundy);
    padding: 46px 40px;
    position: relative;

    @media (max-width: 768px) {
      padding: 30px 20px;
    }
  }

  .banner-inner {
    max-width: 860px;
    margin: 0 auto;
    position: relative;

    .quote-sign {
      position: absolute;
      top: -25px;
      left: -20px;
      font-size: 5rem;
      color: rgba(142, 41, 41, 0.15);
      line-height: 1;
      user-select: none;
    }

    .founder-quote-text {
      font-size: clamp(1.15rem, 2.4vw, 1.45rem);
      line-height: 1.7;
      color: var(--color-ink);
      font-style: italic;
      margin: 0 0 28px;
      position: relative;
      z-index: 1;
    }

    .founder-signature-box {
      display: flex;
      align-items: center;
      justify-content: space-between;
      flex-wrap: wrap;
      gap: 16px;
      padding-top: 20px;
      border-top: 1px dashed var(--color-border);

      .founder-identity {
        .founder-name {
          font-size: 1.2rem;
          font-weight: 700;
          color: var(--color-burgundy);
          margin-bottom: 4px;
        }

        .founder-role {
          font-size: 0.85rem;
          color: var(--color-muted);
        }
      }

      .vermilion-seal {
        width: 72px;
        height: 72px;
        border: 2px solid #8E2929;
        border-radius: 4px;
        display: flex;
        align-items: center;
        justify-content: center;
        background-color: rgba(142, 41, 41, 0.06);
        color: #8E2929;
        font-size: 1.15rem;
        font-weight: bold;
        letter-spacing: 2px;
        transform: rotate(-3deg);
        box-shadow: 0 4px 12px rgba(142, 41, 41, 0.15);
      }
    }
  }
}

// Section 6: Historical Chronological Timeline (Biên Niên Sử Hành Trình)
.historical-timeline-section {
  margin-bottom: 80px;

  .section-header-center {
    text-align: center;
    max-width: 720px;
    margin: 0 auto 50px;

    .center-seal {
      width: 44px;
      height: 44px;
      border-radius: 50%;
      background-color: var(--color-burgundy);
      color: #FAF7F0;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 1.4rem;
      margin: 0 auto 16px;
      box-shadow: 0 4px 12px rgba(142, 41, 41, 0.3);
    }

    .center-title {
      font-size: clamp(1.6rem, 3.2vw, 2.2rem);
      color: var(--color-ink);
      margin-bottom: 14px;
    }

    .center-subtitle {
      font-size: 1.05rem;
      line-height: 1.65;
      color: var(--color-ink-soft);
      font-style: italic;
    }
  }

  .timeline-container {
    position: relative;
    max-width: 900px;
    margin: 0 auto;
    padding: 20px 0;

    .timeline-axis-line {
      position: absolute;
      top: 0;
      bottom: 0;
      left: 50%;
      width: 2px;
      background: linear-gradient(180deg, var(--color-burgundy) 0%, rgba(142, 41, 41, 0.2) 100%);
      transform: translateX(-50%);

      @media (max-width: 768px) {
        left: 24px;
      }
    }
  }

  .timeline-item-row {
    display: flex;
    align-items: center;
    justify-content: flex-end;
    width: 50%;
    margin-bottom: 40px;
    position: relative;
    padding-right: 40px;

    &.row-reverse {
      margin-left: 50%;
      justify-content: flex-start;
      padding-right: 0;
      padding-left: 40px;

      .timeline-node {
        right: auto;
        left: -19px;
      }

      .card-bracket-corner {
        right: auto;
        left: -6px;
        border-right: none;
        border-left: 2px solid var(--color-burgundy);
      }
    }

    @media (max-width: 768px) {
      width: 100% !important;
      margin-left: 0 !important;
      padding-right: 0 !important;
      padding-left: 60px !important;
      justify-content: flex-start !important;

      .timeline-node {
        right: auto !important;
        left: 6px !important;
      }
    }

    .timeline-node {
      position: absolute;
      right: -19px;
      top: 24px;
      z-index: 2;

      .node-dot {
        width: 38px;
        height: 38px;
        border-radius: 50%;
        background-color: var(--color-paper-light);
        border: 2px solid var(--color-burgundy);
        color: var(--color-burgundy);
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 1rem;
        font-weight: bold;
        box-shadow: 0 4px 10px rgba(142, 41, 41, 0.2);
        transition: all 0.3s ease;
      }
    }

    &:hover {
      .node-dot {
        background-color: var(--color-burgundy);
        color: #FAF7F0;
        transform: scale(1.15);
      }

      .timeline-content-card {
        border-color: var(--color-burgundy);
        box-shadow: 0 10px 24px rgba(36, 36, 33, 0.08);
      }
    }

    .timeline-content-card {
      width: 100%;
      padding: 24px 22px;
      background-color: var(--color-paper-light);
      border: 1px solid var(--color-border);
      position: relative;
      transition: all 0.3s ease;

      .card-year-badge {
        display: inline-flex;
        align-items: center;
        background-color: rgba(142, 41, 41, 0.08);
        color: var(--color-burgundy);
        padding: 4px 12px;
        border-radius: 20px;
        font-size: 0.85rem;
        font-weight: 700;
        letter-spacing: 0.5px;
        margin-bottom: 12px;
      }

      .timeline-card-title {
        font-size: 1.25rem;
        font-weight: 600;
        color: var(--color-ink);
        margin-bottom: 10px;
        line-height: 1.35;
      }

      .timeline-card-desc {
        font-size: 0.92rem;
        line-height: 1.65;
        color: var(--color-ink-soft);
        margin: 0;
      }

      .card-bracket-corner {
        position: absolute;
        top: 20px;
        right: -6px;
        width: 12px;
        height: 12px;
        border-top: 2px solid var(--color-burgundy);
        border-right: 2px solid var(--color-burgundy);
      }
    }
  }
}

// Section 7: Studio Experience & Heritage Space
.studio-space-section {
  .space-showcase-box {
    background: linear-gradient(135deg, #FAF7F0 0%, #F5ECE1 100%);
    border: 1px solid var(--color-border);
    padding: 46px 40px;
    display: grid;
    grid-template-columns: 1.3fr 1fr;
    gap: 48px;
    align-items: center;

    @media (max-width: 900px) {
      grid-template-columns: 1fr;
      padding: 30px 20px;
      gap: 36px;
    }
  }

  .space-text-part {
    .space-badge {
      font-size: 0.8rem;
      font-weight: 600;
      color: var(--color-burgundy);
      letter-spacing: 1.5px;
      margin-bottom: 10px;
      display: inline-block;
    }

    .space-title {
      font-size: clamp(1.5rem, 3vw, 2.1rem);
      color: var(--color-ink);
      margin-bottom: 16px;
      line-height: 1.3;
    }

    .space-desc {
      font-size: 0.98rem;
      line-height: 1.75;
      color: var(--color-ink-soft);
      margin-bottom: 24px;
    }

    .space-features-list {
      display: flex;
      flex-direction: column;
      gap: 12px;

      .feature-line {
        display: flex;
        align-items: flex-start;
        gap: 10px;
        font-size: 0.92rem;
        color: var(--color-ink);

        i {
          color: var(--color-burgundy);
          margin-top: 4px;
          flex-shrink: 0;
        }

        strong {
          color: var(--color-burgundy);
        }
      }
    }
  }

  .space-cta-part {
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    padding: 32px 28px;
    text-align: center;
    box-shadow: 0 10px 28px rgba(36, 36, 33, 0.06);

    .invitation-seal {
      width: 48px;
      height: 48px;
      border-radius: 50%;
      background-color: var(--color-burgundy);
      color: #FAF7F0;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 1.5rem;
      margin: 0 auto 16px;
      box-shadow: 0 4px 12px rgba(142, 41, 41, 0.25);
    }

    .invitation-heading {
      font-size: 1.3rem;
      color: var(--color-ink);
      margin-bottom: 10px;
    }

    .invitation-text {
      font-size: 0.9rem;
      color: var(--color-muted);
      line-height: 1.6;
      margin-bottom: 24px;
    }

    .invitation-actions {
      display: flex;
      flex-direction: column;
      gap: 12px;

      .btn-vintage,
      .btn-vintage-outline {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        padding: 12px;
        font-size: 0.95rem;
        text-decoration: none;
      }
    }
  }
}

// Error State Box
.about-error-state {
  min-height: 50vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 60px 0;

  .error-box {
    text-align: center;
    padding: 40px;
    max-width: 540px;
    background-color: var(--color-paper-light);

    .error-icon {
      font-size: 3rem;
      color: var(--color-burgundy);
      margin-bottom: 18px;
    }

    .error-title {
      font-size: 1.6rem;
      color: var(--color-ink);
      margin-bottom: 12px;
    }

    .error-desc {
      font-size: 1rem;
      color: var(--color-muted);
      margin-bottom: 24px;
    }

    .error-actions {
      display: flex;
      justify-content: center;
      gap: 14px;
      flex-wrap: wrap;
    }
  }
}

@keyframes recBlink {
  0%, 100% { opacity: 1; }
  50% { opacity: 0.2; }
}
</style>
