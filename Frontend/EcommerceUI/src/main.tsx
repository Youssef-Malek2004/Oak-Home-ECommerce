import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";
import { AuthProvider } from "./Components/Contexts/Authentication/Authentication.tsx";
import { SpeedInsights } from "@vercel/speed-insights/next";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <AuthProvider>
      <SpeedInsights />
      <App />
    </AuthProvider>
  </StrictMode>
);
