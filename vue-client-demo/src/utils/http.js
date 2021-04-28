/* eslint-disable */

import axios from 'axios';
import { MessageBox, Message } from 'element-ui'
import QS from 'qs'; 


// create an axios instance
const service = axios.create({
    baseURL: 'http://localhost:5000/api', // url = base url + request url
    // withCredentials: true, // send cookies when cross-domain requests
    timeout: 5000 // request timeout
  })


// 请求拦截器
service.interceptors.request.use(
    config => {
      config.data = JSON.stringify(config.data);
      config.headers = {
        'Content-Type' : 'application/json'
      }
      return config
    },
    error => {
      // do something with request error
      return Promise.reject(error)
    }
  )


// respone拦截器
service.interceptors.response.use(
    response => {
        return Promise.resolve(response);
    },
    error => {
        Message({
            message: error.message,
            type: 'error',
            duration: 5 * 1000
          })
      return Promise.reject(error)
    })

  
  export default service
  

