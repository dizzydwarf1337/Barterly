import {createContext, useContext} from 'react'
import UiStore from './uiStore';
import authStore from '../../features/auth/store/authStore';
import { ChatHub } from '../signalR/Hub';
import MessageStore from '../../features/messages/store/messageStore';

interface Store {
    uiStore: UiStore,
    authStore: authStore,
    messageStore: MessageStore
}


const chatHub = new ChatHub();

export const store: Store = {
    uiStore: new UiStore(),
    authStore: new authStore(chatHub),
    messageStore: new MessageStore(chatHub)
}

export const StoreContext = createContext(store);

export default function useStore() {
    return useContext(StoreContext);
}
