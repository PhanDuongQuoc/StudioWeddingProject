<template>
  <div class="contact-page">
    <main class="contact-main-content studio-container">
      <!-- Breadcrumb Trail -->
      <nav class="breadcrumb-trail font-serif" aria-label="Breadcrumb">
        <router-link to="/trang-chu">Trang chủ</router-link>
        <span class="sep">/</span>
        <span class="current-crumb">Liên hệ</span>
      </nav>

      <!-- Page Heading -->
      <div class="page-title-row">
        <h1 class="page-main-title font-serif">Liên hệ</h1>
      </div>

      <!-- Main 3-Column / Editorial Grid matching Mockup -->
      <div class="contact-layout-grid">
        <!-- 1. Left Column: Contact Details & Map Card -->
        <div class="contact-info-col">
          <div class="info-items-list font-serif">
            <!-- Địa chỉ -->
            <div class="info-entry">
              <span class="entry-icon"><i class="fa-solid fa-location-dot"></i></span>
              <div class="entry-text">
                <span class="entry-label">Địa chỉ</span>
                <p class="entry-val">123 Nguyễn Văn Cừ, Quận 5, TP.HCM</p>
              </div>
            </div>

            <!-- Điện thoại -->
            <div class="info-entry">
              <span class="entry-icon"><i class="fa-solid fa-phone"></i></span>
              <div class="entry-text">
                <span class="entry-label">Điện thoại</span>
                <p class="entry-val">+84 912 345 678</p>
              </div>
            </div>

            <!-- Email -->
            <div class="info-entry">
              <span class="entry-icon"><i class="fa-regular fa-envelope"></i></span>
              <div class="entry-text">
                <span class="entry-label">Email</span>
                <p class="entry-val">hello@weddingstudio.vn</p>
              </div>
            </div>

            <!-- Giờ làm việc -->
            <div class="info-entry">
              <span class="entry-icon"><i class="fa-regular fa-clock"></i></span>
              <div class="entry-text">
                <span class="entry-label">Giờ làm việc</span>
                <p class="entry-val">8:00 - 20:00 (T2 - CN)</p>
              </div>
            </div>
          </div>

          <!-- Stylized Map Frame -->
          <div class="map-frame-box">
            <iframe
              src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3919.6512356598586!2d106.68002637480546!3d10.76133998938634!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x31752f1b7c3ed289%3A0xa06651894598e488!2zMTIzIMSQLiBOZ3V54buFbiBWxINuIEPhu6ssIFBoxrDhu51uZyA0LCBRdeG6rW4gNSwgSOG7kyBDaMOtIE1pbmgsIFZp4buHdCBOYW0!5e0!3m2!1svi!2s!4v1710000000000!5m2!1svi!2s"
              class="map-iframe"
              :allowfullscreen="true"
              loading="lazy"
              referrerpolicy="no-referrer-when-downgrade"
              title="Bản đồ chỉ đường đến Hỷ Sự Studio"
            ></iframe>
            <div class="map-quote-badge font-serif">
              <span>Come as you are — We capture the soul</span>
            </div>
          </div>
        </div>

        <!-- 2. Middle Column: Consultation Form -->
        <div class="contact-form-col">
          <div class="form-card-box">
            <h2 class="form-title font-serif">Gửi yêu cầu tư vấn</h2>

            <form @submit.prevent="handleSubmit" class="consultation-form">
              <!-- Row 1: Họ tên & Số điện thoại -->
              <div class="form-row-2">
                <div class="form-field">
                  <label class="field-label font-serif">Họ và tên *</label>
                  <input
                    v-model="form.fullName"
                    type="text"
                    class="vintage-input font-serif"
                    placeholder="Nguyễn Văn A"
                    required
                  />
                </div>

                <div class="form-field">
                  <label class="field-label font-serif">Số điện thoại *</label>
                  <input
                    v-model="form.phone"
                    type="tel"
                    class="vintage-input font-serif"
                    placeholder="0912 345 678"
                    required
                  />
                </div>
              </div>

              <!-- Row 2: Email & Ngày cưới -->
              <div class="form-row-2">
                <div class="form-field">
                  <label class="field-label font-serif">Email</label>
                  <input
                    v-model="form.email"
                    type="email"
                    class="vintage-input font-serif"
                    placeholder="email@example.com"
                  />
                </div>

                <div class="form-field">
                  <label class="field-label font-serif">Ngày cưới dự kiến</label>
                  <input
                    v-model="form.weddingDate"
                    type="date"
                    class="vintage-input font-serif"
                  />
                </div>
              </div>

              <!-- Row 3: Nội dung -->
              <div class="form-field">
                <label class="field-label font-serif">Nội dung</label>
                <textarea
                  v-model="form.message"
                  rows="4"
                  class="vintage-textarea font-serif"
                  placeholder="Nhập nội dung..."
                ></textarea>
              </div>

              <!-- Submit Button -->
              <button
                type="submit"
                class="btn-vintage-submit font-serif"
                :disabled="isSubmitting"
              >
                <span v-if="!isSubmitting">Gửi yêu cầu</span>
                <span v-else class="loading-state">
                  <i class="fa-solid fa-spinner fa-spin q-mr-sm"></i>
                  Đang gửi yêu cầu...
                </span>
              </button>
            </form>
          </div>
        </div>

        <!-- 3. Right Column: Vertical Vintage Photo Banner (Mockup Match) -->
        <div class="contact-photo-col">
          <div class="vertical-photo-frame">
            <img
              src="https://images.unsplash.com/photo-1519741497674-611481863552?auto=format&fit=crop&w=800&q=80"
              alt="Hỷ Sự Studio Hong Kong Vintage Aesthetic"
              class="photo-vertical film-photo"
              loading="lazy"
            />
            <div class="vertical-tag font-chinese">
              喜事・百年好合
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useQuasar } from 'quasar'
import { sendContactEmail } from '@/services/homeService'

