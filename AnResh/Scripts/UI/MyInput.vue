<template>
    <input :value="modelValue" @input="updateInput" type="text">
</template>

<script>
export default {
    name: 'my-input',

    props: {
        modelValue: [String, Number] //как тип влияет на поведение?
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