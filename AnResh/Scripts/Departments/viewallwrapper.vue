<template>
    <div>
        <my-select :modelValue="selectedSort" @changeOption="selectedSort = $event.target.value" :options="sortOptions" />
        <my-input :modelValue="searchQuery" @updateInput="searchQuery = $event.target.value" />
        <view-all 
                  :departments="sortedAndSearchedDepartments" 
                  :createDepartmentUrl="createDepartmentUrl"
                  :editDepartmentUrl="editDepartmentUrl"
                  :deleteDepartmentUrl="deleteDepartmentUrl"
                  :detailsDepartmentUrl="detailsDepartmentUrl"
                  />
    </div>
</template>

<script>
    import ViewAll from './viewall.vue';
    //есть смысл сконфигурировать переменную, которая бы смотрела на корень /Scripts
    //обрати внимание на webpack.config module.exports -> resolve -> alias -> @
    //относительные пути плохо при рефактиринге, придется считать количество выходов вверх по каталогам
    import MySelect from '../UI/MySelect.vue';
    import MyInput from '../UI/MyInput.vue';

    export default {
        components: {
            ViewAll,
            MySelect,
            MyInput
        },

        props: {
            createDepartmentUrl: String,
            editDepartmentUrl: String,
            deleteDepartmentUrl: String,
            detailsDepartmentUrl: String,
            departments: Array
        },

        data() {
            return {
                selectedSort: '',
                searchQuery: '',
                sortOptions: [
                    { value: 'DepartmentName', name: 'По отделу' },
                ]
            }
        },

        methods: {
            compare(el1, el2) {
                if (el1 > el2) {
                    return 1;
                }
                else if (el1 < el2) {
                    return -1;
                }
                else {
                    return 0;
                }
            },
        },

        computed: {
            //можно унести в методы, истпользуется в таком же computed, нет смысла делать observable
            sortedDepartments() {
                //сортировка должна быть серверная
                return [...this.departments].sort((el1, el2) => this.compare(el1[this.selectedSort], el2[this.selectedSort]))
            },

            sortedAndSearchedDepartments() {
                //фильтрация должна быть серверная
                return this.sortedDepartments.filter(dept => dept.DepartmentName.toLowerCase().includes(this.searchQuery.toLowerCase()));
            }
        },
    }
</script>