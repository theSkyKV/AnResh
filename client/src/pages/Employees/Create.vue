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

                <div>
                    <button @click="submit">Создать</button>
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
            createEmployeeUrl: String,
            getAllDepartmentsUrl: String,
        },

        data() {
            return {
                departments: null,
                ok: false,
                name: "",
                departmentId: 0,
                salary: 0
            }
        },

        methods: {
            async submit() {
                await axios.post(this.createEmployeeUrl, { Name: this.name, DepartmentId: this.departmentId, Salary: this.salary })
                           .then(() => {
                               location.reload();
                           })
                           .catch((error) => {
                               console.log(error);
                           });
            },

            async init() {
                await axios.get(this.getAllDepartmentsUrl)
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

        beforeMount() {
            this.init();
        }
    }
</script>

<style scoped>
    
</style>