const $q = useQuasar()
const isSubmitting = ref(false)

const form = reactive({
  fullName: '',
  phone: '',
  email: '',
  weddingDate: '',
  message: '',
})

async function handleSubmit() {
  if (!form.fullName.trim() || !form.phone.trim()) {
    $q.notify({
      type: 'warning',
      message: 'Vui lòng nhập Họ tên và Số điện thoại liên hệ.',
      position: 'top',
      timeout: 2500,
    })
    return
  }

  isSubmitting.value = true

  try {
    const payload = {
      nameCutomer: form.fullName.trim(),
      phoneNumber: form.phone.trim(),
      emailCustomer: form.email.trim() || 'khachhang@weddingstudio.vn',
      dataWedding: form.weddingDate ? new Date(form.weddingDate).toISOString() : new Date().toISOString(),
      note: form.message.trim(),
    }

    const response = await sendContactEmail(payload)

    if (response && response.status) {
      $q.notify({
        type: 'positive',
        message: `Cảm ơn bạn ${form.fullName}! Hỷ Sự Studio đã nhận được yêu cầu tư vấn và sẽ liên hệ trong thời gian sớm nhất.`,
        position: 'top',
        timeout: 4000,
      })

      // Reset form
      form.fullName = ''
      form.phone = ''
      form.email = ''
      form.weddingDate = ''
      form.message = ''
    } else {
      $q.notify({
        type: 'negative',
        message: response?.message || 'Không thể gửi yêu cầu lúc này. Vui lòng thử lại!',
        position: 'top',
        timeout: 3000,
      })
    }
  } catch (err: unknown) {
    console.error('Lỗi khi gửi email liên hệ:', err)
    const errorObj = err as { response?: { data?: { message?: string } }; message?: string }
    $q.notify({
      type: 'negative',
      message: errorObj.response?.data?.message || 'Có lỗi xảy ra khi gửi thông tin. Vui lòng kiểm tra lại!',
      position: 'top',
      timeout: 3000,
    })
  } finally {
    isSubmitting.value = false
  }
}
</script>

<style lang="scss" scoped>
.contact-page {
  min-height: 100vh;
  background-color: var(--color-paper);
  color: var(--color-ink);
  padding: 24px 0 80px;
}

.breadcrumb-trail {
  margin-bottom: 24px;
}

.page-title-row {
  margin-bottom: 36px;

  .page-main-title {
    font-size: clamp(2rem, 3.5vw, 2.5rem);
    font-weight: 600;
    color: var(--color-ink);
    margin: 0;
    letter-spacing: -0.01em;
  }
}

// 3-Column Layout matching Mockup
.contact-layout-grid {
  display: grid;
  grid-template-columns: 1fr 1.35fr 0.65fr;
  gap: 32px;
  align-items: stretch;

  @media (max-width: 1024px) {
    grid-template-columns: 1fr 1.3fr;

    .contact-photo-col {
      display: none;
    }
  }

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
    gap: 28px;
  }
}

