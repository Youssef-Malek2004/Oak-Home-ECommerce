import axios from "axios";
import { VendorGetProducts } from "../Interfaces/Product";

const PRODUCTS_API_BASE_URL = "http://localhost:5175/products-api/api/products-async";

const axiosProductsInstance = axios.create({
  baseURL: PRODUCTS_API_BASE_URL,
  withCredentials: true, // Ensures cookies are included
  headers: {
    "Content-Type": "application/json",
  },
});

export const fetchProductsByVendor = async (): Promise<VendorGetProducts[]> => {
  try {
    const response = await axiosProductsInstance.get<VendorGetProducts[]>(`${PRODUCTS_API_BASE_URL}/vendor`);
    return response.data;
  } catch (error) {
    console.error("Error fetching products:", error);
    throw new Error("Unable to fetch products. Please try again later.");
  }
};
