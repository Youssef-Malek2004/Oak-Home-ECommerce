import "./App.css";
import HeroSection from "./Components/Shared/HeroSectionComponent";
import Layout from "./Pages/Layout";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from "./Components/User/Login";
import Profile from "./Components/User/Profile";
import SignUp from "./Components/User/SignUp";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/sign-up" element={<SignUp />} />

        <Route path="/" element={<Layout />}>
          <Route index element={<HeroSection />} /> {/* Landing Page */}
          <Route path="profile" element={<Profile />} />
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
