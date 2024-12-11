// Components/Background.tsx
import React from "react";
import { Box } from "@mui/material";

interface BackgroundProps {
  image: string;
}

const Background: React.FC<BackgroundProps> = ({ image }) => (
  <>
    <Box
      sx={{
        position: "fixed",
        top: 0,
        left: 0,
        width: "100%",
        height: "100%",
        backgroundImage: `url(${image})`,
        backgroundSize: "cover",
        backgroundPosition: "center",
        zIndex: -2,
      }}
    />
    <Box
      sx={{
        position: "fixed",
        top: 0,
        left: 0,
        width: "100%",
        height: "100%",
        backgroundColor: "rgba(0, 0, 0, 0.6)",
        zIndex: -1,
      }}
    />
  </>
);

export default Background;
