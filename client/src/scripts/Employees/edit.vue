<template>
    <div>
        <h2>Редактировать</h2>
        <form :action="editEmployeeUrl" method="post">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-md-2">ФИО</label>
                    <div class="col-md-10">
                        <input name="employeeName" class="form-control" :value="employee.EmployeeName" />
                    </div>
                </div>

                <input type="hidden" name="employeeId" :value="employee.EmployeeId" />

                <div class="form-group">
                    <label class="control-label col-md-2">Отдел</label>
                    <div class="col-md-10">
                        <select name="departmentId" class="form-control">
                            <option v-for="department in employee.Departments" :key="department.DepartmentId"
                                    :value="department.DepartmentId" v-if="department.DepartmentId==employee.DepartmentId" selected>
                                {{ department.DepartmentName }}
                            </option>
                            <option :value="department.DepartmentId" v-else>
                                {{ department.DepartmentName }}
                            </option>
                        </select>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-md-2">Зарплата</label>
                    <div class="col-md-10">
                        <input name="salary" class="form-control" :value="employee.Salary" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-md-2">Навыки</label>
                    <div class="col-md-10">
                        <select multiple name="skills[]" class="form-control">
                            <option v-for="skill in employee.Skills" :key="skill.SkillId"
                                    :value="skill.SkillId" v-if="skill.IsLearned" selected>
                                {{ skill.SkillName }}
                            </option>
                            <option :value="skill.SkillId" v-else>
                                {{ skill.SkillName }}
                            </option>
                        </select>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <input type="submit" value="Сохранить изменения" class="brand-btn btn" />
                    </div>
                </div>
            </div>
        </form>
        <a :href="indexEmployeeUrl + '/' + employee.DepartmentId" class="brand-back-btn brand-btn btn">Назад</a>
    </div>
</template>

<script>
    export default {
        props: {
            editEmployeeUrl: String,
            indexEmployeeUrl: String,
            employee: Object
        }
    }
</script>