<template>
	<div>
		<el-table :data="tableData" stripe style="width: 100%">
			<el-table-column prop="id" label="id"> </el-table-column>
			<el-table-column prop="name" label="名称" width="180"> </el-table-column>
		</el-table>
	</div>
</template>

<script>
import { getList } from "../api/tenant";
export default {
	data () {
		return {
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

	}
};
</script>
