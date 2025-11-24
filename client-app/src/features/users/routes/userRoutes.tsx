import { RouteObject } from "react-router";
import UserPage from "../pages/UserPage";

export const userRoutes: RouteObject[] = [
    {
        path:'',
        element: <UserPage />
    }
]