using Dapper;
using GalleryApp.IRepository;
using GalleryApp.Models;
using GalleryApp.ResponeData;
using Microsoft.Data.SqlClient;
using System.Data;


namespace GalleryApp.Repository
{
    public class SlikaRepository : ISlikaRepository
    {
        public static IWebHostEnvironment _webHostEnvironment;
        string _connectionString = "";

        public SlikaRepository(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _connectionString = configuration.GetConnectionString("DB_GalleryApp");
        }


        public async Task<string> Delete(int id) // Delete image
        {
            Slika _slika = new Slika();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var Slike = await connection.QueryAsync<Slika>("DeleteSlika", this.SetParamtersDelete(id), commandType: CommandType.StoredProcedure);
                    if (Slike != null && Slike.Count() > 0)
                    {
                        _slika = Slike.FirstOrDefault();
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

                string fileName = _slika.imageURL;

                if (System.IO.File.Exists(path + fileName))
                {
                    System.IO.File.Delete(path + fileName);
                }
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }
            return "Deleted";
        }

        public async Task<List<Slika>> GetAllByAlbumID(int id) // Get all images by AlbumID
        {
            List<Slika> _slike = new List<Slika>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed) connection.Open();
                var slike = await connection.QueryAsync<Slika>("SelectSlikeWithAlbumID", this.SetParamtersSelectWithAlbumID(id), commandType: CommandType.StoredProcedure);
                if (slike != null && slike.Count() > 0)
                {
                    foreach (var item in slike)
                    {
                        item.imageURL = "http://localhost:5125/Slike/" + item.imageURL;
                    }
                    
                    _slike = slike.ToList();
                }
            }
            return _slike;
        }

        public async Task<Slika> Update(UpdateSlikaResponse slika) // Update image
        {
            Slika _slika = new Slika();
            string name;
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    name = slika.Photo_Name;
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var Slike = await connection.QueryAsync<Slika>("UpdateSlika", this.SetParamtersUpdate(slika), commandType: CommandType.StoredProcedure);
                    if (Slike != null && Slike.Count() > 0)
                    {
                        _slika = Slike.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
       
            return _slika;
        }

        public async Task<Slika> Upload(UploadSlikaResponse slika) // Upload image
        {
            Slika _slika = new Slika();
            string imageName = DateTime.UtcNow.Ticks.ToString();
            slika.imageURL =  imageName + ".png";
            string tempName = slika.Photo_Name.Split('.')[0];
            slika.Photo_Name = tempName;

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var Albums = await connection.QueryAsync<Slika>("CreateSlika", this.setParamtersUpload(slika), commandType: CommandType.StoredProcedure);
                    _slika = Albums.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            try
            {
                var files = slika.Files;
                slika.Files = null;

                if (_slika.PhotoID > 0 && files != null && files.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\Slike\\";
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    string fileName = slika.imageURL;
                    if (System.IO.File.Exists(path + fileName))
                    {
                        System.IO.File.Delete(path + fileName);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + fileName))
                    {
                        files.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }
                else
                {
                    throw new Exception("Not found!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            return _slika;
        }

        private DynamicParameters setParamtersUpload(UploadSlikaResponse slika)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AlbumID", slika.AlbumID);
            parameters.Add("@Photo_Name", slika.Photo_Name);
            parameters.Add("@ImageURL", slika.imageURL);
            parameters.Add("@CreatedAt", slika.CreatedAt);
            parameters.Add("@UpdatedAt", slika.UpdatedAt);
            return parameters;
        }
        private DynamicParameters SetParamtersUpdate(UpdateSlikaResponse slika)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PhotoID", slika.PhotoID);
            parameters.Add("@Photo_Name", slika.Photo_Name);
            parameters.Add("@UpdatedAt", slika.UpdatedAt);
            return parameters;
        }

        private DynamicParameters SetParamtersDelete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PhotoID", id);
            return parameters;
        }

        private DynamicParameters SetParamtersSelectWithAlbumID(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AlbumID", id);
            return parameters;
        }
    }
}
