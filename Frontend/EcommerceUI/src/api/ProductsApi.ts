import axios from "axios";
import { Category } from "../Interfaces/Category";
import { Product } from "../Interfaces/Product";
import { CreateProductRequestDto } from "../Interfaces/DTOs/CreateProductDto";
import { GetProductByIdProductDto } from "../Interfaces/DTOs/Products/GetProductByIdDto";
import { UpdateProductDto } from "../Interfaces/DTOs/Products/UpdateProductDto";
import { GateWayUrl } from "./GateWayApi";

const PRODUCTS_API_BASE_URL = `${GateWayUrl}iproducts-api/api/products-async`;

const CATEGORIES_API_BASE_URL = `${GateWayUrl}products-api/api/categories`;

const axiosProductsInstance = axios.create({
  baseURL: PRODUCTS_API_BASE_URL,
  withCredentials: true, // Ensures cookies are included
  headers: {
    "Content-Type": "application/json",
  },
});

const axiosCategoriesInstance = axios.create({
  baseURL: CATEGORIES_API_BASE_URL,
  withCredentials: true, // Ensures cookies are included
  headers: {
    "Content-Type": "application/json",
  },
});

export const fetchProductCategories = async (): Promise<Category[]> => {
  try {
    const response = await axiosCategoriesInstance.get<Category[]>(`${CATEGORIES_API_BASE_URL}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching categories:", error);
    throw new Error("Unable to fetch categories. Please try again later.");
  }
};

export const addProduct = async (createProductRequest: CreateProductRequestDto): Promise<Product> => {
  try {
    const response = await axiosProductsInstance.post<Product>(`${PRODUCTS_API_BASE_URL}`, createProductRequest);
    return response.data;
  } catch (error) {
    console.error("Error adding product:", error);
    throw new Error("Unable to add product. Please try again later.");
  }
};

export const fetchProductById = async (productId: string): Promise<GetProductByIdProductDto> => {
  try {
    const response = await axiosProductsInstance.get<GetProductByIdProductDto>(`${PRODUCTS_API_BASE_URL}/${productId}`);
    return response.data;
  } catch (error) {
    console.error("Error adding product:", error);
    throw new Error("Unable to add product. Please try again later.");
  }
};

export const updateProduct = async (productId: string, updateProductDto: UpdateProductDto): Promise<UpdateProductDto> => {
  try {
    const response = await axiosProductsInstance.put<UpdateProductDto>(`${PRODUCTS_API_BASE_URL}/${productId}`, updateProductDto);
    console.log(updateProductDto.dynamicFields);
    return response.data;
  } catch (error) {
    console.error("Error adding product:", error);
    throw new Error("Unable to add product. Please try again later.");
  }
};
