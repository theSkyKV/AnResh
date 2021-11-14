<template>
    <div>
        <h2>Ошибка при удалении</h2>

        <h3>Сначала удалите всех сотрудников из отдела.</h3>

        <a :href="indexDepartmentUrl" class="brand-back-btn brand-btn btn">Назад</a>
    </div>
</template>

<script>
    export default {
        props: {
            indexDepartmentUrl: String
        }
    }
</script>