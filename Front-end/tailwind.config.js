/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts,css}"],
  theme: {
    extend: {
      colors: {
        primary: {
          50: "#eef2ff",
          100: "#e0e7ff",
          200: "#c7d2fe",
          300: "#a5b4fc",
          400: "#818cf8",
          500: "#6366f1",
          600: "#4f46e5", // indigo-600
          700: "#4338ca",
          800: "#3730a3",
          900: "#312e81",
        },
        neutral: {
          50: "#f8fafc", // slate-50
          100: "#f1f5f9",
          200: "#e2e8f0",
          300: "#cbd5e1",
          400: "#94a3b8",
          500: "#64748b",
          600: "#475569", // slate-600 approximate
          700: "#334155",
          800: "#1e293b",
          900: "#0f172a",
        },
      },
      fontFamily: {
        sans: [
          "Inter",
          "Roboto",
          "ui-sans-serif",
          "system-ui",
          "-apple-system",
          "Segoe UI",
          "Helvetica Neue",
          "Arial",
        ],
      },
      boxShadow: {
        soft: "0 6px 20px rgba(16,24,40,0.06)",
        "soft-md": "0 8px 30px rgba(16,24,40,0.08)",
      },
      borderColor: {
        DEFAULT: "#e6e9ee",
      },
    },
  },
  plugins: [require("@tailwindcss/typography")],
};
