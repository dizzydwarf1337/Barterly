import { observer } from "mobx-react-lite";
import { PostOpinion } from "../types/postTypes";
import { useTranslation } from "react-i18next";
import {
  Avatar,
  Box,
  Card,
  CardContent,
  Chip,
  Divider,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  Rating,
  Typography,
} from "@mui/material";

import MessageIcon from "@mui/icons-material/Message";

interface Props {
  opinions: PostOpinion[];
}

export const PostOpinions = observer(({ opinions }: Props) => {
  const { t } = useTranslation();

  return (
    <Card sx={{ borderRadius: "16px" }}>
      <CardContent sx={{ p: 3 }}>
        <Typography
          variant="h6"
          gutterBottom
          sx={{ display: "flex", alignItems: "center", gap: 1 }}
        >
          <MessageIcon color="primary" />
          {t("adminPosts:opinions")} ({opinions.length})
        </Typography>

        {opinions.length === 0 ? (
          <Box
            sx={{
              textAlign: "center",
              py: 4,
              color: "text.secondary",
            }}
          >
            <MessageIcon sx={{ fontSize: 48, mb: 2, opacity: 0.5 }} />
            <Typography variant="body1">
              {t("adminPosts:noOpinionsYet")}
            </Typography>
          </Box>
        ) : (
          <List sx={{ mt: 2 }}>
            {opinions.map((opinion, index) => (
              <Box key={opinion.id}>
                <ListItem alignItems="flex-start" sx={{ px: 0 }}>
                  <ListItemAvatar>
                    <Avatar sx={{ bgcolor: "primary.main" }}>
                      {opinion.authorId[0]}
                    </Avatar>
                  </ListItemAvatar>

                  <ListItemText
                    primary={
                      <Box display="flex" alignItems="center" gap={2} mb={1}>
                        <Typography variant="subtitle2" fontWeight="bold">
                          User {opinion.authorId.slice(0, 8)}...
                        </Typography>

                        {opinion.rate && (
                          <Rating value={opinion.rate} readOnly size="small" />
                        )}

                        <Typography variant="caption" color="text.secondary">
                          {new Date(opinion.createdAt).toLocaleDateString()}
                        </Typography>

                        {opinion.isHidden === "true" && (
                          <Chip label="Hidden" size="small" color="warning" />
                        )}
                      </Box>
                    }
                    secondary={
                      <Typography variant="body2" sx={{ mt: 1 }}>
                        {opinion.content}
                      </Typography>
                    }
                  />
                </ListItem>

                {index < opinions.length - 1 && (
                  <Divider variant="inset" component="li" />
                )}
              </Box>
            ))}
          </List>
        )}
      </CardContent>
    </Card>
  );
});
