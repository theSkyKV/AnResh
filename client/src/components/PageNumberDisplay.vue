<template>
    <ul v-if="total > 0" class="pagination flex-wrap">
        <li class="page-item" :class="{'disabled': current == 1}">
            <a class="page-link" href="#" @click="changePage(1)">Первая</a>
        </li>
        <li v-for="page in pages" :key="page" class="page-item" :class="{'active': page == current}">
            <a class="page-link" href="#" @click="changePage(page)">{{ page }}</a>
        </li>
        <li class="page-item" :class="{'disabled': current == total}">
            <a class="page-link" href="#" @click="changePage(total)">Последняя</a>
        </li>
    </ul>
</template>

<script>
    export default {
        name: 'page-number-display',

        props: {
            total: Number,
            current: Number
        },

        data() {
            return {
                pages: [],
                pagesToShow: 3,
                minPagesToshow: 3,
                delta: 1,
            }
        },
        methods: {
            changePage(page) {
                this.$emit('changePage', page)
            },

            reRender(current) {
                this.pages = [];

                if (this.total <= this.pagesToShow) {
                    for (var i = 1; i <= this.total; i++) 
                        this.pages.push(i);
                    
                    return;
                }

                if (current <= this.pagesToShow - this.delta) 
                    for (var i = 1; i <= this.pagesToShow; i++) 
                        this.pages.push(i);
                else if (current > this.total - this.pagesToShow + this.delta) 
                    for (var i = this.total - this.pagesToShow + 1; i <= this.total; i++) 
                        this.pages.push(i);
                else 
                    for (var i = current - this.delta; i <= current + this.delta; i++) 
                        this.pages.push(i);
            }
        },

        watch: {
            current(newValue) {
                this.reRender(newValue);
            },
            
            total() {
                this.changePage(1);
                this.reRender(this.current);
            }
        },

        beforeMount() {
            this.reRender(this.current);
        }
    }
</script>