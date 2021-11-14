<template>
    <div>
        <h2>Сотрудники</h2>

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
            </tr>
            <tbody v-for="employee in employees" :key="employee.EmployeeId">
                <tr v-if="isEven">
                    <td class="even">
                        {{ employee.EmployeeName }}
                    </td>
                    <td class="even">
                        {{ employee.DepartmentName }}
                    </td>
                    <td class="even">
                        {{ employee.Salary }}
                    </td>
                </tr>
                <tr v-else>
                    <td>
                        {{ employee.EmployeeName }}
                    </td>
                    <td>
                        {{ employee.DepartmentName }}
                    </td>
                    <td>
                        {{ employee.Salary }}
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    export default {
        props: {
            employees: Array
        },

        data() {
            return {
                isEven: false
            }
        }
    }
</script>