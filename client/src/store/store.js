import {createStore} from 'vuex';
import createPersistedState from 'vuex-persistedstate'

export default createStore({
    plugins: [createPersistedState({
        storage: window.sessionStorage,
    })],

    state: {
        payload: null
    },

    getters: {

    },

    mutations: {
        updatePayload(state, payload) {
            state.payload = payload;
        },

        deletePayload(state) {
            state.payload = null;
            sessionStorage.removeItem("accessToken");
        }
    },

    actions: {

    }
})