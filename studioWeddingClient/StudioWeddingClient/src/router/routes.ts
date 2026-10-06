import type { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {

    path: "/",
    redirect: "/trang-chu"
  },

  {
    path: '/quan-tri-hy-su-studio',
    component: () => import('@/layouts/AdminLayout.vue'),
    children: [
      { path: '', component: () => import('@/pages/IndexPage.vue') },
      { path: 'second', component: () => import('@/pages/SecondPage.vue') },
    ],
  },
  {
    path: '/',
    component: () => import('@/layouts/UserLayout.vue'),
    children: [
      { path: '', redirect: '/trang-chu' },
      { path: 'trang-chu', component: () => import('@/pages/user/UserHomePage.vue') },
      { path: 'album/:slug', component: () => import('@/pages/user/AlbumDetailPage.vue') },
      { path: 'dich-vu/:slug', component: () => import('@/pages/user/ServiceDetailPage.vue') },
      { path: 'goi-cuoi/:slug', component: () => import('@/pages/user/PackageDetailPage.vue') },
      { path: 'second', component: () => import('@/pages/SecondPage.vue') },
    ]
  },
  {
    path: '/dang-nhap-quan-tri-hy-su-studio',
    component: () => import('@/layouts/AuthAdminLayout.vue'),
    children: [
      { path: '', component: () => import('@/pages/IndexPage.vue') },
      { path: 'second', component: () => import('@/pages/SecondPage.vue') },
    ]
  },
  {
    path: '/',
    component: () => import('@/layouts/AuthUserLayout.vue'),
    children: [
      { path: 'dang-nhap', component: () => import('@/pages/auth/LoginPage.vue') },
      { path: 'dang-ky', component: () => import('@/pages/auth/RegisterPage.vue') },
      { path: 'quen-mat-khau', component: () => import('@/pages/auth/ForgotPasswordPage.vue') },
    ],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('@/pages/ErrorNotFound.vue'),
  },
];

export default routes;
