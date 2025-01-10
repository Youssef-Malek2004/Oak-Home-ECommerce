/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useState } from "react";
import { TextField, Button, Box, Typography, Checkbox, FormControlLabel, Link, Alert } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { registerUser } from "../../api/UserApi";
import image from "../../assets/SignUpPageImageGold.webp";

const SignUp: React.FC = () => {
  const [username, setName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [passwordHash, setPassword] = useState<string>("");
  const [confirmPassword, setConfirmPassword] = useState<string>("");
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSignUp = async () => {
    if (passwordHash !== confirmPassword) {
      setErrorMessage("Passwords do not match.");
      return;
    }

    try {
      await registerUser({ username, email, passwordHash });
      navigate("/login"); // Redirect to login page after successful registration
    } catch (error: any) {
      console.error("Error:", error.message);
      setErrorMessage(error.message); // Display the error message
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
      {/* Left Signup Form Section */}
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
            Create an Account
          </Typography>
          <Typography variant="body1" color="text.secondary" gutterBottom sx={{ marginBottom: "20px" }}>
            Sign up to start your journey with us.
          </Typography>

          {/* Display Error Message */}
          {errorMessage && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {errorMessage}
            </Alert>
          )}

          <TextField
            label="Name"
            type="text"
            value={username}
            onChange={(e) => setName(e.target.value)}
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
            onChange={(e) => setPassword(e.target.value)}
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
            label="Confirm Password"
            type="password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
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
            label="I agree to the terms and conditions"
            sx={{ marginTop: "10px", marginBottom: "20px" }}
          />
          <Button
            variant="contained"
            fullWidth
            size="large"
            onClick={handleSignUp}
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
            Sign Up
          </Button>
          <Typography variant="body2" color="text.secondary" sx={{ marginTop: "20px" }}>
            Already have an account?{" "}
            <Link
              href="/login"
              underline="hover"
              sx={{
                color: "#007bff",
                fontWeight: "bold",
                "&:hover": {
                  color: "#0056b3",
                },
              }}
            >
              Login
            </Link>
          </Typography>
        </Box>
      </Box>

      {/* Right Image Section */}
      <Box
        sx={{
          flex: 1,
          backgroundImage: `url(${image})`,
          backgroundSize: "cover",
          backgroundPosition: "center",
        }}
      />
    </Box>
  );
};

export default SignUp;
