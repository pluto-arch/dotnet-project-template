<template>
	<div>
		<el-table :data="tableData" stripe style="width: 100%">
			<el-table-column prop="id" label="id" width="350"> </el-table-column>
			<el-table-column prop="name" label="名称" width="180"> </el-table-column>
			<el-table-column prop="remark" label="备注"></el-table-column>
			<el-table-column align="right" width="250">
				<template slot="header" slot-scope="scope">
					<el-input v-model="search" size="mini" placeholder="输入关键字搜索" />
				</template>
				<template slot-scope="scope">
					<el-button size="mini" @click="handleEdit(scope.$index, scope.row)"
						>查看</el-button
					>
				</template>
			</el-table-column>
		</el-table>
	</div>
</template>

<script>
import { getList } from "../api/product";
export default {
	data () {
		return {
			search: '',
			tableData: []
		};
	},
	mounted () {
		let v = this;
		getList().then(res => {
			v.tableData = [...res.data];
		});
	},
	methods: {
		handleEdit (index, row) {
			this.$Alert("info", `${row.name}`);
		}
	}
}
</script>

<style lang="scss" scoped>
</style>
