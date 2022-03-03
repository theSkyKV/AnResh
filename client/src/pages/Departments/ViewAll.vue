<template>
    <div>
        <h2>Отделы</h2>
        <div v-if="ok">
            <custom-dialog v-model:show="createVisible" :title="createDialogTitle">
                <create :createDepartmentUrl="createDepartmentUrl"></create>
            </custom-dialog>
            <custom-dialog v-model:show="editVisible" :title="editDialogTitle">
                <edit :editDepartmentUrl="editDepartmentUrl" :id="id"></edit>
            </custom-dialog>
            <custom-dialog v-model:show="deleteVisible" :title="deleteDialogTitle">
                <delete :deleteDepartmentUrl="deleteDepartmentUrl" :id="id"></delete>
            </custom-dialog>

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
            </div>

            <table v-if="departments.length > 0" class="brand-table">
                <tr>
                    <th>
                        Название
                    </th>
                    <th>
                        Средняя зарплата
                    </th>
                    <th></th>
                </tr>
                <tbody >
                    <tr v-for="department in departments" :key="department.Id">
                        <td>
                            {{ department.Name }}
                        </td>
                        <td>
                            {{ department.AverageSalary }}
                        </td>
                        <td>
                            <button @click="onEditButtonClick(department.Id)" class="brand-action-link">Редактировать</button><span class="brand-span">|</span>
                            <button @click="onDeleteButtonClick(department.Id)" class="brand-action-link">Удалить</button><span class="brand-span">|</span>
                            <button @click="onDetailsButtonClick(department.Id)" class="brand-action-link">Посмотреть информацию</button><span class="brand-span">|</span>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div v-else>
                Поиск не дал результатов
            </div>

            <page-number-display :total="totalPages" :current="pageNumber" @changePage="changePage" />

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
import Create from '@/pages/Departments/Create.vue';
import Edit from '@/pages/Departments/Edit.vue';
import Delete from '@/pages/Departments/Delete.vue';

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
            departments: [],
            ok: false,
            id: 0,
            pageNumber: 1,
            totalPages: 0,
            limit: 0,
            selectedSort: '',
            searchedName: '',
            itemsPerPage: [
                { value: 5, name: '5' },
                { value: 10, name: '10' },
                { value: 20, name: '20' },
            ],
            sortOptions: [
                { value: 'Name ASC', name: 'Название (По возрастанию)' },
                { value: 'Name DESC', name: 'Название (По Убыванию)' },
            ],

            createDialogTitle: 'Создать',
            editDialogTitle: 'Редактировать',
            deleteDialogTitle: 'Удалить',
            createVisible: false,
            editVisible: false,
            deleteVisible: false,
            selectItemsPerPageLabel: 'Элементов на странице',
            selectSortingLabel: 'Сортировать по',

            viewAllUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_GET_ALL_DEPARTMENTS}`,
            createDepartmentUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_CREATE_DEPARTMENT}`,
            editDepartmentUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_EDIT_DEPARTMENT}`,
            deleteDepartmentUrl: `${process.env.VUE_APP_SERVER}${process.env.VUE_APP_DELETE_DEPARTMENT}`,
        }
    },


    methods: {
        changePage(page) {
            this.pageNumber = page;
            this.getData();
        },

        onDetailsButtonClick(id) {
            this.$router.push(`/Employees/${id}`);
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

        async getData() {
            await axios.get(this.viewAllUrl, { PageNumber: this.pageNumber, Limit: this.limit, OrderBy: this.selectedSort, SearchName: this.searchedName })
                        .then((response) => {
                            this.departments = response.data.departments;
                            this.totalPages = response.data.totalPages;
                            this.ok = true;
                        })
                        .catch((error) => {
                            console.log(error);
                        });
        },
    },

    beforeMount() {
        this.limit = this.itemsPerPage[0].value;
        this.selectedSort = this.sortOptions[0].value;
        this.getData();
    }
}
</script>