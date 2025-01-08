import axios from "axios";

export interface RegisterPayload {
  username: string;
  email: string;
  passwordHash: string;
}

export interface LoginPayload {
  email: string;
  passwordHash: string;
}

export interface UserProfile {
  name: string;
  email: string;
}

const API_BASE_URL = "http://localhost:5175/users-api/api/users";
//Docker
// const API_BASE_URL = "http://localhost:5010/users-api/api/users";

// Create an Axios instance with default configurations
const axiosInstance = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true, // Ensures cookies are included
  headers: {
    "Content-Type": "application/json",
  },
});

// Login function
export const loginUser = async (payload: LoginPayload): Promise<void> => {
  try {
    const response = await axiosInstance.post("/login", payload);
    return response.data; // Return the response data
  } catch (error: any) {
    // Extract error response
    const errorResponse = error.response?.data;
    console.error("Login Error:", errorResponse);
    throw new Error(errorResponse?.description || "Login failed.");
  }
};

export const registerUser = async (payload: RegisterPayload): Promise<void> => {
  try {
    const response = await axiosInstance.post("/signup/user", payload);
    return response.data; // Return the response data
  } catch (error: any) {
    // Extract error response
    const errorResponse = error.response?.data;
    console.error("Login Error:", errorResponse);
    throw new Error(errorResponse?.description || "Login failed.");
  }
};

// Fetch user profile function
export const fetchUserProfile = async (): Promise<UserProfile> => {
  try {
    const response = await axiosInstance.get("/profile");
    return response.data; // Return the user profile
  } catch (error: any) {
    // Extract error response
    const errorResponse = error.response?.data;
    console.error("Fetch User Profile Error:", errorResponse);
    throw new Error(errorResponse?.description || "Failed to fetch user profile.");
  }
};
