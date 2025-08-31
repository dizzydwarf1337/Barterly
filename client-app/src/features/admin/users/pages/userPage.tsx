import { useParams } from "react-router";
import UserProfile from "../components/UserProfile";
import UserSettingsForm from "../components/UserSettings";
import { useEffect, useState } from "react";
import { UserData, UserSettings } from "../types/userTypes";
import useStore from "../../../../app/stores/store";
import usersApi from "../api/usersApi";


export const UserPage = () => {
    const {uiStore} = useStore();
    const {id} = useParams<{id:string}>();
    const [userProfile,setUserProfile] = useState<UserData | null>(null);
    const [userSettings, setUserSettings] = useState<UserSettings | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    useEffect(()=> {
        const FetchUser = async () => {
            setIsLoading(true);
            try
            {
                const response = await usersApi.getUser({id:id as string});
                if(response.isSuccess){
                    setUserProfile(response.value.userData);
                    setUserSettings(response.value.userSettings);
                }
            }
            catch
            {
                uiStore.showSnackbar("Error loading user","error");
            }
            finally{
                setIsLoading(false);
            }
        }
        FetchUser();
    })
    return (
        <>
            <UserProfile userData={userProfile ?? {} as UserData}/>
            <UserSettingsForm userSettings={userSettings ?? {} as UserSettings} onSubmit={()=>{console.log("aboba")}}/>
        </>
    )
}