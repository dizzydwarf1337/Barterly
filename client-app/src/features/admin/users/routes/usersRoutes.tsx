import { RouteObject } from "react-router";
import { UsersPage } from "../pages/usersPage";
import { UserPage } from "../pages/userPage";


export const adminUserRoutes: RouteObject[] = [
    {
        path:"users",
        element: <UsersPage/>
    },
    {
        path:"users/:id",
        element: <UserPage/>
    }
]