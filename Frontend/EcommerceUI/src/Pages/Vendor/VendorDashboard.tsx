import React, { useState, useEffect } from "react";
import { Outlet } from "react-router-dom";
import Sidebar from "./Sidebar";
import VendorNavBar from "../../Components/VendorComponents/VendorNavBar";
import { Notification } from "../../Components/Shared/Notifications";
import * as signalR from "@microsoft/signalr";
import { NotificationsContext } from "../Layout";

const VendorDashboard: React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

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
      <div
        style={{
          display: "flex",
          width: "100vw",
          height: "100vh",
          overflow: "hidden",
        }}
      >
        <Sidebar collapsed={collapsed} onCollapseToggle={() => setCollapsed(!collapsed)} />

        <main
          style={{
            flex: 1,
            padding: "1rem",
            overflowY: "auto",
            marginLeft: collapsed ? "5%" : "250px",
            width: collapsed ? "calc(100% - 5%)" : "calc(100% - 250px)",
            transition: "margin-left 0.3s ease-in-out, width 0.3s ease-in-out",
          }}
        >
          <VendorNavBar collapsed={collapsed} />
          <div style={{ marginTop: "64px" }}>
            <Outlet />
          </div>
        </main>
      </div>
    </NotificationsContext.Provider>
  );
};

export default VendorDashboard;
