import { Product } from "../../Product";
import { DynamicFields } from "../CreateProductDto";

export interface UpdateProductDto {
  updateProductDto: Partial<Product>;
  dynamicFields: DynamicFields;
}
