<template>
    <div>
        <h2>Редактировать</h2>
        <form :action="editDepartmentUrl" method="post">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-md-2">Название отдела</label>
                    <div class="col-md-10">
                        <input name="DepartmentName" class="form-control" :value="department.DepartmentName" />
                    </div>
                </div>

                <input type="hidden" name="DepartmentId" :value="department.DepartmentId" />

                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <input type="submit" value="Сохранить изменения" class="brand-btn btn" />
                    </div>
                </div>
            </div>
        </form>
        <a :href="indexDepartmentUrl" class="brand-back-btn brand-btn btn">Назад</a>
    </div>
</template>

<script>
    export default {
        props: {
            editDepartmentUrl: String,
            indexDepartmentUrl: String,
            department: Object
        }
    }
</script>