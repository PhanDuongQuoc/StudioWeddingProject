<template>
  <div class="auth-layout">
    <!-- Top Minimal Navigation -->
    <header class="auth-header">
      <div class="studio-container header-wrapper">
        <router-link to="/trang-chu" class="brand-logo">
          <svg class="flower-icon" viewBox="0 0 24 24" width="22" height="22" fill="none" stroke="currentColor">
            <circle cx="12" cy="12" r="11" fill="#8E2929" stroke="#FAF7F0" stroke-width="0.75" />
            <path d="M12 4C13 6.5 14.5 8.5 17 9C14.5 10.5 13 12.5 12 15C11 12.5 9.5 10.5 7 9C9.5 8.5 11 6.5 12 4Z"
              fill="#FAF7F0" stroke="#FAF7F0" stroke-width="0.3" />
            <path
              d="M20 12C17.5 13 15.5 14.5 15 17C13.5 14.5 11.5 13 9 12C11.5 11 13.5 9.5 15 7C15.5 9.5 17.5 11 20 12Z"
              fill="#FAF7F0" stroke="#FAF7F0" stroke-width="0.3" />
            <path
              d="M12 20C11 17.5 9.5 15.5 7 15C9.5 13.5 11 11.5 12 9C13 11.5 14.5 13.5 17 15C14.5 15.5 13 17.5 12 20Z"
              fill="#FAF7F0" stroke="#FAF7F0" stroke-width="0.3" />
            <path d="M4 12C6.5 11 8.5 9.5 9 7C10.5 9.5 12.5 11 15 12C12.5 13 10.5 14.5 9 17C8.5 14.5 6.5 13 4 12Z"
              fill="#FAF7F0" stroke="#FAF7F0" stroke-width="0.3" />
            <circle cx="12" cy="12" r="2.2" fill="#8E2929" stroke="#FAF7F0" stroke-width="0.6" />
          </svg>
          <span class="brand-title font-serif">Hỷ Sự Studio</span>
          <span class="chinese-seal font-chinese">囍</span>
        </router-link>

        <router-link to="/trang-chu" class="back-home-link">
          <i class="fa-solid fa-arrow-left"></i>
          <span>Trang chủ</span>
        </router-link>
      </div>
    </header>

    <!-- Main Auth Shell (Editorial 2-Column Split) -->
    <main class="auth-main">
      <div class="studio-container auth-wrapper">
        <div class="auth-card-container">

          <!-- Left Column: Visual Brand Showcase (Hong Kong 90s Film Mood) -->
          <div class="auth-visual-side">
            <!-- Analog Film Badges -->
            <div class="visual-top-badge">
              <span class="film-date-label">REC ● 1998.10.24</span>
              <span class="stamp-tag font-chinese">喜事・百年好合</span>
            </div>

            <div class="visual-content">
              <div class="quote-symbol font-serif">“</div>
              <h2 class="visual-title font-serif">Lưu Giữ Trăm Năm</h2>
              <p class="visual-desc font-serif">
                Mỗi bức ảnh cưới là một thước phim nhuốm màu thời gian, ghi lại trọn vẹn sự chân thành và những khoảnh
                khắc đẹp nhất của tình yêu đôi lứa.
              </p>

              <div class="vintage-divider">
                <span class="divider-diamond">✦</span>
              </div>

              <div class="visual-meta">
                <span class="meta-item font-serif">HỶ SỰ STUDIO ARCHIVE</span>
                <span class="meta-dot">·</span>
                <span class="meta-item">EST. 1998</span>
              </div>
            </div>
          </div>

          <!-- Right Column: Form Router View -->
          <div class="auth-form-side">
            <router-view #default="{ Component }">
              <transition name="film-dissolve" mode="out-in">
                <component :is="Component" />
              </transition>
            </router-view>
          </div>

        </div>
      </div>
    </main>

    <!-- Minimal Footer -->
    <footer class="auth-footer">
      <div class="studio-container footer-inner">
        <span class="footer-copy">© 2026 HỶ SỰ STUDIO. BẢN QUYỀN HÌNH ẢNH & THIẾT KẾ THUỘC VỀ STUDIO.</span>
        <div class="footer-links">
          <a href="#dieu-khoan">Chính sách bảo mật</a>
          <span class="sep">·</span>
          <a href="#ho-tro">Hỗ trợ khách hàng</a>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
</script>

<style scoped lang="scss">
.auth-layout {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  background-color: var(--color-paper);
  color: var(--color-ink);
}

