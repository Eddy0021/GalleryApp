<template>
  <nav class="nav-bar" v-cloak>
    <div class="left">
      <p>{{ userName }}</p> 
    </div>
    <div class="right">
      <button @click="Logout">Logout</button>
    </div>
  </nav>
</template>

<script>
import "../assets/styles.css";

export default {
  name: "Gallery-NavBar",
   data (){
    return {
      userName: "UserName",
      user: localStorage.user,
    }
  },
  
  methods: {
    Logout() // Logout method
    {
      localStorage.removeItem('token');    
      localStorage.removeItem('username'); 
      localStorage.removeItem('userID');   
      this.$router.push({ name: "LoginPage"});
    }
  },
  mounted() // Reads from localStorage the username 
  {
    if(localStorage.token != null){
      var tempUsername = localStorage.username;
      this.userName = tempUsername.replace(/"/g,"");
    }
    
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->


<style scoped>
button{
  margin-bottom: 10px !;
  font: inherit;
  cursor: pointer;
  border: 1px solid black;
  background-color: lightgreen;
  color: black;
  padding: 0.05rem 1rem;
  box-shadow: 1px 1px 2px rgb(0 0 0 / 26%);
}

button:hover,
button:active {
  background-color: #ec3169;
  border-color: #ec3169;
  color: white;
  box-shadow: 1px 1px 4px rgba(0, 0, 0, 0.26);
}

</style>
