<template>
  <header id="Header">
    <h1>Welcome to the Gallery</h1>
    <h3>Please fill the filds below to register:</h3>
  </header>
  <section class="loginSection" id="registerTab">
    <div class="loginDiv">
      <input
        v-model="user.username"
        type="text"
        placeholder="Username"
        ref="Username"
        @keyup.enter="btn_register"
      />
      <input
        v-model="user.password"
        type="password"
        placeholder="Password"
        ref="Password"
        @keyup.enter="btn_register"
      />
      <input v-model="user.email" type="text" placeholder="Email" ref="Email" @keyup.enter="btn_register" />
      <button @click="btn_register">Register</button>
      <button @click="backToLogin">Back</button>
    </div>
  </section>
</template>

<script>
import "../../assets/styles.css";
import axios from "axios";
import Swal from "sweetalert2";

export default {
  name: "RegisterTab",
  props: {},
  data() {
    return {
      user: {
        username: "",
        password: "",
        email: "",
        token: "",
      },
    };
  },//END OF DATA


  methods: {
    btn_register() //Register user method
    {  
      if (this.checkValidation()) {
        axios
          .post(this.hostname + "/api/User/Registration", this.user)
          .then((response) => {                  
            if (response.data.userID > 0) {
              Swal.fire("Successfully registered!").then(() => {
                this.$router.push({ name: "LoginPage" });
              });
            } else {           
              Swal.fire("Error : Something went wrong!");
            }
          })
          .catch((error) => {
            if (error.response) {
              Swal.fire(error.response.data);
            }
          });
      }
    },
    backToLogin() //Return to login Page
    {
      this.$router.push({ name: "LoginPage" });
    },
    checkValidation() // Check input validation
    {
      if (!this.user.username) {
        this.$refs.Username.focus();
        Swal.fire("Username can't be empty!");
        return;
      }
      if (!this.user.password) {
        this.$refs.Password.focus();
        Swal.fire("Password can't be empty!");
        return;
      }
      if (!this.user.email) {
        this.$refs.Email.focus();
        Swal.fire("Email can't be empty!");
        return;
      }
      if (
        !/\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/.test(
          this.user.email
        )
      ) {
        this.$refs.Email.focus();
        Swal.fire("Invalid email!");
        return;
      }
      return true;
    },
  },//END OF METHODS


};//END OF EXPORT DEFAULT
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->


<style scoped>
#forgotPasswordTab {
  display: none;
}
</style>
