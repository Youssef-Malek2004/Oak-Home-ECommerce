import React, { useContext } from "react";
import { Box, Typography, Paper, Fade } from "@mui/material";
import { NotificationsContext } from "../../Pages/Layout";
import NotificationCard from "./NotificationCard";

export interface Notification {
  Id: string;
  Title: string;
  Message: string;
  Type: string;
  UserId?: string | null;
  Group: string;
  Channel: string;
  CreatedAt: string;
  SentAt?: string | null;
  IsRead: boolean;
  IsDelivered: boolean;
}

const Notifications: React.FC = () => {
  const { notifications, unreadCount, connection, setNotifications } =
    useContext(NotificationsContext);

  const handleMarkAsRead = async (id: string) => {
    try {
      if (!connection) {
        console.error("SignalR connection not established.");
        return;
      }

      await connection.invoke("MarkNotificationAsRead", id);

      setNotifications((prev) =>
        prev.filter((notification) => notification.Id !== id)
      );
    } catch (error) {
      console.error("Error marking notification as read:", error);
    }
  };

  return (
    <Paper
      elevation={4}
      sx={{
        position: "absolute",
        top: "125%",
        right: "1%",
        width: "350px",
        maxHeight: "500px",
        overflowY: "auto",
        display: "flex",
        flexDirection: "column",
        alignContent: "center",
        backgroundColor: "rgba(255, 255, 255, 0.4)",
        backdropFilter: "blur(10px)",
        border: "1px solid rgba(255, 255, 255, 0.3)",
        borderRadius: "16px",
        zIndex: 1000,
        padding: 2,
        transition: "all 0.3s ease",
        "&:hover": {
          backgroundColor: "rgba(255, 255, 255, 0.3)",
          boxShadow: "0 8px 16px rgba(0, 0, 0, 0.2)",
        },
        scrollbarWidth: "thin",
        scrollbarColor: "rgba(0,0,0,0.2) transparent",
        "&::-webkit-scrollbar": {
          width: "8px",
        },
        "&::-webkit-scrollbar-thumb": {
          backgroundColor: "rgba(0,0,0,0.2)",
          borderRadius: "10px",
        },
      }}
    >
      <Typography
        variant="h6"
        sx={{
          marginBottom: 2,
          color: "#222",
          fontFamily: "'Roboto', sans-serif",
          textAlign: "center",
          borderBottom: "1px solid rgba(0,0,0,0.1)",
          paddingBottom: 1,
        }}
      >
        Notifications ({unreadCount})
      </Typography>

      {notifications.length === 0 ? (
        <Fade in={true}>
          <Typography
            variant="body2"
            sx={{
              textAlign: "center",
              marginTop: 2,
              fontFamily: "'Roboto', sans-serif",
              color: "#555",
              fontStyle: "italic",
            }}
          >
            No new notifications
          </Typography>
        </Fade>
      ) : (
        <Box
          sx={{
            maxHeight: "400px",
            maxWidth: "400px",
            display: "flex",
            flexDirection: "column",
            alignContent: "center",
            overflowY: "auto",
            paddingRight: 0.5,
          }}
        >
          {notifications.map((notification) => (
            <Fade key={notification.Id} in={true} timeout={500}>
              <div>
                <NotificationCard
                  key={notification.Id}
                  notification={notification}
                  onClose={handleMarkAsRead}
                />
              </div>
            </Fade>
          ))}
        </Box>
      )}
    </Paper>
  );
};

export default Notifications;
