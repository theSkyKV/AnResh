<template>
    <!-- а как будет отображаться выбранный элемент, если он был отличным от "Выберите элемент"? -->
    <select @change="changeOption">
        <option selected disabled>Выберите элемент</option>
        <option v-for="option in options" :key="option.value" :value="option.value">{{ option.name }}</option>
    </select>
</template>

<script>
    export default {
        name: 'my-select',
        props: {
            modelValue: {
                type: String
            },
            options: {
                type: Array,
                default: () => []
            }
        },
        methods: {
            changeOption(event) {
                this.$emit('changeOption', event);
            }
        }
    }
</script>