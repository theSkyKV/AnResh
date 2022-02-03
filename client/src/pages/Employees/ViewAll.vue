<template>
    <div>
        <h2>Сотрудники</h2>
        <div v-if="ok">
            <custom-dialog v-model:show="dialogVisible">
                <create v-if="createVisible" :createEmployeeUrl="createEmployeeUrl" :getAllDepartmentsUrl="getAllDepartmentsUrl"></create>
                <edit v-if="editVisible" :id="id" :editEmployeeUrl="editEmployeeUrl" :getAllDepartmentsUrl="getAllDepartmentsUrl" :getAllSkillsUrl="getAllSkillsUrl"></edit>
                <delete v-if="deleteVisible" :id="id" :deleteEmployeeUrl="deleteEmployeeUrl"></delete>
            </custom-dialog>

            <button @click="onCreateButtonClick" class="brand-btn btn">Создать</button>

            <div class="brand-div">
                Сортировка:
                <custom-select :modelValue="selectedSort" @changeOption="selectedSort = $event.target.value" :options="sortOptions" @change="getData" />
            </div>
            <div class="brand-div"><custom-input :modelValue="searchNameQuery" @updateInput="searchNameQuery = $event.target.value" @input="getData" placeholder="Имя..." /></div>
            <div class="brand-div"><custom-input :modelValue="searchDepartmentQuery" @updateInput="searchDepartmentQuery = $event.target.value" @input="getData" placeholder="Отдел..." /></div>
            
            <div class="brand-div"><custom-input :modelValue="searchSkillQuery" @updateInput="searchSkillQuery = $event.target.value" @input="getSkills" placeholder="Навык..." /></div>
            <div v-for="skill in skills" :key="skill.Id">
                <input type="checkbox" :value="skill.Id" v-model="searchedSkills" @change="getData">
                <label>{{ skill.Name }}</label>
            </div>
            <page-number-display :total="totalSkillPages" :current="skillPageNumber" @changePage="changeSkillPage" />

            <div class="brand-div">
                Элементов на странице:
                <custom-select :modelValue="limit" @changeOption="limit = $event.target.value" :options="itemsPerPage" @change="getData" />
            </div>
            <div class="brand-div">
                Найдено записей:
                {{ totalRecords }}
            </div>
            <div class="brand-div">
                Средняя зарплата сотрудников по запросу:
                {{ averageSalary }}
            </div>

            <table class="brand-table">
                <tr>
                    <th>
                        Имя
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
                <tbody >
                    <tr v-for="employee in employees" :key="employee.Id">
                        <td>
                            {{ employee.Name }}
                        </td>
                        <td>
                            {{ employee.DepartmentName }}
                        </td>
                        <td>
                            {{ employee.Salary }}
                        </td>
                        <td>
                            <ul v-for="skill in employee.Skills" :key="skill.Id">{{ skill.SkillName }}</ul>
                        </td>
                        <td>
                            <button @click="onEditButtonClick(employee.Id)" class="brand-action-link">Редактировать</button><span class="brand-span">|</span>
                            <button @click="onDeleteButtonClick(employee.Id)" class="brand-action-link">Удалить</button><span class="brand-span">|</span>
                        </td>
                    </tr>
                </tbody>
            </table>

            <page-number-display :total="totalPages" :current="pageNumber" @changePage="changePage" />

        </div>
        <div v-else>
            Загрузка...
        </div>
    </div>
</template>

<script>
import * as axios from '@/custom_plugins/axiosApi.js';
import * as path from '@/config/path.js';
import CustomDialog from '@/components/CustomDialog.vue';
import CustomSelect from '@/components/CustomSelect.vue';
import CustomInput from '@/components/CustomInput.vue';
import PageNumberDisplay from '@/components/PageNumberDisplay.vue';
import Create from '@/pages/Employees/Create.vue';
import Edit from '@/pages/Employees/Edit.vue';
import Delete from '@/pages/Employees/Delete.vue';

export default {
    components: {
        CustomDialog,
        CustomSelect,
        CustomInput,
        PageNumberDisplay,
        Create,
        Edit,
        Delete
    },

    data() {
        return {
            employees: [],
            skills: [],
            searchedSkills: [],
            ok: false,
            id: 0,
            dialogVisible: false,
            createVisible: false,
            editVisible: false,
            deleteVisible: false,

            pageNumber: 1,
            skillPageNumber: 1,
            skillLimit: 5,
            limit: 0,
            totalPages: 0,
            totalSkillPages: 0,
            totalRecords: 0,
            averageSalary: 0,
            selectedSort: '',
            searchNameQuery: '',
            searchDepartmentQuery: '',
            searchSkillQuery: '',
            sortOptions: [
                { value: 'ByName', name: 'По имени' },
                { value: 'ByDepartment', name: 'По отделу' },
            ],
            itemsPerPage: [
                { value: 5, name: '5' },
                { value: 10, name: '10' },
                { value: 20, name: '20' },
            ],

            viewAllUrl: `${path.SERVER}${path.GET_ALL_EMPLOYEES}`,
            createEmployeeUrl: `${path.SERVER}${path.CREATE_EMPLOYEE}`,
            editEmployeeUrl: `${path.SERVER}${path.EDIT_EMPLOYEE}`,
            deleteEmployeeUrl: `${path.SERVER}${path.DELETE_EMPLOYEE}`,
            getAllDepartmentsUrl: `${path.SERVER}${path.GET_ALL_DEPARTMENTS}`,
            getAllSkillsUrl: `${path.SERVER}${path.GET_ALL_SKILLS}`,
        }
    },

    methods: {
        changePage(page) {
            this.pageNumber = page;
            this.getData();
        },

        changeSkillPage(page) {
            this.skillPageNumber = page;
            this.getSkills();
        },

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

        async getData() {
            await axios.get(this.viewAllUrl, { PageNumber: this.pageNumber, Limit: this.limit, SearchNameQuery: this.searchNameQuery, SearchDepartmentQuery: this.searchDepartmentQuery, SelectedSort: this.selectedSort, SearchedSkills: this.searchedSkills })
                       .then((response) => {
                           this.employees = response.data.employees;
                           this.totalPages = response.data.total.Pages;
                           this.totalRecords = response.data.total.Records;
                           this.averageSalary = response.data.total.AverageSalary;
                           this.ok = true;
                       })
                       .catch((error) => {
                           console.log(error);
                       });
        },

        async getSkills() {
            await axios.get(this.getAllSkillsUrl, { PageNumber: this.skillPageNumber, Limit: this.skillLimit, SearchQuery: this.searchSkillQuery })
                       .then((response) => {
                           this.skills = response.data.skills;
                           this.totalSkillPages = response.data.total.Pages;
                       })
                       .catch((error) => {
                           console.log(error);
                       });
        },
    },

    watch: {
        dialogVisible(newValue) {
            if (newValue == false) {
                this.createVisible = false;
                this.editVisible = false;
                this.deleteVisible = false;
            }
        },
    },

    beforeMount() {
        this.limit = this.itemsPerPage[2].value;
        this.selectedSort = this.sortOptions[0].value;
        this.getData();
        this.getSkills();
    }
}
</script>

<style scoped>

</style>