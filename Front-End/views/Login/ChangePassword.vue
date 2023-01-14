<template>
  <header id="Header">
    <h1>Welcome to the Gallery</h1>
    <h3>Enter your new password</h3>
  </header>
    <section class="loginSection" id="ResetPasswordDiv">
    <div class="loginDiv" id="ResetPasswordinputs">
        <input v-model="user.password" placeholder="Password" type="password" @keyup.enter="ResetPassword" />
        <input v-model="confirmPassword" placeholder="Confirm Password" type="password" @keyup.enter="ResetPassword" />
        <button @click="ResetPassword">Reset Password</button>
    </div>
  </section>
</template>

<script>
import "../../assets/styles.css";
import axios from "axios";
import Swal from "sweetalert2";
import { useRoute } from "vue-router";

export default {
  name: "Resetpassword",
  props: {},
  data() {
    return {
        confirmPassword: "",
        user: {
            GUID: '',
            password: '',
        }
    };
  },//END OF DATA

async created() { //Read GUID from URL and assign it to user.GUID
    const route = useRoute();
    this.user.GUID = route.params.guid;
},

  methods: {
      getTokenConfig()//Read token from localStorage
      { 
      var token = JSON.parse(localStorage.getItem("token"));
      const config = {
        headers: { Authorization: `Bearer ${token}` },
      };
      return config;
    },
      ResetPassword() //Reset Password method    
      {     
        if(this.confirmPassword != this.user.password) {
            Swal.fire({icon: 'warning',text: "Password does not match!"})
            return
        }

        if(this.user.password === null || this.user.password === "")
        {
          Swal.fire({icon: 'warning',text: "Password can't be empty!"})
          return
        }
        
        try {
            axios.put(this.hostname + "/api/User", this.user, this.getTokenConfig() )
              .then((response) => {
                Swal.fire(response.data)
                this.$router.push({ name: "LoginPage" })
              })
              .catch((error) => {
                if (error.response) {
                  Swal.fire(error.response.data);
                }
              });
          } catch (error) {
            if (error.response) Swal.fire(error.response.data);
          }
        

      },
  },//END OF METHODS


};//END OF EXPORT DEFAULT
</script>
