import { AllDataPage } from "../pages/AllDataPage"
import { HomePage } from "../pages/HomePage"
import { MainLayout } from "./Layout"

export interface ProjetLink {
    path: string,
    element: any,
    isVisible: boolean,
    label:string
}
export const projectComponents = [
    {
        label:"Home",
        path: "/",
        element: <MainLayout>
            <HomePage />
        </MainLayout>,
        isVisible: true,

    },
    {
        label:"All data",
        path: "/GetData",
        element: <MainLayout>
            <AllDataPage />
        </MainLayout>,
        isVisible: true,

    },
] as ProjetLink[]