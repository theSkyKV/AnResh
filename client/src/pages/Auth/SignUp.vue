<template>
    <div>
        <div v-if="ok">
            <div class="brand-div">
                <custom-select :modelValue="role" @changeOption="role = $event.target.value" :options="roles" :label="roleLabel" />
            </div>

            <div>
                <label class="form-label">Имя</label>
                <div>
                    <custom-input v-model="name" :class="{'is-invalid': v$.name.$errors.length > 0}" />
                    <div class="text-danger my-3" v-for="error in v$.name.$errors" :key="error.$uid">{{ error.$message }}</div>
                </div>
            </div>

            <div>
                <label class="form-label">Логин</label>
                <div>
                    <custom-input v-model="login" :class="{'is-invalid': v$.login.$errors.length > 0}" />
                    <div class="text-danger my-3" v-for="error in v$.login.$errors" :key="error.$uid">{{ error.$message }}</div>
                </div>
            </div>

            <div>
                <label class="form-label">Пароль</label>
                <div>
                    <custom-input v-model="password.password" type="password" :class="{'is-invalid': v$.password.password.$errors.length > 0}" />
                    <div class="text-danger my-3" v-for="error in v$.password.password.$errors" :key="error.$uid">{{ error.$message }}</div>
                </div>
            </div>

            <div>
                <label class="form-label">Подтверждение пароля</label>
                <div>
                    <custom-input v-model="password.confirmPassword" type="password" :class="{'is-invalid': v$.password.confirmPassword.$errors.length > 0}" />
                    <div class="text-danger my-3" v-for="error in v$.password.confirmPassword.$errors" :key="error.$uid">{{ error.$message }}</div>
                </div>
            </div>
            
            <button type="button" class="brand-btn btn" @click="submit">Зарегистрироваться</button>
        </div>
        <div v-else>
            <div class="spinner-border text-dark"></div>
        </div>
    </div>
</template>

<script>
    import * as axios from '@/custom_plugins/axiosApi.js';
    import * as validation from '@/custom_plugins/validation.js';
    import CustomSelect from '@/components/CustomSelect.vue';
    import CustomInput from '@/components/CustomInput.vue';
    import useValidate from "@vuelidate/core";
    import { required, email, minLength, sameAs, helpers } from "@vuelidate/validators";
    
    export default {
        components: {
            CustomSelect,
            CustomInput
        },
        
        data() {
            return {
                v$: useValidate(),
                ok: true,
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
                vuelidateExternalResults: {
                    login: []
                },
                roleLabel: "Роль",
                signUpUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_SIGN_UP}`,
            }
        },
        methods: {
            validate () {
                const errors = { login: [validation.EXISTS] };
                Object.assign(this.vuelidateExternalResults, errors);
            },

            async submit() {
                this.v$.$error = null;
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
                            this.validate();
                        });
            },
        },

        validations() {
            return {
                name: { required, name: helpers.withMessage(validation.NAME_MESSAGE, validation.name) },
                login: { required, email },
                password: {
                    password: { required, minLength: minLength(6), password: helpers.withMessage(validation.PASSWORD_MESSAGE, validation.password) },
                    confirmPassword: { required, sameAs: sameAs(this.password.password) }
                },
            }
        },

        beforeMount() {
            this.role = this.roles[0].value;
        }
    }  
</script>