<template>
    <div>
        <div v-if="ok">
            <h2>Редактировать</h2>
            <div>
                <div>
                    <label>Название</label>
                    <div>
                        <input name="Name" v-model="name" oninput="this.value = this.value.replace(/\s+/g, ' ')" />
                    </div>
                </div>

                <input type="hidden" name="Id" :value="id" />

                <div>
                    <button @click="submit">Сохранить</button>
                </div>
            </div>
        </div>
        <div v-else>
            Загрузка...
        </div>
    </div>
</template>

<script>
    import * as axios from '@/custom_plugins/axiosApi.js';

    export default {
        props: {
            id: Number,
            editSkillUrl: String,
        },

        data() {
            return {
                skill: null,
                ok: false,
                name: "",
            }
        },

        methods: {
            async submit() {
                await axios.post(this.editSkillUrl, { Id: this.id, Name: this.name })
                           .then(() => {
                               location.reload();
                           })
                           .catch((error) => {
                               console.log(error);
                           });
            },

            async init() {
                await axios.get(this.editSkillUrl, { id: this.id })
                            .then((response) => {
                                this.skill = response.data.skill;
                                this.name = this.skill.Name;
                                this.ok = true;
                            })
                            .catch((error) => {
                                console.log(error);
                            });
            }
        },

        beforeMount() {
            this.init();
        }
    }
</script>

<style scoped>
    
</style>