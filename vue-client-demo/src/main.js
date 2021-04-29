import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import ElementUI from "element-ui";
import { AlertMessage } from "./utils/alert";
import "normalize.css/normalize.css";
import Cookies from "js-cookie";
import HttpRequest from "./utils/request";

Vue.config.productionTip = false;
Vue.prototype.$Alert = AlertMessage;
Vue.prototype.$Cookie = Cookies;

Vue.use(ElementUI);

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
