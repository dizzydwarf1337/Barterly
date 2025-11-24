import { PostCurrency } from "../../posts/types/postTypes";

export enum OrderStatus {
  Paid = "paid",
  Shipped = "shipped",
  Delivered = "delivered",
  Canceled = "canceled",
}

export interface Order {
  id: string;
  seller: OrderUser;
  buyer: OrderUser;
  post: OrderPost;
  status: OrderStatus,
  createdAt: Date;
  updatedAt?: Date | null;
}

export interface OrderUser {
  id: string;
  firstName: string;
  lastName: string;
  imagePath?: string | null;
}

export interface OrderPost {
  id: string;
  price: number;
  currency: PostCurrency;
  name: string;
  imagePath?: string | null;
}
