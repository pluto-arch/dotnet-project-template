<template>
	<div>
		<el-dialog
			title="提示"
			:visible.sync="showDialog"
			width="30%"
			destroy-on-close
			center
			:before-close="handleClose"
		>
			<el-form
				:label-position="labelPosition"
				label-width="80px"
				:model="formLabelAlign"
				ref="signInForm"
			>
				<el-form-item prop="user" label="登录名">
					<el-input
						v-model="formLabelAlign.user"
						placeholder="格式：<username>@<tenant_Id>"
					></el-input>
				</el-form-item>
				<el-form-item prop="role" label="用户角色">
					<el-input
						v-model="formLabelAlign.role"
						placeholder="用户角色"
					></el-input>
				</el-form-item>
				<el-form-item prop="userId" label="用户id">
					<el-input
						v-model="formLabelAlign.userId"
						placeholder="用户id"
					></el-input>
				</el-form-item>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="close()">取 消</el-button>
					<el-button type="primary" @click="submit()">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>
<script>
import { getToken } from "../api/account";
export default {
	props: {
		traggerDialog: Boolean
	},
	data () {
		return {
			showDialog: false,
			labelPosition: 'right',
			formLabelAlign: {
				user: '',
				role: '',
				userId: '',
				tenant: ''
			}
		};
	},
	mounted () {
		this.showDialog = this.traggerDialog;
	},
	methods: {
		close () {
			this.showDialog = false;
			this.$emit("doalogClosed");
		},
		handleClose () {
			this.showDialog = false;
			this.$emit("doalogClosed");
		},
		submit () {
			let v = this;
			getToken(this.formLabelAlign).then(res => {
				v.$Cookie.set("token", res.data);
				v.$Alert("success", "登录成功");
				this.showDialog = false;
				this.$refs['signInForm'].resetFields();
				this.$emit("doalogClosed", true);
			});
		}
	},
	watch: {
		traggerDialog: function (newValue, oldValue) {
			this.showDialog = newValue;
		}
	}
};
</script>
