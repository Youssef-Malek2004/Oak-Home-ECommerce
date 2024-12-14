import React, { useState, useEffect } from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Button,
  Box,
  Card,
  CardContent,
} from "@mui/material";
import { Add as AddIcon } from "@mui/icons-material";
import { useNavigate } from "react-router-dom";
import { fetchProductsByVendor } from "../../api/VendorApi";
import { VendorGetProducts } from "../../Interfaces/Product";

export const AllProductsTable: React.FC = () => {
  const [products, setProducts] = useState<VendorGetProducts[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const productsPerPage = 20;
  const totalPages = Math.ceil(products.length / productsPerPage);

  useEffect(() => {
    const loadProducts = async () => {
      try {
        const fetchedProducts = await fetchProductsByVendor();
        setProducts(fetchedProducts);
      } catch (error) {
        console.error("Failed to load products:", error);
      } finally {
        setLoading(false);
      }
    };

    loadProducts();
  }, []);

  const displayedProducts = products.slice(0, currentPage * productsPerPage);

  const handleProductClick = (id: string) => {
    navigate(`/vendor/products/details/${id}`);
  };

  const handleAddProductClick = () => {
    navigate("/vendor/products/create");
  };

  if (loading) {
    return <div>Loading products...</div>;
  }

  return (
    <Card
      elevation={3}
      sx={{
        borderRadius: "16px", // Smooth edges for the card
        padding: "16px",
      }}
    >
      <CardContent>
        <Box display="flex" justifyContent="flex-end" mb={2}>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={handleAddProductClick}
            sx={{
              backgroundColor: "#f5deb3", // Beige color
              color: "#000", // Black text for contrast
              "&:hover": {
                backgroundColor: "#e5ce93", // Slightly darker beige on hover
              },
            }}
          >
            Add Product
          </Button>
        </Box>
        <TableContainer
          component={Paper}
          variant="outlined"
          sx={{ borderRadius: "16px" }} // Smooth edges for the table container
        >
          <Table>
            <TableHead>
              <TableRow>
                {["ID", "Name", "Category", "Price", "Created At"].map(
                  (header) => (
                    <TableCell
                      key={header}
                      sx={{
                        fontWeight: "bold",
                        backgroundColor: "#f5f5dc", // Cream-like color
                        fontStyle: "italic",
                        padding: "16px", // Add padding
                        borderRadius: "8px", // Smooth edges for header cells
                      }}
                    >
                      {header}
                    </TableCell>
                  )
                )}
              </TableRow>
            </TableHead>
            <TableBody>
              {displayedProducts.map((product) => (
                <TableRow
                  key={product.product.id}
                  hover
                  onClick={() => handleProductClick(product.product.id)}
                  sx={{
                    cursor: "pointer",
                    "&:hover": { backgroundColor: "#f9f9f9" }, // Highlight on hover
                  }}
                >
                  <TableCell sx={{ padding: "16px", borderRadius: "8px" }}>
                    {product.product.id}
                  </TableCell>
                  <TableCell sx={{ padding: "16px", borderRadius: "8px" }}>
                    {product.product.name}
                  </TableCell>
                  <TableCell sx={{ padding: "16px", borderRadius: "8px" }}>
                    {product.category}
                  </TableCell>
                  <TableCell sx={{ padding: "16px", borderRadius: "8px" }}>
                    ${product.product.price}
                  </TableCell>
                  <TableCell sx={{ padding: "16px", borderRadius: "8px" }}>
                    {new Date(product.product.createdAt).toLocaleDateString()}
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>

        {currentPage < totalPages && (
          <Box display="flex" justifyContent="center" mt={3}>
            <Button
              variant="contained"
              color="primary"
              onClick={() => setCurrentPage(currentPage + 1)}
            >
              Load More Products
            </Button>
          </Box>
        )}
      </CardContent>
    </Card>
  );
};
