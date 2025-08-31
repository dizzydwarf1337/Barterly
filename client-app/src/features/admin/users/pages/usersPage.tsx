import { useEffect, useState } from "react"
import BasicTable from "../../../../app/components/basicTable"
import usersApi from "../api/usersApi"
import { useSorting } from "../../../../app/hooks/useSorting"
import { usePagination } from "../../../../app/hooks/usePagination"
import { useDebounce } from "../../../../app/hooks/useDebounce"
import useStore from "../../../../app/stores/store"
import { useTranslation } from "react-i18next"
import { UserPreview } from "../types/userTypes"
import { Box } from "@mui/material"
import { usersColumns } from "../components/table/userColumns"


export const UsersPage = () => {
    const {pagination}  = usePagination();
    const sorting = useSorting();
    const [search,setSearch] = useState("");
    const debouncedSearch = useDebounce(search);

    const {uiStore} = useStore();
    const {t} = useTranslation();

    const [users,setUsers] = useState<UserPreview[]>([]);
    const [isLoading,setIsLoading] = useState<boolean>(false); 

    const handleDelete = async (userId:string) => {
        try{
            await usersApi.deleteUser({id:userId});
            uiStore.showSnackbar("User has been deleted","success");
        }
        catch {
            uiStore.showSnackbar("Error deleting user","error");
        }
    } 

    useEffect(()=>{
        const FetchUsers = async () => {
            setIsLoading(true);
            try{
                const response = await usersApi.getUsers({
                    filterBy: {
                        search: debouncedSearch,
                        pageNumber: pagination.page.pageNumber,
                        pageSize: pagination.page.pageSize
                    },
                    sortBy: {
                        sortBy: sorting.sortBy.sortBy,
                        isDescending: sorting.isDescending.isDescending
                    }
                })
                pagination.total.setTotalCount(response.value.totalCount);
                pagination.total.setTotalPagesCount(response.value.totalPages);
                setUsers(response.value.items);
            }
            catch {
                uiStore.showSnackbar(t('errorLoadingUsers'),"error","right");
            }
            finally{
                setIsLoading(false);
            }
        }
        FetchUsers();
    },[debouncedSearch,pagination.page,sorting])

    return (
        <Box >
            <BasicTable
                data={users}
                loading={isLoading}
                columns={usersColumns(handleDelete)}
                {...pagination.page}
                />
        </Box>
    )
}