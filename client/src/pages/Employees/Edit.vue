<template>
    <div>
        <div v-if="ok">
            <h2>Редактировать</h2>
            <div>
                <input type="hidden" name="Id" :value="id" />

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

                <div>
                    <label>Зарплата</label>
                    <div>
                        <input name="Salary" v-model="salary" type="number" oninput="this.value = this.value.replace(/\s+/g, ' ')" />
                    </div>
                </div>
                <span v-if="v$.salary.$error" class="error-message">
                        {{ v$.salary.$errors[0].$message }}
                </span>

                <div>
                    <label>Навыки</label>
                    <div v-for="skill in skills" :key="skill.Id">
                        <input type="checkbox" :value="skill.Id" v-model="learnedSkills">
                        <label>{{ skill.Name }}</label>
                    </div>
                </div>

                <page-number-display :total="totalPages" :current="pageNumber" @changePage="changePage" />

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
    import PageNumberDisplay from '@/components/PageNumberDisplay.vue';

    import useValidate from "@vuelidate/core";
    import { required, numeric, helpers } from "@vuelidate/validators";

    export default {
        components: {
            PageNumberDisplay,
        },

        props: {
            id: Number,
            editEmployeeUrl: String,
            getAllDepartmentsUrl: String,
            getAllSkillsUrl: String,
        },

        data() {
            return {
                v$: useValidate(),

                employee: null,
                departments: [],
                skills: [],
                ok: false,
                name: "",
                departmentId: 0,
                salary: 0,
                learnedSkills: [],

                intMaxValue: Math.pow(2, 31) - 1,
                pageNumber: 1,
                limit: 5,
                totalPages: 0,
            }
        },

        methods: {
            changePage(page) {
                this.pageNumber = page;
                this.getSkills();
            },

            async getSkills() {
                await axios.get(this.getAllSkillsUrl, { PageNumber: this.pageNumber, Limit: this.limit })
                            .then((response) => {
                                this.skills = response.data.skills;
                                this.totalPages = response.data.total.Pages;
                                this.ok = true;
                            })
                            .catch((error) => {
                                console.log(error);
                            });
            },

            async submit() {
                this.v$.$validate();
                
                if (this.v$.$error) {
  					return;
  				}
                
                await axios.post(this.editEmployeeUrl, { Id: this.id, Name: this.name, DepartmentId: this.departmentId, Salary: this.salary, Skills: this.learnedSkills })
                           .then(() => {
                               location.reload();
                           })
                           .catch((error) => {
                               console.log(error);
                           });
            },

            async init() {
                await axios.get(this.editEmployeeUrl, { id: this.id })
                            .then((response) => {
                                this.employee = response.data.employee;
                                this.name = this.employee.Name;
                                this.salary = this.employee.Salary;
                            })
                            .catch((error) => {
                                console.log(error);
                            });

                await axios.get(this.getAllDepartmentsUrl, { PageNumber: this.pageNumber, Limit: this.intMaxValue })
                            .then((response) => {
                                this.departments = response.data.departments;
                                this.departmentId = this.selectedDepartment[0].Id;
                            })
                            .catch((error) => {
                                console.log(error);
                            });

                await axios.get(this.getAllSkillsUrl, { PageNumber: this.pageNumber, Limit: this.intMaxValue })
                            .then((response) => {
                                this.learnedSkills = response.data.skills.filter(skill => this.check(skill.Id) == true).map(skill => skill.Id);
                            })
                            .catch((error) => {
                                console.log(error);
                            });

                this.getSkills();
            },

            check(id) {
                var skills = this.employee.Skills;
                
                for (var i = 0; i < skills.length; i++) {
                    if (id == skills[i].SkillId) {
                        return true;
                    }
                }

                return false;
            }
        },

        validations() {
            return {
                name: { required, name: helpers.withMessage(validate.NAME_MESSAGE, validate.name) },
                salary: { required, numeric }
            }
        },

        computed: {
            selectedDepartment: function() {
                return this.departments.filter(department => department.Id == this.employee.DepartmentId);
            },

            unselectedDepartments: function() {
                return this.departments.filter(department => department.Id != this.employee.DepartmentId);
            }
        },

        beforeMount() {
            this.init();
        }
    }
</script>

<style scoped>
    
</style>