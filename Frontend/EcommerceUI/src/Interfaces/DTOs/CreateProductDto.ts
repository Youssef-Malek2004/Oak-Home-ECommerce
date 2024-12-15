import { Product } from "../Product";

export interface CreateProductRequestDto {
  createProductDto: Partial<Product>;
  addProductInventoryFields: AddProductInventoryFields;
  dynamicFields: DynamicFields;
}

export interface AddProductInventoryFields {
  warehouseId: string;
}

export interface DynamicFields {
  Material?: string;
  Finish?: string;
  Length?: number;
  Width?: number;
  Height?: number;
  Weight?: number;
  Color?: string;
  SubCategory?: string;
  Usage?: string;
  IsCustomizable?: boolean;
  Features?: string;
  WarrantyInYears?: number;
  MaintenanceInstructions?: string;
  Brand?: string;
  Manufacturer?: string;
  ManufactureDate?: string; // ISO date format
  CountryOfOrigin?: string;
  IsEcoFriendly?: boolean;
}
