<template>
    <div>
        <my-select :modelValue="selectedSort" @changeOption="selectedSort = $event.target.value" :options="sortOptions" />
        <my-input :modelValue="searchQuery" @updateInput="searchQuery = $event.target.value" />
        <view-by-id :employees="sortedAndSearchedEmployees" 
                    :createEmployeeUrl="createEmployeeUrl"
                    :editEmployeeUrl="editEmployeeUrl"
                    :deleteEmployeeUrl="deleteEmployeeUrl"
                    :detailsEmployeeUrl="detailsEmployeeUrl"
                    :indexDepartmentUrl="indexDepartmentUrl"
                    :departmentName="departmentName"
                    :departmentId="departmentId"
                    />
    </div>
</template>

<script>
    import ViewById from './viewbyid.vue';
    import MySelect from '../UI/MySelect.vue';
    import MyInput from '../UI/MyInput.vue';

    export default {
        components: {
            ViewById,
            MySelect,
            MyInput
        },

        props: {
            createEmployeeUrl: String,
            editEmployeeUrl: String,
            deleteEmployeeUrl: String,
            detailsEmployeeUrl: String,
            indexDepartmentUrl: String,
            employees: Array,
            departmentName: String,
            departmentId: Number
        },

        data() {
            return {
                selectedSort: '',
                searchQuery: '',
                sortOptions: [
                    { value: 'EmployeeName', name: 'По имени' },
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