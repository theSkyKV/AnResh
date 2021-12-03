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

                <div>
                    <label>Навыки</label>
                    <select multiple id="Skills" name="Skills[]">
                        <option v-for="skill in learnedSkills" :key="skill.Id" 
                                :value="skill.Id" selected>
                            {{ skill.Name }}
                        </option>
                        <option v-for="skill in unlearnedSkills" :key="skill.Id" 
                                :value="skill.Id">
                            {{ skill.Name }}
                        </option>
                    </select>
                </div>

                <div>
                    <button @click="submit">Сохранить</button>
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

    export default {
        props: {
            id: Number,
            editEmployeeUrl: String,
            getAllDepartmentsUrl: String,
            getAllSkillsUrl: String,
        },

        data() {
            return {
                employee: null,
                departments: null,
                skills: null,
                ok: false,
                name: "",
                departmentId: 0,
                salary: 0
            }
        },

        methods: {
            async submit() {
                await axios.post(this.editEmployeeUrl, { Id: this.id, Name: this.name, DepartmentId: this.departmentId, Salary: this.salary })
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

                await axios.get(this.getAllDepartmentsUrl)
                            .then((response) => {
                                this.departments = response.data.departments;
                                this.departmentId = this.selectedDepartment[0].Id;
                            })
                            .catch((error) => {
                                console.log(error);
                            });

                await axios.get(this.getAllSkillsUrl)
                            .then((response) => {
                                this.skills = response.data.skills;
                                this.ok = true;
                            })
                            .catch((error) => {
                                console.log(error);
                            });
                
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

        computed: {
            selectedDepartment: function() {
                return this.departments.filter(department => department.Id == this.employee.DepartmentId);
            },

            unselectedDepartments: function() {
                return this.departments.filter(department => department.Id != this.employee.DepartmentId);
            },

            learnedSkills: function() {
                return this.skills.filter(skill => this.check(skill.Id) == true);
            },

            unlearnedSkills: function() {
                return this.skills.filter(skill => this.check(skill.Id) == false);
            },
        },

        beforeMount() {
            this.init();
        }
    }
</script>

<style scoped>
    
</style>