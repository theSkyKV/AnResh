<template>
    <div>
        <h2>Авторизация</h2>
        <div>
            <div>
                <label>Логин</label>
                <div>
                    <input name="Login" type="email" v-model="login" />
                </div>
            </div>

            <div>
                <label>Пароль</label>
                <div>
                    <input name="Password" type="password" v-model="password" />
                </div>
            </div>

            <div>
                <button @click="submit" class="brand-btn btn">Войти</button>
            </div>
        </div>
    </div>
</template>

<script>
import * as axios from '@/custom_plugins/axiosApi.js';
import * as path from '@/config/path.js';

export default {
    data() {
        return {
            login: "",
            password: "",

            signInUrl: `${path.SERVER}${path.SIGN_IN}`,
        }
    },

    methods: {
        async submit() {
                await axios.post(this.signInUrl, { Login: this.login, Password: this.password })
                           .then((response) => {
                               sessionStorage.setItem("accessToken", response.data.accessToken);
                               this.$store.commit('updatePayload', response.data.payload);
                               this.$router.push(`/`);
                           })
                           .catch((error) => {
                               console.log(error);
                           });
        },
    },
}
</script>

<style scoped>

</style>