<template>
    <div>
        <h2>{{ departmentName }}</h2>
        <div v-if="totalPages === 0">
            Поиск не дал результатов
        </div>
        <table v-else class="brand-table">
            <tr>
                <th>
                    ФИО
                </th>
                <th>
                    Отдел
                </th>
                <th>
                    Зарплата
                </th>
                <th>
                    Навыки
                </th>
                <th></th>
            </tr>
            <tbody>
                <tr v-for="index in currentLimit" :key="employees[index + deltaIndex].EmployeeId">
                    <td :class="{'even': index % 2 === 0}">
                        {{ employees[index + deltaIndex].EmployeeName }}
                    </td>
                    <td :class="{'even': index % 2 === 0}">
                        {{ DepartmentName }}
                    </td>
                    <td :class="{'even': index % 2 === 0}">
                        {{ employees[index + deltaIndex].Salary }}
                    </td>
                    <td :class="{'even': index % 2 === 0}">
                        <ul v-for="skill in employees[index + deltaIndex].LearnedSkills" :key="skill">
                            <li>{{ skill }}</li>
                        </ul>
                    </td>
                    <td>
                        <a :href="editEmployeeUrl + '?id=' + employees[index + deltaIndex].EmployeeId +
                                                    '&name=' + employees[index + deltaIndex].EmployeeName +
                                                    '&departmentId=' + departmentId +
                                                    '&salary=' + employees[index + deltaIndex].Salary"
                           class="brand-action-link">Редактировать</a>
                        <span class="brand-span">|</span>
                        <a :href="detailsEmployeeUrl + '?id=' + employees[index + deltaIndex].EmployeeId +
                                                       '&name=' + employees[index + deltaIndex].EmployeeName +
                                                       '&departmentId=' + departmentId +
                                                       '&salary=' + employees[index + deltaIndex].Salary"
                           class="brand-action-link">Посмотреть информацию</a>
                        <span class="brand-span">|</span>
                        <a :href="deleteEmployeeUrl + '?id=' + employees[index + deltaIndex].EmployeeId +
                                                      '&name=' + employees[index + deltaIndex].EmployeeName +
                                                      '&departmentId=' + departmentId +
                                                      '&salary=' + employees[index + deltaIndex].Salary"
                           class="brand-action-link">Удалить</a>
                    </td>
                </tr>
            </tbody>
        </table>
        <!-- а что, если страниц будет несколько сотен? -->
        <div v-for="page in totalPages" :key="page" class="page" :class="{'current-page': pageNumber === page}" @click="changePage(page)">
            {{ page }}
        </div>
        <div><a :href="createEmployeeUrl + '?departmentId=' + departmentId" class="brand-btn btn">Добавить сотрудника</a></div>
        <div><a :href="indexDepartmentUrl" class="brand-back-btn brand-btn btn">Назад</a></div>
    </div>
</template>

<script>
    export default {
        props: {
            createEmployeeUrl: String,
            editEmployeeUrl: String,
            deleteEmployeeUrl: String,
            detailsEmployeeUrl: String,
            indexDepartmentUrl: String,
            employees: Array,
            departmentName: String,
            departmentId: Number
        },

        data() {
            return {
                averageSalary: 0,
                limit: 5,
                currentLimit: 0,
                pageNumber: 1,
                totalPages: 0,
                remainder: 0,
                deltaIndex: 0,
            }
        },

        methods: {
            changePage(page) {
                this.pageNumber = page;
            },
            updateValues() {
                //клиентская пагинация имеет место быть, в редких случаях, в данном случае нужна серверная
                //как вариант, посмотреть как работает KendoGrid
                this.totalPages = Math.ceil(this.employees.length / this.limit);
                this.remainder = this.employees.length % this.limit;

                if (this.totalPages != 0) {
                    this.changePage(1);
                }

                this.currentLimit = this.limit;
                if (this.pageNumber === this.totalPages && this.remainder != 0) {
                    this.currentLimit = this.remainder;
                }

                this.averageSalary = 0;
                for (var i = 0; i < this.employees.length; i++) {
                    this.averageSalary += this.employees[i].Salary;
                }
                //будет ошибка, если сотрудников 0
                this.averageSalary = Math.round(this.averageSalary / this.employees.length);
            }
        },

        beforeMount() {
            this.updateValues();
            this.deltaIndex = - 1 + this.limit * (this.pageNumber - 1);
        },

        watch: {
            employees() {
                this.updateValues();
            },
            pageNumber(newValue) {
                // а что мешало это сделать в функции changePage?
                this.deltaIndex = - 1 + this.limit * (newValue - 1);

                this.currentLimit = this.limit;
                if (this.pageNumber === this.totalPages && this.remainder != 0) {
                    this.currentLimit = this.remainder;
                }
            }
        },
    }
</script>

<style scoped>
    .page {
        border: 1px solid black;
    }

    .current-page {
        border: 2px solid green;
    }
</style>