import { RouteObject } from "react-router";
import PostDetails from "../pages/postDetails";

export const postRoutes: RouteObject[] = [
  {
    path: "/posts/:postId",
    element: <PostDetails />,
  },
];
