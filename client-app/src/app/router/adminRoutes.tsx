import { RouteObject } from "react-router";
import { adminUserRoutes } from "../../features/admin/users/routes/usersRoutes";

export const adminRoutes: RouteObject[] = [
    ...adminUserRoutes
]