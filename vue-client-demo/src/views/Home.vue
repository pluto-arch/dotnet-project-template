<template>
	<el-container style="min-height: 100vh">
		<el-aside width="200px">
			<side-menu></side-menu>
		</el-aside>
		<el-container>
			<el-header>
				<el-button
					v-if="!isSignIned"
					size="mini"
					type="primary"
					@click="openSignInDialog()"
					>登录</el-button
				>
				<el-button
					v-if="isSignIned"
					size="mini"
					type="danger"
					@click="signOut()"
					>{{ loginUser }} | 注销</el-button
				>
			</el-header>
			<el-main>
				<router-view v-if="isRouterAlive"></router-view>
			</el-main>
			<el-footer>Footer</el-footer>
		</el-container>
		<signInDialog
			:traggerDialog="showSignDialog"
			@doalogClosed="closed"
		></signInDialog>
	</el-container>
</template>

<script>
import { Message } from 'element-ui';
import { getLoginedUser } from "../api/account";
import sideMenu from '../components/sideMenu.vue';
import signInDialog from "../components/signInDialog";
export default {
	name: 'home',
	provide () {
		return {
			reload: this.reload
		}
	},
	data () {
		return {
			showSignDialog: false,
			isSignIned: false,
			loginUser: '',
			isRouterAlive: true,
			user: {
				id: 0,
				name: '',
				tel: 0
			},
			userlist: []
		}
	},
	components: {
		sideMenu,
		signInDialog
	},
	mounted () {
		var v = this;
		let d = window.location.pathname;
		if (v.$Cookie.get('token')) {
			v.isSignIned = true;
			getLoginedUser().then(res => {
				v.loginUser = res.data;
			});
		}
	},
	methods: {
		openSignInDialog () {
			this.showSignDialog = true;
		},
		signOut () {
			this.$Cookie.remove('token');
			this.isSignIned = false;
		},
		closed (signed) {
			this.isSignIned = signed;
			this.showSignDialog = false;
			this.reload();
		},
		reload () {
			this.isRouterAlive = false
			this.$nextTick(function () {
				this.isRouterAlive = true
			})
		}
	},
}
</script>

<style lang="scss" scoped>
.el-header,
.el-footer {
	background-color: #566270;
	color: #333;
	text-align: right;
	line-height: 60px;
}

.el-main {
	background-color: #edf0f3;
}

body > .el-container {
	margin-bottom: 40px;
}
</style>
