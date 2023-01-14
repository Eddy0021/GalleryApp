<template>
  <header id="Header">
    <h1>Welcome to the Gallery</h1>
    <h3>Please login</h3>
  </header>
  <section class="loginSection" id="loginTab">
    <div class="loginDiv">
      <input v-model="user.username" type="text" placeholder="Username" ref="Username" @keyup.enter="Login" />
      <input v-model="user.password" type="password" placeholder="Password" ref="Password" @keyup.enter="Login" />
      <div>
        <button @click="Login">Login</button>
        <button @click="Register">Register</button>
      </div>
      <button id="ForgotPWBTN" @click="forgotPassword">Forgott Password</button>
    </div>
  </section>
</template>

<script>
import "../../assets/styles.css";
import axios from "axios";
import Swal from "sweetalert2";
import jwt_decode from "jwt-decode";

export default {
  name: "LoginPage",
  data() {
    return {
      user: {
        username: "",
        password: "",
      },
    };
  },//END OF DATA

  
  methods: {
    Login() { //Login method
      if(this.checkValidation()){    
        axios.post(this.hostname + "/api/User/Signin", this.user, )
          .then(response => {

            var token = response.data;
            var decoded = jwt_decode(token);
            
            if(decoded.userID > 0){      
              localStorage.setItem('token', JSON.stringify(response.data));           
              response.data = "";
              localStorage.setItem('username', decoded.username); 
              localStorage.setItem('userID', decoded.userID); 
             
              this.$router.push({ name: "Gallery"});
            }
          })
          .catch(error => {
            if (error.response) {
              Swal.fire(error.response.data);
            }
          })
      }
    },
    Register() { //Move to Register page
      this.$router.push({ name: "Register" });
    },
    forgotPassword() { //Move to Forgot-Password page
      this.$router.push({ name: "ForgotPassword" });
    },
    checkValidation(){ //Checking input validation
      if(!this.user.username){
        this.$refs.Username.focus();
        Swal.fire("Enter username!");
        return;
      }
      if(!this.user.password){
        this.$refs.Password.focus();
        Swal.fire("Enter password!");
        return;
      }
      return true;
    }
  },//END OF METHODS


};//END OF EXPORT DEFAULT
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->


<style scoped>
#ForgotPWBTN {
  background-color: lightgray !important;
  border-color: white !important;
  color: black !important;
}
</style>
