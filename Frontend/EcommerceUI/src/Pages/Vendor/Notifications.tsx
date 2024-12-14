// Notifications.tsx
import React from "react";
import { Box, Typography, List, ListItem, ListItemText } from "@mui/material";

const Notifications: React.FC = () => {
  const notifications = [
    { id: 1, message: "New order received" },
    { id: 2, message: "Low stock alert for product X" },
    { id: 3, message: "Negative customer review for product Y" },
  ];

  return (
    <Box>
      <Typography variant="h6" sx={{ mb: 2 }}>
        Notifications
      </Typography>
      <List>
        {notifications.map((notification) => (
          <ListItem key={notification.id}>
            <ListItemText primary={notification.message} />
          </ListItem>
        ))}
      </List>
    </Box>
  );
};

export default Notifications;
