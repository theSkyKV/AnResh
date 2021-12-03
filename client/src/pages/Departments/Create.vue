<template>
  <div>
        <h2>Создать клан</h2>
        <div>
            <div >
                <label>Название</label>
                <div>
                    <input name="Name" :value="name" @input="name = $event.target.value" oninput="this.value = this.value.replace(/\s+/g, ' ')" />
                </div>
            </div>

            <div>
                <button @click="submit">Создать</button>
            </div>
        </div>
    </div>
</template>

<script>
    import * as axios from '@/custom_plugins/axiosApi.js';

    export default {
        props: {
            createDepartmentUrl: String,
        },

        data() {
            return {
                name: ""
            }
        },

        methods: {
            async submit() {
                await axios.post(this.createDepartmentUrl, { Name: this.name })
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