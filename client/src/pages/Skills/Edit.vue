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
                <span v-if="v$.name.$error" class="error-message">
                        {{ v$.name.$errors[0].$message }}
                </span>

                <input type="hidden" name="Id" :value="id" />

                <div>
                    <button @click="submit" class="brand-btn btn">Сохранить</button>
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
    import * as validate from '@/custom_plugins/validate.js';

    import useValidate from "@vuelidate/core";
    import { required, helpers } from "@vuelidate/validators";

    export default {
        props: {
            id: Number,
            editSkillUrl: String,
        },

        data() {
            return {
                v$: useValidate(),

                skill: null,
                ok: false,
                name: "",
            }
        },

        methods: {
            async submit() {
                this.v$.$validate();
                
                if (this.v$.$error) {
  					return;
  				}

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

        validations() {
            return {
                name: { required, skillName: helpers.withMessage(validate.SKILL_NAME_MESSAGE, validate.skillName) },
            }
        },

        beforeMount() {
            this.init();
        }
    }
</script>

<style scoped>
    
</style>