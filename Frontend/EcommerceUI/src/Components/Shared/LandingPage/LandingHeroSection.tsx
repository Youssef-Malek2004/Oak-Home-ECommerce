// Components/HeroSection.tsx
import React from "react";
import { Box, Typography, Button } from "@mui/material";

const LandingHeroSection: React.FC = () => (
  <Box
    sx={{
      display: "flex",
      flexDirection: "column",
      justifyContent: "center",
      alignItems: "center",
      minHeight: "100vh",
      textAlign: "center",
      color: "white",
    }}
  >
    <Typography
      variant="h6"
      sx={{
        letterSpacing: 2,
        textTransform: "uppercase",
        mb: 2,
        opacity: 0.8,
      }}
    >
      New Arrivals
    </Typography>
    <Typography
      variant="h1"
      sx={{
        fontSize: { xs: "2.5rem", md: "4rem" },
        fontWeight: "bold",
        mb: 4,
      }}
    >
      New Designs.
    </Typography>
    <Button
      variant="contained"
      sx={{
        backgroundColor: "white",
        color: "black",
        padding: "10px 20px",
        fontSize: "1rem",
        fontWeight: "bold",
        textTransform: "uppercase",
        "&:hover": {
          backgroundColor: "#f5f5f5",
          transform: "scale(1.05)",
        },
        transition: "transform 0.3s ease",
      }}
    >
      Shop Now →
    </Button>
  </Box>
);

export default LandingHeroSection;
