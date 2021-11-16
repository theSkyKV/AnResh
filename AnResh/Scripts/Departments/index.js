import Vue from 'vue';
import CreateComponent from './create.vue';
import DeleteComponent from './delete.vue';
import EditComponent from './edit.vue';
import ViewAllComponent from './viewallwrapper.vue';
import ViewByIdComponent from './viewbyidwrapper.vue';
import DeleteErrorComponent from './deleteerror.vue';

//хорошим тоном будет 1 точка входа, которая умеет рисовать 1 корневой компонент, все остальное разруливать движком vue
new Vue({
    el: "#app",
    components: {
        CreateComponent,
        DeleteComponent,
        EditComponent,
        ViewAllComponent,
        ViewByIdComponent,
        DeleteErrorComponent
    }
})