<template>
    <div>
        <h2>Сотрудники</h2>
        <div v-if="totalPages === 0">
            Поиск не дал результатов
        </div>
        <div v-else>
            Найдено сотрудников по запросу - {{ employees.length }};
            Средняя зарплата - {{ averageSalary }}
            <table class="brand-table">
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
                </tr>
                <tbody>
                    <tr v-for="index in currentLimit" :key="employees[index + deltaIndex].EmployeeId">
                        <td :class="{'even': index % 2 === 0}">
                            {{ employees[index + deltaIndex].EmployeeName }}
                        </td>
                        <td :class="{'even': index % 2 === 0}">
                            {{ employees[index + deltaIndex].DepartmentName }}
                        </td>
                        <td :class="{'even': index % 2 === 0}">
                            {{ employees[index + deltaIndex].Salary }}
                        </td>
                        <td :class="{'even': index % 2 === 0}">
                            <ul v-for="skill in employees[index + deltaIndex].LearnedSkills" :key="skill">
                                <li>{{ skill }}</li>
                            </ul>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div v-for="page in totalPages" :key="page" class="page" :class="{'current-page': pageNumber === page}" @click="changePage(page)">
            {{ page }}
        </div>
    </div>
</template>

<script>
    export default {
        props: {
            employees: Array
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