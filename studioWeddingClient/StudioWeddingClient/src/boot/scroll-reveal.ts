import type { App, DirectiveBinding } from 'vue'

export default ({ app }: { app: App }) => {
  if (typeof window === 'undefined') return

  const observerOptions: IntersectionObserverInit = {
    threshold: 0.12,
    rootMargin: '0px 0px -40px 0px',
  }

  const observer = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        entry.target.classList.add('is-revealed')
        observer.unobserve(entry.target)
      }
    })
  }, observerOptions)

  // Custom Vue Directive: v-reveal
  app.directive('reveal', {
    mounted(el: HTMLElement, binding: DirectiveBinding) {
      const modifier = binding.arg || (binding.value as string) || 'fade-up'
      el.classList.add('reveal-on-scroll', `reveal-${modifier}`)

      // Immediate check if element is already in viewport
      const rect = el.getBoundingClientRect()
      if (rect.top < window.innerHeight && rect.bottom > 0) {
        // Small delay for initial render polish
        setTimeout(() => {
          el.classList.add('is-revealed')
        }, 80)
      } else {
        observer.observe(el)
      }
    },
    unmounted(el: HTMLElement) {
      observer.unobserve(el)
    },
  })

  // Also auto-observe any element with class .reveal-on-scroll or .film-reveal-on-scroll
  const initClassObserver = () => {
    const elements = document.querySelectorAll('.reveal-on-scroll, .film-reveal-on-scroll')
    elements.forEach((el) => {
      if (!el.classList.contains('is-revealed')) {
        observer.observe(el)
      }
    })
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initClassObserver)
  } else {
    initClassObserver()
  }
}
