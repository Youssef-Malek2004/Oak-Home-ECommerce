import { Box, Typography, Button, Container } from "@mui/material";
import image from "../../assets/EcommerceShop.jpg";
import { keyframes } from "@emotion/react";
import { ProductCategories } from "../VendorComponents/ProductCategories";
import { useRef } from "react";
import ArrowDownwardIcon from "@mui/icons-material/ArrowDownward";

const fadeIn = keyframes`
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
`;

const bounce = keyframes`
  0%, 20%, 50%, 80%, 100% {
    transform: translateY(0);
  }
  40% {
    transform: translateY(-10px);
  }
  60% {
    transform: translateY(-5px);
  }
`;

const HeroSection = () => {
  const categoriesRef = useRef<HTMLDivElement>(null);
  const handleScrollToCategories = () => {
    categoriesRef.current?.scrollIntoView({ behavior: "smooth" });
  };
  return (
    <Box>
      {/* Hero Section */}
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
          backgroundRepeat: "no-repeat",
          color: "white",
          textAlign: "center",
          overflow: "hidden",
          width: "93%",
          margin: "0 auto",
          animation: `${fadeIn} 1s ease-in-out`,
        }}
      >
        {/* Overlay */}
        <Box
          sx={{
            position: "absolute",
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            backgroundColor: "rgba(0, 0, 0, 0.5)",
            zIndex: 1,
          }}
        />
        {/* Content */}
        <Container sx={{ position: "relative", zIndex: 2 }}>
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
          <Typography variant="h5" component="p" gutterBottom>
            Discover the best products at unbeatable prices.
          </Typography>
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              gap: 1,
              mt: 2,
            }}
          >
            <Button
              variant="contained"
              color="primary"
              onClick={handleScrollToCategories}
              sx={{
                fontSize: "1rem",
                fontWeight: "bold",
                padding: "0.75rem 2rem",
                borderRadius: "50px",
                boxShadow: "0px 4px 10px rgba(0, 0, 0, 0.25)",
                position: "relative",
                overflow: "hidden",
                transition: "transform 0.3s ease, box-shadow 0.3s ease",
                "&:hover": {
                  transform: "scale(1.1)",
                  boxShadow: "0px 6px 14px rgba(0, 0, 0, 0.35)",
                },
                "&:focus": {
                  outline: "none",
                  boxShadow: "0px 6px 14px rgba(0, 0, 0, 0.35)",
                },
              }}
            >
              Shop Now
              <ArrowDownwardIcon
                sx={{
                  ml: 1,
                  animation: `${bounce} 2s infinite`,
                  fontSize: "1.5rem",
                }}
              />
            </Button>
          </Box>
        </Container>
      </Box>

      {/* Product Categories Section */}
      <Box
        ref={categoriesRef}
        sx={{
          mt: 4,
          px: { xs: 2, sm: 4, md: 8 },
          textAlign: "center",
        }}
      >
        <ProductCategories />
      </Box>
    </Box>
  );
};

export default HeroSection;
