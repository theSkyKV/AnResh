<template>
    <div>
        <div v-if="ok">
            <h2>Удалить</h2>
            <h3>Вы действительно хотите удалить {{ employee.Name }}?</h3>
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
            deleteEmployeeUrl: String,
        },

        data() {
            return {
                employee: null,
                ok: false,
            }
        },

        methods: {
            async submit() {
                await axios.post(this.deleteEmployeeUrl, { Id: this.id })
                            .then(() => {
                                location.reload();
                            })
                            .catch((error) => {
                                console.log(error);
                            });
            },

            async init() {
                await axios.get(this.deleteEmployeeUrl, { id: this.id })
                            .then((response) => {
                                this.employee = response.data.employee;
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