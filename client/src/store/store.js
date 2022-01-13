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
        }
    },

    actions: {

    }
})