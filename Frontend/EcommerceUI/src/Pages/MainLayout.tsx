import React from "react";
import { Box } from "@mui/material";
import { Outlet } from "react-router-dom";
import SocialMediaBar from "../Components/Shared/SocialMediaBar";

const MainLayout: React.FC = () => {
  return (
    <Box
      sx={{
        minHeight: "100vh",
        minWidth: "100%",
        display: "flex",
        flexDirection: "column",
        alignContent: "center",
      }}
    >
      <Outlet />
      <SocialMediaBar />
    </Box>
  );
};

export default MainLayout;
