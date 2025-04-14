import { createContext, useContext } from 'react'
import UserStore from './userStore';
import UiStore from './uiStore';
import CategoryStore from './categoryStore';


interface Store {
    userStore: UserStore,
    uiStore: UiStore,
    categoryStore:CategoryStore,
}
export const store: Store = {
    userStore: new UserStore(),
    uiStore: new UiStore(),
    categoryStore: new CategoryStore(),
}

export const StoreContext = createContext(store);

export default function useStore() {
    return useContext(StoreContext);
}
