/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './**/*.{razor,html}',
    '!./**/bin/**',
    '!./**/obj/**'
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}

