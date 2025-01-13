import axios from "axios";
import { ProductsWarehouses } from "../Interfaces/DTOs/Warehouse/ProductsGetWarehouses";
import { Warehouse } from "../Interfaces/Warehouse";
import { GateWayUrl } from "./GateWayApi";

// const INVENTORY_API_BASE_URL = `${GateWayUrl}inventory-api/api/inventory`;

const WAREHOUSE_API_BASE_URL = `${GateWayUrl}iinventory-api/api/warehouses`;

// const axiosInventoryInstance = axios.create({
//   baseURL: INVENTORY_API_BASE_URL,
//   withCredentials: true, // Ensures cookies are included
//   headers: {
//     "Content-Type": "application/json",
//   },
// });

const axiosWarehouseInstance = axios.create({
  baseURL: WAREHOUSE_API_BASE_URL,
  withCredentials: true, // Ensures cookies are included
  headers: {
    "Content-Type": "application/json",
  },
});

export const fetchWarehouseNames = async (): Promise<ProductsWarehouses> => {
  try {
    const response = await axiosWarehouseInstance.get<ProductsWarehouses>(`${WAREHOUSE_API_BASE_URL}/names`);
    return response.data;
  } catch (error) {
    console.error("Error fetching warehouses for products:", error);
    throw new Error("Unable to fetch warehouses. Please try again later.");
  }
};

export const fetchWarehouseForProductId = async (productId?: string): Promise<Warehouse> => {
  try {
    const response = await axiosWarehouseInstance.get<Warehouse>(`${WAREHOUSE_API_BASE_URL}/product-warehouse/${productId}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching warehouses for products:", error);
    throw new Error("Unable to fetch warehouses. Please try again later.");
  }
};
