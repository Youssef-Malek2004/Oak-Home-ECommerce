export interface Product {
  id: string;
  name: string;
  description: string;
  createdAt: string;
  updatedAt: string;
  categoryId: string;
  vendorId: string;
  imageUrls: string[];
  tags: string[];
  sku: string;
  price: number;
  isDeleted: boolean;
  featured: boolean;
}

export interface VendorGetProducts {
  product: Product;
  category: string;
}
