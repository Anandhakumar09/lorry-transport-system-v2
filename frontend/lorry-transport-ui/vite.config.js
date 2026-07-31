import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    // If 5173 is already busy, Vite would otherwise silently jump to
    // 5174/5175/etc. That new port is NOT in the backend's CORS allow-list,
    // so every save/edit/delete call would fail with a CORS error.
    // strictPort makes Vite fail loudly instead, so you always know the
    // frontend is really running on 5173 (matching backend CORS + this file).
    strictPort: true
  }
})
