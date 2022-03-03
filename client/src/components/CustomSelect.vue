<template>
    <div class="d-flex my-4">
        <label class="form-label me-3">{{ label }}</label>
        <select class="form-select mb-3 w-25" @change="changeOption">
            <option v-for="option in options" :key="option.value" :value="option.value">{{ option.name }}</option>
        </select>
    </div>
</template>

<script>
    export default {
        name: 'custom-select',

        props: {
            modelValue: {
                type: [String, Number]
            },
            options: {
                type: Array,
                default: () => []
            },
            label: String
        },
        
        methods: {
            changeOption(event) {
                this.$emit('changeOption', event);
            }
        }
    }
</script>