import { Box, Fade, Container } from "@mui/material";
import { observer } from "mobx-react-lite";
import { Outlet } from "react-router";
import useStore from "../stores/store";
import { useEffect } from "react";
import authApi from "../../features/auth/api/authApi";
import AdminNavBar from "../layout/adminNavBar";
import { AdminDrawer } from "../layout/adminDrawer";

export const AdminWrapper = observer(() => {
  const { uiStore, authStore } = useStore();
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
      flexDirection="column"
      minHeight="100vh"
      sx={{
        position: "relative",
        overflow: "hidden",
      }}
    >
      <AdminNavBar />

      <Box
        component="main"
        display="flex"
        flexDirection="row"
        sx={{
          minHeight: "calc(100vh - 140px)",
        }}
      >
      <Box flex={1}>
       <AdminDrawer/>
      </Box>
        <Fade in timeout={300}>
            
          <Container
            maxWidth={false}
            sx={{
              flex: 1,
              display: "flex",
              flexDirection: "column",
              px: 2,
              py: 2,
              mx: "auto",
              width: "100%",
              maxWidth: 1200,
            }}
          >

            <Outlet />
          </Container>
        </Fade>
      </Box>
    </Box>
  );
});
