<template>
  <div class="app">
    <div class="brand-wrapper">
        <header class="brand-header">
            <div class="brand-header__container brand-container">
                <div class="logo">
                    <div class="logo__label">
                        <span class="_icon-o"></span>
                        <span class="_icon-t"></span>
                        <span class="_icon-c"></span>
                    </div>
                </div>
                <div class="brand-header__menu brand-header-menu">
                    <ul class="brand-header-menu__list" v-if="$store.getters.getPayload == null">
                        <li><a href="#" class="brand-header-menu__link" @click="onSignUpButtonClick">Зарегистрироваться</a></li>
                        <li><a href="#" class="brand-header-menu__link" @click="onSignInButtonClick">Войти</a></li>
                    </ul>
                    <ul class="brand-header-menu__list" v-else>
                        <li><div class="brand-header-menu__link">{{ $store.getters.getPayload["Name"] }}</div></li>
                        <li><a href="#" class="brand-header-menu__link" @click="this.$store.commit('deletePayload')">Выйти</a></li>
                    </ul>
                </div>
            </div>
        </header>
        <main class="brand-main">
            <div class="brand-main__container brand-container">
                <aside class="brand-aside">
                    <div class="brand-aside__container brand-container">
                        <div class="brand-aside__menu brand-side-menu">
                            <ul class="brand-side-menu__list">
                                <li><router-link to="/Departments" class="brand-side-menu__link">Отделы</router-link></li>
                                <li><router-link to="/Employees" class="brand-side-menu__link">Сотрудники</router-link></li>
                                <li><router-link to="/Skills" class="brand-side-menu__link">Навыки</router-link></li>
                            </ul>
                        </div>
                    </div>
                </aside>
                <div class="brand-content">
                    <div class="brand-content__container brand-container">
                        <custom-dialog v-model:show="signInVisible" :title="signInDialogTitle">
                            <sign-in></sign-in>
                        </custom-dialog>
                        <custom-dialog v-model:show="signUpVisible" :title="signUpDialogTitle">
                            <sign-up></sign-up>
                        </custom-dialog>

                        <router-view></router-view>
                    </div>
                </div>
            </div>
        </main>
        <footer class="brand-footer">
        </footer>
    </div>
  </div>
</template>

<script>
    import CustomDialog from '@/components/CustomDialog.vue';
    import SignIn from '@/pages/Auth/SignIn.vue';
    import SignUp from '@/pages/Auth/SignUp.vue';

    export default {
        components: {
            CustomDialog,
            SignIn,
            SignUp
        },

        data() {
            return {
                signInDialogTitle: 'Авторизация',
                signUpDialogTitle: 'Регистрация',
                signInVisible: false,
                signUpVisible: false,
            }
        },

        methods: {
            onSignInButtonClick() {
                this.signInVisible = true;
            },

            onSignUpButtonClick() {
                this.signUpVisible = true;
            },
        }
    }
</script>

<style src="@/content/brand.css"></style>