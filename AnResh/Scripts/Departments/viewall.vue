<template>
    <div>
        <h2>Отделы</h2>

        <table class="brand-table">
            <tr>
                <th>
                    Название отдела
                </th>
                <th>
                    Средняя зарплата по отделу
                </th>
                <th></th>
            </tr>
            <tbody v-for="department in departments" :key="department.DepartmentId">
                <tr v-if="isEven">
                    <td class="even">
                        {{ department.DepartmentName }}
                    </td>
                    <td class="even">
                        {{ department.AverageSalary }}
                    </td>
                    <td class="even">
                        <a :href="editDepartmentUrl + '/' + department.DepartmentId + '?name=' + department.DepartmentName" class="brand-action-link">Редактировать</a>
                        <span class="brand-span">|</span>
                        <a :href="detailsDepartmentUrl + '/' + department.DepartmentId + '?name=' + department.DepartmentName" class="brand-action-link">Посмотреть информацию</a>
                        <span class="brand-span">|</span>
                        <a :href="deleteDepartmentUrl + '/' + department.DepartmentId + '?name=' + department.DepartmentName" class="brand-action-link">Удалить</a>
                    </td>
                </tr>
                <tr v-else>
                    <td>
                        {{ department.DepartmentName }}
                    </td>
                    <td>
                        {{ department.AverageSalary }}
                    </td>
                    <td>
                        <a :href="editDepartmentUrl + '/' + department.DepartmentId + '?name=' + department.DepartmentName" class="brand-action-link">Редактировать</a>
                        <span class="brand-span">|</span>
                        <a :href="detailsDepartmentUrl + '/' + department.DepartmentId + '?name=' + department.DepartmentName" class="brand-action-link">Посмотреть информацию</a>
                        <span class="brand-span">|</span>
                        <a :href="deleteDepartmentUrl + '/' + department.DepartmentId + '?name=' + department.DepartmentName" class="brand-action-link">Удалить</a>
                    </td>
                </tr>
            </tbody>
        </table>
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
                isEven: false
            }
        }
    }
</script>