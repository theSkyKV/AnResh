<template>
  <div>
        <h2>Регистрация пользователя</h2>
        <div>
            <div class="brand-div">
                <label>Роль</label>
                <custom-select :modelValue="role" @changeOption="role = $event.target.value" :options="roles" />
            </div>

            <div>
                <label>Имя</label>
                <div>
                    <input name="Name" v-model="name" />
                </div>
            </div>
            <span v-if="v$.name.$error">
                    {{ v$.name.$errors[0].$message }}
            </span>

            <div>
                <label>Логин</label>
                <div>
                    <input name="Login" type="email" v-model="login" />
                </div>
            </div>
            <span v-if="v$.login.$error">
                    {{ v$.login.$errors[0].$message }}
            </span>

            <div>
                <label>Пароль</label>
                <div>
                    <input name="Password" type="password" v-model="password.password" />
                </div>
            </div>
            <span v-if="v$.password.password.$error">
                    {{ v$.password.password.$errors[0].$message }}
            </span>

            <div>
                <label>Подтверждение пароля</label>
                <div>
                    <input name="Password" type="password" v-model="password.confirmPassword" />
                </div>
            </div>
            <span v-if="v$.password.confirmPassword.$error">
                    {{ v$.password.confirmPassword.$errors[0].$message }}
            </span>

            <div>
                <button @click="submit" class="brand-btn btn">Зарегистрировать</button>
            </div>
        </div>
    </div>
</template>

<script>
import * as axios from '@/custom_plugins/axiosApi.js';
import * as path from '@/config/path.js';
import * as validate from '@/custom_plugins/validate.js';
import CustomSelect from '@/components/CustomSelect.vue';

import useValidate from "@vuelidate/core";
import { required, email, minLength, sameAs, helpers } from "@vuelidate/validators";

export default {
    components: {
        CustomSelect
    },

    data() {
        return {
            v$: useValidate(),

            name: "",
            role: "",
            login: "",
            password: {
                password: "",
                confirmPassword: ""
            },

            roles: [
                { value: 'User', name: 'Пользователь' },
                { value: 'Admin', name: 'Администратор' },
            ],

            signUpUrl: `${path.SERVER}${path.SIGN_UP}`,
        }
    },

    methods: {
        async submit() {
                this.v$.$validate();
                
                if (this.v$.$error) {
  					return;
  				}

                await axios.post(this.signUpUrl, { Name: this.name, Role: this.role, Login: this.login, Password: this.password.password })
                           .then(() => {
                               location.reload();
                           })
                           .catch((error) => {
                               console.log(error);
                           });
        },
    },

    validations() {
        return {
            name: { required },
            login: { required, email },
            password: {
                password: { required, minLength: minLength(6), noSpaces: helpers.withMessage(validate.NO_SPACES_MESSAGE, validate.noSpaces) },
                confirmPassword: { required, sameAs: sameAs(this.password.password) }
            },
        }
    },

    beforeMount() {
        this.role = this.roles[0].value;
    }
}
</script>

<style scoped>

</style>