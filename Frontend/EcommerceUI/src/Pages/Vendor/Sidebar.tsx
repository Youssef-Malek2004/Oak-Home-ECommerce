import React, { useState } from "react";
import {
  Box,
  Typography,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Badge,
  IconButton,
  Divider,
  Tooltip,
} from "@mui/material";
import {
  Inventory as InventoryIcon,
  ShoppingCart as ShoppingCartIcon,
  Storefront as StorefrontIcon,
  EventNote as EventNoteIcon,
  MenuOpen as MenuOpenIcon,
  Home as HomeIcon,
} from "@mui/icons-material";
import { NavLink } from "react-router-dom";

const Sidebar: React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);
  const [hoveredItem, setHoveredItem] = useState<string | null>(null);

  const menuItems = [
    {
      text: "Home",
      icon: <HomeIcon />,
      badge: null,
      link: "/vendor",
      end: true,
    },
    {
      text: "Products",
      icon: <StorefrontIcon />,
      badge: null,
      link: "/vendor/products",
    },
    {
      text: "Inventory",
      icon: <InventoryIcon />,
      badge: null,
      link: "/vendor/inventory",
    },
    {
      text: "Sales",
      icon: <ShoppingCartIcon />,
      badge: null,
      link: "/vendor/sales",
    },
    {
      text: "Workshops",
      icon: <EventNoteIcon />,
      badge: 4,
      link: "/vendor/events",
    },
  ];

  return (
    <Box
      sx={{
        width: collapsed ? "5%" : "250px", // More precise collapsing
        height: "100vh",
        background: "linear-gradient(to bottom, #2C2316, #1A1409)", // Rich wood-tone gradient
        color: "#D4C1A3", // Warm, aged wood color
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        paddingTop: "1rem",
        boxShadow: "4px 0 15px rgba(0, 0, 0, 0.7)", // Deeper shadow
        transition: "width 0.3s ease-in-out", // Smoother transition
        position: "fixed", // Keep sidebar in place
        left: 0,
        top: 0,
        zIndex: 1000,
        overflow: "hidden",
      }}
    >
      {/* Toggle Button with Tooltip */}
      <Tooltip
        title={collapsed ? "Expand Sidebar" : "Collapse Sidebar"}
        placement="right"
      >
        <IconButton
          onClick={() => setCollapsed(!collapsed)}
          sx={{
            color: "#F5E6D3", // Soft cream color
            alignSelf: "flex-end",
            marginRight: "8px",
            "&:hover": {
              backgroundColor: "rgba(245, 230, 211, 0.1)", // Subtle hover effect
              transform: "scale(1.1)",
            },
            transition: "all 0.3s ease",
          }}
        >
          <MenuOpenIcon />
        </IconButton>
      </Tooltip>

      {/* Sidebar Header */}
      <Typography
        variant="h6"
        sx={{
          mb: 2,
          fontWeight: "bold",
          textAlign: "center",
          color: "#F5E6D3", // Soft cream color
          letterSpacing: "0.1em",
          display: collapsed ? "none" : "block",
          textShadow: "1px 1px 2px rgba(0,0,0,0.5)", // Subtle text depth
        }}
      >
        Oak & Home
      </Typography>

      {/* Sidebar Menu */}
      <List
        sx={{
          width: "90%",
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        {menuItems.map((item) => (
          <Tooltip
            key={item.text}
            title={item.text}
            placement="right"
            disableHoverListener={!collapsed}
          >
            <ListItem
              component={NavLink}
              to={item.link}
              end={item.end}
              onMouseEnter={() => setHoveredItem(item.text)}
              onMouseLeave={() => setHoveredItem(null)}
              sx={{
                color: "#D4C1A3", // Warm wood tone
                marginY: "8px",
                maxWidth: "90%",
                padding: collapsed ? "8px" : "8px 16px",
                borderRadius: "12px", // More rounded corners
                cursor: "pointer",
                position: "relative",
                overflow: "hidden",
                "&.active": {
                  color: "#F5E6D3", // Brighter on active
                  background: "rgba(82, 67, 45, 0.7)", // Deep wood brown
                  boxShadow: "0 4px 6px rgba(0,0,0,0.2)", // Subtle depth
                },
                "&:hover": {
                  background: "rgba(82, 67, 45, 0.5)", // Warm brown overlay
                  color: "#F5E6D3",
                  transform: collapsed ? "scale(1.1)" : "scale(1.02)",
                  boxShadow: "0 4px 8px rgba(0, 0, 0, 0.3)",
                },
                transition: "all 0.3s ease-in-out",
                ...(hoveredItem === item.text && {
                  "&::before": {
                    content: '""',
                    position: "absolute",
                    top: 0,
                    left: 0,
                    width: "100%",
                    height: "100%",
                    background:
                      "linear-gradient(to right, rgba(245, 230, 211, 0.1), transparent)",
                    zIndex: 1,
                    pointerEvents: "none",
                  },
                }),
              }}
            >
              <ListItemIcon
                sx={{
                  color: "inherit",
                  minWidth: collapsed ? "24px" : "36px",
                  justifyContent: "center",
                }}
              >
                {item.icon}
              </ListItemIcon>
              {!collapsed && <ListItemText primary={item.text} />}
              {item.badge && (
                <Badge
                  badgeContent={item.badge}
                  color="error"
                  sx={{
                    "& .MuiBadge-badge": {
                      fontSize: "0.7rem",
                      backgroundColor: "#8B4513", // Saddle Brown
                      color: "#F5E6D3",
                      boxShadow: "0 2px 4px rgba(0,0,0,0.3)",
                    },
                  }}
                />
              )}
            </ListItem>
          </Tooltip>
        ))}
      </List>

      {/* Divider */}
      <Divider
        sx={{
          width: "90%",
          marginY: "16px",
          background: "rgba(212, 193, 163, 0.2)", // Translucent wood tone
          height: "2px",
        }}
      />
    </Box>
  );
};

export default Sidebar;
