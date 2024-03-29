import { fileURLToPath, URL } from "node:url"

import { defineConfig } from "vite"
import vue from "@vitejs/plugin-vue"

// https://vitejs.dev/config/
//
export default defineConfig({
    plugins: [ 
        vue()
    ],
    resolve: {
        alias: {
            "@": fileURLToPath(new URL("./src", import.meta.url)),
            "@common": fileURLToPath(new URL("../Common", import.meta.url))
        }
    },
    build: {
        target: "es2015"
    },
    esbuild: {
        charset: "utf8"
    }
})
