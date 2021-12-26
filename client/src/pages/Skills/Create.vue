<template>
  <div>
        <h2>Создать навык</h2>
        <div>
            <div >
                <label>Название</label>
                <div>
                    <input name="Name" v-model="name" oninput="this.value = this.value.replace(/\s+/g, ' ')" />
                </div>
            </div>

            <div>
                <button @click="submit" class="brand-btn btn">Создать</button>
            </div>
        </div>
    </div>
</template>

<script>
    import * as axios from '@/custom_plugins/axiosApi.js';

    export default {
        props: {
            createSkillUrl: String,
        },

        data() {
            return {
                name: ""
            }
        },

        methods: {
            async submit() {
                await axios.post(this.createSkillUrl, { Name: this.name })
                           .then(() => {
                               location.reload();
                           })
                           .catch((error) => {
                               console.log(error);
                           });
            }
        }
    }
</script>

<style scoped>
    
</style>