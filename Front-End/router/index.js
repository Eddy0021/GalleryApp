import { createRouter, createWebHistory } from 'vue-router'
import NotFound from '../views/NotFound.vue'
import LoginPage from '../views/Login/LoginPage.vue'
import Register from '../views/Login/RegisterTab.vue'
import ForgotPassword from '../views/Login/ResetPW.vue'
import Gallery from '../views/Gallery/Gallery.vue'
import GalleryAlbum from '../views/Gallery/Gallery-Album.vue'
import ResetPassword from '../views/Login/ChangePassword.vue'

const routes = [
  {
    path: '/',
    name: 'LoginPage',
    component: LoginPage
  },
  {
    path: '/Register',
    name: 'Register',
    component: Register
  },
  {
    path: '/Forgot-Password',
    name: 'ForgotPassword',
    component: ForgotPassword
  },
  {
    path: '/Reset-Password/:guid',
    name: 'ResetPassword',
    component: ResetPassword
  },
  {
    path: '/Gallery',
    name: 'Gallery',
    component: Gallery
  },
  {
    path: '/Gallery/:albumID',
    name: 'GalleryAlbum',
    component: GalleryAlbum
  },
  



  // catchall 404
  {
    path: '/:catchAll(.*)',
    name: 'NotFound',
    component: NotFound
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
