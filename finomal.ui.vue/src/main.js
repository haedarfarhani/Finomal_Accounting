/**
 * main.js
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */
import './assets/styles/main.css'
// Plugins
import { registerPlugins } from '@/plugins'

// Components
import App from './App.vue'

// Composables
import { createApp } from 'vue'

// Styles
import 'unfonts.css'

const app = createApp(App)

registerPlugins(app)

app.mount('#app')
