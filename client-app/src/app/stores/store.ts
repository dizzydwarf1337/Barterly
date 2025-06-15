import { createContext, useContext } from 'react'
import UserStore from './userStore';
import UiStore from './uiStore';
import CategoryStore from './categoryStore';
import PostStore from './postStore';


interface Store {
    userStore: UserStore,
    uiStore: UiStore,
    categoryStore: CategoryStore,
    postStore: PostStore,
}
export const store: Store = {
    userStore: new UserStore(),
    uiStore: new UiStore(),
    categoryStore: new CategoryStore(),
    postStore: new PostStore(),
}

export const StoreContext = createContext(store);

export default function useStore() {
    return useContext(StoreContext);
}
