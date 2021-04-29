
<template>
	<div>
		<div class="app-logo">PNCT</div>
		<el-menu
			:default-active="defaultActive"
			:collapse="isCollapse"
			active-text-color="#30A9DE"
			:router="true"
		>
			<el-menu-item index="/dashboard" route="dashboard">
				<template>
					<i class="el-icon-s-home"></i>
					<span>Dashboard</span>
				</template>
			</el-menu-item>
			<el-submenu index="2">
				<template #title>
					<i class="el-icon-location"></i>
					<span>产品管理</span>
				</template>
				<el-menu-item index="/product" route="product">产品列表</el-menu-item>
			</el-submenu>

			<el-submenu index="3">
				<template #title>
					<i class="el-icon-setting"></i>
					<span>权限管理</span>
				</template>
				<el-menu-item index="/permission" route="permission"
					>权限列表</el-menu-item
				>
			</el-submenu>

			<el-submenu index="4">
				<template #title>
					<i class="el-icon-menu"></i>
					<span>租户管理</span>
				</template>
				<el-menu-item index="/tenant" route="tenant">租户列表</el-menu-item>
			</el-submenu>
		</el-menu>
	</div>
</template>

<style lang="scss" scoped>
.app-logo {
	text-align: center;
	line-height: 60px;
	color: white;
	background-color: #566270;
}
</style>

<script>
export default {
	data () {
		return {
			isCollapse: false,
			menuIndex: 'dashboard'
		};
	},
	mounted () {

	},
	methods: {

	},
	computed: {
		defaultActive () {
			var path = '/' + this.$route.path.split('/').reverse()[0];
			console.log(path, "defaultActive");
			return path;
		}
	}
}
</script>
