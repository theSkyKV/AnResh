<template>
  <div>
        <h2>Создать отдел</h2>
        <div>
            <div >
                <label>Название</label>
                <div>
                    <input name="Name" v-model="name" oninput="this.value = this.value.replace(/\s+/g, ' ')" />
                </div>
            </div>
            <span v-if="v$.name.$error" class="error-message">
                    {{ v$.name.$errors[0].$message }}
            </span>

            <div>
                <button @click="submit" class="brand-btn btn">Создать</button>
            </div>
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
            createDepartmentUrl: String,
        },

        data() {
            return {
                v$: useValidate(),

                name: ""
            }
        },

        methods: {
            async submit() {
                this.v$.$validate();
                
                if (this.v$.$error) {
  					return;
  				}
                
                await axios.post(this.createDepartmentUrl, { Name: this.name })
                           .then(() => {
                               location.reload();
                           })
                           .catch((error) => {
                               console.log(error);
                               this.$router.push(`/SignIn`);
                           });
            },

            async init() {
                await axios.get(this.createDepartmentUrl, null,
                                { 
                                    'Authorization': sessionStorage.getItem("accessToken")
                                }
                            )
                            .then((response) => {
                                console.log(response);
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