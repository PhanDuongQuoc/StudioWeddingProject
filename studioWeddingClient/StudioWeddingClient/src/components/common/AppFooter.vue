<template>
  <footer class="app-footer">
    <div class="studio-container footer-content">
      <div class="footer-top">
        <div class="footer-brand">
          <div class="footer-logo font-serif">Hỷ Sự Studio</div>
          <p class="footer-slogan">Lưu giữ những khoảnh khắc đẹp nhất của bạn bằng những thước phim và bức ảnh hoài
            niệm.</p>
        </div>

        <div class="footer-links">
          <div class="footer-nav">
            <router-link to="/trang-chu">Trang chủ</router-link>
            <a href="#gioi-thieu">Giới thiệu</a>
            <a href="#albums">Album</a>
            <a href="#dich-vu">Dịch vụ</a>
            <a href="#goi-cuoi">Gói cưới</a>
            <a href="#lien-he">Liên hệ</a>
          </div>
          <div class="footer-contact-info">
            <p><strong>Địa chỉ:</strong> 123 Nguyễn Văn Cừ, Quận 5, TP. Hồ Chí Minh</p>
            <p><strong>Hotline:</strong> +84 912 345 678</p>
            <p><strong>Email:</strong> hello@weddingstudio.vn</p>
          </div>
        </div>
      </div>

      <div class="footer-bottom">
        <p class="copyright">© 2026 . All rights reserved. Hong Kong & Vietnamese 90s Wedding Aesthetics.</p>
      </div>
    </div>
  </footer>
</template>

<script setup lang="ts">
</script>

<style lang="scss" scoped>
.app-footer {
  background-color: var(--color-film-dark);
  color: #DCD5C8;
  padding: 60px 0 30px;
  border-top: 1px solid #2C3B3A;
  margin-top: 80px;
}

.footer-top {
  display: grid;
  grid-template-columns: 1.2fr 2fr;
  gap: 48px;
  padding-bottom: 40px;
  border-bottom: 1px solid rgba(201, 192, 178, 0.15);

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
    gap: 32px;
  }
}

.footer-brand {
  .footer-logo {
    font-size: 1.8rem;
    font-weight: 600;
    color: var(--color-paper-light);
    margin-bottom: 12px;
  }

  .footer-slogan {
    font-size: 0.95rem;
    line-height: 1.6;
    color: #A8A296;
    max-width: 320px;
  }
}

.footer-links {
  display: flex;
  flex-direction: column;
  gap: 24px;

  .footer-nav {
    display: flex;
    flex-wrap: wrap;
    gap: 24px;

    a {
      color: #DCD5C8;
      font-size: 0.95rem;
      transition: color 0.2s;

      &:hover {
        color: #F8F4EA;
      }
    }
  }

  .footer-contact-info {
    font-size: 0.9rem;
    color: #A8A296;
    line-height: 1.8;

    strong {
      color: #DCD5C8;
    }
  }
}

.footer-bottom {
  padding-top: 24px;
  text-align: center;

  .copyright {
    font-size: 0.85rem;
    color: #7E786E;
  }
}
</style>
