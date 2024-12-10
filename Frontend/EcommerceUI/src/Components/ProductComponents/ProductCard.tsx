import {
  Card,
  CardMedia,
  CardContent,
  Typography,
  Button,
} from "@mui/material";

const ProductCard = () => {
  return (
    <Card sx={{ maxWidth: 345 }}>
      <CardMedia
        component="img"
        height="200"
        image="/path-to-product-image.jpg"
        alt="Product Name"
      />
      <CardContent>
        <Typography gutterBottom variant="h5" component="div">
          Frame: Clifton
        </Typography>
        <Button variant="outlined" fullWidth>
          View Details
        </Button>
      </CardContent>
    </Card>
  );
};

export default ProductCard;
