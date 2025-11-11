import { observer } from "mobx-react-lite";
import { OwnerData } from "../types/postTypes";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router";
import {
  alpha,
  Avatar,
  Box,
  Card,
  CardContent,
  Typography,
} from "@mui/material";
import PersonIcon from "@mui/icons-material/Person";

interface Props {
  owner: OwnerData;
}

export const PostOwner = observer(({ owner }: Props) => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  return (
    <Card sx={{ borderRadius: "16px" }}>
      <CardContent sx={{ p: 3 }}>
        <Typography
          variant="h6"
          gutterBottom
          sx={{ display: "flex", alignItems: "center", gap: 1 }}
        >
          <PersonIcon color="primary" />
          {t("adminPosts:postOwner")}
        </Typography>

        <Box
          onClick={() => navigate(`/admin/users/${owner.ownerId}`)}
          sx={{
            display: "flex",
            alignItems: "center",
            gap: 2,
            p: 2,
            borderRadius: "12px",
            border: "1px solid",
            borderColor: "divider",
            cursor: "pointer",
            transition: "all 0.2s ease",
            "&:hover": {
              borderColor: "primary.main",
              backgroundColor: alpha("#000", 0.02),
              transform: "translateY(-2px)",
            },
          }}
        >
          <Avatar sx={{ width: 48, height: 48, bgcolor: "primary.main" }}>
            {owner.firstName[0]}
            {owner.lastName[0]}
          </Avatar>

          <Box>
            <Typography variant="h6" fontWeight="bold">
              {owner.firstName} {owner.lastName}
            </Typography>
            <Typography variant="body2" color="text.secondary">
              {t("adminPosts:clickToViewProfile")}
            </Typography>
          </Box>
        </Box>
      </CardContent>
    </Card>
  );
});
