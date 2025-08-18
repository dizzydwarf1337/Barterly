import { RouteObject } from "react-router";
import PostDetails from "../pages/postDetails";
import { PostCreatePage } from "../pages/PostCreatePage";

export const postRoutes: RouteObject[] = [
  {
    path: "/posts/:postId",
    element: <PostDetails />,
  },
  {
    path: "/posts/create",
    element: <PostCreatePage />,
  }
];
