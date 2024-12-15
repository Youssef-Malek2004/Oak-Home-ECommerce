import React, { useState, useRef, useEffect, useContext } from "react";
import { AppBar, Toolbar, IconButton, Badge, Box } from "@mui/material";
import NotificationsIcon from "@mui/icons-material/Notifications";
import Notifications from "../Shared/Notifications";
import { NotificationsContext } from "../../Pages/Layout";

interface VendorNavBarProps {
  collapsed?: boolean;
}

const VendorNavBar: React.FC<VendorNavBarProps> = ({ collapsed = false }) => {
  const [showNotifications, setShowNotifications] = useState<boolean>(false);
  const notificationsRef = useRef<HTMLDivElement | null>(null);
  const { unreadCount } = useContext(NotificationsContext);

  const toggleNotifications = () => {
    setShowNotifications((prev) => !prev);
  };

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (notificationsRef.current && !notificationsRef.current.contains(event.target as Node)) {
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
        backgroundColor: "transparent", // Remove background color
        boxShadow: "none",
        zIndex: 999, // Ensure it's behind the sidebar
        left: collapsed ? "5%" : "250px", // Adjust based on sidebar width
        width: collapsed ? "95%" : "calc(100% - 250px)", // Adjust width
        transition: "left 0.3s ease-in-out, width 0.3s ease-in-out",
        background: "linear-gradient(to bottom, #2C2316, #1A1409)", // Match sidebar gradient
      }}
    >
      <Toolbar
        sx={{
          display: "flex",
          justifyContent: "flex-end", // Align to the right
          alignItems: "center",
          padding: "0 1rem",
          minHeight: "64px", // Maintain consistent height
        }}
      >
        {/* Notifications */}
        <Box sx={{ display: "flex", gap: "15px", alignItems: "center" }}>
          <IconButton
            sx={{
              color: "#F5E6D3", // Soft cream color to match sidebar
              "&:hover": {
                backgroundColor: "rgba(245, 230, 211, 0.1)",
              },
            }}
            onClick={toggleNotifications}
          >
            <Badge badgeContent={unreadCount} color="error">
              <NotificationsIcon />
            </Badge>
          </IconButton>

          {showNotifications && (
            <Box
              ref={notificationsRef}
              sx={{
                position: "absolute",
                top: "110%",
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

export default VendorNavBar;
