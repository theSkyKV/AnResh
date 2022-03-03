<template>
    <div>
        <div v-if="ok">
            <custom-dialog v-model:show="createVisible" :title="createDialogTitle">
                <create 
                    :createEmployeeUrl="createEmployeeUrl" 
                    :getAllDepartmentsUrl="getAllDepartmentsUrl">
                </create>
            </custom-dialog>
            <custom-dialog v-model:show="editVisible" :title="editDialogTitle">
                <edit 
                    :editEmployeeUrl="editEmployeeUrl" 
                    :id="id" 
                    :getAllDepartmentsUrl="getAllDepartmentsUrl" 
                    :getAllSkillsUrl="getAllSkillsUrl" 
                    :getSkillbookUrl="getAllForOneSkillbookUrl" 
                    :updateSkillbookUrl="updateSkillbookUrl">
                </edit>
            </custom-dialog>
            <custom-dialog v-model:show="deleteVisible" :title="deleteDialogTitle">
                <delete 
                    :deleteEmployeeUrl="deleteEmployeeUrl" 
                    :id="id">
                </delete>
            </custom-dialog>

            <h2>{{ department.Name }}</h2>

            <div class="d-flex justify-content-end my-3">
                <button type="button" class="brand-btn btn" @click="onCreateButtonClick()">Создать</button>
            </div>

            <custom-select :modelValue="limit" @changeOption="limit = $event.target.value" :options="itemsPerPage" :label="selectItemsPerPageLabel" @change="getData" />
            <custom-select :modelValue="selectedSort" @changeOption="selectedSort = $event.target.value" :options="sortOptions" :label="selectSortingLabel" @change="getData" />

            <div class="my-3">
                <h6>Фильтры</h6>
                <div class="w-100 align-items-center justify-content-start flex-wrap d-inline-flex">
                    <label class="form-label">Название</label>
                    <custom-input v-model="searchedName" class="w-25 mx-3" @input="getData" />
                </div>
                <label class="form-label">Навыки</label>
                <div class="w-100 align-items-top justify-content-start flex-wrap d-inline-flex">
                    <div class="w-25 mx-3">
                        <div v-for="skill in allSkills.slice(0, skillsLimit / 2)" :key="skill.Id">
                            <input type="checkbox" :value="skill.Id" v-model="searchedskills" @change="getData">
                            <label class="mx-2">{{ skill.Name }}</label>
                        </div>
                    </div>
                    <div class="w-25 mx-3">
                        <div v-for="skill in allSkills.slice(skillsLimit / 2, skillsLimit)" :key="skill.Id">
                            <input type="checkbox" :value="skill.Id" v-model="searchedskills" @change="getData">
                            <label class="mx-2">{{ skill.Name }}</label>
                        </div>
                    </div>
                </div>
                <page-number-display :total="skillsTotalPages" :current="skillsPageNumber" @changePage="changeSkillsPage" class="pagination-sm my-3" />
            </div>

            <table v-if="employees.length > 0" class="brand-table">
                <tr>
                    <th>
                        Имя
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
                            {{ employee.Salary }}
                        </td>
                        <td>
                            <div class="skill-cell">
                                <div v-for="skill in getEmployeeSkills(employee.Id)" :key="skill.Id">{{ skill.Name }}</div>
                            </div>
                        </td>
                        <td>
                            <button @click="onEditButtonClick(employee.Id)" class="brand-action-link">Редактировать</button><span class="brand-span">|</span>
                            <button @click="onDeleteButtonClick(employee.Id)" class="brand-action-link">Удалить</button><span class="brand-span">|</span>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div v-else>
                Поиск не дал результатов
            </div>

            <page-number-display :total="totalPages" :current="pageNumber" @changePage="changePage" />
            
            <div class="d-flex justify-content-start my-5">
                <button @click="onBackButtonClick()" class="brand-back-btn brand-btn btn">Назад</button>
            </div>
        </div>
        <div v-else>
            <div class="spinner-border text-dark"></div>
        </div>
    </div>
</template>

