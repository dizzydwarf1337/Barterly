import { createContext, useContext } from 'react'
import AdminCategoryStore from './adminCategoryStore';


interface Store {
    adminCategoryStore: AdminCategoryStore,
}
export const store: Store = {
    adminCategoryStore: new AdminCategoryStore(),
}

export const StoreContext = createContext(store);

export default function useAdminStore() {
    return useContext(StoreContext);
}
