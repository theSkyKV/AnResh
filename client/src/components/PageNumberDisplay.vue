<template>
    <div class="wrapper">
        <div class="list" v-if="total > 0">
            <div v-for="page in firstOnes" :key="page" class="item" :class="{'current': page == current}" @click="changePage(page)">{{ page }}</div>
            <div v-for="page in middleOnes" :key="page" class="item" :class="{'current': page == current}" @click="changePage(page)">{{ page }}</div>
            <div v-for="page in lastOnes" :key="page" class="item" :class="{'current': page == current}" @click="changePage(page)">{{ page }}</div>
        </div>
    </div>
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
            firstOnes: [],
            middleOnes: [],
            lastOnes: [],

            pagesNumberBetweenFirstAndLast: 0,
            pagesNumberBetweenFirstAndLastLimit: 3,
        }
    },

    methods: {
        changePage(page) {
            this.$emit('changePage', page)
        },

        reRender(current) {
            this.firstOnes = [];
            this.lastOnes = [];
            this.middleOnes = [];

            if (this.total <= 3) {
                for (var i = 1; i <= this.total; i++) {
                    this.middleOnes.push(i);
                }
                return;
            }

            if (current <= this.pagesNumberBetweenFirstAndLast) {
                for (var i = 1; i <= this.pagesNumberBetweenFirstAndLast + 1; i++) {
                    this.firstOnes.push(i);
                }
                this.lastOnes.push(this.total);
            }
            else if (current >= this.total - this.pagesNumberBetweenFirstAndLast + 1) {
                for (var i = this.total - this.pagesNumberBetweenFirstAndLast; i <= this.total; i++) {
                    this.lastOnes.push(i);
                }
                this.firstOnes.push(1);
            }
            else {
                for (var i = current - 1; i <= current + 1; i++) {
                    this.middleOnes.push(i);
                }
                this.firstOnes.push(1);
                this.lastOnes.push(this.total);
            }
        }
    },

    watch: {
        current(newValue) {
            this.reRender(newValue);
        },

        total(newValue) {
            while (newValue - this.pagesNumberBetweenFirstAndLast >= 2 && this.pagesNumberBetweenFirstAndLast < this.pagesNumberBetweenFirstAndLastLimit) {
                this.pagesNumberBetweenFirstAndLast++;
            }
            
            while (newValue - this.pagesNumberBetweenFirstAndLast < 2) {
                this.pagesNumberBetweenFirstAndLast--;
            }

            this.reRender(1);
        }
    },

    beforeMount() {
        this.pagesNumberBetweenFirstAndLast = this.pagesNumberBetweenFirstAndLastLimit;
        this.reRender(this.current);
    }
}
</script>

<style scoped>
    .wrapper {
        display: flex;
        align-items: center;
    }

    .list {
        display: flex;
        align-items: center;
    }

    .item {
        padding: 5px;
    }

    .current {
        text-decoration: underline;
    }
</style>