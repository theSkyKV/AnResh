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
                <th>
                    Навыки
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
                    <td class="even">
                        <ul v-for="skill in employee.Skills" :key="skill.SkillId">
                            <li v-if="skill.IsLearned">{{ skill.SkillName }}</li>
                        </ul>
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
                    <td>
                        <ul v-for="skill in employee.Skills" :key="skill.SkillId">
                            <li v-if="skill.IsLearned">{{ skill.SkillName }}</li>
                        </ul>
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