import Navbar from "../Components/Shared/NavBar";
import React, { useState, useEffect } from "react";
import { Box } from "@mui/material";
import { Outlet } from "react-router-dom";
import SocialMediaBar from "../Components/Shared/SocialMediaBar";
import * as signalR from "@microsoft/signalr";
import { Notification } from "../Components/Shared/Notifications";

export const NotificationsContext = React.createContext<{
  notifications: Notification[];
  unreadCount: number;
  connection: signalR.HubConnection | null;
  setNotifications: React.Dispatch<React.SetStateAction<Notification[]>>;
  setUnreadCount: React.Dispatch<React.SetStateAction<number>>;
}>({
  notifications: [],
  unreadCount: 0,
  connection: null,
  setNotifications: () => {},
  setUnreadCount: () => {},
});

const Layout: React.FC = () => {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5175/notifications-hub/chat-hub", {
        accessTokenFactory: () => {
          const authToken = document.cookie
            .split("; ")
            .find((row) => row.startsWith("auth_token="))
            ?.split("=")[1];
          return authToken || "";
        },
      })
      .withAutomaticReconnect()
      .build();

    newConnection.start().then(() => {
      console.log("SignalR Connected.");
      setConnection(newConnection);

      newConnection.on("ReceiveNotification", (notificationJson: string) => {
        const notification: Notification = JSON.parse(notificationJson);
        setNotifications((prev) => {
          const exists = prev.some((n) => n.Id === notification.Id);
          if (!exists) {
            return [notification, ...prev];
          }
          return prev;
        });

        setUnreadCount((prev) => {
          const exists = notifications.some((n) => n.Id === notification.Id);
          return exists ? prev : prev + 1;
        });
      });
    });

    return () => {
      newConnection.stop();
    };
  }, []);

  useEffect(() => {
    const count = notifications.filter((n) => !n.IsRead).length;
    setUnreadCount(count);
  }, [notifications]);

  return (
    <NotificationsContext.Provider
      value={{
        notifications,
        unreadCount,
        connection,
        setNotifications,
        setUnreadCount,
      }}
    >
      <Box
        sx={{
          minHeight: "100vh",
          minWidth: "100%",
          display: "flex",
          flexDirection: "column",
          alignContent: "center",
        }}
      >
        <Navbar />
        <Outlet />
        <SocialMediaBar />
      </Box>
    </NotificationsContext.Provider>
  );
};

export default Layout;
