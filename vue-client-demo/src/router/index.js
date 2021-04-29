import Vue from "vue";
import VueRouter from "vue-router";
import Home from "../views/Home.vue";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    name: "Home",
    redirect: "dashboard"
  },
  {
    path: "/dashboard",
    name: "Dashboard",
    component: () => import("../views/Dashboard")
  },
  {
    path: "/permission",
    name: "Permission",
    component: () => import("../views/Permission")
  },
  {
    path: "/product",
    name: "Product",
    component: () => import("../views/Product")
  },
  {
    path: "/tenant",
    name: "Tenant",
    component: () => import("../views/Tenant")
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
});

export default router;
