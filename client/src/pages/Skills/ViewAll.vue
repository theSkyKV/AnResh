<template>
    <div>
        <h2>Отделы</h2>
        <div v-if="ok">
            <custom-dialog v-model:show="dialogVisible">
                <create v-if="createVisible" :createDepartmentUrl="createDepartmentUrl"></create>
                <edit v-if="editVisible" :id="id" :editDepartmentUrl="editDepartmentUrl"></edit>
                <delete v-if="deleteVisible" :id="id" :deleteDepartmentUrl="deleteDepartmentUrl"></delete>
            </custom-dialog>
            <button @click="onCreateButtonClick">Создать</button>
            <table>
                <tr>
                    <th>
                        Название
                    </th>
                    <th>
                        Средняя зарплата
                    </th>
                    <th></th>
                </tr>
                <tbody v-for="department in departments" :key="department.Id">
                    <tr>
                        <td>
                            {{ department.Name }}
                        </td>
                        <td>
                            {{ department.Name }}
                        </td>
                        <td>
                            <button @click="onEditButtonClick(department.Id)">Редактировать</button><span>|</span>
                            <button @click="onDeleteButtonClick(department.Id)">Удалить</button><span>|</span>
                            <button @click="onDetailsButtonClick(department.Id)">Посмотреть информацию</button><span>|</span>
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
import Create from '@/pages/Departments/Create.vue';
import Edit from '@/pages/Departments/Edit.vue';
import Delete from '@/pages/Departments/Delete.vue';

export default {
    components: {
        CustomDialog,
        Create,
        Edit,
        Delete
    },

    data() {
        return {
            departments: null,
            ok: false,
            id: 0,
            dialogVisible: false,
            createVisible: false,
            editVisible: false,
            deleteVisible: false,

            viewAllUrl: `${path.SERVER}${path.GET_ALL_DEPARTMENTS}`,
            createDepartmentUrl: `${path.SERVER}${path.CREATE_DEPARTMENT}`,
            editDepartmentUrl: `${path.SERVER}${path.EDIT_DEPARTMENT}`,
            deleteDepartmentUrl: `${path.SERVER}${path.DELETE_DEPARTMENT}`,
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

        onDetailsButtonClick(id) {
            this.$router.push(`/Employees/${id}`);
        },

        async init() {
                await axios.get(this.viewAllUrl)
                           .then(function (response) {
                               this.departments = response.data.departments;
                               this.ok = true;
                           })
                           .catch(function (error) {
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