import { RouteObject } from "react-router";
import { CategoriesPage } from "../pages/CategoriesPage";
import { CategoryPage } from "../pages/CategoryPage";


export const categoriesRoutes: RouteObject[] = [
    {
        path: "/admin/categories",
        element: <CategoriesPage />,
    },
    {
        path:"/admin/categories/:id",
        element:<CategoryPage/>
    }

]