import React from "react";
import { Grid, Typography, Card, CardMedia, Box } from "@mui/material";

import LivingRoomImage from "../../assets/LivingRoomVendor.webp";
import DiningRoomImage from "../../assets/DiningRoomVendor.webp";
import BedroomImage from "../../assets/BedroomVendor.webp";
import OfficeImage from "../../assets/OfficeVendor.webp";

const TOP_SELLING_PRODUCTS = [
  {
    id: 1,
    name: "Elegant Oak Dining Table",
    image: DiningRoomImage,
    price: 1299.99,
    sales: 124,
  },
  {
    id: 2,
    name: "Modern Minimalist Bed Frame",
    image: BedroomImage,
    price: 899.99,
    sales: 98,
  },
  {
    id: 3,
    name: "Scandinavian Living Room Set",
    image: LivingRoomImage,
    price: 2499.99,
    sales: 76,
  },
  {
    id: 4,
    name: "Workspace Efficiency Desk",
    image: OfficeImage,
    price: 649.99,
    sales: 56,
  },
];

export const TopSellingProducts: React.FC = () => (
  <>
    <Typography
      variant="h5"
      sx={{
        textAlign: "center",
        mb: 3,
        fontWeight: 600,
        fontStyle: "italic",
      }}
    >
      Your Top Selling Products
    </Typography>
    <Grid container spacing={3}>
      {TOP_SELLING_PRODUCTS.map((product) => (
        <Grid item xs={12} sm={6} key={product.id}>
          <Card
            sx={{
              height: "100%",
              position: "relative",
              borderRadius: "16px",
              overflow: "hidden",
              transition: "all 0.3s ease",
              boxShadow: "0 8px 15px rgba(0,0,0,0.1)",
              "&:hover": {
                transform: "translateY(-10px)",
                boxShadow: "0 12px 20px rgba(0,0,0,0.15)",
              },
            }}
          >
            <CardMedia
              component="img"
              height="250"
              image={product.image}
              alt={product.name}
              sx={{
                objectFit: "cover",
                position: "relative",
              }}
            />
            {/* Dark Overlay */}
            <div
              style={{
                position: "absolute",
                top: 0,
                left: 0,
                width: "100%",
                height: "100%",
                backgroundColor: "rgba(0,0,0,0.4)",
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              <Typography
                variant="h5"
                sx={{
                  color: "white",
                  fontStyle: "italic",
                  fontWeight: 600,
                  textAlign: "center",
                  textShadow: "2px 2px 4px rgba(0,0,0,0.5)",
                }}
              >
                {product.name}
              </Typography>
              <Box
                sx={{
                  mt: 1,
                  backgroundColor: "#F5F5DC", // Cream color
                  borderRadius: "8px",
                  padding: "0.5rem 1rem",
                  textAlign: "center",
                  boxShadow: "0 4px 6px rgba(0, 0, 0, 0.1)",
                }}
              >
                <Typography
                  variant="body2"
                  sx={{
                    fontStyle: "italic",
                    color: "#333",
                  }}
                >
                  ${product.price.toFixed(2)} | {product.sales} Sold
                </Typography>
              </Box>
            </div>
          </Card>
        </Grid>
      ))}
    </Grid>
  </>
);
