using Dapper;
using GalleryApp.DTO;
using GalleryApp.IRepository;
using GalleryApp.IServices;
using GalleryApp.Models;
using GalleryApp.ResponeData;
using GalleryApp.Services.Service;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GalleryApp.Repository
{
    public class UserRepository : IUserRepository
    {
        string _connectionString = "";
        private IMailServices _mailServices;

        public UserRepository(IConfiguration configuration, IMailServices mailServices)
        {
            _connectionString = configuration.GetConnectionString("DB_GalleryApp");
            _mailServices = mailServices;
        }

        public async Task<User> Create(UploadUserResponse user)  // Register user by the entered data
        {
            User _user = new User();

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    DynamicParameters tempParameters = new DynamicParameters();
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var users = await connection.QueryAsync<UploadUserResponse>("CheckExistence", tempParameters = setParamtersCheckExistence(user), commandType: CommandType.StoredProcedure);

                    string message = tempParameters.Get<string>("@Message").ToString(); // From StoredProcedures OUTPUT

                    if (message != "-")
                    {
                        throw new Exception(message);
                    }                

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var users = await connection.QueryAsync<User>("CreateUser", this.setParamtersRegister(user), commandType: CommandType.StoredProcedure);
                    if (users != null && users.Count() > 0)
                    {
                        _user = users.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _user;
        }

        public async Task<User> GetByUsernamePassword(LoginDTO user) // Check entered username and password
        {
            User _user = new User();
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var users = await connection.QueryAsync<User>("LoginConfirmation", this.setParamtersLogin(user), commandType: CommandType.StoredProcedure);
                    if (users != null && users.Count() > 0)
                    {
                        _user = users.FirstOrDefault(); //LoginConfirmation
                    }
                    else
                    {
                        throw new Exception("Wrong Username or Password");
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
            return _user;
        }

        public async Task<string> Update(UpdatePasswordResponse user) // Update User
        {
            string message = string.Empty;
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    DynamicParameters tempParameters = new DynamicParameters();
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var users = await connection.QueryAsync<User>("UpdateUser", tempParameters = this.setParamtersUpdatePassword(user), commandType: CommandType.StoredProcedure);

                    int counter = tempParameters.Get<Int32>("@Counter");

                    if (counter > 0)
                    {
                        message = "Password updated.";
                    }
                    else
                    {
                        throw new Exception("Not valid token, please send a new password request.");
                    }

                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        public async Task<string> ChangePasswordRequest(CheckEmailResponse user) // Check if Email exists
        {
            ResetPasswordData _user = new ResetPasswordData();
            string NewGUID = System.Guid.NewGuid().ToString().Replace("-", "");
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var users = await connection.QueryAsync<User>("CheckEmailExistence", this.setParamtersCheckEmail(user), commandType: CommandType.StoredProcedure);
                    if (users != null && users.Count() > 0)
                    {
                        foreach (var item in users)
                        {
                            _user.UserID = item.UserID;
                        } 
                    }
                }
            }
            catch (Exception)
            {

                throw new Exception("Username or password does not match!");
            }

            try
            {
                if (_user.UserID > 0)
                {
                    
                    _user.GUID = NewGUID;

                    try
                    {
                        using (IDbConnection connection = new SqlConnection(_connectionString))
                        {
                            if (connection.State == ConnectionState.Closed) connection.Open();
                            var users = await connection.QueryAsync<User>("SaveGUID", this.setParamtersSaveResetRequest(_user), commandType: CommandType.StoredProcedure);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception(ex.Message);
                    }
                    

                    Mail mail = this.GetMailObject(user, NewGUID);
                    await _mailServices.SendMail(mail);
                }
                else
                {
                    throw new Exception("Email does not exist!");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            
            return "Sent";
        }

        public Mail GetMailObject(CheckEmailResponse user,string NewGUID) // Creates Email Object
        {
            string link = "http://localhost:8080/Reset-Password/" + NewGUID;
            Mail mail = new Mail();
            mail.Subject = "Reset Password";
            mail.Body = "Click on the <a href="+link+">link</a> to reset your password";
            mail.ToMailIds = new List<string>()
            {
                user.Email
            };
            return mail;
        }

        private DynamicParameters setParamtersRegister(UploadUserResponse user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
            parameters.Add("@Password", user.Password);
            parameters.Add("@Email", user.Email);
            parameters.Add("@CreatedAt", user.CreatedAt);
            parameters.Add("@UpdatedAt", user.UpdatedAt);
            return parameters;
        }

        private DynamicParameters setParamtersLogin(LoginDTO user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
            parameters.Add("@Password", user.Password);
            return parameters;
        }

        private DynamicParameters setParamtersCheckEmail(CheckEmailResponse user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Email", user.Email);
            return parameters;
        }

        private DynamicParameters setParamtersSaveResetRequest(ResetPasswordData user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", user.UserID);
            parameters.Add("@GUID", user.GUID);
            return parameters;
        }

        private DynamicParameters setParamtersUpdatePassword(UpdatePasswordResponse user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@GUID", user.GUID);
            parameters.Add("@Password", user.Password);
            parameters.Add("@UpdatedAt", user.UpdatedAt);
            parameters.Add("@Counter", 0, DbType.Int32, ParameterDirection.Output);
            return parameters;
        }

        private DynamicParameters setParamtersCheckExistence(UploadUserResponse user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Email", user.Email);
            parameters.Add("@Username", user.Username);
            parameters.Add("@Message", "" , DbType.String, ParameterDirection.Output, size: 200);
            return parameters;
        }
    }
}
