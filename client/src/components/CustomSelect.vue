<template>
    <select @change="changeOption">
        <option v-for="option in options" :key="option.value" :value="option.value">{{ option.name }}</option>
    </select>
</template>

<script>
    export default {
        name: 'custom-select',
        props: {
            modelValue: {
                type: [String, Number] //Если оставить просто string, то при передаче сюда числа, будет warning, но произойдет автоматическое приведение типов
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