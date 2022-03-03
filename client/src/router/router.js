import Home from '@/pages/Home.vue';
import ViewAllDepartments from '@/pages/Departments/ViewAll.vue';
import ViewAllEmployees from '@/pages/Employees/ViewAll.vue';
import ViewByDepartmentId from '@/pages/Employees/ViewByDepartmentId.vue';
import ViewAllSkills from '@/pages/Skills/ViewAll.vue';
import {createRouter, createWebHistory} from 'vue-router';

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
]

const router = createRouter({
    routes,
    history: createWebHistory(process.env.BASE_URL),
})

export default router;