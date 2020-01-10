import { MessageBox, Message } from 'element-ui'

export function AlertMessage(type,message){
    Message({
        type:type,
        message:message,
        duration:5000
      });
}