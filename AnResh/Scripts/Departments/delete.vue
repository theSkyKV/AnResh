<template>
    <div>
        <h2>Удалить отдел</h2>
        <h3>Вы действительно хотите удалить отдел {{ department.DepartmentName }}?</h3>
        <form :action="deleteDepartmentUrl" method="post">
            <div class="form-actions no-color">
                <input type="submit" value="Удалить" class="brand-btn btn" />
            </div>
        </form>
        <a :href="indexDepartmentUrl" class="brand-back-btn brand-btn btn">Назад</a>
    </div>
</template>

<script>
    export default {
        props: {
            deleteDepartmentUrl: String,
            indexDepartmentUrl: String,
            department: Object
        }
    }
</script>