import {createBrowserRouter, RouteObject} from "react-router";
import App from "../layout/App";
import MainPage from "../../features/home/mainPage";
import RegisterMain from "../../features/registration/registerMain";
import EmailConfirmLink from "../../features/registration/emailConfirmLink";
import PostDetails from "../../features/posts/Details/postDetails";
import EmailConfirm from "../../features/registration/emailConfirm";


const routes: RouteObject[] = [
    {
        path: '/',
        element: <App/>,
        children: [
            {
                index: true,
                element: <MainPage/>
            },
            {
                path: '/sign-in',
                element: <RegisterMain/>
            },
            {
                path: `/email-confirm/:email`,
                element: <EmailConfirm/>
            },
            {
                path: "/confirm/",
                element: <EmailConfirmLink/>
            },
            {
                path: '/posts/:postId',
                element: <PostDetails/>
            }
        ],
    },

];

export const router = createBrowserRouter(routes);
