import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'

import { resolve } from 'path'

// https://vitejs.dev/config/
export default defineConfig({
    build: {
        // minify: false,
        lib: {
            entry: resolve(__dirname, "./main.js"),
            name: "Common",
            fileName: "common"
        }
    }
})
