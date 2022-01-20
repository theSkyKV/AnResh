import {createStore} from 'vuex';

export default createStore({
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