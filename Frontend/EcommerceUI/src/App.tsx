import "./App.css";
import HeroSection from "./Components/Shared/HeroSectionComponent";
import Layout from "./Pages/Layout";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from "./Components/User/Login";
import Profile from "./Components/User/Profile";
import SignUp from "./Components/User/SignUp";
import MainLayout from "./Pages/MainLayout";
import ProtectedRoute from "./Components/Contexts/Authentication/ProtectedRoute";
import VendorDashboard from "./Pages/Vendor/VendorDashboard";
import VendorProductsPage from "./Pages/Vendor/VendorProductsPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/sign-up" element={<SignUp />} />
        <Route path="/vendor" element={<VendorDashboard />}>
          <Route path="products" element={<VendorProductsPage />} />
        </Route>
        <Route path="/" element={<MainLayout />}>
          <Route
            path="/shop"
            element={
              <ProtectedRoute>
                <Layout />
              </ProtectedRoute>
            }
          >
            <Route index element={<HeroSection />} />
            <Route
              path="profile"
              element={
                <ProtectedRoute>
                  <Profile />
                </ProtectedRoute>
              }
            />
          </Route>
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
