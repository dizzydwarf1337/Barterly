import { createBrowserRouter, RouteObject } from "react-router";
import App from "../layout/App";
import MainPage from "../../features/home/mainPage";
import RegisterMain from "../../features/registration/registerMain";
import EmailConfirm from "../../features/registration/EmailConfirm";
import EmailConfirmLink from "../../features/registration/emailConfirmLink";


const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            {
                index:true,
                element: <MainPage />
            },
            {
                path: '/sign-in',
                element:<RegisterMain/>
            },
            {
                path: `/email-confirm/:email`,
                element: <EmailConfirm />
            },
            {
                path: "/confirm/",
                element: <EmailConfirmLink />
            }
        ],
    },
    
];

export const router = createBrowserRouter(routes);
