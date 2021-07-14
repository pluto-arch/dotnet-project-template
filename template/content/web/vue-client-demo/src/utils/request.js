import http from "./http";
import QS from "qs";

/**
 * get方法，对应get请求
 * @param {String} url [请求的url地址]
 * @param {Object} params [请求时携带的参数]
 */
export const get = (url, params) => {
  return new Promise((resolve, reject) => {
    http
      .get(url, {
        params: params
      })
      .then(res => {
        resolve(res.data);
      })
      .catch(err => {
        reject(err);
      });
  });
};
/**
 * post方法，对应post请求
 * @param {String} url [请求的url地址]
 * @param {Object} params [请求时携带的参数]
 */
export const post = (url, ...params) => {
  return new Promise((resolve, reject) => {
    http
      .post(url, QS.stringify(...params))
      .then(res => {
        resolve(res.data);
      })
      .catch(err => {
        reject(err.data);
      });
  });
};
