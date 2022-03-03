<template>
    <div>
        <div v-if="ok">
            <label class="form-label">Название</label>
            <div>
                <custom-input v-model="name" :class="{'is-invalid': v$.name.$errors.length > 0}" />
                <div class="text-danger my-3" v-for="error in v$.name.$errors" :key="error.$uid">{{ error.$message }}</div>
            </div>

            <div class="text-danger my-3" v-for="error in v$.data.$errors" :key="error.$uid">{{ error.$message }}</div>
            
            <button type="button" class="brand-btn btn" @click="submit">Создать</button>
        </div>
        <div v-else>
            <div class="spinner-border text-dark"></div>
        </div>
    </div>
</template>

<script>
    import * as axios from '@/custom_plugins/axiosApi.js';
    import * as validation from '@/custom_plugins/validation.js';
    import CustomInput from '@/components/CustomInput.vue';
    import useVuelidate from '@vuelidate/core';
    import { required, helpers } from '@vuelidate/validators';

    export default {
        components: {
            CustomInput,
        },

        props: {
            createDepartmentUrl: String,
        },

        setup () {
            return { v$: useVuelidate() }
        },

        data() {
            return {
                ok: false,
                name: "",
                data: "",
                vuelidateExternalResults: {
                    name: [],
                    data: []
                }
            }
        },

        methods: {
            validate (status) {
                var errors = [];
                if (status == 401)
                    errors = { data: [validation.UNAUTHORIZED] };
                else
                    errors = { name: [validation.EXISTS] };
                
                Object.assign(this.vuelidateExternalResults, errors);
            },

            async init() {
                await axios.get(this.createDepartmentUrl)
                            .then(() => {
                                this.ok = true;
                            })
                            .catch((error) => {
                                console.log(error);
                            });
            },

            async submit() {
                this.v$.$error = null;
                this.v$.$validate();
                
                if (this.v$.$error) {
                    console.log(this.v$.name.$errors);
  					return;
  				}

                await axios.post(this.createDepartmentUrl, { Name: this.name })
                           .then(() => {
                               location.reload();
                           })
                           .catch((error) => {
                               console.log(error);
                               this.validate(error.response.status);
                           });
            },
        },

        validations() {
            return {
                name: { required, name: helpers.withMessage(validation.NAME_MESSAGE, validation.name) },
                data: { data: validation.ok },
            }
        },
        
        beforeMount() {
            this.init();
        }
    }
</script>