import React, { useState } from "react";
import { Box } from "@mui/material";
import image from "../../assets/LandingPageImage.webp";
// import LandingHeroSection from "./LandingPage/LandingHeroSection";
import LandingNavBar from "./LandingPage/LandingNavBar";
import Background from "./Background";

const navItems: string[] = ["Living", "Dining", "Bedroom", "Outdoor", "Office", "Storage", "Lighting", "Accessories"];

const LandingPage: React.FC = () => {
  const [activeSection, setActiveSection] = useState<string | null>(null);

  return (
    <Box>
      <Background image={image} />
      <LandingNavBar navItems={navItems} activeSection={activeSection} setActiveSection={setActiveSection} />
      {/* <LandingHeroSection /> */}
    </Box>
  );
};

export default LandingPage;
