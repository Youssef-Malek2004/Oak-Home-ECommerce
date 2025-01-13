import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import dotenv from "dotenv";

dotenv.config();

export default defineConfig({
  plugins: [react()],
  server: {
    https: {
      key: Buffer.from(process.env.SSL_KEY ?? "", "utf-8"),
      cert: Buffer.from(process.env.SSL_CERT ?? "", "utf-8"),
    },
    host: true, // Allows access on the local network
    port: 5173, // Default Vite port, can be customized
  },
});
