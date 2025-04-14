import { Box, Typography } from "@mui/material";

export default function Footer() {
    return (
        <Box
            sx={{
                backgroundColor: "black",
                bottom: 0,  
                left: 0,
                width:"100%",
                position: "absolute",
            }}
        >
            <Typography variant="h6" color="white" textAlign="center">Aboba</Typography>
        </Box>
    );
}
