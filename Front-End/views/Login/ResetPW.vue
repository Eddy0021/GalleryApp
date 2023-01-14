<template>
<header id="Header">
    <h1>Welcome to the Gallery</h1>
    <h3>Please enter your email address below to reset your password:</h3>  
  </header>
  <section class="loginSection" id="forgotPasswordTab">
        <div class="loginDiv">
            <input v-model="email" type="text" placeholder="Email" ref="email" @keyup.enter="btn_ResetPW(email)" />
            <button @click="btn_ResetPW(email)">Reset Password</button>
            <button @click="backToLogin">Back</button>
        </div>
    </section>
</template>

<script>
import '../../assets/styles.css';
import axios from "axios";
import Swal from "sweetalert2";

export default {
  name: 'ResetPW',
  props: {
    
  },
  data() {
        return {
            user:{
                email: '',
            },
        }
    },

    methods: {
        btn_ResetPW(input) //Reset password after email was given (calls the API to send mail)
        { 
          if(this.checkValidation(input)){
            this.user.email = input;

            axios.post(this.hostname + "/api/User/CheckEmail", this.user)
            .then((response) => {             
              Swal.fire("We have sent you a mail with the link to reset your password.")   
              this.$router.push({ name: "LoginPage" })
            })
            .catch((error) => {
              if (error.response) {
                Swal.fire(error.response.data);
              }
            });           
          }     
        },
        backToLogin(){
            this.$router.push({ name: "LoginPage"});       
        },

        checkValidation(input){
        if (!input) {
          this.$refs.email.focus();
          Swal.fire("Email can't be empty!");
          return;
        }
        if (!/\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/.test(input))
        {
          this.$refs.email.focus();
          Swal.fire("Invalid email!");
          return;
        }
        return true;
      }
    }
}
</script>
