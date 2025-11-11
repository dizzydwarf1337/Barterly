import { Outlet } from "react-router";
import AdminNavBar from "../layout/adminNavBar";
import { AdminDrawer } from "../layout/adminDrawer";
import { Box } from "@mui/material";
import { observer } from "mobx-react-lite";
import useStore from "../stores/store";
import { useEffect } from "react";
import authApi from "../../features/auth/api/authApi";

export const AdminWrapper = observer(() => {
  const { authStore } = useStore();

  useEffect(() => {
    const fetchMe = async () => {
      try {
        if (authStore.isLoggedIn) {
          const result = await authApi.getMe();
          authStore.setUser(result.value);
        }
      } catch (error) {
        console.error("Error during UserWrapper initialization:", error);
      }
    };
    fetchMe();
  }, [authStore.token]);

  return (
    <Box
      display="flex"
      minHeight="100vh"
      sx={{ bgcolor: "background.default" }}
    >
      <Box sx={{ width: 240, flexShrink: 0 }}>
        <AdminDrawer />
      </Box>

      <Box
        sx={{
          flex: 1,
          display: "flex",
          flexDirection: "column",
          marginLeft: 0,
          width: "100%",
        }}
      >
        <AdminNavBar />
        <Box
          component="main"
          sx={{
            flex: 1,
            width: "100%",
            maxWidth: "100%",
          }}
        >
          <Outlet />
        </Box>
      </Box>
    </Box>
  );
});
