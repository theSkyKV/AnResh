<template>
    <div>
        <div v-if="ok">
            <label class="form-label">Имя</label>
            <div>
                <custom-input v-model="name" :class="{'is-invalid': v$.name.$errors.length > 0}" />
                <div class="text-danger my-3" v-for="error in v$.name.$errors" :key="error.$uid">{{ error.$message }}</div>
            </div>

            <label class="form-label">Отдел</label>
            <div>
                <select class="form-select form-select-lg mb-3" v-model="departmentId">
                    <option disabled value="">Выберите отдел</option>
                    <option v-for="department in departments" :key="department.Id" :value="department.Id">
                        {{ department.Name }}
                    </option>
                </select>
            </div>

            <label class="form-label">Зарплата</label>
            <div>
                <custom-input v-model="salary" :class="{'is-invalid': v$.salary.$errors.length > 0}" />
                <div class="text-danger my-3" v-for="error in v$.salary.$errors" :key="error.$uid">{{ error.$message }}</div>
            </div>

            <div class="text-danger my-3" v-for="error in v$.data.$errors" :key="error.$uid">{{ error.$message }}</div>
            
            <button type="button" class="brand-btn btn" @click="submit">Создать</button>
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
    import { required, numeric, helpers } from '@vuelidate/validators';

    export default {
        components: {
            CustomInput,
        },

        props: {
            createEmployeeUrl: String,
            getAllDepartmentsUrl: String
        },

        setup () {
            return { v$: useVuelidate() }
        },

        data() {
            return {
                departments: [],
                ok: false,
                name: "",
                departmentId: 0,
                salary: 0,
                data: "",
                intMaxValue: Math.pow(2, 31) - 1,
                pageNumber: 1,
                vuelidateExternalResults: {
                    data: []
                }
            }
        },

        methods: {
            validate (status) {
                var errors = [];
                if (status == 401)
                    errors = { data: [validation.UNAUTHORIZED] };
                else
                    errors = { data: [validation.INTERNAL] };
                
                Object.assign(this.vuelidateExternalResults, errors);
            },

            async init() {
                await axios.get(this.getAllDepartmentsUrl, { PageNumber: this.pageNumber, Limit: this.intMaxValue })
                            .then((response) => {
                                this.departments = response.data.departments;
                                this.departmentId = this.departments[0].Id;
                            })
                            .catch((error) => {
                                console.log(error);
                            });
                await axios.get(this.createEmployeeUrl)
                            .then(() => {
                                this.ok = true;
                            })
                            .catch((error) => {
                                console.log(error);
                            });
            },

            async submit() {
                this.v$.$error = null;
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
                               this.validate(error.response.status);
                           });
            },
        },

        validations() {
            return {
                name: { required, name: helpers.withMessage(validation.NAME_MESSAGE, validation.name) },
                salary: { required, numeric },
                data: { data: validation.ok },
            }
        },

        beforeMount() {
            this.init();
        }
    }
</script>