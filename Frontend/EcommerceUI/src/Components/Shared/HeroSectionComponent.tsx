import { Box, Typography, Button } from "@mui/material";
import image from "../../assets/EcommerceShop.jpg";

const HeroSection = () => {
  return (
    <Box
      sx={{
        height: "90vh",
        backgroundImage: `url(${image})`,
        backgroundSize: "cover",
        backgroundPosition: "center",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        textAlign: "center",
        color: "white",
        padding: 4,
        width: "100%",
      }}
    >
      <Typography variant="h2" fontWeight="bold">
        ELEVATE YOUR VISION
      </Typography>
      <Button
        variant="contained"
        color="secondary"
        sx={{ marginTop: 4, fontSize: "1.2rem", padding: "0.8rem 2rem" }}
      >
        Shop Now
      </Button>
    </Box>
  );
};

export default HeroSection;
