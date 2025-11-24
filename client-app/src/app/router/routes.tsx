import { createBrowserRouter, RouteObject } from "react-router";
import App from "../../App";
import HomePage from "../../pages/homePage";
import { authRoutes } from "../../features/auth/routes/authRoutes";
import { postRoutes } from "../../features/posts/routes/postRoutes";
import { UserWrapper } from "../wrappers/userWrapper";
import { AdminWrapper } from "../wrappers/adminWrapper";
import { adminRoutes } from "./adminRoutes";
import { userRoutes } from "../../features/users/routes/userRoutes";

const routes: RouteObject[] = [
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "/",
        element: <UserWrapper />,
        children: [
          {
            index: true,
            element: <HomePage />,
          },
          ...postRoutes,
          ...authRoutes,
        ],
      },
      {
        path:"/admin",
        element:<AdminWrapper/>,
        children:[
          ...adminRoutes
        ]
      },
      {
        path:"/profile",
        element:<UserWrapper/>,
        children:[
          ...userRoutes
        ]
      }
    ],
  },
];

export const router = createBrowserRouter(routes);
