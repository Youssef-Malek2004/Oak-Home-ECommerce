import React from "react";
import { Card, CardContent, Typography, Box, IconButton } from "@mui/material";
import {
  Info as InfoIcon,
  CheckCircle as SuccessIcon,
  Warning as WarningIcon,
  Error as ErrorIcon,
  Close as CloseIcon,
} from "@mui/icons-material";
import { Notification } from "./Notifications";

interface NotificationCardProps {
  notification: Notification;
  onClose: (id: string) => void;
}

const NotificationCard: React.FC<NotificationCardProps> = ({
  notification,
  onClose,
}) => {
  // Define styling for each notification type
  const getNotificationStyles = () => {
    switch (notification.Type.toLowerCase()) {
      case "info":
        return {
          color: "#2196f3", // Blue
          icon: <InfoIcon sx={{ color: "#2196f3" }} />,
          background: "linear-gradient(145deg, #e3f2fd 0%, #eaf6fb 100%)",
        };
      case "success":
        return {
          color: "#4caf50", // Green
          icon: <SuccessIcon sx={{ color: "#4caf50" }} />,
          background: "linear-gradient(145deg, #e8f5e9 0%, #f0f9f1 100%)",
        };
      case "warning":
        return {
          color: "#ff9800", // Orange
          icon: <WarningIcon sx={{ color: "#ffecb3" }} />,
          background: "linear-gradient(145deg, #fff8e1 0%, #fffcf2 100%)",
        };
      case "error":
        return {
          color: "#f44336", // Red
          icon: <ErrorIcon sx={{ color: "#f44336" }} />,
          background: "linear-gradient(145deg, #ffcdd2 0%, #ffe6e9 100%)",
        };
      default:
        return {
          color: "#333", // Default dark grey
          icon: null,
          background: "linear-gradient(145deg, #f5f5f5 0%, #ffffff 100%)",
        };
    }
  };

  const styles = getNotificationStyles();

  return (
    <Card
      sx={{
        marginBottom: 1.5,
        boxShadow: "0 4px 8px rgba(0, 0, 0, 0.08)",
        borderRadius: "10px",
        background: styles.background,
        position: "relative",
        overflow: "visible",
        transition: "transform 0.2s ease-in-out",
        "&:hover": {
          transform: "scale(1.02)",
        },
      }}
    >
      <IconButton
        size="small"
        sx={{
          position: "absolute",
          top: 6,
          right: 6,
          color: styles.color,
          opacity: 0.7,
          "&:hover": {
            opacity: 1,
            backgroundColor: "transparent",
          },
        }}
        onClick={() => onClose(notification.Id)}
      >
        <CloseIcon fontSize="small" />
      </IconButton>

      <CardContent
        sx={{
          display: "flex",
          alignItems: "center",
          padding: "8px 12px",
        }}
      >
        {styles.icon && (
          <Box
            sx={{
              marginRight: 1.5,
              display: "flex",
              alignItems: "center",
              flexShrink: 0,
            }}
          >
            {styles.icon}
          </Box>
        )}

        <Box sx={{ flex: 1 }}>
          <Typography
            variant="subtitle1"
            sx={{
              fontWeight: 500,
              fontFamily: "'Inter', 'Roboto', sans-serif",
              color: styles.color,
              lineHeight: 1.2,
              fontSize: "0.9rem",
            }}
          >
            {notification.Title}
          </Typography>

          <Typography
            variant="body2"
            sx={{
              color: "#555",
              fontFamily: "'Inter', 'Roboto', sans-serif",
              fontSize: "0.8rem",
              marginTop: "4px",
            }}
          >
            {notification.Message}
          </Typography>

          <Typography
            variant="caption"
            sx={{
              color: "#999",
              marginTop: "6px",
              fontSize: "0.7rem",
              display: "block",
              textAlign: "right",
            }}
          >
            {new Date(notification.CreatedAt).toLocaleTimeString()}
          </Typography>
        </Box>
      </CardContent>
    </Card>
  );
};

export default NotificationCard;
