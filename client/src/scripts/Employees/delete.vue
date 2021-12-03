<template>
    <div>
        <h2>Удалить сотрудника</h2>
        <h3>Вы действительно хотите удалить сотрудника {{ employee.EmployeeName }}?</h3>
        <form :action="deleteEmployeeUrl + '?id=' + employee.EmployeeId + 
                                           '&departmentId=' + employee.DepartmentId" 
              method="post">
            <div class="form-actions no-color">
                <input type="submit" value="Удалить" class="brand-btn btn" />
            </div>
        </form>
        <a :href="indexEmployeeUrl + '/' + employee.DepartmentId" class="brand-back-btn brand-btn btn">Назад</a>
    </div>
</template>

<script>
    export default {
        props: {
            deleteEmployeeUrl: String,
            indexEmployeeUrl: String,
            employee: Object
        }
    }
</script>