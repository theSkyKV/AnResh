import Home from '@/pages/Home.vue';
import ViewAllDepartments from '@/pages/Departments/ViewAll.vue';
import ViewAllEmployees from '@/pages/Employees/ViewAll.vue';
import ViewByDepartmentId from '@/pages/Employees/ViewByDepartmentId.vue';
import ViewAllSkills from '@/pages/Skills/ViewAll.vue';
import SignIn from '@/pages/Auth/SignIn.vue';
import SignUp from '@/pages/Auth/SignUp.vue';
import {createRouter, createWebHashHistory} from 'vue-router';

const routes = [
    {
        path: '/',
        component: Home,
    },
    {
        path: '/Departments',
        component: ViewAllDepartments,
    },
    {
        path: '/Employees',
        component: ViewAllEmployees,
    },
    {
        path: '/Employees/:departmentId',
        component: ViewByDepartmentId,
    },
    {
        path: '/Skills',
        component: ViewAllSkills,
    },
    {
        path: '/SignIn',
        component: SignIn,
    },
    {
        path: '/SignUp',
        component: SignUp,
    },
]

const router = createRouter({
    routes,
    history: createWebHashHistory(process.env.BASE_URL)
})

export default router;