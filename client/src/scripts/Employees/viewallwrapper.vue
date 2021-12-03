<template>
    <div>
        <my-select :modelValue="selectedSort" @changeOption="selectedSort = $event.target.value" :options="sortOptions" />
        <my-input :modelValue="searchQuery" @updateInput="searchQuery = $event.target.value" />
        <view-all :employees="sortedAndSearchedEmployees" />
    </div>
</template>

<script>
    import ViewAll from './viewall.vue';
    import MySelect from '../UI/MySelect.vue';
    import MyInput from '../UI/MyInput.vue';

    export default {
        components: {
            ViewAll,
            MySelect,
            MyInput
        },

        props: {
            employees: Array
        },

        data() {
            return {
                selectedSort: '',
                searchQuery: '',
                sortOptions: [
                    { value: 'EmployeeName', name: 'По имени' },
                    { value: 'DepartmentName', name: 'По отделу' },
                    { value: 'Skills', name: 'По навыкам' },
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

            filterSkills() {
                var filtred = [];
                var allLearnedSkills = [];
                var employeeLearnedSkills = "";
                for (var i = 0; i < this.employees.length; i++) {
                    employeeLearnedSkills = this.employees[i].LearnedSkills.join(" ");
                    allLearnedSkills.push(employeeLearnedSkills);
                    employeeLearnedSkills = [];
                }

                for (var i = 0; i < allLearnedSkills.length; i++) {
                    if (allLearnedSkills[i].toLowerCase().includes(this.searchQuery.toLowerCase())) {
                        filtred.push(this.employees[i]);
                    }
                }

                return filtred;
            }
        },

        computed: {
            sortedEmployees() {
                return [...this.employees].sort((el1, el2) => this.compare(el1[this.selectedSort], el2[this.selectedSort]))
            },

            sortedAndSearchedEmployees() {
                switch (this.selectedSort) {
                    case 'EmployeeName':
                        return this.sortedEmployees.filter(emp => emp.EmployeeName.toLowerCase().includes(this.searchQuery.toLowerCase()));
                        break;
                    case 'DepartmentName':
                        return this.sortedEmployees.filter(emp => emp.DepartmentName.toLowerCase().includes(this.searchQuery.toLowerCase()));
                        break;
                    case 'Skills':
                        return this.filterSkills();
                        break;
                    default:
                        return this.sortedEmployees.filter(emp => emp.EmployeeName.toLowerCase().includes(this.searchQuery.toLowerCase()));
                        break;
                }
            }
        },
    }
</script>