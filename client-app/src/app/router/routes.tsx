import { createBrowserRouter, RouteObject } from "react-router";
import App from "../../App";
import HomePage from "../../pages/homePage";
import { authRoutes } from "../../features/auth/routes/authRoutes";
import { postRoutes } from "../../features/posts/routes/postRoutes";

const routes: RouteObject[] = [
    {
        path: '/',
        element: <App/>,
        children: [
            {
                index: true, 
                element: <HomePage/>,
            },
            ...authRoutes,
            ...postRoutes
        ],
    },
];

export const router = createBrowserRouter(routes);