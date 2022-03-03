<template>
    <div>
        <div v-if="ok">
            <h4 class="mt-3 mb-5">Вы действительно хотите удалить {{ department.Name }}?</h4>
            <div class="text-danger my-3" v-for="error in v$.department.$errors" :key="error.$uid">{{ error.$message }}</div>
            <button type="button" class="brand-btn btn" @click="submit">Удалить</button>
        </div>
        <div v-else>
            <div class="spinner-border text-dark"></div>
        </div>
    </div>
</template>

<script>
    import * as axios from '@/custom_plugins/axiosApi.js';
    import * as validation from '@/custom_plugins/validation.js';
    import useVuelidate from '@vuelidate/core';

    export default {
        props: {
            id: Number,
            deleteDepartmentUrl: String,
        },

        setup () {
            return { v$: useVuelidate() }
        },

        data() {
            return {
                department: null,
                ok: false,
                vuelidateExternalResults: {
                    department: []
                }
            }
        },

        methods: {
            validate (status) {
                var errors = [];
                if (status == 401)
                    errors = { department: [validation.UNAUTHORIZED] };
                else
                    errors = { department: [validation.MUST_BE_EMPTY] };
                Object.assign(this.vuelidateExternalResults, errors);
            },

            async submit() {
                this.v$.$error = null;
                this.v$.$validate();
                
                if (this.v$.$error) {
  					return;
  				}

                await axios.post(this.deleteDepartmentUrl, { Id: this.id })
                           .then(() => {
                               location.reload();
                           })
                           .catch((error) => {
                               console.log(error);
                               this.validate(error.response.status);
                           });
            },

            async init() {
                await axios.get(this.deleteDepartmentUrl, { Id: this.id })
                           .then((response) => {
                               this.department = response.data.department;
                               this.ok = true;
                           })
                           .catch((error) => {
                               console.log(error);
                           });
            }
        },

        validations() {
            return {
                department: { department: validation.ok },
            }
        },

        beforeMount() {
            this.init();
        }
    }
</script>