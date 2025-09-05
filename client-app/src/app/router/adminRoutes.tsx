import { RouteObject } from "react-router";
import { adminUserRoutes } from "../../features/admin/users/routes/usersRoutes";
import { categoriesRoutes } from "../../features/admin/categories/routes/categoriesRoutes";

export const adminRoutes: RouteObject[] = [
    ...adminUserRoutes,
    ...categoriesRoutes,
]