<template>
    <div>
        <div v-if="ok">
            <h2>Удалить</h2>
            <h3>Вы действительно хотите удалить {{ department.Name }}?</h3>
            <div>
                <button @click="submit" class="brand-btn btn">Удалить</button>
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
            deleteDepartmentUrl: String,
        },

        data() {
            return {
                department: null,
                ok: false,
            }
        },

        methods: {
            async submit() {
                await axios.post(this.deleteDepartmentUrl, { Id: this.id })
                           .then(() => {
                               location.reload();
                           })
                           .catch((error) => {
                               console.log(error);
                               this.$router.push(`/SignIn`);
                           });
            },

            async init() {
                await axios.get(this.deleteDepartmentUrl, { Id: this.id },
                                { 
                                    'Authorization': sessionStorage.getItem("accessToken")
                                }
                            )
                           .then((response) => {
                               this.department = response.data.department;
                               this.ok = true;
                           })
                           .catch((error) => {
                               console.log(error);
                               this.$router.push(`/SignIn`);
                           });
            }
        },

        beforeMount() {
            this.init();
        }
    }
</script>