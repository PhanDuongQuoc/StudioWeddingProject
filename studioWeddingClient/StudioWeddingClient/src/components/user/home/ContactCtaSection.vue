<template>
  <section id="lien-he" class="contact-section">
    <div class="studio-container">
      <SectionTitle
        title="Liên hệ với chúng tôi"
        subtitle="Để lại thông tin để nhận tư vấn chi tiết về phong cách chụp và bảng giá ưu đãi"
        tag="Contact"
      />

      <div class="contact-grid">
        <!-- Left: Contact Details & Studio Info -->
        <div class="contact-info-card paper-card animate-fade-in-up stagger-1">
          <h3 class="info-title font-serif">Hỷ Sự Studio</h3>
          
          <div class="info-list">
            <div class="info-item">
              <span class="info-icon"><i class="fa-solid fa-location-dot"></i></span>
              <div>
                <strong>Địa chỉ:</strong>
                <p>123 Nguyễn Văn Cừ, Phường 4, Quận 5, TP. Hồ Chí Minh</p>
              </div>
            </div>

            <div class="info-item">
              <span class="info-icon"><i class="fa-solid fa-phone"></i></span>
              <div>
                <strong>Điện thoại:</strong>
                <p>+84 912 345 678</p>
              </div>
            </div>

            <div class="info-item">
              <span class="info-icon"><i class="fa-solid fa-envelope"></i></span>
              <div>
                <strong>Email:</strong>
                <p>contact@hysustudio.vn</p>
              </div>
            </div>

            <div class="info-item">
              <span class="info-icon"><i class="fa-regular fa-clock"></i></span>
              <div>
                <strong>Giờ làm việc:</strong>
                <p>8:00 - 21:00 (Thứ 2 - Chủ Nhật)</p>
              </div>
            </div>
          </div>

          <!-- Mini Map or Studio Vibe Image -->
          <div class="studio-vibe-img">
            <img
              src="https://images.unsplash.com/photo-1519741497674-611481863552?q=80&w=600&auto=format&fit=crop"
              alt="Studio Hong Kong"
              class="film-photo"
            />
            <span class="vibe-caption">Come as you are — We capture the soul</span>
          </div>
        </div>

        <!-- Right: Consultation Form -->
        <div class="contact-form-card paper-card animate-fade-in-up stagger-2">
          <h3 class="form-title font-serif">Gửi yêu cầu tư vấn</h3>
          
          <form @submit.prevent="handleSubmit" class="consult-form">
            <div class="form-row">
              <div class="form-group">
                <label>Họ và tên *</label>
                <input
                  v-model="form.fullName"
                  type="text"
                  placeholder="Nguyễn Văn A"
                  required
                />
              </div>

              <div class="form-group">
                <label>Số điện thoại *</label>
                <input
                  v-model="form.phone"
                  type="tel"
                  placeholder="0912345678"
                  required
                />
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label>Email</label>
                <input
                  v-model="form.email"
                  type="email"
                  placeholder="email@domain.com"
                />
              </div>

              <div class="form-group">
                <label>Ngày cưới dự kiến</label>
                <input
                  v-model="form.weddingDate"
                  type="date"
                />
              </div>
            </div>

            <div class="form-group">
              <label>Nội dung cần tư vấn</label>
              <textarea
                v-model="form.message"
                rows="4"
                placeholder="Bạn đang quan tâm đến phong cách hay gói dịch vụ nào..."
              ></textarea>
            </div>

            <button type="submit" class="btn-vintage submit-btn font-serif">
              Gửi yêu cầu tư vấn
            </button>
          </form>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import SectionTitle from '@/components/common/SectionTitle.vue'

const form = reactive({
  fullName: '',
  phone: '',
  email: '',
  weddingDate: '',
  message: ''
})

const handleSubmit = () => {
  alert(`Cảm ơn bạn ${form.fullName}! Hỷ Sự Studio đã nhận được yêu cầu và sẽ liên hệ sớm nhất.`)
  form.fullName = ''
  form.phone = ''
  form.email = ''
  form.weddingDate = ''
  form.message = ''
}
</script>

<style lang="scss" scoped>
.contact-section {
  padding: 70px 0;
  background-color: var(--color-paper);
}

.contact-grid {
  display: grid;
  grid-template-columns: 1fr 1.3fr;
  gap: 32px;

  @media (max-width: 900px) {
    grid-template-columns: 1fr;
  }
}

.contact-info-card,
.contact-form-card {
  padding: 34px;
}

.info-title,
.form-title {
  font-size: 1.6rem;
  font-weight: 600;
  color: var(--color-ink);
  margin: 0 0 24px;
}

.info-list {
  display: flex;
  flex-direction: column;
  gap: 16px;
  margin-bottom: 24px;
}

.info-item {
  display: flex;
  align-items: flex-start;
  gap: 12px;

  .info-icon {
    font-size: 1.1rem;
    margin-top: 2px;
  }

  strong {
    display: block;
    font-size: 0.88rem;
    color: var(--color-ink-soft);
  }

  p {
    margin: 2px 0 0;
    font-size: 0.95rem;
    color: var(--color-ink);
  }
}

.studio-vibe-img {
  position: relative;
  width: 100%;
  height: 150px;
  overflow: hidden;
  border-radius: var(--radius-xs);
  border: 1px solid var(--color-border);

  img {
    width: 100%;
    height: 100%;
    object-fit: cover;
    transition: transform 0.6s cubic-bezier(0.25, 1, 0.5, 1);
  }

  &:hover img {
    transform: scale(1.08);
  }

  .vibe-caption {
    position: absolute;
    bottom: 8px;
    left: 12px;
    color: #F8F4EA;
    font-size: 0.78rem;
    font-style: italic;
    background-color: rgba(24, 35, 34, 0.75);
    padding: 3px 10px;
    backdrop-filter: blur(2px);
  }
}

.consult-form {
  display: flex;
  flex-direction: column;
  gap: 18px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;

  @media (max-width: 600px) {
    grid-template-columns: 1fr;
  }
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 6px;

  label {
    font-size: 0.85rem;
    font-weight: 600;
    color: var(--color-ink-soft);
  }

  input,
  textarea {
    width: 100%;
    padding: 11px 14px;
    background-color: var(--color-paper);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    font-family: var(--font-sans);
    font-size: 0.95rem;
    color: var(--color-ink);
    outline: none;
    transition: border-color 0.25s ease, box-shadow 0.25s ease;

    &:focus {
      border-color: var(--color-burgundy);
      box-shadow: 0 0 0 2px rgba(142, 41, 41, 0.12);
    }
  }
}

.submit-btn {
  padding: 13px 0;
  font-size: 1.1rem;
  letter-spacing: 0.5px;
  margin-top: 8px;
  cursor: pointer;
}
</style>
