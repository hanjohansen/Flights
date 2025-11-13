/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './**/*.{razor,html}',
    '!./**/bin/**',
    '!./**/obj/**'
  ],
  darkMode: 'selector',
  theme: {
    extend: {
      colors: {
        //general
        'light-background': '#d1d1d1',
        'light-surface': '#ffffff',
        'light-border': '#b6b7b1',

        'dark-background': '#010409',        
        'dark-surface': '#212427',
        'dark-border': '#373a3e',

        //game
        "light-player-bd-active": '#16a34a', //green-600
        "light-player-bd-inactive": '#9ca3af', //gray-400
        "light-player-text-active": '#1f2937', //gray-800
        "light-player-text-inactive": '#4b5563', //gray-600

        "dark-player-bd-active": '#16a34a', //green-600
        "dark-player-bd-inactive": '#373a3e',
        "dark-player-text-active": '#e8e8e8',
        "dark-player-text-inactive": '#9e9fa2'
      },
    },
  },
  plugins: [],
}

