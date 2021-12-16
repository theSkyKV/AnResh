<template>
    <div>
        <h2>Навыки</h2>
        <div v-if="ok">
            <custom-dialog v-model:show="dialogVisible">
                <create v-if="createVisible" :createSkillUrl="createSkillUrl"></create>
                <edit v-if="editVisible" :id="id" :editSkillUrl="editSkillUrl"></edit>
                <delete v-if="deleteVisible" :id="id" :deleteSkillUrl="deleteSkillUrl"></delete>
            </custom-dialog>
            <button @click="onCreateButtonClick">Создать</button>
            <table>
                <tr>
                    <th>
                        Название
                    </th>
                    <th></th>
                </tr>
                <tbody v-for="skill in skills" :key="skill.Id">
                    <tr>
                        <td>
                            {{ skill.Name }}
                        </td>
                        <td>
                            <button @click="onEditButtonClick(skill.Id)">Редактировать</button><span>|</span>
                            <button @click="onDeleteButtonClick(skill.Id)">Удалить</button><span>|</span>
                        </td>
                    </tr>
                </tbody>
            </table>

            <div v-for="page in totalPages" :key="page" @click="changePage(page)">
                {{ page }}
            </div>
            <custom-select :modelValue="selectedSort" @changeOption="selectedSort = $event.target.value" :options="sortOptions" @change="getData" />
            <custom-input :modelValue="searchQuery" @updateInput="searchQuery = $event.target.value" @input="getData" />
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
import Create from '@/pages/Skills/Create.vue';
import Edit from '@/pages/Skills/Edit.vue';
import Delete from '@/pages/Skills/Delete.vue';

export default {
    components: {
        CustomDialog,
        CustomSelect,
        CustomInput,
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
            limit: 3,
            totalPages: 0,
            selectedSort: '',
            searchQuery: '',
            sortOptions: [
                { value: 'ById', name: 'По ID' },
                { value: 'ByName', name: 'По названию' },
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
            await axios.get(this.viewAllUrl, { pageNumber: this.pageNumber, limit: this.limit, searchQuery: this.searchQuery, selectedSort: this.selectedSort })
                       .then((response) => {
                           this.skills = response.data.skills;
                           this.totalPages = response.data.totalPages;
                           this.ok = true;
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
        }
    },

    beforeMount() {
        this.getData();
    }
}
</script>

<style scoped>

</style>