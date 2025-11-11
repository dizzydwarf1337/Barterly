import { RouteObject } from "react-router";
import { EmailConfirm } from "../pages/emailConfirm";
import { EmailConfirmLink } from "../pages/emailConfirmLink";
import LoginPage from "../pages/loginPage";

export const authRoutes: RouteObject[] = [
    {
        path:'login/resend-email-confirmation/:email',
        element: <EmailConfirm/>
    },
    {
        path:'confirm',
        element: <EmailConfirmLink/>
    },
    {
        path:'login',
        element: <LoginPage/>
    }

];
