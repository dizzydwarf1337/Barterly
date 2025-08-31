import { Box, Fade, Container } from "@mui/material";
import { observer } from "mobx-react-lite";
import { Outlet, useNavigate } from "react-router";
import Footer from "../layout/footer";
import NavBar from "../layout/navBar";
import useStore from "../stores/store";
import { useEffect } from "react";
import authApi from "../../features/auth/api/authApi";

export const UserWrapper = observer(() => {
  const { uiStore, authStore } = useStore();
  const navigate = useNavigate();
  useEffect(() => {
    const fetchMe = async () => {
      try {
        if (authStore.isLoggedIn) {
          const result = await authApi.getMe();
          authStore.setUser(result.value);
          if(authStore.user?.role === "Admin")
            navigate("/admin", {replace:true});
            if( authStore.user?.role === "Moderator") 
              navigate("/moderator", {replace:true})
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
      <NavBar />

      <Box
        component="main"
        flex="1"
        display="flex"
        flexDirection="column"
        sx={{
          pt: uiStore.isMobile ? "60px" : "80px",
          pb: "80px",
          minHeight: "calc(100vh - 140px)",
          position: "relative",
          width: "100%",
        }}
      >
        <Fade in timeout={300}>
          <Container
            maxWidth={false}
            sx={{
              flex: 0,
              display: "flex",
              flexDirection: "column",
              px: uiStore.isMobile ? 2 : 4,
              py: uiStore.isMobile ? 2 : 3,
              maxWidth: "1400px",
              mx: "auto",
              width: "100%",
            }}
          >
            <Outlet />
          </Container>
        </Fade>
      </Box>

      <Box
        component="footer"
        sx={{
          mt: "auto",
          position: "relative",
          width: "100%",
          zIndex: 1,
        }}
      >
        <Footer />
      </Box>
    </Box>
  );
});
