<template>
    <div>
        <h2>Редактировать</h2>
        <form :action="editEmployeeUrl" method="post">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-md-2">ФИО</label>
                    <div class="col-md-10">
                        <input name="EmployeeName" class="form-control" :value="employee.EmployeeName" />
                    </div>
                </div>

                <input type="hidden" name="EmployeeId" :value="employee.EmployeeId" />

                <div class="form-group">
                    <label class="control-label col-md-2">ID отдела</label>
                    <div class="col-md-10">
                        <input name="DepartmentId" class="form-control" :value="employee.DepartmentId" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-md-2">Зарплата</label>
                    <div class="col-md-10">
                        <input name="Salary" class="form-control" :value="employee.Salary" />
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