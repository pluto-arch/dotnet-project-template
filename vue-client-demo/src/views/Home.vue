<template>
  <div class="home">
    <el-input v-model="user.name" style="width:250px" placeholder="user name"></el-input>
    <el-input v-model="user.tel" style="width:250px" placeholder="tel"></el-input>
    <br/>
    <el-button type="success" @click="createUser()">创建用户</el-button>
    <el-button type="info" @click='getList()'>获取列表</el-button>
    <el-button type="warning">警告按钮</el-button>
    <el-button type="danger">危险按钮</el-button>

    <el-table
      :data="userlist"
      style="width: 100%">
      <el-table-column
        prop="id"
        label="编号"
        width="180">
      </el-table-column>
      <el-table-column
        prop="name"
        label="名称"
        width="180">
      </el-table-column>
      <el-table-column
        prop="tel"
        label="电话">
      </el-table-column>
      <el-table-column
      fixed="right"
      label="操作"
      width="100">
      <template slot-scope="scope">
        <el-button @click="getUserInfo(scope.row.id)" type="text" size="small">查看</el-button>
      </template>
    </el-table-column>
    </el-table>

  </div>
</template>

<script>
import * as userapi from '../api/user'
import { Message } from 'element-ui';
export default {
  name: 'home',
  data() {
    return {
      user:{
        id:0,
        name:'',
        tel:0
      },
      userlist:[]
    }
  },
  components: {
  },
  methods: {
    getUserInfo(param){
      let v=this;
      userapi.getInfoRequest.id=param;
      userapi.getInfo(userapi.getInfoRequest).then(res=>{
        v.userlist=[{
           id:res.data.id,
           name:res.data.userName,
           tel:res.data.tel,
        }];
      }); 
    },
    createUser(){
      let v=this;
      userapi.createRequest.userName=v.user.name;
      userapi.createRequest.tel=v.user.tel;
      userapi.create(userapi.createRequest).then(res=>{
        if(res.isError){
          v.$Alert("error",res.msg);
        }else{
          v.$Alert("info",res.msg);
        }        
      }); 
    },
    getList(){
      let v=this;
      userapi.getListRequest.index=1;
      userapi.getListRequest.size=30;
      userapi.getList(userapi.getListRequest).then(res=>{
        if(res.isError){
          v.$Alert("error",res.msg);
        }else{
          for (let i = 0; i < res.data.length; i++) {
            v.userlist.push({
              id:res.data[i].id,
              name:res.data[i].userName,
              tel:res.data[i].tel
            })
          }
        }
      })
    }
  },
}
</script>
