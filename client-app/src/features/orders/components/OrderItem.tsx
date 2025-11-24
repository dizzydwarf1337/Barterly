import { observer } from "mobx-react-lite";
import {
  Card,
  CardContent,
  Box,
  Typography,
  Avatar,
  Chip,
  Button,
} from "@mui/material";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router";
import { Order, OrderStatus } from "../../orders/types/orderTypes";
import { PostCurrency } from "../../posts/types/postTypes";
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';

interface OrderItemProps {
  order: Order;
  isSeller: boolean;
  onMarkAsShipped?: (orderId: string) => void;
  onMarkAsDelivered?: (orderId: string) => void;
}

const OrderItem = observer(
  ({ order, isSeller, onMarkAsShipped, onMarkAsDelivered }: OrderItemProps) => {
    const { t } = useTranslation();
    const navigate = useNavigate();

    const otherUser = isSeller ? order.buyer : order.seller;
    const statusColor = {
      [OrderStatus.Paid]: "warning",
      [OrderStatus.Shipped]: "info",
      [OrderStatus.Delivered]: "success",
      [OrderStatus.Canceled]: "error",
    }[order.status] as "warning" | "info" | "success" | "error";

    const showMarkAsShipped =
      isSeller && order.status === OrderStatus.Paid && onMarkAsShipped;
    const showMarkAsDelivered =
      !isSeller && order.status === OrderStatus.Shipped && onMarkAsDelivered;

    const handlePostClick = () => {
      navigate(`/posts/${order.post.id}`);
    };

    const handleUserClick = () => {
      navigate(`/users/${otherUser.id}`);
    };
    console.log(order.post.imagePath);
    return (
      <Card sx={{ mb: 2 }}>
        <CardContent>
          <Box display="flex" gap={2}>
            {/* Post Image */}
            <Avatar
              src={
                order.post.imagePath
                  ? `${import.meta.env.VITE_API_URL}/${order.post.imagePath}`
                  : undefined
              }
              variant="rounded"
              sx={{ 
                width: 80, 
                height: 80,
                cursor: "pointer",
                "&:hover": {
                  opacity: 0.8,
                }
              }}
              onClick={handlePostClick}
            >
              <ShoppingCartIcon/>
            </Avatar>

            {/* Order Details */}
            <Box flex={1}>
              <Typography 
                variant="h6" 
                gutterBottom
                sx={{ 
                  cursor: "pointer",
                  "&:hover": {
                    textDecoration: "underline",
                  }
                }}
                onClick={handlePostClick}
              >
                {order.post.name}
              </Typography>

              <Box 
                display="flex" 
                alignItems="center" 
                gap={1} 
                mb={1}
                sx={{ 
                  cursor: "pointer",
                  "&:hover .user-name": {
                    textDecoration: "underline",
                  }
                }}
                onClick={handleUserClick}
              >
                <Avatar
                  src={
                    otherUser.imagePath
                      ? `${import.meta.env.VITE_API_URL}/${otherUser.imagePath}`
                      : undefined
                  }
                  sx={{ width: 24, height: 24 }}
                >
                  {!otherUser.imagePath && otherUser.firstName[0]}
                </Avatar>
                <Typography variant="body2" color="text.secondary" className="user-name">
                  {isSeller ? t("orders:Buyer") : t("orders:Seller")}: {otherUser.firstName} {otherUser.lastName}
                </Typography>
              </Box>

              <Box display="flex" alignItems="center" gap={2} mb={1}>
                <Typography variant="body1" fontWeight="bold">
                  {order.post.price.toFixed(2)} {PostCurrency[order.post.currency]}
                </Typography>
                <Chip
                  label={t(`orders:Status${order.status}`)}
                  color={statusColor}
                  size="small"
                />
              </Box>

              <Typography variant="caption" color="text.secondary">
                {t("orders:CreatedAt")}: {new Date(order.createdAt).toLocaleDateString()}
              </Typography>

              {/* Action Buttons */}
              {showMarkAsShipped && (
                <Box mt={2}>
                  <Button
                    variant="contained"
                    color="primary"
                    size="small"
                    onClick={() => onMarkAsShipped(order.id)}
                  >
                    {t("orders:MarkAsShipped")}
                  </Button>
                </Box>
              )}

              {showMarkAsDelivered && (
                <Box mt={2}>
                  <Button
                    variant="contained"
                    color="primary"
                    size="small"
                    onClick={() => onMarkAsDelivered(order.id)}
                  >
                    {t("orders:MarkAsDelivered")}
                  </Button>
                </Box>
              )}
            </Box>
          </Box>
        </CardContent>
      </Card>
    );
  }
);

export default OrderItem;