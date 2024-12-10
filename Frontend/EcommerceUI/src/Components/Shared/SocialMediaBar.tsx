import { Box, IconButton } from "@mui/material";
import FacebookIcon from "@mui/icons-material/Facebook";
import InstagramIcon from "@mui/icons-material/Instagram";
import YouTubeIcon from "@mui/icons-material/YouTube";
import TwitterIcon from "@mui/icons-material/Twitter";

const SocialMediaBar = () => {
  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        padding: 2,
        width: "100%",
      }}
    >
      <IconButton color="primary" href="https://facebook.com">
        <FacebookIcon />
      </IconButton>
      <IconButton color="primary" href="https://instagram.com">
        <InstagramIcon />
      </IconButton>
      <IconButton color="primary" href="https://youtube.com">
        <YouTubeIcon />
      </IconButton>
      <IconButton color="primary" href="https://twitter.com">
        <TwitterIcon />
      </IconButton>
    </Box>
  );
};

export default SocialMediaBar;
