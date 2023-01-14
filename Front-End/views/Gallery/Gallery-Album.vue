<template>
  <div id="background">
    <NavBar />
    <div class="control-buttons">
      <input
        type="file"
        style="display: none"
        @change="fileChange"
        ref="fileInput"
      />
      <button @click="$refs.fileInput.click()">Upload</button>
    </div>
    <div id="slika-list">
      <div v-for="item in slike" :key="item.photoID" v-cloak>
        <div class="slika" @click="openImage(item)">
          {{ item.photo_Name }}
          <img :src="item.imageURL" />
        </div>
        <div class="control-buttons">
          <button @click="deleteImage(item.photoID)">Delete</button>
          <button @click="editImage(item)">Edit</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import "../../assets/styles.css";
import NavBar from "../../components/Gallery-NavBar.vue";
import axios from "axios";
import Swal from "sweetalert2";
import { useRoute } from "vue-router";

export default {
  name: "Gallery-Album",
  components: {
    NavBar,
  },
  data() {
    return {
      currentAlbumID: "",
      slika: {
        file: "",
        imageURL: require("@/assets/defaultImage.png"),
        albumID: "",
        photo_Name: "",
      },

      slikaUpdate: {
        photoID: "",
        photo_Name: "",
      },

      slike: [],
    };
  },
  async created() //Takes albumID from the URL and assigns it to currentAlbumID
  {
    const route = useRoute();
    this.currentAlbumID = route.params.albumID;
    await this.getSlike(this.currentAlbumID);
  },
  methods: {
    getTokenConfig() // Makes token into Authorization
    {
      var token = JSON.parse(localStorage.getItem("token"));
      const config = {
        headers: { Authorization: `Bearer ${token}` },
      };
      return config;
    },

    async getSlike(albumID) // Request pictures method
    {
      await axios.get(this.hostname + `/api/Slika/${albumID}`, this.getTokenConfig())
        .then((response) => {
          this.slike = response.data;
        })
        .catch((error) => {
          if (error.response) Swal.fire(error.response.data);
        });
    },

    fileChange(event) // Upload Image method
    {
      var file = event.target.files[0];
      
      var formdData = new FormData();

      formdData.append("AlbumID", this.currentAlbumID);
      formdData.append("Photo_Name", file.name);
      formdData.append("Files", file);
      formdData.append("imageURL", null);

      try {axios.post(this.hostname + "/api/Slika/UploadImage", formdData, this.getTokenConfig() )
          .then((response) => {
            if (response.data.photoID > 0) {
              Swal.fire("Uploaded successfully!");
              this.getSlike(this.currentAlbumID);
            } else {
              Swal.fire("Error: Something went wrong.");
            }
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

    openImage(image) // Open image in pop-up
    {
      Swal.fire({
        title: image.photo_Name,
        imageUrl: image.imageURL,
        imageWidth: 600,
        imageHeight: 400,
        imageAlt: "Custom image",
      });
    },

    deleteImage(item) // Delete Image method
    {
      try {
        axios.delete(this.hostname + `/api/Slika/DeleteImage/${item}`, this.getTokenConfig() )
          .then((response) => {
            Swal.fire("Picture was delted successfully!");
            var removeIndex = this.slike.findIndex((x) => x.photoID === item);
            ~removeIndex && this.slike.splice(removeIndex, 1);
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

    editImage(item) // Edit Image method
    { 
      var inputValue = item.photo_Name;

      const { value: photoName } = Swal.fire({
        title: "Change your photo name:",
        input: "text",
        inputValue: inputValue,
        showCancelButton: true,
        inputValidator: (value) => {
          if (!value) {
            return "You need to write something!";
          }else if(value === inputValue){
            return "It can't be the same name!"
          } else {
            inputValue = value;
          }
        },
      }).then((result) => {
        if (result.isConfirmed) {
          this.slike.forEach((element) => {
            if (element.photoID === item.photoID) {
              this.slikaUpdate.photoID = element.photoID;
              this.slikaUpdate.photo_Name = inputValue;
              element.photo_Name = inputValue;
            }
          });

          try {
            axios.put(this.hostname + "/api/Slika", this.slikaUpdate, this.getTokenConfig() )
              .then((response) => {
                Swal.fire("Name updated successfully!");
              })
              .catch((error) => {
                if (error.response) {
                  Swal.fire(error.response.data);
                }
              });
          } catch (error) {
            if (error.response) Swal.fire(error.response.data);
          }
        }
      });
    },
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->


<style scoped>
#background {
  height: 100vh;
  background-color: rgb(52, 56, 59);
}
</style>
