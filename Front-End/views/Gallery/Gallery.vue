<template>
<div id="background">
 <NavBar />
 <Albums/>
</div>
</template>

<script>
import Albums from "../../components/Gallery-Albums.vue";
import NavBar from "../../components/Gallery-NavBar.vue";


export default {
  name: 'LoginPage',
  components: {
    Albums,
    NavBar,
  },
   methods: {
  }

}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->


<style scoped>

#background{
  height: 100vh;
  background-color: rgb(52, 56, 59);
}

</style>
