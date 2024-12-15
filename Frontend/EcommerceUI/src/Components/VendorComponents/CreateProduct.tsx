import React, { useState, useEffect, FormEvent } from "react";
import {
  Box,
  Button,
  Card,
  CardContent,
  TextField,
  Typography,
  MenuItem,
  Chip,
  Select,
  InputLabel,
  FormControl,
  OutlinedInput,
  Grid,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { Product } from "../../Interfaces/Product";
import { addProduct } from "../../api/ProductsApi";
import { Add as AddIcon } from "@mui/icons-material";
import { Category } from "../../Interfaces/Category";
import { fetchProductCategories } from "../../api/ProductsApi";
import { AddProductInventoryFields, CreateProductRequestDto, DynamicFields } from "../../Interfaces/DTOs/CreateProductDto";
import { fetchWarehouseNames } from "../../api/InventoryApi";
import { ProductsWarehouses } from "../../Interfaces/DTOs/Warehouse/ProductsGetWarehouses";

export const CreateProduct: React.FC = () => {
  const navigate = useNavigate();
  const [categories, setCategories] = useState<Category[]>([]);
  const [warehouses, setWarehouses] = useState<ProductsWarehouses>();
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [categoryId, setCategoryId] = useState("");
  const [sku, setSku] = useState("");
  const [price, setPrice] = useState<number | "">("");
  const [tags, setTags] = useState<string[]>([]);
  const [imageUrls, setImageUrls] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);
  const [warehouseId, setWarehouseId] = useState("");
  const [dynamicFields, setDynamicFields] = useState<DynamicFields>({});
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const handleDynamicFieldChange = (field: keyof DynamicFields, value: unknown) => {
    setDynamicFields((prev) => ({ ...prev, [field]: value }));
  };

  useEffect(() => {
    const loadCategories = async () => {
      try {
        const fetchedCategories = await fetchProductCategories();
        setCategories(fetchedCategories);
      } catch (error) {
        console.error("Failed to load categories:", error);
      } finally {
        setLoading(false);
      }
    };

    const loadWarehouses = async () => {
      try {
        const fetchedWarehouses = await fetchWarehouseNames();
        setWarehouses(fetchedWarehouses);
      } catch (error) {
        console.error("Failed to load warehouses:", error);
      } finally {
        setLoading(false);
      }
    };

    loadCategories();
    loadWarehouses();
  }, []);

  const handleTagKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter" && e.currentTarget.value.trim() !== "") {
      e.preventDefault();
      setTags([...tags, e.currentTarget.value.trim()]);
      e.currentTarget.value = "";
    }
  };

  const handleImageUrlKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter" && e.currentTarget.value.trim() !== "") {
      e.preventDefault();
      setImageUrls([...imageUrls, e.currentTarget.value.trim()]);
      e.currentTarget.value = "";
    }
  };

  const handleRemoveTag = (tagToRemove: string) => {
    setTags(tags.filter((tag) => tag !== tagToRemove));
  };

  const handleRemoveImageUrl = (urlToRemove: string) => {
    setImageUrls(imageUrls.filter((url) => url !== urlToRemove));
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    if (!name || !description || !categoryId || !price || !sku) {
      setErrorMessage("Please fill out all required fields.");
      return;
    }

    const newProduct: Partial<Product> = {
      name,
      description,
      categoryId,
      sku,
      price: Number(price),
      tags,
      imageUrls,
    };

    const inventoryFields: AddProductInventoryFields = {
      warehouseId,
    };

    const createProductRequestDto: CreateProductRequestDto = {
      createProductDto: newProduct,
      addProductInventoryFields: inventoryFields,
      dynamicFields,
    };

    try {
      await addProduct(createProductRequestDto);
      navigate("/vendor/products");
    } catch (error) {
      console.error("Error creating product:", error);
      setErrorMessage("Failed to create product. Please try again later.");
    }
  };

  if (loading) return <div>Loading categories...</div>;

  return (
    <Card
      elevation={3}
      sx={{
        borderRadius: "16px",
        padding: "24px",
        maxWidth: "90%",
        margin: "0 auto",
        mt: 4,
        backgroundColor: "#F5F5F5", // Light gray, almost white background
        boxShadow: "0 4px 6px rgba(0,0,0,0.1)", // Subtle shadow for depth
      }}
    >
      <CardContent>
        <Typography
          variant="h4"
          mb={4}
          sx={{
            fontWeight: 600,
            color: "#333", // Darker text for better readability
            textAlign: "center",
          }}
        >
          Create a New Product
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
              <Typography
                variant="h6"
                mb={3}
                sx={{
                  color: "#444",
                  borderBottom: "2px solid #666",
                  paddingBottom: "8px",
                }}
              >
                Basic Information
              </Typography>
              <Box display="flex" flexDirection="column" gap={3}>
                <TextField label="Name" value={name} required fullWidth variant="outlined" onChange={(e) => setName(e.target.value)} />
                <TextField
                  label="Description"
                  value={description}
                  required
                  fullWidth
                  multiline
                  rows={4}
                  variant="outlined"
                  onChange={(e) => setDescription(e.target.value)}
                />
                <FormControl required fullWidth>
                  <InputLabel id="category-label">Category</InputLabel>
                  <Select labelId="category-label" value={categoryId} onChange={(e) => setCategoryId(e.target.value)} label="Category">
                    {categories.map((cat) => (
                      <MenuItem key={cat.id} value={cat.id}>
                        {cat.name}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
                <TextField label="SKU" value={sku} required fullWidth variant="outlined" onChange={(e) => setSku(e.target.value)} />
                <TextField
                  label="Price"
                  type="number"
                  value={price}
                  required
                  fullWidth
                  variant="outlined"
                  onChange={(e) => setPrice(e.target.value === "" ? "" : Number(e.target.value))}
                />
              </Box>
            </Grid>

            {/* Second Column - Tags, Images, and Warehouse */}
            <Grid item xs={12} md={6}>
              <Typography
                variant="h6"
                mb={3}
                sx={{
                  color: "#444",
                  borderBottom: "2px solid #666",
                  paddingBottom: "8px",
                }}
              >
                Additional Details
              </Typography>
              <Box display="flex" flexDirection="column" gap={3}>
                <FormControl fullWidth>
                  <InputLabel htmlFor="tags-input">Tags (press Enter to add)</InputLabel>
                  <OutlinedInput
                    id="tags-input"
                    fullWidth
                    onKeyDown={handleTagKeyDown}
                    endAdornment={
                      <Box display="flex" flexWrap="wrap" gap={1} maxHeight="100px" overflow="auto">
                        {tags.map((tag) => (
                          <Chip key={tag} label={tag} onDelete={() => handleRemoveTag(tag)} size="small" />
                        ))}
                      </Box>
                    }
                    label="Tags (press Enter to add)"
                  />
                </FormControl>

                <FormControl fullWidth>
                  <InputLabel htmlFor="image-urls-input">Image URLs (press Enter to add)</InputLabel>
                  <OutlinedInput
                    id="image-urls-input"
                    fullWidth
                    onKeyDown={handleImageUrlKeyDown}
                    endAdornment={
                      <Box display="flex" flexWrap="wrap" gap={1} maxHeight="100px" overflow="auto">
                        {imageUrls.map((url) => (
                          <Chip key={url} label={url} onDelete={() => handleRemoveImageUrl(url)} size="small" />
                        ))}
                      </Box>
                    }
                    label="Image URLs (press Enter to add)"
                  />
                </FormControl>

                <FormControl required fullWidth>
                  <InputLabel id="warehouse-label">Warehouse</InputLabel>
                  <Select labelId="warehouse-label" value={warehouseId} onChange={(e) => setWarehouseId(e.target.value)} label="Warehouse">
                    {warehouses?.warehouses.map((warehouse) => (
                      <MenuItem key={warehouse.warehouseId} value={warehouse.warehouseId}>
                        {warehouse.name}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Box>
            </Grid>

            {/* Third Column - Product Essentials and Dynamic Fields */}
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
                  {/* Column 1 */}
                  <Grid item xs={12} md={6}>
                    <Box display="flex" flexDirection="column" gap={3}>
                      <TextField
                        label="Material"
                        value={dynamicFields.Material || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("Material", e.target.value)}
                      />
                      <TextField
                        label="Finish"
                        value={dynamicFields.Finish || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("Finish", e.target.value)}
                      />
                      <TextField
                        label="Length"
                        type="number"
                        value={dynamicFields.Length || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("Length", Number(e.target.value))}
                      />
                      <TextField
                        label="Width"
                        type="number"
                        value={dynamicFields.Width || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("Width", Number(e.target.value))}
                      />
                      <TextField
                        label="Height"
                        type="number"
                        value={dynamicFields.Height || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("Height", Number(e.target.value))}
                      />
                      <TextField
                        label="Color"
                        value={dynamicFields.Color || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("Color", e.target.value)}
                      />
                      <TextField
                        label="Usage"
                        value={dynamicFields.Usage || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("Usage", e.target.value)}
                      />
                    </Box>
                  </Grid>

                  {/* Column 2 */}
                  <Grid item xs={12} md={6}>
                    <Box display="flex" flexDirection="column" gap={3}>
                      <TextField
                        label="Features"
                        value={dynamicFields.Features || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("Features", e.target.value)}
                      />
                      <TextField
                        label="Warranty (Years)"
                        type="number"
                        value={dynamicFields.WarrantyInYears || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("WarrantyInYears", Number(e.target.value))}
                      />
                      <TextField
                        label="Weight (kg)"
                        type="number"
                        value={dynamicFields.Weight || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("Weight", Number(e.target.value))}
                      />
                      <TextField
                        label="Sub-Category"
                        value={dynamicFields.SubCategory || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("SubCategory", e.target.value)}
                      />
                      <FormControl fullWidth>
                        <InputLabel id="is-customizable-label">Customizable</InputLabel>
                        <Select
                          labelId="is-customizable-label"
                          value={dynamicFields.IsCustomizable ? "Yes" : "No"}
                          onChange={(e) => handleDynamicFieldChange("IsCustomizable", e.target.value === "Yes")}
                        >
                          <MenuItem value="Yes">Yes</MenuItem>
                          <MenuItem value="No">No</MenuItem>
                        </Select>
                      </FormControl>
                      <TextField
                        label="Maintenance Instructions"
                        multiline
                        rows={2}
                        value={dynamicFields.MaintenanceInstructions || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("MaintenanceInstructions", e.target.value)}
                      />
                      <TextField
                        label="Brand"
                        value={dynamicFields.Brand || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("Brand", e.target.value)}
                      />
                    </Box>
                  </Grid>

                  {/* Additional Fields */}
                  <Grid item xs={12} md={6}>
                    <Box display="flex" flexDirection="column" gap={3}>
                      <TextField
                        label="Manufacturer"
                        value={dynamicFields.Manufacturer || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("Manufacturer", e.target.value)}
                      />
                      <TextField
                        label="Manufacture Date"
                        type="date"
                        InputLabelProps={{ shrink: true }}
                        value={dynamicFields.ManufactureDate || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("ManufactureDate", e.target.value)}
                      />
                    </Box>
                  </Grid>
                  <Grid item xs={12} md={6}>
                    <Box display="flex" flexDirection="column" gap={3}>
                      <TextField
                        label="Country of Origin"
                        value={dynamicFields.CountryOfOrigin || ""}
                        fullWidth
                        onChange={(e) => handleDynamicFieldChange("CountryOfOrigin", e.target.value)}
                      />
                      <FormControl fullWidth>
                        <InputLabel id="eco-friendly-label">Eco-Friendly</InputLabel>
                        <Select
                          labelId="eco-friendly-label"
                          value={dynamicFields.IsEcoFriendly ? "Yes" : "No"}
                          onChange={(e) => handleDynamicFieldChange("IsEcoFriendly", e.target.value === "Yes")}
                        >
                          <MenuItem value="Yes">Yes</MenuItem>
                          <MenuItem value="No">No</MenuItem>
                        </Select>
                      </FormControl>
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
              startIcon={<AddIcon />}
              sx={{
                backgroundColor: "#2196f3", // Material blue
                color: "white",
                "&:hover": {
                  backgroundColor: "#1976d2", // Darker blue on hover
                },
                padding: "12px 24px",
                fontSize: "1rem",
                borderRadius: "8px",
                boxShadow: "0 2px 4px rgba(0,0,0,0.2)",
              }}
            >
              Create Product
            </Button>
          </Box>
        </Box>
      </CardContent>
    </Card>
  );
};

export default CreateProduct;
