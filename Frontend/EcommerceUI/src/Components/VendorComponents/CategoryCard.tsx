import React from "react";
import { Typography, Card, CardActionArea, CardMedia } from "@mui/material";
import { CategoryItem } from "../../Pages/Vendor/CategoryItem";

const CategoryCard: React.FC<{
  category: CategoryItem;
  onCategorySelect: (slug: string) => void;
}> = ({ category, onCategorySelect }) => {
  return (
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
      <CardActionArea
        onClick={() => onCategorySelect(category.slug)}
        sx={{
          height: "100%",
          position: "relative",
        }}
      >
        <CardMedia
          component="img"
          height="250"
          image={category.image}
          alt={category.name}
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
            {category.name}
          </Typography>
        </div>
      </CardActionArea>
    </Card>
  );
};

export default CategoryCard;
