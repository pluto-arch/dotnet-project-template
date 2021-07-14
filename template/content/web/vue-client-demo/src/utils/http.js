/* eslint-disable */

import axios from "axios";
import { MessageBox, Message } from "element-ui";
import Cookies from "js-cookie";
import QS from "qs";

// create an axios instance
const service = axios.create({
  baseURL: "http://localhost:8000/api", // url = base url + request url
  // withCredentials: true, // send cookies when cross-domain requests
  timeout: 5000 // request timeout
});

// 请求拦截器
service.interceptors.request.use(
  config => {
    let token = Cookies.get("token");
    config.data = JSON.stringify(config.data);
    config.headers = {
      "Content-Type": "application/json"
    };
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  error => {
    // do something with request error
    return Promise.reject(error);
  }
);

// respone拦截器
service.interceptors.response.use(
  response => {
    return Promise.resolve(response);
  },
  error => {
    if (error.response) {
      let resp = error.response;
      switch (resp.status) {
        case 401:
          Message({
            message: "登录信息已丢失，请重新登录",
            type: "error",
            duration: 5 * 1000
          });
          Cookies.remove("token");
          return Promise.reject(error);
        case 403:
          Message({
            message: "权限不足",
            type: "error",
            duration: 5 * 1000
          });
          return Promise.reject(error);
        default:
          Message({
            message: resp.data.message,
            type: "error",
            duration: 5 * 1000
          });
          return Promise.reject(error);
      }
    }
  }
);

export default service;
