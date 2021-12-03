<template>
    <div>
        <div v-if="ok">
            <custom-dialog v-model:show="dialogVisible">
                <create v-if="createVisible" :createEmployeeUrl="createEmployeeUrl" :getAllDepartmentsUrl="getAllDepartmentsUrl"></create>
                <edit v-if="editVisible" :id="id" :editEmployeeUrl="editEmployeeUrl" :getAllDepartmentsUrl="getAllDepartmentsUrl" :getAllSkillsUrl="getAllSkillsUrl"></edit>
                <delete v-if="deleteVisible" :id="id" :deleteEmployeeUrl="deleteEmployeeUrl"></delete>
            </custom-dialog>
            <h2>{{ department.Name }}</h2>
            <button @click="onCreateButtonClick">Create</button>
            <table>
                <tr>
                    <th>
                        Name
                    </th>
                    <th>
                        Power
                    </th>
                    <th>
                        Skills
                    </th>
                    <th></th>
                </tr>
                <tbody v-for="employee in employees" :key="employee.Id">
                    <tr>
                        <td>
                            {{ employee.Name }}
                        </td>
                        <td>
                            {{ employee.Salary }}
                        </td>
                        <td>
                            <ul v-for="skill in employee.Skills" :key="skill.Id">{{ skill.SkillName }}</ul>
                        </td>
                        <td>
                            <button @click="onEditButtonClick(employee.Id)">Edit</button><span>|</span>
                            <button @click="onDeleteButtonClick(employee.Id)">Delete</button><span>|</span>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div v-else>
            Loading...
        </div>
    </div>
</template>

<script>
import * as axios from '@/custom_plugins/axiosApi.js';
import * as path from '@/config/path.js';
import CustomDialog from '@/components/CustomDialog.vue';
import Create from '@/pages/Employees/Create.vue';
import Edit from '@/pages/Employees/Edit.vue';
import Delete from '@/pages/Employees/Delete.vue';

export default {
    components: {
        CustomDialog,
        Create,
        Edit,
        Delete,
    },

    data() {
        return {
            employees: null,
            department: null,
            ok: false,
            id: 0,
            dialogVisible: false,
            createVisible: false,
            editVisible: false,
            deleteVisible: false,

            viewByDepartmentIdUrl: `${path.SERVER}${path.GET_EMPLOYEES_BY_DEPARTMENT_ID}`,
            createEmployeeUrl: `${path.SERVER}${path.CREATE_EMPLOYEE}`,
            editEmployeeUrl: `${path.SERVER}${path.EDIT_EMPLOYEE}`,
            deleteEmployeeUrl: `${path.SERVER}${path.DELETE_EMPLOYEE}`,
            getAllDepartmentsUrl: `${path.SERVER}${path.GET_ALL_DEPARTMENTS}`,
            getDepartmentUrl: `${path.SERVER}${path.GET_DEPARTMENT}`,
            getAllSkillsUrl: `${path.SERVER}${path.GET_ALL_SKILLS}`,
        }
    },

    methods: {
        onCreateButtonClick() {
            this.createVisible = true;
            this.dialogVisible = true;
        },

        onEditButtonClick(id) {
            this.editVisible = true;
            this.dialogVisible = true;
            this.id = id;
        },

        onDeleteButtonClick(id) {
            this.deleteVisible = true;
            this.dialogVisible = true;
            this.id = id;
        },

        async init() {
            await axios.get(this.getDepartmentUrl, { id: this.$route.params.departmentId })
                       .then((response) => {
                           this.department = response.data.department;
                       })
                       .catch((error) => {
                           console.log(error);
                       });

            await axios.get(this.viewByDepartmentIdUrl, { id: this.$route.params.departmentId })
                       .then((response) => {
                           this.employees = response.data.employees;
                           this.ok = true;
                       })
                       .catch((error) => {
                           console.log(error);
                       });
        }
    },

    watch: {
        dialogVisible(newValue) {
            if (newValue == false) {
                this.createVisible = false;
                this.editVisible = false;
                this.deleteVisible = false;
            }
        }
    },

    beforeMount() {
        this.init();
    }
}
</script>

<style scoped>

</style>