// Header
.auth-header {
  height: 64px;
  border-bottom: 1px solid var(--color-border);
  background-color: var(--color-paper-light);
  display: flex;
  align-items: center;

  .header-wrapper {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
  }

  .brand-logo {
    display: inline-flex;
    align-items: center;
    gap: 10px;
    text-decoration: none;
    color: var(--color-ink);

    .brand-title {
      font-size: 1.25rem;
      font-weight: 600;
      letter-spacing: 0.5px;
    }

    .chinese-seal {
      display: inline-flex;
      align-items: center;
      justify-content: center;
      width: 22px;
      height: 22px;
      background-color: var(--color-burgundy);
      color: #FAF7F0;
      font-size: 0.8rem;
      border-radius: 2px;
      margin-left: 2px;
    }
  }

  .back-home-link {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    font-size: 0.9rem;
    font-weight: 500;
    color: var(--color-ink-soft);
    padding: 6px 12px;
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    background-color: var(--color-paper);
    transition: all 0.25s ease;

    &:hover {
      color: var(--color-burgundy);
      border-color: var(--color-burgundy);
      background-color: rgba(142, 41, 41, 0.04);
    }
  }
}

// Main Content
.auth-main {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40px 0;
}

.auth-card-container {
  display: grid;
  grid-template-columns: 1fr 1.15fr;
  max-width: 1080px;
  margin: 0 auto;
  background-color: var(--color-paper-light);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-xs);
  box-shadow: 0 12px 32px rgba(36, 36, 33, 0.06);
  overflow: hidden;

  @media (max-width: 860px) {
    grid-template-columns: 1fr;
    max-width: 520px;
  }
}

// Left Visual Side (Hong Kong 90s Vibe)
.auth-visual-side {
  position: relative;
  background-image: url('https://anhvienmimosa.com.vn/wp-content/uploads/2025/08/anh-cuoi-trung-quoc-1.jpg');
  background-size: cover;
  background-position: center 30%;
  color: #FAF7F0;
  padding: 48px 40px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  min-height: 560px;
  overflow: hidden;

  // Lớp phủ điện ảnh Hong Kong 90s (Film Grain & Warm Vignette)
  &::before {
    content: '';
    position: absolute;
    inset: 0;
    background: linear-gradient(180deg,
        rgba(35, 20, 20, 0.55) 0%,
        rgba(24, 20, 20, 0.68) 40%,
        rgba(18, 20, 20, 0.92) 100%);
    z-index: 1;
  }

  .visual-top-badge {
    display: flex;
    justify-content: space-between;
    align-items: center;
    position: relative;
    z-index: 2;

    .film-date-label {
      font-family: monospace;
      font-size: 0.8rem;
      letter-spacing: 1.5px;
      color: #E89A3C;
      text-shadow: 0 0 6px rgba(232, 154, 60, 0.4);
    }

    .stamp-tag {
      padding: 4px 10px;
      border: 1px dashed rgba(250, 247, 240, 0.4);
      font-size: 0.85rem;
      letter-spacing: 1px;
      color: #FAF7F0;
    }
  }

  .visual-content {
    position: relative;
    z-index: 2;
    margin-top: auto;
    margin-bottom: auto;

    .quote-symbol {
      font-size: 3.5rem;
      line-height: 1;
      color: var(--color-burgundy-soft);
      opacity: 0.8;
      margin-bottom: -10px;
    }

    .visual-title {
      font-size: 2.2rem;
      font-weight: 500;
      letter-spacing: 1px;
      margin-bottom: 14px;
      color: #FAF7F0;
    }

    .visual-desc {
      font-size: 1.05rem;
      line-height: 1.7;
      color: #DCD5C8;
      font-style: italic;
      margin-bottom: 24px;
    }

    .vintage-divider {
      display: flex;
      align-items: center;
      gap: 12px;
      margin-bottom: 20px;

      &::before,
      &::after {
        content: '';
        flex: 1;
        height: 1px;
        background: linear-gradient(90deg, transparent, rgba(201, 192, 178, 0.3), transparent);
      }

      .divider-diamond {
        color: #C5A880;
        font-size: 0.75rem;
      }
    }

    .visual-meta {
      display: flex;
      align-items: center;
      gap: 8px;
      font-size: 0.78rem;
      letter-spacing: 1.5px;
      color: #A8A196;

      .meta-dot {
        color: var(--color-burgundy);
      }
    }
  }

  @media (max-width: 860px) {
    display: none; // Ẩn cột thị giác trên màn hình nhỏ để tối ưu trải nghiệm form
  }
}

// Right Form Side
.auth-form-side {
  padding: 44px 40px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  background-color: var(--color-paper-light);

  @media (max-width: 560px) {
    padding: 32px 20px;
  }
}

// Footer
.auth-footer {
  border-top: 1px solid var(--color-border);
  padding: 18px 0;
  background-color: var(--color-paper);
  font-size: 0.82rem;
  color: var(--color-muted);

  .footer-inner {
    display: flex;
    justify-content: space-between;
    align-items: center;
    flex-wrap: wrap;
    gap: 12px;

    @media (max-width: 680px) {
      flex-direction: column;
      text-align: center;
    }
  }

  .footer-links {
    display: flex;
    align-items: center;
    gap: 10px;

    a {
      color: var(--color-muted);
      transition: color 0.2s ease;

      &:hover {
        color: var(--color-burgundy);
      }
    }

    .sep {
      color: var(--color-border);
    }
  }
}
</style>
