import { Box, Typography, Button, Container } from "@mui/material";
import image from "../../assets/EcommerceShop.jpg";

const HeroSection = () => {
  return (
    <Box
      sx={{
        position: "relative",
        height: { xs: "60vh", sm: "70vh", md: "90vh" },
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        backgroundImage: `url(${image})`,
        backgroundSize: "cover",
        backgroundPosition: "center",
        color: "white",
        textAlign: "center",
        overflow: "hidden",
      }}
    >
      <Box
        sx={{
          position: "absolute",
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          backgroundColor: "rgba(0,0,0,0.5)", // Overlay for better text readability
        }}
      />
      <Container maxWidth="md">
        <Box
          sx={{
            position: "relative",
            zIndex: 1,
            textAlign: "center",
            px: { xs: 2, md: 0 },
          }}
        >
          <Typography
            variant="h2"
            component="h1"
            sx={{
              fontWeight: "bold",
              mb: 4,
              fontSize: { xs: "2.5rem", sm: "3.5rem", md: "4.5rem" },
            }}
          >
            ELEVATE YOUR VISION
          </Typography>
          <Button
            variant="contained"
            color="primary"
            size="large"
            sx={{
              px: 4,
              py: 1.5,
              fontSize: "1.2rem",
              fontWeight: "bold",
              "&:hover": {
                transform: "scale(1.05)",
                transition: "transform 0.3s ease",
              },
            }}
          >
            Shop Now
          </Button>
        </Box>
      </Container>
    </Box>
  );
};

export default HeroSection;
