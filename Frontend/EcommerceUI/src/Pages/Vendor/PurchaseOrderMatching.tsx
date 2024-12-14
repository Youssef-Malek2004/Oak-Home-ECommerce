import React, { useState } from "react";
import { Box, Avatar, Typography, Collapse } from "@mui/material";
import { CheckCircle, Warning } from "@mui/icons-material";

const PurchaseOrderMatching: React.FC = () => {
  const [poDetailsOpen, setPoDetailsOpen] = useState(false);

  const handleToggle = () => setPoDetailsOpen(!poDetailsOpen);

  return (
    <Box>
      {/* Card 1 */}
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          mb: 2,
          p: 2,
          border: "1px solid #e2e8f0",
          borderRadius: "6px",
          backgroundColor: "#edf2f7",
          cursor: "pointer",
          "&:hover": {
            backgroundColor: "#e6fffa",
            transition: "background-color 0.3s ease",
          },
        }}
        onClick={handleToggle}
      >
        <Avatar
          sx={{
            width: 40,
            height: 40,
            mr: 2,
          }}
          alt="Carla Dokidis"
          src="/path/to/avatar.jpg"
        />
        <Box>
          <Typography
            variant="subtitle1"
            sx={{
              fontWeight: "bold",
              color: "#2d3748",
            }}
          >
            PO-6402{" "}
            <CheckCircle fontSize="small" sx={{ color: "#48bb78", ml: 1 }} />
          </Typography>
          <Typography variant="body2" color="#718096">
            Balance: $0.00/$10,222.45
          </Typography>
          <Typography variant="body2" color="#718096">
            Requester: Carla Dokidis
          </Typography>
          <Typography variant="body2" color="#718096">
            Date: 17/09/2022
          </Typography>
        </Box>
      </Box>
      <Collapse in={poDetailsOpen}>
        <Box
          sx={{
            p: 2,
            border: "1px solid #e2e8f0",
            borderRadius: "6px",
            backgroundColor: "#fff",
          }}
        >
          <Typography variant="body2" color="#718096">
            Additional details for PO-6402...
          </Typography>
        </Box>
      </Collapse>

      {/* Card 2 */}
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          p: 2,
          border: "1px solid #e2e8f0",
          borderRadius: "6px",
          backgroundColor: "#ffffff",
          cursor: "pointer",
        }}
      >
        <Avatar
          sx={{
            width: 40,
            height: 40,
            mr: 2,
          }}
          alt="Jaylon Bator"
          src="/path/to/avatar2.jpg"
        />
        <Box>
          <Typography
            variant="subtitle1"
            sx={{
              fontWeight: "bold",
              color: "#2d3748",
            }}
          >
            PO-6391{" "}
            <Warning fontSize="small" sx={{ color: "#ecc94b", ml: 1 }} />
          </Typography>
          <Typography variant="body2" color="#718096">
            Balance: $7,541.29/$10,000.00
          </Typography>
        </Box>
      </Box>
    </Box>
  );
};

export default PurchaseOrderMatching;
