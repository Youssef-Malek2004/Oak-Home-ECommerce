import { createContext } from "react";
import { AuthContextProps } from "./Authentication";

export const AuthContext = createContext<AuthContextProps | undefined>(
  undefined
);
