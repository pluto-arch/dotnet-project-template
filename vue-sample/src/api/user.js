/* eslint-disable */

import request from '../utils/http'
import {getResponse} from '../api/base'
import { MessageBox, Message } from 'element-ui'


// 获取信息
export var getInfoRequest={
  id:0
};
function getInfo(params) {
  var result = request({
     url: '/default/getOne',
     method: 'get',
     params:params
  });
  return getResponse(result);
}
export {getInfo}


// 创建user
export var createRequest={
  userName: "",
  tel: ""
}
function create(params){
  var result=request({
    url:'/Default/createUser',
    method:'POST',
    data:params
  })
  return getResponse(result);
}
export {create}


// 获取列表
export var getListRequest={
  index:1,
  size:20
}
function getList(params){
  var result=request({
    url:'/Default/getList',
    method:'GET',
    data:params
  })
  return getResponse(result);
}
export {getList}
