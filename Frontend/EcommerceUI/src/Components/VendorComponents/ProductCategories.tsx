import React from "react";
import { useNavigate } from "react-router-dom";
import { Grid } from "@mui/material";
import { CategoryItem } from "../../Pages/Vendor/CategoryItem";
import CategoryCard from "../../Components/VendorComponents/CategoryCard";

import LivingRoomImage from "../../assets/LivingRoomVendor.webp";
import DiningRoomImage from "../../assets/DiningRoomVendor.webp";
import BedroomImage from "../../assets/BedroomVendor.webp";
import OfficeImage from "../../assets/OfficeVendor.webp";
import AccessoriesImage from "../../assets/AccesoriesVendor.webp";
import LightingImage from "../../assets/Lighting.webp";
import StorageImage from "../../assets/Storage.webp";

const PRODUCT_CATEGORIES: CategoryItem[] = [
  { name: "Living", image: LivingRoomImage, slug: "living" },
  { name: "Dining", image: DiningRoomImage, slug: "dining" },
  { name: "Bedroom", image: BedroomImage, slug: "bedroom" },
  { name: "Accessories", image: AccessoriesImage, slug: "accessories" },
  { name: "Office", image: OfficeImage, slug: "office" },
  { name: "Storage", image: StorageImage, slug: "storage" },
  { name: "Lighting", image: LightingImage, slug: "lighting" },
];

export const ProductCategories: React.FC = () => {
  const navigate = useNavigate();
  const handleCategorySelect = (slug: string) => {
    navigate(`/vendor/products/${slug}`);
  };

  return (
    <Grid container spacing={3}>
      {PRODUCT_CATEGORIES.map((category) => (
        <Grid item xs={12} sm={6} key={category.slug}>
          <CategoryCard
            category={category}
            onCategorySelect={handleCategorySelect}
          />
        </Grid>
      ))}
    </Grid>
  );
};
