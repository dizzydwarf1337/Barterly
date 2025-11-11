import {
  Drawer,
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
} from "@mui/material";
import { observer } from "mobx-react-lite";
import GroupIcon from "@mui/icons-material/Group";
import CategoryIcon from "@mui/icons-material/Category";
import MessageIcon from "@mui/icons-material/Message";
import ForumIcon from "@mui/icons-material/Forum";
import useStore from "../stores/store";
import { useNavigate, useLocation } from "react-router";
import { useTranslation } from "react-i18next";

const adminElements = [
  { name: "adminUsers", icon: <GroupIcon />, path: "/admin/users" },
  {
    name: "adminCategories",
    icon: <CategoryIcon />,
    path: "/admin/categories",
  },
  { name: "adminPosts", icon: <MessageIcon />, path: "/admin/posts" },
  { name: "adminChats", icon: <ForumIcon />, path: "/admin/chats" },
];

const moderatorElements = [
  { name: "Posts", icon: <MessageIcon />, path: "/moderator/posts" },
  { name: "Chats", icon: <ForumIcon />, path: "/moderator/chats" },
  { name: "Users", icon: <GroupIcon />, path: "/moderator/users" },
];

export const AdminDrawer = observer(() => {
  const { authStore } = useStore();
  const navigate = useNavigate();
  const location = useLocation();
  const { t } = useTranslation();

  const elements =
    authStore.user?.role == "Admin" ? adminElements : moderatorElements;

  return (
    <Drawer
      variant="persistent"
      anchor="left"
      open={true}
      PaperProps={{
        sx: {
          width: 240,
          height: "100vh",
          bgcolor: (theme) => theme.palette.background.paper,
        },
      }}
    >
      <List>
        {elements.map((el) => {
          const isActive = location.pathname === el.path;

          return (
            <ListItemButton
              key={el.name}
              onClick={() => navigate(el.path)}
              sx={{
                mb: 1,
                borderRadius: 1,
                bgcolor: isActive ? "primary.main" : "transparent",
                color: isActive ? "white" : "text.primary",
                transition: "0.2s",
                "&:hover": {
                  bgcolor: (theme) => theme.palette.background.default,
                  color: (theme) => theme.palette.primary.main,
                },
              }}
            >
              <ListItemIcon sx={{ color: isActive ? "white" : "inherit" }}>
                {el.icon}
              </ListItemIcon>
              <ListItemText primary={t(`adminUsers:${el.name}`)} />
            </ListItemButton>
          );
        })}
      </List>
    </Drawer>
  );
});
