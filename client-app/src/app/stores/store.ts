import {createContext, useContext} from 'react'
import UiStore from './uiStore';
import authStore from '../../features/auth/store/authStore';


interface Store {
    uiStore: UiStore,
    authStore: authStore,
}

export const store: Store = {
    uiStore: new UiStore(),
    authStore: new authStore(),
}

export const StoreContext = createContext(store);

export default function useStore() {
    return useContext(StoreContext);
}
