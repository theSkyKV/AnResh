<template>
    <div>
        <h2>Удалить навык</h2>
        <h3>Вы действительно хотите удалить навык {{ skill.SkillName }}?</h3>
        <form :action="deleteSkillUrl" method="post">
            <div class="form-actions no-color">
                <input type="submit" value="Удалить" class="brand-btn btn" />
            </div>
        </form>
        <a :href="indexSkillUrl" class="brand-back-btn brand-btn btn">Назад</a>
    </div>
</template>

<script>
    export default {
        props: {
            deleteSkillUrl: String,
            indexSkillUrl: String,
            skill: Object
        }
    }
</script>