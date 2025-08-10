import {createBrowserRouter, RouteObject} from "react-router";
import App from "../../App";
import { authRoutes } from "../../features/auth/routes/authRoutes";


const routes: RouteObject[] = [
    {
        path: '/',
        element: <App/>,
        children: [...authRoutes],
    },

];

export const router = createBrowserRouter(routes);
