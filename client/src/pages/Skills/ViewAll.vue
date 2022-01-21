<template>
    <div>
        <h2>Навыки</h2>
        <div v-if="ok">
            <custom-dialog v-model:show="dialogVisible">
                <create v-if="createVisible" :createSkillUrl="createSkillUrl"></create>
                <edit v-if="editVisible" :id="id" :editSkillUrl="editSkillUrl"></edit>
                <delete v-if="deleteVisible" :id="id" :deleteSkillUrl="deleteSkillUrl"></delete>
            </custom-dialog>

            <button @click="onCreateButtonClick" class="brand-btn btn">Создать</button>

            <div class="brand-div">
                Сортировка:
                <custom-select :modelValue="selectedSort" @changeOption="selectedSort = $event.target.value" :options="sortOptions" @change="getData" />
            </div>
            <div class="brand-div"><custom-input :modelValue="searchQuery" @updateInput="searchQuery = $event.target.value" @input="getData" /></div>
            <div class="brand-div">
                Элементов на странице:
                <custom-select :modelValue="limit" @changeOption="limit = $event.target.value" :options="itemsPerPage" @change="getData" />
            </div>
            <div class="brand-div">
                Найдено записей:
                {{ totalRecords }}
            </div>

            <table class="brand-table">
                <tr>
                    <th>
                        Название
                    </th>
                    <th></th>
                </tr>
                <tbody>
                    <tr v-for="skill in skills" :key="skill.Id">
                        <td>
                            {{ skill.Name }}
                        </td>
                        <td>
                            <button @click="onEditButtonClick(skill.Id)" class="brand-action-link">Редактировать</button><span class="brand-span">|</span>
                            <button @click="onDeleteButtonClick(skill.Id)" class="brand-action-link">Удалить</button><span class="brand-span">|</span>
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
import Create from '@/pages/Skills/Create.vue';
import Edit from '@/pages/Skills/Edit.vue';
import Delete from '@/pages/Skills/Delete.vue';

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
            skills: [],
            ok: false,
            id: 0,

            pageNumber: 1,
            limit: 0,
            totalPages: 0,
            totalRecords: 0,
            selectedSort: '',
            searchQuery: '',
            sortOptions: [
                { value: 'ByName', name: 'По названию' },
            ],
            itemsPerPage: [
                { value: 5, name: '5' },
                { value: 10, name: '10' },
                { value: 20, name: '20' },
            ],
          
            dialogVisible: false,
            createVisible: false,
            editVisible: false,
            deleteVisible: false,

            viewAllUrl: `${path.SERVER}${path.GET_ALL_SKILLS}`,
            createSkillUrl: `${path.SERVER}${path.CREATE_SKILL}`,
            editSkillUrl: `${path.SERVER}${path.EDIT_SKILL}`,
            deleteSkillUrl: `${path.SERVER}${path.DELETE_SKILL}`,
        }
    },

    methods: {
        changePage(page) {
            this.pageNumber = page;
            this.getData();
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
            await axios.get(this.viewAllUrl, { PageNumber: this.pageNumber, Limit: this.limit, SearchQuery: this.searchQuery, SelectedSort: this.selectedSort })
                       .then((response) => {
                           this.skills = response.data.skills;
                           this.totalPages = response.data.total.Pages;
                           this.totalRecords = response.data.total.Records;
                           this.ok = true;
                       })
                       .catch((error) => {
                           console.log(error);
                           this.$router.push(`/SignIn`);
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
        }
    },

    beforeMount() {
        this.limit = this.itemsPerPage[0].value;
        this.selectedSort = this.sortOptions[0].value;
        this.getData();
    }
}
</script>

<style scoped>

</style>