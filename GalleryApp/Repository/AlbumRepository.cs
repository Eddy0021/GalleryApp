using Dapper;
using GalleryApp.IRepository;
using GalleryApp.Models;
using GalleryApp.ResponeData;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GalleryApp.Repository
{
    public class AlbumRepository : IAlbumRepository
    {
        public static IWebHostEnvironment _webHostEnvironment;
        string _connectionString = "";

        public AlbumRepository(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _connectionString = configuration.GetConnectionString("DB_GalleryApp");
        }


        public async Task<Album> Create(UploadAlbumResponse album) // Creates Album
        {
           Album _album = new Album();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var albums = await connection.QueryAsync<Album>("CreateAlbum", this.SetParamtersUpload(album), commandType: CommandType.StoredProcedure);
                    if (albums != null && albums.Count() > 0)
                    {
                        _album = albums.FirstOrDefault();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _album;
        }

        public async Task<string> Delete(int id) // Delete Album AND all pictures in it
        {
            List<Slika> _slike = new List<Slika>();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var slika = await connection.QueryAsync<Slika>("DeleteAlbum", this.SetParamtersDelete(id), commandType: CommandType.StoredProcedure);
                    if (slika != null && slika.Count() > 0)
                    {
                        foreach (var item in slika)
                        {
                            _slike.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            try
            {
                string path = _webHostEnvironment.WebRootPath + "\\Slike\\";
                if (!Directory.Exists(path)) throw new Exception("Picture does not exist!");

                foreach (var item in _slike)
                {
                    string fileName = item.imageURL;

                    if (System.IO.File.Exists(path + fileName))
                    {
                        System.IO.File.Delete(path + fileName);
                    }
                }     
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return "Deleted";
        }

        public async Task<List<Album>> GetAlbumByUserId(int id) // Get Albums by UserID
        {
            List<Album> _albumsList = new List<Album>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed) connection.Open();
                var albums = await connection.QueryAsync<Album>("SelectAlbumsWithUserID", this.SetParamtersAlbumsByUserID(id), commandType: CommandType.StoredProcedure);
                if (albums != null && albums.Count() > 0)
                {
                    _albumsList = albums.ToList();
                }
            }
            return _albumsList;
        }

        public async Task<Album> Update(UpdateAlbumResponse album) // Update Album
        {
            Album _album = new Album();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var albums = await connection.QueryAsync<Album>("UpdateAlbum", this.SetParamtersUpdate(album), commandType: CommandType.StoredProcedure);
                    if (albums != null && albums.Count() > 0)
                    {
                        _album = albums.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _album;
        }

        #region Parameters
        private DynamicParameters SetParamtersUpload(UploadAlbumResponse album)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Album_Name", album.Album_Name);
            parameters.Add("@UserID", album.UserID);
            parameters.Add("@CreatedAt", album.CreatedAt);
            parameters.Add("@UpdatedAt", album.UpdatedAt);
            return parameters;
        }

        private DynamicParameters SetParamtersUpdate(UpdateAlbumResponse album)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AlbumID", album.AlbumID);
            parameters.Add("@Album_Name", album.Album_Name);
            parameters.Add("@UpdatedAt", album.UpdatedAt);
            return parameters;
        }

        private DynamicParameters SetParamtersDelete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AlbumID", id);
            return parameters;
        }

        private DynamicParameters SetParamtersAlbumsByUserID(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", id);
            return parameters;
        }
        #endregion
    }
}
