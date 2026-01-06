import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import {
  Box,
  CircularProgress,
  Typography,
  Divider,
} from "@mui/material";
import { useTranslation } from "react-i18next";
import useStore from "../../../app/stores/store";
import { Order, OrderStatus } from "../../orders/types/orderTypes";
import OrderItem from "../../orders/components/OrderItem";
import ordersApi from "../../orders/api/orderApi";

const UserOrderPage = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(false);
  const { authStore, uiStore } = useStore();
  const { t } = useTranslation();

  const loadOrders = async () => {
    setLoading(true);
    try {
      const [myOrdersResult, placedOrdersResult] = await Promise.all([
        ordersApi.getMyOrders(),
        ordersApi.getMyPlacedOrders(),
      ]);

      const allOrders: Order[] = [];
      if (myOrdersResult.isSuccess) {
        allOrders.push(...myOrdersResult.value);
      }
      if (placedOrdersResult.isSuccess) {
        allOrders.push(...placedOrdersResult.value);
      }

      setOrders(allOrders);
    } catch (err) {
      console.log(err);
      uiStore.showSnackbar(t("common:ErrorOccurred"), "error");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadOrders();
  }, []);

  const handleMarkAsShipped = async (orderId: string) => {
    try {
      var result = await ordersApi.markAsShipped(orderId);
      if(result.isSuccess){
        uiStore.showSnackbar(t("user:MarkedAsShipped"), "success");
        setOrders(prev =>
          prev.map(o =>
            o.id === orderId ? { ...o, status: OrderStatus.Shipped } : o
          )
        );
      }
    } catch (err) {
      console.log(err);
      uiStore.showSnackbar(t("common:ErrorOccurred"), "error");
    }
  };

  const handleMarkAsDelivered = async (orderId: string) => {
    try {
      var result = await ordersApi.markAsDelivered(orderId);
      if(result.isSuccess){
        uiStore.showSnackbar(t("user:MarkedAsDelivered"), "success");
        setOrders(prev =>
          prev.map(o =>
            o.id === orderId ? { ...o, status: OrderStatus.Delivered } : o
          )
        );
      }
    } catch (err) {
      console.log(err);
      uiStore.showSnackbar(t("common:ErrorOccurred"), "error");
    }
  };

  if (loading) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight="400px"
      >
        <CircularProgress />
      </Box>
    );
  }

  const userId = authStore.user?.id;
  const sellerOrders = orders
    .filter((order) => order.seller.id === userId)
    .sort((a, b) => {
      const statusOrder = [
        OrderStatus.Paid,
        OrderStatus.Shipped,
        OrderStatus.Delivered,
        OrderStatus.Canceled,
      ];
      return statusOrder.indexOf(a.status) - statusOrder.indexOf(b.status);
    });

  const buyerOrders = orders
    .filter((order) => order.buyer.id === userId)
    .sort((a, b) => {
      const statusOrder = [
        OrderStatus.Shipped,
        OrderStatus.Paid,
        OrderStatus.Delivered,
        OrderStatus.Canceled,
      ];
      return statusOrder.indexOf(a.status) - statusOrder.indexOf(b.status);
    });

  if (orders.length === 0) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight="400px"
      >
        <Typography variant="h5" sx={{color: theme => theme.palette.primary.main }}>
          {t("orders:NoOrders")}
        </Typography>
      </Box>
    );
  }

  const renderOrderSection = (
    sectionOrders: Order[],
    titleKey: string,
    isSeller: boolean
  ) => {
    if (sectionOrders.length === 0) return null;

    return (
      <Box mb={4}>
        <Typography variant="h4" gutterBottom sx={{ mb: 2, color: (theme) => theme.palette.primary.main }}>
          {t(titleKey)}
        </Typography>
        {sectionOrders.map((order) => (
          <OrderItem
            key={order.id}
            order={order}
            onMarkAsShipped={isSeller ? handleMarkAsShipped : undefined}
            onMarkAsDelivered={!isSeller ? handleMarkAsDelivered : undefined}
          />
        ))}
      </Box>
    );
  };

  return (
    <Box>
      {renderOrderSection(sellerOrders, "user:MySales", true)}

      {sellerOrders.length > 0 && buyerOrders.length > 0 && (
        <Divider sx={{ my: 4 }} />
      )}

      {renderOrderSection(buyerOrders, "user:MyPurchases", false)}
    </Box>
  );
};

export default observer(UserOrderPage);