<template>
    <input class="form-control form-control-lg mb-3" type="text" :value="modelValue" @input="updateInput">
</template>

<script>
    export default {
        name: 'custom-input',

        props: {
            modelValue: {
                    type: [String, Number]
                }
        },

        methods: {
            updateInput(event) {
                this.$emit('update:modelValue', event.target.value);
            }
        }
    }
</script>