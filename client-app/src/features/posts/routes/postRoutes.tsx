import { RouteObject } from "react-router";
import PostDetails from "../pages/postDetails";
import { PostCreatePage } from "../pages/PostCreatePage";
import FavouritePostsPage from "../pages/FavouritePostPage";
import SearchPostsPage from "../pages/SearchPostsPage";

export const postRoutes: RouteObject[] = [
  {
    path: "/posts/:postId",
    element: <PostDetails />,
  },
  {
    path: "/posts/create",
    element: <PostCreatePage />,
  },
  {
    path: "/posts/favourite",
    element: <FavouritePostsPage />
  },
  {
    path:"/search",
    element: <SearchPostsPage/>
  }
];
