<template>
    <div class="dialog" v-if="show" @click="hideDialog">
        <div @click.stop class="dialog__content">
            <slot></slot>
            <button @click="hideDialog" class="brand-back-btn brand-btn btn">Отмена</button>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'custom-dialog',

        props: {
            show: {
                type: Boolean,
                default: false
            }
        },

        methods: {
            hideDialog() {
                this.$emit('update:show', false);
            }
        }
    }
</script>

<style scoped>
    .dialog {
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        background: rgba(0, 0, 0, 0.5);
        position: fixed;
        display: flex;
    }

    .dialog__content {
        margin: auto;
        padding: 20px;
        background: white;
        min-width: 300px;
        min-height: 200px;
    }
</style>