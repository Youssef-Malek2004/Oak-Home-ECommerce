import React from "react";
import { Outlet } from "react-router-dom";
import Sidebar from "./Sidebar";

const VendorDashboard: React.FC = () => {
  return (
    <div style={{ display: "flex", width: "100vw", height: "100vh" }}>
      <Sidebar />
      <main
        style={{
          flex: 1,
          padding: "1rem",
          overflowY: "auto",
          marginLeft: "160px", // Match collapsed sidebar width
          width: "calc(100% - 80px)", // Adjust width to prevent overflow
        }}
      >
        <Outlet />
      </main>
    </div>
  );
};

export default VendorDashboard;
