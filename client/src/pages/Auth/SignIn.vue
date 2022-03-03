<template>
    <div>
        <div v-if="ok">
            <label class="form-label">Логин</label>
            <div>
                <custom-input v-model="login" :class="{'is-invalid': v$.login.$errors.length > 0}" />
                <div class="text-danger my-3" v-for="error in v$.login.$errors" :key="error.$uid">{{ error.$message }}</div>
            </div>

            <label class="form-label">Пароль</label>
            <div>
                <custom-input v-model="password" type="password" :class="{'is-invalid': v$.password.$errors.length > 0}" />
                <div class="text-danger my-3" v-for="error in v$.password.$errors" :key="error.$uid">{{ error.$message }}</div>
            </div>

            <div class="text-danger my-3" v-for="error in v$.data.$errors" :key="error.$uid">{{ error.$message }}</div>
            
            <button type="button" class="brand-btn btn" @click="submit">Войти</button>
        </div>
        <div v-else>
            <div class="spinner-border text-dark"></div>
        </div>
    </div>
</template>

<script>
    import * as axios from '@/custom_plugins/axiosApi.js';
    import * as validation from '@/custom_plugins/validation.js';
    import CustomInput from '@/components/CustomInput.vue';
    import useVuelidate from '@vuelidate/core';
    import { required, email, helpers } from '@vuelidate/validators';
    import jwt_decode from "jwt-decode";
    
    export default {
        components: {
            CustomInput,
        },

        setup () {
            return { v$: useVuelidate() }
        },

        data() {
            return {
                ok: true,
                login: "",
                password: "",
                data: "",
                vuelidateExternalResults: {
                    data: []
                },

                signInUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_SIGN_IN}`,
            }
        },
        methods: {
            validate () {
                const errors = { data: [validation.INCORRECT_DATA] };
                Object.assign(this.vuelidateExternalResults, errors);
            },

            async submit() {
                this.v$.$error = null;
                this.v$.$validate();
                
                if (this.v$.$error)
  					return;

                await axios.post(this.signInUrl, { Login: this.login, Password: this.password })
                           .then((response) => {
                                sessionStorage.setItem("accessToken", response.data.accessToken);
                                this.getPayload(response.data.accessToken);
                                location.reload();
                           })
                           .catch((error) => {
                                console.log(error);
                                this.validate();
                           });
            },

            getPayload(token) {
                var payload = jwt_decode(token);
                this.$store.commit('updatePayload', payload);
            }
        },

        validations() {
            return {
                login: { required, email },
                password: { required, password: helpers.withMessage(validation.PASSWORD_MESSAGE, validation.password) },
                data: { data: validation.ok },
            }
        },
    }
</script>