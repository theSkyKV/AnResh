<template>
    <div>
        <div v-if="ok">
            <h2>Создать сотрудника</h2>
            <div>
                <div>
                    <label>Имя</label>
                    <div>
                        <input name="Name" v-model="name" oninput="this.value = this.value.replace(/\s+/g, ' ')" />
                    </div>
                </div>
                <span v-if="v$.name.$error" class="error-message">
                        {{ v$.name.$errors[0].$message }}
                </span>

                <div>
                    <label>Отдел</label>
                    <select name="DepartmentId" @change="departmentId = $event.target.value">
                        <option v-for="department in departments" :key="department.Id" :value="department.Id">
                            {{ department.Name }}
                        </option>
                    </select>
                </div>

                <div>
                    <label>Зарплата</label>
                    <div>
                        <input name="Salary" type="number" v-model="salary" oninput="this.value = this.value.replace(/\s+/g, ' ')" />
                    </div>
                </div>
                <span v-if="v$.salary.$error" class="error-message">
                        {{ v$.salary.$errors[0].$message }}
                </span>

                <div>
                    <button @click="submit" class="brand-btn btn">Создать</button>
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
    import { required, numeric, helpers } from "@vuelidate/validators";

    export default {
        props: {
            createEmployeeUrl: String,
            getAllDepartmentsUrl: String,
        },

        data() {
            return {
                v$: useValidate(),

                departments: [],
                ok: false,
                name: "",
                departmentId: 0,
                salary: 0,

                intMaxValue: Math.pow(2, 31) - 1,
                pageNumber: 1,
            }
        },

        methods: {
            async submit() {
                this.v$.$validate();
                
                if (this.v$.$error) {
  					return;
  				}
                
                await axios.post(this.createEmployeeUrl, { Name: this.name, DepartmentId: this.departmentId, Salary: this.salary })
                           .then(() => {
                               location.reload();
                           })
                           .catch((error) => {
                               console.log(error);
                           });
            },

            async init() {
                await axios.get(this.getAllDepartmentsUrl, { PageNumber: this.pageNumber, Limit: this.intMaxValue })
                            .then((response) => {
                                this.departments = response.data.departments;
                                this.departmentId = this.departments[0].Id;
                                this.ok = true;
                            })
                            .catch((error) => {
                                console.log(error);
                            });
            }
        },

        validations() {
            return {
                name: { required, name: helpers.withMessage(validate.NAME_MESSAGE, validate.name) },
                salary: { required, numeric }
            }
        },

        beforeMount() {
            this.init();
        }
    }
</script>

<style scoped>
    
</style>