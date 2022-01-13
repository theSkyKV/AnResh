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
                    <ul class="brand-header-menu__list" v-if="$store.state.payload == null">
                        <li><router-link to="/SignUp" class="brand-header-menu__link">Зарегистрироваться</router-link></li>
                        <li><router-link to="/SignIn" class="brand-header-menu__link">Войти</router-link></li>
                    </ul>
                    <ul class="brand-header-menu__list" v-else>
                        <li><div class="brand-header-menu__link">{{ $store.state.payload["Name"] }}</div></li>
                        <li><div class="brand-header-menu__link">Выйти</div></li>
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
export default {
    
}
</script>

<style src="@/content/bootstrap.css"></style>
<style src="@/content/brand.css"></style>