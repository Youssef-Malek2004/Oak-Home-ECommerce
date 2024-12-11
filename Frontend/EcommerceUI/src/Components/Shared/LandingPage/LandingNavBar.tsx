// Components/Navbar.tsx
import React, { useState, useRef, useEffect, useContext } from "react";
import {
  AppBar,
  Toolbar,
  Typography,
  IconButton,
  Badge,
  Box,
} from "@mui/material";
import NotificationsIcon from "@mui/icons-material/Notifications";
import SearchIcon from "@mui/icons-material/Search";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";
import { NotificationsContext } from "../../../Pages/Layout";
import Notifications from "../Notifications";

interface NavbarProps {
  navItems: string[];
  activeSection: string | null;
  setActiveSection: (section: string | null) => void;
}

const LandingNavBar: React.FC<NavbarProps> = ({
  navItems,
  activeSection,
  setActiveSection,
}) => {
  const { unreadCount } = useContext(NotificationsContext);
  const [showNotifications, setShowNotifications] = useState(false);
  const notificationsRef = useRef<HTMLDivElement | null>(null);

  const toggleNotifications = () => {
    setShowNotifications((prev) => !prev);
  };

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        notificationsRef.current &&
        !notificationsRef.current.contains(event.target as Node)
      ) {
        setShowNotifications(false);
      }
    };

    if (showNotifications) {
      document.addEventListener("mousedown", handleClickOutside);
    } else {
      document.removeEventListener("mousedown", handleClickOutside);
    }

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [showNotifications]);

  return (
    <AppBar
      position="fixed"
      sx={{
        background: "transparent",
        boxShadow: "none",
        transition: "background-color 0.3s ease",
      }}
    >
      <Toolbar
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          width: "90%",
          margin: "0 auto",
          padding: "15px 0",
        }}
      >
        {/* Logo */}
        <Typography
          variant="h6"
          sx={{
            fontWeight: "bold",
            fontSize: "1.5rem",
            color: "white",
          }}
        >
          Audo
        </Typography>

        {/* Navigation Items */}
        <Box sx={{ display: "flex", gap: "20px" }}>
          {navItems.map((item) => (
            <Typography
              key={item}
              variant="body1"
              sx={{
                cursor: "pointer",
                textTransform: "uppercase",
                fontSize: "0.875rem",
                color: "white",
                "&:hover": { color: "#aaaaaa" },
                fontWeight: activeSection === item ? "bold" : "normal",
              }}
              onMouseEnter={() => setActiveSection(item)}
              onMouseLeave={() => setActiveSection(null)}
            >
              {item}
            </Typography>
          ))}
        </Box>

        {/* Action Items */}
        <Box sx={{ display: "flex", gap: "15px", alignItems: "center" }}>
          <IconButton sx={{ color: "white" }}>
            <SearchIcon />
          </IconButton>
          <IconButton sx={{ color: "white" }} onClick={toggleNotifications}>
            <Badge badgeContent={unreadCount} color="error">
              <NotificationsIcon />
            </Badge>
          </IconButton>
          <IconButton sx={{ color: "white" }}>
            <ShoppingCartIcon />
          </IconButton>
          {showNotifications && (
            <Box
              ref={notificationsRef}
              sx={{
                position: "absolute",
                top: "100%",
                right: 0,
                backgroundColor: "white",
                boxShadow: "0 4px 8px rgba(0, 0, 0, 0.2)",
                borderRadius: "8px",
                zIndex: 10,
              }}
            >
              <Notifications />
            </Box>
          )}
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default LandingNavBar;
