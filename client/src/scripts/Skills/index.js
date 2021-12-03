import Vue from 'vue';
import CreateComponent from './create.vue';
import DeleteComponent from './delete.vue';
import ViewAllComponent from './viewall.vue';

new Vue({
    el: "#app",
    components: {
        CreateComponent,
        DeleteComponent,
        ViewAllComponent
    }
})