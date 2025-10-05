import { RouteObject } from "react-router";
import { adminUserRoutes } from "../../features/admin/users/routes/usersRoutes";
import { categoriesRoutes } from "../../features/admin/categories/routes/categoriesRoutes";
import { postRoutes } from "../../features/admin/posts/routes/postRoutes";

export const adminRoutes: RouteObject[] = [
  ...adminUserRoutes,
  ...categoriesRoutes,
  ...postRoutes,
];
