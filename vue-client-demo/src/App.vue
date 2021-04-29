<template>
	<div id="app">
		<home></home>
	</div>
</template>

<script>
import Home from "./views/Home";
export default {
	components: {
		Home
	}
}
</script>

<style lang="scss">
#app {
	font-family: "Avenir", Helvetica, Arial, sans-serif;
	-webkit-font-smoothing: antialiased;
	-moz-osx-font-smoothing: grayscale;
	color: #2c3e50;
}
</style>
