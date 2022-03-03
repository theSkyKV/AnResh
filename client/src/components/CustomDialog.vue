<template>
    <div class="modal modal-sheet position-fixed d-block bg-secondary py-5" tabindex="-1" role="dialog" id="modalSheet" v-if="show">
        <div class="modal-dialog" role="document">
            <div class="modal-content rounded-6 shadow">
                <div class="modal-header border-bottom-0">
                    <h5 class="modal-title">{{ title }}</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" @click="hideDialog"></button>
                </div>
                <div class="modal-body py-0">
                    <slot></slot>
                </div>
                <div class="modal-footer flex-column border-top-0">
                    <button type="button" class="btn btn-lg btn-light w-100 mx-0" data-bs-dismiss="modal" @click="hideDialog">Закрыть</button>
                </div>
            </div>
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
            },
            title: String
        },

        methods: {
            hideDialog() {
                this.$emit('update:show', false);
            }
        }
    }
</script>