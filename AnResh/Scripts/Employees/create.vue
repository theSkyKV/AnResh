<template>
    <div>
        <h2>Добавить сотрудника</h2>
        <form :action="createEmployeeUrl" method="post">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-md-2">ФИО</label>
                    <div class="col-md-10">
                        <input name="EmployeeName" class="form-control" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-md-2">ID отдела</label>
                    <div class="col-md-10">
                        <input name="DepartmentId" class="form-control" :value="employee.DepartmentId" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-md-2">Зарплата</label>
                    <div class="col-md-10">
                        <input name="Salary" class="form-control" />
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <input type="submit" value="Добавить" class="brand-btn btn" />
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
            createEmployeeUrl: String,
            indexEmployeeUrl: String,
            employee: Object
        }
    }
</script>