import { RouteObject } from "react-router";
import { PostsPage } from "../pages/PostsPage";
import { PostPage } from "../pages/PostPage";

export const postRoutes: RouteObject[] = [
  {
    path: "/admin/posts",
    element: <PostsPage />,
  },
  {
    path: "/admin/posts/:id",
    element: <PostPage />,
  },
];
