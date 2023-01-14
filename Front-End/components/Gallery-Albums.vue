<template>
  <div id="album">
    <div class="control-buttons">
      <button @click="createAlbum()">Create</button>
    </div>
    <div id="album-list">
      <div v-for="album in albums" :key="album.albumID" v-cloak>
        <div class="album-folders" @click="redirectUser(album.albumID)">
          {{ album.album_Name }}
        </div>
        <div class="control-buttons">
          <button @click="deleteAlbum(album.albumID)">Delete</button>
          <button @click="editAlbum(album)">Edit</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import "../assets/styles.css";
import axios from "axios";
import Swal from "sweetalert2";

export default {
  name: "Gallery-Albums",
  props: {},
  data() {
    return {
      album: {
        album_Name: "",
        userID: "",
      },

      albumUpload: {
        albumID: "",
        album_Name: "",
      },
      albums: [],
    };
  },

  async created() // Calls getAlbums method to load all the albums
  {
    this.getAlbums();
  },

  methods: {
    redirectUser(albumID) // Redirection for the albums
    {
      this.$router.push({ name: "GalleryAlbum", params: { albumID: albumID } });
    },
    getTokenConfig() // Makes token to be Authorization header
    {
      var token = JSON.parse(localStorage.getItem("token"));
      const config = {
        headers: { Authorization: `Bearer ${token}` },
      };
      return config;
    },
    async getAlbums() // Requests all the albums where userID is matched
    {
      var userID = JSON.parse(localStorage.getItem("userID"));
      await axios
        .get(
          this.hostname + `/api/Album/GetAlbum/${userID}`,
          this.getTokenConfig()
        )
        .then((response) => {
          this.albums = response.data;
        })
        .catch((error) => {
          if (error.response) {
            Swal.fire({
              icon: "error",
              title: "Error Code: 401 (Unauthorized)",
            }).then((result) => {
              if (result.isConfirmed) {
                this.$router.push({ name: "LoginPage" });
              }
            });
          }
        });
    },
    createAlbum() // Creats album method
    {
      var tempAlbumName = "";
      const { value: galleryName } = Swal.fire({
        title: "Create a new Gallery:",
        input: "text",
        inputLabel: "Name",
        showCancelButton: true,
        inputValidator: (value) => {
          if (!value) {
            return "You need to write something!";
          } else {
            tempAlbumName = value;
          }
        },
      }).then((result) => {
        if (result.isConfirmed) {
          var userID = JSON.parse(localStorage.getItem("userID"));

          this.album.album_Name = tempAlbumName;
          this.album.userID = userID;

          try {
            axios
              .post(
                this.hostname + "/api/Album",
                this.album,
                this.getTokenConfig()
              )
              .then((response) => {
                if (response.data.albumID > 0) {
                  this.albums.push({
                    albumID: response.data.albumID,
                    album_Name: tempAlbumName,
                    userID: userID,
                  });
                  Swal.fire("Album uploaded successfully!");
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
        }
      });
    },

    deleteAlbum(album) // Delete album method
    {
      try {
        axios
          .delete(
            this.hostname + `/api/Album/Delete/${album}`,
            this.getTokenConfig()
          )
          .then((response) => {
            Swal.fire("Album was delted successfully!");
            var removeIndex = this.albums.findIndex(
              (x) => x.albumID === album.albumID
            );
            removeIndex && this.albums.splice(removeIndex, 1);
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

    editAlbum(album) // Edit album method
    {
      var inputValue = album.album_Name;

      const { value: AlbumName } = Swal.fire({
        title: "Change your album name:",
        input: "text",
        inputValue: inputValue,
        showCancelButton: true,
        inputValidator: (value) => {
          if (!value) {
            return "You need to write something!";
          }else if(value == inputValue){
            return "It can't be the same name!"
          } else {
            inputValue = value;
          }
        },
      }).then((result) => {
        if (result.isConfirmed) {
          this.albums.forEach((element) => {
            if (element.albumID === album.albumID) {
              this.albumUpload.albumID = element.albumID;
              this.albumUpload.album_Name = inputValue;
              element.album_Name = inputValue;
            }
          });

          try {
            axios
              .put(
                this.hostname + "/api/Album",
                this.albumUpload,
                this.getTokenConfig()
              )
              .then((response) => {
                Swal.fire("Album name updated successfully!");
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