// Left Column: Contact Info & Map
.contact-info-col {
  display: flex;
  flex-direction: column;
  gap: 24px;

  .info-items-list {
    display: flex;
    flex-direction: column;
    gap: 18px;
  }

  .info-entry {
    display: flex;
    align-items: flex-start;
    gap: 14px;

    .entry-icon {
      font-size: 1.1rem;
      color: var(--color-ink);
      margin-top: 2px;
      width: 20px;
      display: flex;
      justify-content: center;
    }

    .entry-text {
      display: flex;
      flex-direction: column;

      .entry-label {
        font-size: 0.95rem;
        font-weight: 600;
        color: var(--color-ink);
        margin-bottom: 2px;
      }

      .entry-val {
        font-size: 0.95rem;
        color: var(--color-ink-soft);
        margin: 0;
      }
    }
  }

  .map-frame-box {
    position: relative;
    width: 100%;
    height: 180px;
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    overflow: hidden;
    background-color: var(--color-paper-light);

    .map-iframe {
      width: 100%;
      height: 100%;
      border: 0;
      filter: saturate(0.85) contrast(0.95);
    }

    .map-quote-badge {
      position: absolute;
      bottom: 8px;
      left: 8px;
      right: 8px;
      background-color: rgba(24, 35, 34, 0.85);
      color: #FAF7F0;
      font-size: 0.78rem;
      padding: 4px 10px;
      text-align: center;
      backdrop-filter: blur(2px);
      border-radius: 2px;
    }
  }
}

// Center Column: Form
.contact-form-col {
  .form-card-box {
    background-color: var(--color-paper-light);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    padding: 28px 32px;
    height: 100%;
    display: flex;
    flex-direction: column;

    @media (max-width: 600px) {
      padding: 20px;
    }
  }

  .form-title {
    font-size: 1.4rem;
    font-weight: 600;
    color: var(--color-ink);
    margin: 0 0 20px;
  }

  .consultation-form {
    display: flex;
    flex-direction: column;
    gap: 16px;
    flex-grow: 1;
  }

  .form-row-2 {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 16px;

    @media (max-width: 600px) {
      grid-template-columns: 1fr;
      gap: 14px;
    }
  }

  .form-field {
    display: flex;
    flex-direction: column;
    gap: 6px;

    .field-label {
      font-size: 0.88rem;
      font-weight: 500;
      color: var(--color-ink);
    }

    .vintage-input,
    .vintage-textarea {
      width: 100%;
      background-color: var(--color-paper);
      border: 1px solid var(--color-border);
      border-radius: var(--radius-xs);
      padding: 9px 12px;
      font-size: 0.92rem;
      color: var(--color-ink);
      outline: none;
      transition: border-color 0.2s ease;

      &:focus {
        border-color: var(--color-burgundy);
      }

      &::placeholder {
        color: var(--color-muted);
        font-style: italic;
      }
    }

    .vintage-textarea {
      resize: vertical;
    }
  }

  .btn-vintage-submit {
    margin-top: auto;
    padding: 12px;
    background-color: var(--color-burgundy);
    color: #FAF7F0;
    border: 1px solid var(--color-burgundy-dark);
    border-radius: 2px;
    font-size: 1.05rem;
    font-weight: 500;
    cursor: pointer;
    transition: background-color 0.25s ease;
    width: 100%;

    &:hover:not(:disabled) {
      background-color: var(--color-burgundy-dark);
    }

    &:disabled {
      opacity: 0.7;
      cursor: not-allowed;
    }
  }
}

// Right Column: Vertical Photo Frame
.contact-photo-col {
  .vertical-photo-frame {
    position: relative;
    width: 100%;
    height: 100%;
    min-height: 400px;
    overflow: hidden;
    border: 1px solid var(--color-border);
    border-radius: var(--radius-xs);
    background-color: var(--color-paper-dark);

    .photo-vertical {
      width: 100%;
      height: 100%;
      object-fit: cover;
      display: block;
      transition: transform 0.6s ease;

      &:hover {
        transform: scale(1.03);
      }
    }

    .vertical-tag {
      position: absolute;
      top: 16px;
      right: 16px;
      writing-mode: vertical-rl;
      background-color: var(--color-burgundy);
      color: #FAF7F0;
      padding: 12px 6px;
      font-size: 0.95rem;
      letter-spacing: 4px;
      border-radius: 2px;
    }
  }
}
</style>
