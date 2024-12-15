/* eslint-disable @typescript-eslint/no-unused-vars */
import React, { useState, useEffect, FormEvent } from "react";
import {
  Box,
  Button,
  Card,
  CardContent,
  TextField,
  Typography,
  MenuItem,
  Select,
  InputLabel,
  FormControl,
  Grid,
  Tooltip,
  IconButton,
} from "@mui/material";
import InfoIcon from "@mui/icons-material/Info";
import { useNavigate, useParams } from "react-router-dom";
import { fetchProductById } from "../../api/ProductsApi";
import { Category } from "../../Interfaces/Category";
import { fetchProductCategories, updateProduct } from "../../api/ProductsApi";
import { fetchWarehouseNames, fetchWarehouseForProductId } from "../../api/InventoryApi";
import { ProductsWarehouses } from "../../Interfaces/DTOs/Warehouse/ProductsGetWarehouses";
import { GetProductByIdProductDto } from "../../Interfaces/DTOs/Products/GetProductByIdDto";
import { Warehouse } from "../../Interfaces/Warehouse";
import { UpdateProductDto } from "../../Interfaces/DTOs/Products/UpdateProductDto";
import { DynamicFields } from "../../Interfaces/DTOs/CreateProductDto";

const EditProduct: React.FC = () => {
  const navigate = useNavigate();
  const { productId } = useParams<{ productId: string }>();
  const [product, setProduct] = useState<GetProductByIdProductDto | null>(null);
  const [categories, setCategories] = useState<Category[]>([]);
  const [warehouses, setWarehouses] = useState<ProductsWarehouses | null>(null);
  const [productWarehouse, setProductWarehouse] = useState<Warehouse | null>(null);
  const [loading, setLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const formatDateForInput = (dateString: string): string => {
    const date = new Date(dateString);
    if (isNaN(date.getTime())) {
      return ""; // Return an empty string if the date is invalid
    }
    return date.toISOString().split("T")[0]; // Format as YYYY-MM-DD
  };

  useEffect(() => {
    const loadProduct = async () => {
      try {
        const fetchedProduct = await fetchProductById(productId!);
        setProduct(fetchedProduct);
      } catch (error) {
        console.error("Failed to load product:", error);
        setErrorMessage("Failed to load product details.");
      }
    };

    const loadCategories = async () => {
      try {
        const fetchedCategories = await fetchProductCategories();
        setCategories(fetchedCategories);
      } catch (error) {
        console.error("Failed to load categories:", error);
      }
    };

    const loadWarehouses = async () => {
      try {
        const fetchedWarehouses = await fetchWarehouseNames();
        setWarehouses(fetchedWarehouses);
      } catch (error) {
        console.error("Failed to load warehouses:", error);
      }
    };

    const loadWarehouse = async () => {
      try {
        const fetchedWarehouse = await fetchWarehouseForProductId(productId);
        setProductWarehouse(fetchedWarehouse);
      } catch (error) {
        console.error("Failed to load warehouses:", error);
      }
    };

    Promise.all([loadProduct(), loadCategories(), loadWarehouses(), loadWarehouse()]).finally(() => setLoading(false));
  }, [productId]);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    if (!product) return;

    const dynamicFields: DynamicFields = {
      Material: product.material,
      Finish: product.finish,
      Length: product.length,
      Width: product.width,
      Height: product.height,
      Weight: product.weight,
      Color: product.color,
      SubCategory: product.subCategory,
      Usage: product.usage,
      IsCustomizable: product.isCustomizable,
      Features: product.features,
      WarrantyInYears: product.warrantyInYears,
      MaintenanceInstructions: product.maintenanceInstructions,
      Brand: product.brand,
      Manufacturer: product.manufacturer,
      ManufactureDate: product.manufactureDate,
      CountryOfOrigin: product.countryOfOrigin,
      IsEcoFriendly: product.isEcoFriendly,
    };

    const {
      material,
      finish,
      length,
      width,
      height,
      weight,
      color,
      subCategory,
      usage,
      isCustomizable,
      features,
      warrantyInYears,
      maintenanceInstructions,
      brand,
      manufacturer,
      manufactureDate,
      countryOfOrigin,
      isEcoFriendly,
      ...baseProductData
    } = product;

    console.log(dynamicFields);

    const updatedProduct: UpdateProductDto = {
      updateProductDto: baseProductData,
      dynamicFields: dynamicFields,
    };

    try {
      await updateProduct(productId!, updatedProduct);
      navigate("/vendor/products");
    } catch (error) {
      console.error("Error updating product:", error);
      setErrorMessage("Failed to update product. Please try again later.");
    }
  };

  const handleFieldChange = (field: keyof GetProductByIdProductDto, value: unknown) => {
    setProduct((prev) => (prev ? { ...prev, [field]: value } : prev));
  };

  if (loading) return <div>Loading product details...</div>;

  return (
    <Card
      elevation={3}
      sx={{
        borderRadius: "16px",
        padding: "24px",
        maxWidth: "90%",
        margin: "0 auto",
        mt: 4,
        backgroundColor: "#F5F5F5",
        boxShadow: "0 4px 6px rgba(0,0,0,0.1)",
      }}
    >
      <CardContent>
        <Typography
          variant="h4"
          mb={4}
          sx={{
            fontWeight: 600,
            color: "#333",
            textAlign: "center",
          }}
        >
          Edit Product
        </Typography>
        {errorMessage && (
          <Typography variant="body2" color="error" mb={3} textAlign="center">
            {errorMessage}
          </Typography>
        )}
        <Box component="form" onSubmit={handleSubmit}>
          <Grid container spacing={4}>
            {/* First Column - Basic Product Information */}
            <Grid item xs={12} md={6}>
              <Typography variant="h6" mb={3} sx={{ color: "#444", borderBottom: "2px solid #666", paddingBottom: "8px" }}>
                Basic Information
              </Typography>
              <Box display="flex" flexDirection="column" gap={3}>
                <TextField
                  label="Name"
                  value={product?.name || ""}
                  required
                  fullWidth
                  variant="outlined"
                  onChange={(e) => handleFieldChange("name", e.target.value)}
                />
                <TextField
                  label="Description"
                  value={product?.description || ""}
                  required
                  fullWidth
                  multiline
                  rows={4}
                  variant="outlined"
                  onChange={(e) => handleFieldChange("description", e.target.value)}
                />
                <FormControl required fullWidth>
                  <InputLabel id="category-label">Category</InputLabel>
                  <Select
                    labelId="category-label"
                    value={product?.categoryId || ""}
                    onChange={(e) => handleFieldChange("categoryId", e.target.value)}
                    label="Category"
                  >
                    {categories.map((cat) => (
                      <MenuItem key={cat.id} value={cat.id}>
                        {cat.name}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
                <TextField
                  label="SKU"
                  value={product?.sku || ""}
                  required
                  fullWidth
                  variant="outlined"
                  onChange={(e) => handleFieldChange("sku", e.target.value)}
                />
                <TextField
                  label="Price"
                  type="number"
                  value={product?.price || ""}
                  required
                  fullWidth
                  variant="outlined"
                  onChange={(e) => handleFieldChange("price", Number(e.target.value))}
                />
              </Box>
            </Grid>

            {/* Second Column - Warehouse */}
            <Grid item xs={12} md={6}>
              <Typography variant="h6" mb={3} sx={{ color: "#444", borderBottom: "2px solid #666", paddingBottom: "8px" }}>
                Warehouse Details
              </Typography>
              <Box display="flex" flexDirection="column" gap={3}>
                <Box display="flex" alignItems="center" gap={2}>
                  <Typography>
                    <strong>Warehouse:</strong>{" "}
                    {warehouses?.warehouses.find((w) => w.warehouseId === productWarehouse?.warehouseId)?.name || "N/A"}
                  </Typography>
                  {productWarehouse && (
                    <Tooltip
                      title={
                        <Box>
                          <Typography>
                            <strong>Operational:</strong> {productWarehouse.isOperational ? "Yes" : "No"}
                          </Typography>
                        </Box>
                      }
                      arrow
                    >
                      <IconButton>
                        <InfoIcon color="primary" />
                      </IconButton>
                    </Tooltip>
                  )}
                </Box>
              </Box>
            </Grid>
            <Grid item xs={12} md={12}>
              <Typography
                variant="h6"
                mb={3}
                sx={{
                  color: "#444",
                  borderBottom: "2px solid #666",
                  paddingBottom: "8px",
                }}
              >
                Product Essentials
              </Typography>
              <Box display="flex" flexDirection="column" gap={3}>
                <Grid container spacing={3}>
                  {/* First Column */}
                  <Grid item xs={12} md={6}>
                    <Box display="flex" flexDirection="column" gap={3}>
                      <TextField
                        label="Material"
                        value={product?.material || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("material", e.target.value)}
                      />
                      <TextField
                        label="Finish"
                        value={product?.finish || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("finish", e.target.value)}
                      />
                      <TextField
                        label="Length"
                        type="number"
                        value={product?.length || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("length", Number(e.target.value))}
                      />
                      <TextField
                        label="Width"
                        type="number"
                        value={product?.width || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("width", Number(e.target.value))}
                      />
                      <TextField
                        label="Height"
                        type="number"
                        value={product?.height || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("height", Number(e.target.value))}
                      />
                      <TextField
                        label="Weight"
                        type="number"
                        value={product?.weight || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("weight", Number(e.target.value))}
                      />
                      <TextField
                        label="Color"
                        value={product?.color || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("color", e.target.value)}
                      />
                      <TextField
                        label="Sub-Category"
                        value={product?.subCategory || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("subCategory", e.target.value)}
                      />
                    </Box>
                  </Grid>

                  {/* Second Column */}
                  <Grid item xs={12} md={6}>
                    <Box display="flex" flexDirection="column" gap={3}>
                      <TextField
                        label="Usage"
                        value={product?.usage || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("usage", e.target.value)}
                      />
                      <TextField
                        label="Customizable (Yes/No)"
                        value={product?.isCustomizable ? "Yes" : "No"}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("isCustomizable", e.target.value.toLowerCase() === "yes")}
                      />
                      <TextField
                        label="Features"
                        value={product?.features || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("features", e.target.value)}
                      />
                      <TextField
                        label="Warranty (Years)"
                        type="number"
                        value={product?.warrantyInYears || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("warrantyInYears", Number(e.target.value))}
                      />
                      <TextField
                        label="Maintenance Instructions"
                        value={product?.maintenanceInstructions || ""}
                        fullWidth
                        multiline
                        rows={3}
                        variant="outlined"
                        onChange={(e) => handleFieldChange("maintenanceInstructions", e.target.value)}
                      />
                      <TextField
                        label="Brand"
                        value={product?.brand || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("brand", e.target.value)}
                      />
                      <TextField
                        label="Manufacturer"
                        value={product?.manufacturer || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("manufacturer", e.target.value)}
                      />
                      <TextField
                        label="Manufacture Date"
                        type="date"
                        InputLabelProps={{ shrink: true }}
                        value={product?.manufactureDate ? formatDateForInput(product.manufactureDate) : ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("manufactureDate", e.target.value)}
                      />
                      <TextField
                        label="Country of Origin"
                        value={product?.countryOfOrigin || ""}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("countryOfOrigin", e.target.value)}
                      />
                      <TextField
                        label="Eco-Friendly (Yes/No)"
                        value={product?.isEcoFriendly ? "Yes" : "No"}
                        fullWidth
                        variant="outlined"
                        onChange={(e) => handleFieldChange("isEcoFriendly", e.target.value.toLowerCase() === "yes")}
                      />
                    </Box>
                  </Grid>
                </Grid>
              </Box>
            </Grid>
          </Grid>
          <Box display="flex" justifyContent="center" mt={4}>
            <Button
              type="submit"
              variant="contained"
              sx={{
                backgroundColor: "#2196f3",
                color: "white",
                "&:hover": {
                  backgroundColor: "#1976d2",
                },
              }}
            >
              Save Changes
            </Button>
          </Box>
        </Box>
      </CardContent>
    </Card>
  );
};

export default EditProduct;
