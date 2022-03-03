<template>
    <div>
        <div v-if="ok">
            <div>
                <label class="form-label">Имя</label>
                <div>
                    <custom-input v-model="name" :class="{'is-invalid': v$.name.$errors.length > 0}" />
                    <div class="text-danger my-3" v-for="error in v$.name.$errors" :key="error.$uid">{{ error.$message }}</div>
                </div>
            </div>

            <label class="form-label">Отдел</label>
            <div>
                <select class="form-select form-select-lg mb-3" v-model="departmentId">
                    <option v-for="department in selectedDepartment" :key="department.Id" 
                                :value="department.Id" selected>
                            {{ department.Name }}
                    </option>
                    <option v-for="department in unselectedDepartments" :key="department.Id" 
                            :value="department.Id">
                        {{ department.Name }}
                    </option>
                </select>
            </div>

            <label class="form-label">Зарплата</label>
            <div>
                <custom-input v-model="salary" :class="{'is-invalid': v$.salary.$errors.length > 0}" />
                <div class="text-danger my-3" v-for="error in v$.salary.$errors" :key="error.$uid">{{ error.$message }}</div>
            </div>

            <label class="form-label">Навыки</label>
            <div class="w-100 align-items-top justify-content-start flex-wrap d-inline-flex">
                <div class="w-25 mx-3">
                    <div v-for="skill in skills.slice(0, limit / 2)" :key="skill.Id">
                        <input type="checkbox" :value="skill.Id" v-model="learnedSkills">
                        <label class="mx-2">{{ skill.Name }}</label>
                    </div>
                </div>
                <div class="w-25 mx-3">
                    <div v-for="skill in skills.slice(limit / 2, limit)" :key="skill.Id">
                        <input type="checkbox" :value="skill.Id" v-model="learnedSkills">
                        <label class="mx-2">{{ skill.Name }}</label>
                    </div>
                </div>
            </div>

            <page-number-display :total="totalPages" :current="pageNumber" @changePage="changePage" class="pagination-sm my-3" />

            <div class="text-danger my-3" v-for="error in v$.data.$errors" :key="error.$uid">{{ error.$message }}</div>
            
            <button type="button" class="brand-btn btn" @click="submit">Подтвердить</button>
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
    import PageNumberDisplay from '@/components/PageNumberDisplay.vue';
    import useVuelidate from '@vuelidate/core';
    import { required, numeric, helpers } from '@vuelidate/validators';

    export default {
        components: {
            CustomInput,
            PageNumberDisplay
        },

        props: {
            id: Number,
            editEmployeeUrl: String,
            getAllDepartmentsUrl: String,
            getAllSkillsUrl: String,
            getSkillbookUrl: String,
            updateSkillbookUrl: String
        },

        setup () {
            return { v$: useVuelidate() }
        },

        data() {
            return {
                employee: null,
                departments: [],
                ok: false,
                name: "",
                departmentId: 0,
                salary: 0,
                data: "",
                vuelidateExternalResults: {
                    data: []
                },
                skills: [],
                learnedSkills: [],

                intMaxValue: Math.pow(2, 31) - 1,
                pageNumber: 1,
                limit: 10,
                totalPages: 0
            }
        },

        methods: {
            changePage(page) {
                this.pageNumber = page;
                this.getSkills();
            },

            validate (status) {
                var errors = [];
                if (status == 401)
                    errors = { data: [validation.UNAUTHORIZED] };
                else
                    errors = { data: [validation.INTERNAL] };

                Object.assign(this.vuelidateExternalResults, errors);
            },

            async submit() {
                this.v$.$error = null;
                this.v$.$validate();
                
                if (this.v$.$error) {
  					return;
  				}

                await axios.post(this.updateSkillbookUrl, { employeeId: this.id, skillIds: this.learnedSkills })
                           .then((response) => {
                                console.log(response);
                           })
                           .catch((error) => {
                                console.log(error);
                                this.validate(error.response.status);
                           });

                await axios.post(this.editEmployeeUrl, { Id: this.id, Name: this.name, DepartmentId: this.departmentId, Salary: this.salary })
                           .then(() => {
                                location.reload();
                           })
                           .catch((error) => {
                                console.log(error);
                                this.validate(error.response.status);
                           });
            },

            async init() {
                await axios.get(this.editEmployeeUrl, { Id: this.id })
                           .then((response) => {
                                this.employee = response.data.employee;
                                this.name = this.employee.Name;
                                this.salary = this.employee.Salary;
                           })
                           .catch((error) => {
                                console.log(error);
                           });

                this.getSkills();

                await axios.get(this.getSkillbookUrl, { Id: this.id })
                           .then((response) => {
                                this.learnedSkills = response.data.skills.map(skill => skill.Id);
                           })
                           .catch((error) => {
                                console.log(error);
                           });

                await axios.get(this.getAllDepartmentsUrl, { PageNumber: this.pageNumber, Limit: this.intMaxValue })
                            .then((response) => {
                                this.departments = response.data.departments;
                                this.departmentId = this.selectedDepartment[0].Id;
                                this.ok = true;
                            })
                            .catch((error) => {
                                console.log(error);
                            });
            },

            async getSkills() {
                await axios.get(this.getAllSkillsUrl, { PageNumber: this.pageNumber, Limit: this.limit })
                            .then((response) => {
                                this.skills = response.data.skills;
                                this.totalPages = response.data.totalPages;
                            })
                            .catch((error) => {
                                console.log(error);
                            });
            },
        },

        computed: {
            selectedDepartment: function() {
                return this.departments.filter(department => department.Id == this.employee.DepartmentId);
            },
            
            unselectedDepartments: function() {
                return this.departments.filter(department => department.Id != this.employee.DepartmentId);
            }
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