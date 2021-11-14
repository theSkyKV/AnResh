<template>
    <div>
        <h2>Навыки</h2>

        <table class="brand-table">
            <tr>
                <th>
                    Навык
                </th>
                <th></th>
            </tr>
            <tbody v-for="skill in skills" :key="skill.SkillId">
                <tr v-if="isEven">
                    <td class="even">
                        {{ skill.SkillName }}
                    </td>
                    <td class="even">
                        <a :href="deleteSkillUrl + '/' + skill.SkillId + '?name=' + skill.SkillName" class="brand-action-link">Удалить</a>
                    </td>
                </tr>
                <tr v-else>
                    <td>
                        {{ skill.SkillName }}
                    </td>
                    <td>
                        <a :href="deleteSkillUrl + '/' + skill.SkillId + '?name=' + skill.SkillName" class="brand-action-link">Удалить</a>
                    </td>
                </tr>
            </tbody>
        </table>
        <a :href="createSkillUrl" class="brand-btn btn">Добавить навык</a>
    </div>
</template>

<script>
    export default {
        props: {
            skills: Array,
            createSkillUrl: String,
            deleteSkillUrl: String
        },

        data() {
            return {
                isEven: false
            }
        }
    }
</script>