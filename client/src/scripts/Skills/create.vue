<template>
    <div>
        <h2>Добавить навык</h2>
        <form :action="createSkillUrl" method="post">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-md-2">Название навыка</label>
                    <div class="col-md-10">
                        <input name="SkillName" class="form-control" />
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <input type="submit" value="Добавить" class="brand-btn btn" />
                    </div>
                </div>
            </div>
        </form>
        <a :href="indexSkillUrl" class="brand-back-btn brand-btn btn">Назад</a>
    </div>
</template>

<script>
    export default {
        props: {
            createSkillUrl: String,
            indexSkillUrl: String
        }
    }
</script>