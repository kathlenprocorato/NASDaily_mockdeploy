/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
    "node_modules/flowbite-react/**/*.{js,jsx,ts,tsx}",
  ],

  theme: {
    colors: {
      primary: "#8A353C",
      secondary: "#E9C434",
      gray: "#717171",
      white: "#FFFFFF",
      transparent: "transparent",
      black: "#000000",
      grey: "#e4e5e5",
      green: "#089306",
      red: "#FF0000",
      okay: "#fff6d1",
      warning: "#f0c7c7",
      good: "#c4e5c3",
    },
    extend: {},
  },
  plugins: ["flowbite/plugin"],
};
