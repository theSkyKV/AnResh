<template>
    <div>
        <h2>{{ departmentName }}</h2>

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
                <th></th>
            </tr>
            <tbody v-for="employee in employees" :key="employee.EmployeeId">
                <tr v-if="isEven">
                    <td class="even">
                        {{ employee.EmployeeName }}
                    </td>
                    <td class="even">
                        {{ departmentName }}
                    </td>
                    <td class="even">
                        {{ employee.Salary }}
                    </td>
                    <td class="even">
                        <a :href="editEmployeeUrl + '?id=' + employee.EmployeeId +
                                                    '&name=' + employee.EmployeeName +
                                                    '&departmentId=' + departmentId +
                                                    '&salary=' + employee.Salary"
                           class="brand-action-link">Редактировать</a>
                        <span class="brand-span">|</span>
                        <a :href="detailsEmployeeUrl + '?id=' + employee.EmployeeId +
                                                       '&name=' + employee.EmployeeName +
                                                       '&departmentId=' + departmentId +
                                                       '&salary=' + employee.Salary"
                           class="brand-action-link">Посмотреть информацию</a>
                        <span class="brand-span">|</span>
                        <a :href="deleteEmployeeUrl + '?id=' + employee.EmployeeId +
                                                      '&name=' + employee.EmployeeName +
                                                      '&departmentId=' + departmentId +
                                                      '&salary=' + employee.Salary"
                           class="brand-action-link">Удалить</a>
                    </td>
                </tr>
                <tr v-else>
                    <td>
                        {{ employee.EmployeeName }}
                    </td>
                    <td>
                        {{ departmentName }}
                    </td>
                    <td>
                        {{ employee.Salary }}
                    </td>
                    <td>
                        <a :href="editEmployeeUrl + '?id=' + employee.EmployeeId +
                                                    '&name=' + employee.EmployeeName +
                                                    '&departmentId=' + departmentId +
                                                    '&salary=' + employee.Salary"
                           class="brand-action-link">Редактировать</a>
                        <span class="brand-span">|</span>
                        <a :href="detailsEmployeeUrl + '?id=' + employee.EmployeeId +
                                                       '&name=' + employee.EmployeeName +
                                                       '&departmentId=' + departmentId +
                                                       '&salary=' + employee.Salary"
                           class="brand-action-link">Посмотреть информацию</a>
                        <span class="brand-span">|</span>
                        <a :href="deleteEmployeeUrl + '?id=' + employee.EmployeeId +
                                                      '&name=' + employee.EmployeeName +
                                                      '&departmentId=' + departmentId +
                                                      '&salary=' + employee.Salary"
                           class="brand-action-link">Удалить</a>
                    </td>
                </tr>
            </tbody>
        </table>
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
                isEven: false
            }
        }
    }
</script>