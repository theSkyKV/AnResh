<template>
    <div>
        <div v-if="ok">
            <h2>Редактировать</h2>
            <div>
                <div>
                    <label>Название</label>
                    <div>
                        <input name="Name" v-model="name" oninput="this.value = this.value.replace(/\s+/g, ' ')" />
                    </div>
                </div>
                <span v-if="v$.name.$error" class="error-message">
                        {{ v$.name.$errors[0].$message }}
                </span>

                <input type="hidden" name="Id" :value="id" />

                <div>
                    <button @click="submit" class="brand-btn btn">Сохранить</button>
                </div>
            </div>
        </div>
        <div v-else>
            Загрузка...
        </div>
    </div>
</template>

<script>
    import * as axios from '@/custom_plugins/axiosApi.js';
    import * as validate from '@/custom_plugins/validate.js';

    import useValidate from "@vuelidate/core";
    import { required, helpers } from "@vuelidate/validators";

    export default {
        props: {
            id: Number,
            editDepartmentUrl: String,
        },

        data() {
            return {
                v$: useValidate(),

                department: null,
                ok: false,
                name: ""
            }
        },

        methods: {
            async submit() {
                this.v$.$validate();
                
                if (this.v$.$error) {
  					return;
  				}
                
                await axios.post(this.editDepartmentUrl, { Id: this.id, Name: this.name })
                           .then(() => {
                               location.reload();
                           })
                           .catch((error) => {
                               console.log(error);
                               this.$router.push(`/SignIn`);
                           });
            },

            async init() {
                await axios.get(this.editDepartmentUrl, { Id: this.id },
                                { 
                                    'Authorization': sessionStorage.getItem("accessToken")
                                }
                            )
                           .then((response) => {
                               this.department = response.data.department;
                               this.name = this.department.Name;
                               this.ok = true;
                           })
                           .catch((error) => {
                               console.log(error);
                               this.$router.push(`/SignIn`);
                           });
            }
        },

        validations() {
            return {
                name: { required, name: helpers.withMessage(validate.NAME_MESSAGE, validate.name) },
            }
        },

        beforeMount() {
            this.init();
        }
    }
</script>

<style scoped>
    
</style>