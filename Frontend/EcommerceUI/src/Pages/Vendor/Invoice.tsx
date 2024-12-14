import React from "react";
import { Box, Typography, Divider } from "@mui/material";

const Invoice: React.FC = () => {
  return (
    <Box>
      <Typography variant="body2" color="#718096">
        Invoice #INV-0512
      </Typography>
      <Typography variant="body2" color="#718096">
        Invoice Date: 10 Feb 2023
      </Typography>
      <Typography variant="body2" color="#718096">
        Due Date: 20 Feb 2023
      </Typography>
      <Divider sx={{ my: 2 }} />
      <Box>
        <Typography variant="body2" color="#2d3748">
          27" 2K UHD Monitor - $8,398.60
        </Typography>
        <Typography variant="body2" color="#2d3748">
          KB990 Keyboard - $1,448.55
        </Typography>
        <Typography variant="body2" color="#2d3748">
          Wireless Mouse MS1293Q - $573.95
        </Typography>
        <Divider sx={{ my: 2 }} />
        <Typography variant="body2" color="#2d3748">
          Subtotal (excl. GST): $10,421.10
        </Typography>
        <Typography variant="body2" color="#2d3748">
          GST: $0.00
        </Typography>
        <Typography
          variant="body1"
          sx={{
            fontWeight: "bold",
            color: "#2d3748",
          }}
        >
          Total: $10,421.10
        </Typography>
      </Box>
    </Box>
  );
};

export default Invoice;
