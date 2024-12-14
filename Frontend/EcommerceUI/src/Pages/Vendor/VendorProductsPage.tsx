// VendorProductsPage.jsx

import React from "react";
import { Box, Grid, Typography } from "@mui/material";
import { AllProductsTable } from "../../Components/VendorComponents/AllProductsTable";
import { TopSellingProducts } from "../../Components/VendorComponents/TopSellingProducts";
// import { ProductCategories } from "../../Components/VendorComponents/ProductCategories";

const VendorProductsPage: React.FC = () => {
  return (
    <Box
      sx={{
        padding: { xs: "1rem", sm: "2rem" },
        backgroundColor: "#f9f9f9",
        minHeight: "100vh",
        marginLeft: { xs: 0, md: "80px" },
        display: "flex",
        flexDirection: "column",
      }}
    >
      <Grid container spacing={4}>
        <Grid item xs={12} md={7} sx={{ flexBasis: "60%" }}></Grid>
        <Grid item xs={12} md={5} sx={{ flexBasis: "40%" }}></Grid>
      </Grid>

      <TopSellingProducts />
      <Typography
        variant="h5"
        sx={{
          mt: 4,
          mb: 2,
          textAlign: "center",
          fontWeight: 600,
          textTransform: "uppercase",
        }}
      >
        Products
      </Typography>
      <AllProductsTable />

      {/* <Typography
        variant="h5"
        sx={{
          mt: 4,
          mb: 2,
          textAlign: "center",
          fontWeight: 600,
          textTransform: "uppercase",
        }}
      >
        Categories
      </Typography> */}
      {/* <ProductCategories /> */}
    </Box>
  );
};

export default VendorProductsPage;
