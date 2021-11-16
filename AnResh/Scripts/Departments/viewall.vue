<template>
    <div>
        <h2>Отделы</h2>
        <div v-if="totalPages === 0">
            Поиск не дал результатов
        </div>
        <table v-else class="brand-table">
            <tr>
                <th>
                    Название отдела
                </th>
                <th>
                    Средняя зарплата по отделу
                </th>
                <th></th>
            </tr>
            <tbody>
                <tr v-for="index in currentLimit" :key="departments[index + deltaIndex].DepartmentId">
                    <!-- even - переделай на css, не нужно в каждом файле делать логикой то, что делается стилями
                         псевдокласс :nth-child(even) тебе поможет -->
                    <td :class="{'even': index % 2 === 0}">
                        {{ departments[index + deltaIndex].DepartmentName }}
                    </td>
                    <td :class="{'even': index % 2 === 0}">
                        {{ departments[index + deltaIndex].AverageSalary }}
                    </td>
                    <td :class="{'even': index % 2 === 0}">
                        <!-- в целом, это подход MVC, используй vue по максимуму: например, почему бы не сделать диалог на редактирвоание и на подтверждение
                            без запроса отдельной страницы? -->
                        <a :href="editDepartmentUrl + '/' + departments[index + deltaIndex].DepartmentId + '?name=' + departments[index + deltaIndex].DepartmentName" class="brand-action-link">Редактировать</a>
                        <span class="brand-span">|</span>
                        <a :href="detailsDepartmentUrl + '/' + departments[index + deltaIndex].DepartmentId + '?name=' + departments[index + deltaIndex].DepartmentName" class="brand-action-link">Посмотреть информацию</a>
                        <span class="brand-span">|</span>
                        <a :href="deleteDepartmentUrl + '/' + departments[index + deltaIndex].DepartmentId + '?name=' + departments[index + deltaIndex].DepartmentName" class="brand-action-link">Удалить</a>
                    </td>
                </tr>
            </tbody>
        </table>
        <div v-for="page in totalPages" :key="page" class="page" :class="{'current-page': pageNumber === page}" @click="changePage(page)">
            {{ page }}
        </div>
        <a :href="createDepartmentUrl" class="brand-btn btn">Добавить отдел</a>
    </div>
</template>

<script>
    export default {
        props: {
            createDepartmentUrl: String,
            editDepartmentUrl: String,
            deleteDepartmentUrl: String,
            detailsDepartmentUrl: String,
            departments: Array
        },

        data() {
            return {
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
                this.totalPages = Math.ceil(this.departments.length / this.limit);
                this.remainder = this.departments.length % this.limit;

                if (this.totalPages != 0) {
                    this.changePage(1);
                }

                this.currentLimit = this.limit;
                if (this.pageNumber === this.totalPages && this.remainder != 0) {
                    this.currentLimit = this.remainder;
                }
            }
        },

        beforeMount() {
            this.updateValues();
            this.deltaIndex = - 1 + this.limit * (this.pageNumber - 1);
        },

        watch: {
            departments() {
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