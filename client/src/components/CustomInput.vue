<template>
    <input :value="modelValue" @input="updateInput" type="text" placeholder="Поиск...">
</template>

<script>
export default {
    name: 'custom-input',

    props: {
        modelValue: {
                type: String
            },
    },

    methods: {
        updateInput(event) {
            this.$emit('updateInput', event)
        }
    }
}
</script>

<style scoped>

</style>