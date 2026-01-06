import { Box, ThemeProvider } from "@mui/material";
import "./index.css";
import { useEffect } from "react";
import { Outlet } from "react-router";
import { observer } from "mobx-react-lite";
import useStore from "./app/stores/store";
import CustromSnackbar from "./app/layout/CustromSnackbar";
import apiClient from "./app/API/apiClient";

export default observer(function App() {
  const { uiStore, authStore } = useStore();
  const theme = uiStore.getTheme();

  useEffect(() => {
    document.body.style.backgroundColor = theme.palette.background.default;
  }, [theme]);

  useEffect(() => {
      apiClient.setOnUnauthorized(() => {
        authStore.logout();
    }); 
  }, [authStore]);

  return (
    <ThemeProvider theme={theme}>
      <Box
        display="flex"
        flexDirection="column"
        width="99vw"
        minHeight="99vh"
        sx={{
          position: "relative",
          overflow: "hidden",
        }}
      >
        <Outlet />

        <CustromSnackbar
          message={uiStore.snackbarMessage}
          severity={uiStore.snackbarSeverity}
          open={uiStore.snackbarOpen}
          side={uiStore.snackbarPosition}
          onClose={() => uiStore.closeSnackbar()}
        />
      </Box>
    </ThemeProvider>
  );
});
