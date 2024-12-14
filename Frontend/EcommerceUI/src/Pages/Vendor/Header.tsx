import React from "react";
import { Box, Typography } from "@mui/material";

const Header: React.FC = () => {
  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        mb: 3,
      }}
    >
      <Typography
        variant="h4"
        sx={{
          fontWeight: "bold",
          color: "#2d3748",
        }}
      >
        $10,421.10 to Marvin LTD
      </Typography>
      <Typography variant="subtitle1" color="#718096">
        Draft
      </Typography>
    </Box>
  );
};

export default Header;
