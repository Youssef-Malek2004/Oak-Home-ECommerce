import React, { useContext, useState, useRef, useEffect } from "react";
import {
  AppBar,
  Toolbar,
  Typography,
  IconButton,
  Box,
  Button,
  Badge,
} from "@mui/material";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import SearchIcon from "@mui/icons-material/Search";
import NotificationsIcon from "@mui/icons-material/Notifications";
import Notifications from "./Notifications";
import { NotificationsContext } from "../../Pages/Layout";

const Navbar: React.FC = () => {
  const { unreadCount } = useContext(NotificationsContext);
  const [showNotifications, setShowNotifications] = useState(false);
  const notificationsRef = useRef<HTMLDivElement | null>(null);

  const toggleNotifications = () => {
    setShowNotifications((prev) => !prev);
  };

  // Close notifications when clicking outside
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
    <AppBar position="static" color="transparent" elevation={0}>
      <Toolbar
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          width: "97%",
        }}
      >
        {/* Left: Title */}
        <Typography variant="h6" component="div" sx={{ textAlign: "left" }}>
          EYEWEAR JUNKIE
        </Typography>

        {/* Center: Menu Items */}
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            flexGrow: 1,
            gap: 2,
          }}
        >
          <Button color="inherit">Shop</Button>
          <Button color="inherit">FAQ</Button>
          <Button color="inherit">Blog</Button>
          <Button color="inherit">Locations</Button>
        </Box>

        {/* Right: Icons */}
        <Box
          sx={{
            display: "flex",
            justifyContent: "flex-end",
            gap: 1,
            position: "relative", // Required to position notifications
          }}
        >
          <IconButton color="inherit">
            <SearchIcon />
          </IconButton>
          <IconButton color="inherit" onClick={toggleNotifications}>
            <Badge badgeContent={unreadCount} color="error">
              <NotificationsIcon />
            </Badge>
          </IconButton>
          <IconButton color="inherit">
            <ShoppingCartIcon />
          </IconButton>
          <IconButton color="inherit">
            <AccountCircleIcon />
          </IconButton>

          {showNotifications && (
            <Box
              ref={notificationsRef}
              sx={{ position: "absolute", top: "150%", right: 0 }}
            >
              <Notifications />
            </Box>
          )}
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;
