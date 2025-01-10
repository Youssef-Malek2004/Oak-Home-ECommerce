/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useState } from "react";
import { TextField, Button, Box, Typography, Checkbox, FormControlLabel, Link, Alert } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { loginUser } from "../../api/UserApi";
import { useAuth } from "../Contexts/Authentication/useAuth";
import image from "../../assets/LoginPageImage.webp";

const Login: React.FC = () => {
  const [email, setEmail] = useState<string>("");
  const [passwordHash, setPasswordHash] = useState<string>("");
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const { login } = useAuth(); // Access the login function from AuthContext
  const navigate = useNavigate();

  const handleLogin = async () => {
    try {
      // Call the login API
      await loginUser({ email, passwordHash });

      // Update authentication state
      login();

      // Redirect to the home page
      navigate("/shop");
    } catch (error: any) {
      console.error("Error:", error.message);
      setErrorMessage("Invalid email or password. Please try again."); // Display a friendly error message
    }
  };

  return (
    <Box
      sx={{
        display: "flex",
        height: "100vh",
        width: "100vw",
        bgcolor: "#f7f7f7",
      }}
    >
      {/* Left Image Section */}
      <Box
        sx={{
          flex: 1,
          backgroundImage: `url(${image})`,
          backgroundSize: "cover",
          backgroundPosition: "center",
        }}
      />

      {/* Right Login Form Section */}
      <Box
        sx={{
          flex: 1,
          display: "flex",
          flexDirection: "column",
          justifyContent: "center",
          alignItems: "center",
          padding: 4,
          bgcolor: "white",
          boxShadow: "0px 4px 20px rgba(0, 0, 0, 0.1)",
          borderRadius: "8px",
        }}
      >
        <Box
          sx={{
            maxWidth: 400,
            width: "100%",
            textAlign: "center",
          }}
        >
          <Typography variant="h4" fontWeight="bold" gutterBottom sx={{ color: "#333" }}>
            Welcome Back!
          </Typography>
          <Typography variant="body1" color="text.secondary" gutterBottom sx={{ marginBottom: "20px" }}>
            Log in to access your account and explore all the benefits of our platform.
          </Typography>

          {/* Display Error Message */}
          {errorMessage && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {errorMessage}
            </Alert>
          )}

          <TextField
            label="Email"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            fullWidth
            margin="normal"
            variant="outlined"
            sx={{
              borderRadius: "8px",
              "& .MuiOutlinedInput-root": {
                borderRadius: "8px",
              },
            }}
          />
          <TextField
            label="Password"
            type="password"
            value={passwordHash}
            onChange={(e) => setPasswordHash(e.target.value)}
            fullWidth
            margin="normal"
            variant="outlined"
            sx={{
              borderRadius: "8px",
              "& .MuiOutlinedInput-root": {
                borderRadius: "8px",
              },
            }}
          />
          <FormControlLabel
            control={<Checkbox sx={{ color: "#333" }} />}
            label="Remember me"
            sx={{ marginTop: "10px", marginBottom: "20px" }}
          />
          <Link
            href="#"
            underline="hover"
            sx={{
              fontSize: "0.9rem",
              display: "block",
              textAlign: "right",
              marginBottom: "20px",
              color: "#007bff",
            }}
          >
            Forgot Password?
          </Link>
          <Button
            variant="contained"
            fullWidth
            size="large"
            onClick={handleLogin}
            sx={{
              backgroundColor: "#007bff",
              color: "#fff",
              padding: "10px",
              borderRadius: "8px",
              "&:hover": {
                backgroundColor: "#0056b3",
              },
            }}
          >
            Login
          </Button>
          <Typography variant="body2" color="text.secondary" sx={{ marginTop: "20px" }}>
            Don’t have an account?{" "}
            <Link
              href="/sign-up"
              underline="hover"
              sx={{
                color: "#007bff",
                fontWeight: "bold",
                "&:hover": {
                  color: "#0056b3",
                },
              }}
            >
              Register Now
            </Link>
          </Typography>
        </Box>
      </Box>
    </Box>
  );
};

export default Login;