<script>
import * as axios from '@/custom_plugins/axiosApi.js';
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
        Delete,
    },

    data() {
        return {
            employees: [],
            department: null,
            employeeSkills: [],
            allSkills: [],
            searchedskills: [],
            ok: false,
            pageNumber: 1,
            totalPages: 0,
            limit: 0,
            skillsPageNumber: 1,
            skillsTotalPages: 0,
            skillsLimit: 10,
            selectedSort: '',
            searchedName: '',
            itemsPerPage: [
                { value: 5, name: '5' },
                { value: 10, name: '10' },
                { value: 20, name: '20' },
            ],
            sortOptions: [
                { value: 'Name ASC', name: 'Имя (По возрастанию)' },
                { value: 'Name DESC', name: 'Имя (По убыванию)' },
                { value: 'Salary ASC', name: 'Зарплата (По возрастанию)' },
                { value: 'Salary DESC', name: 'Зарплата (По убыванию)' },
            ],
            createDialogTitle: 'Создать',
            editDialogTitle: 'Редактировать',
            deleteDialogTitle: 'Удалить',
            createVisible: false,
            editVisible: false,
            deleteVisible: false,
            selectItemsPerPageLabel: 'Элементов на странице',
            selectSortingLabel: 'Сортировать по',

            viewByDepartmentIdUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_GET_EMPLOYEES_BY_DEPARTMENT_ID}`,
            getDepartmentUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_GET_DEPARTMENT}`,
            createEmployeeUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_CREATE_EMPLOYEE}`,
            editEmployeeUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_EDIT_EMPLOYEE}`,
            deleteEmployeeUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_DELETE_EMPLOYEE}`,
            getAllDepartmentsUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_GET_ALL_DEPARTMENTS}`,
            getAllForManySkillbookUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_GET_ALL_FOR_MANY_SKILLBOOK}`,
            getAllForOneSkillbookUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_GET_ALL_FOR_ONE_SKILLBOOK}`,
            getAllSkillsUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_GET_ALL_SKILLS}`,
            updateSkillbookUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_UPDATE_SKILLBOOK}`,
        }
    },

    methods: {
        changePage(page) {
            this.pageNumber = page;
            this.getData();
        },

        changeSkillsPage(page) {
            this.skillsPageNumber = page;
            this.getAllSkills();
        },

        onCreateButtonClick() {
            this.createVisible = true;
        },

        onEditButtonClick(id) {
            this.editVisible = true;
            this.id = id;
        },

        onDeleteButtonClick(id) {
            this.deleteVisible = true;
            this.id = id;
        },

        onBackButtonClick() {
            this.$router.push(`/Departments/`);
        },

        getEmployeeSkills(id) {
            return this.employeeSkills.filter(skill => skill.EmployeeId == id);
        },

        async init() {
            await axios.get(this.getDepartmentUrl, { id: this.$route.params.departmentId })
                    .then((response) => {
                        this.department = response.data.department;
                    })
                    .catch((error) => {
                        console.log(error);
                    });

            this.getData();
        },

        async getData() {
            await axios.get(this.viewByDepartmentIdUrl, { id: this.$route.params.departmentId, PageNumber: this.pageNumber, Limit: this.limit, OrderBy: this.selectedSort, SearchName: this.searchedName, SearchSkills: this.searchedskills })
                        .then((response) => {
                            this.employees = response.data.employees;
                            this.totalPages = response.data.totalPages;
                            this.ok = true;
                        })
                        .catch((error) => {
                            console.log(error);
                        });

            var employeeIds = this.employees.map(emp => emp.Id);

            await axios.get(this.getAllForManySkillbookUrl, { ids: employeeIds })
                    .then((response) => {
                        this.employeeSkills = response.data.skills;
                    })
                    .catch((error) => {
                        console.log(error);
                    });
        },

        async getAllSkills() {
            await axios.get(this.getAllSkillsUrl, { PageNumber: this.skillsPageNumber, Limit: this.skillsLimit })
                        .then((response) => {
                            this.allSkills = response.data.skills;
                            this.skillsTotalPages = response.data.totalPages;
                        })
                        .catch((error) => {
                            console.log(error);
                        });
        },
    },

    beforeMount() {
        this.limit = this.itemsPerPage[0].value;
        this.selectedSort = this.sortOptions[0].value;
        this.init();
        this.getAllSkills();
    }
}
</script>

<style scoped>
    .skill-cell {
        height: 70px;
        overflow: auto;
    }
</style>