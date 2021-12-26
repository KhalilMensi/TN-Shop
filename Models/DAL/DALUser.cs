using AngularAspCore.Models.BLL;
using AngularAspCore.Models.Entity;
using System.Data;
using System.Data.SqlClient;

namespace AngularAspCore.Models.DAL
{
    public class DALUser
    {
        //CheckUserUnicity
        private static bool CheckUserUnicityBy(string Field,string Value)
        {
            try
            {
                using (SqlConnection Cnn = DBConnection.GetConnection())
                {
                    string StrSQL = @"  SELECT TOP 1 * 
                                        FROM [User] 
                                        WHERE "+ Field +" = @Value";

                    SqlCommand Cmd = new SqlCommand(StrSQL, Cnn);
                    Cmd.Parameters.AddWithValue("@Value", Value);
                    using (SqlDataReader Reader = Cmd.ExecuteReader())
                    {
                        if (Reader.Read())
                            return false;
                    }
                }
            }
            catch (Exception e) {
                throw e;
            }

            return true;
        }
        //Create User Table in DB
        public static void CreateTable()
        {
            try
            {
                SqlConnection cnn = DBConnection.GetConnection();
                cnn.Open();
                string sql = @"IF NOT EXISTS (  SELECT * 
                                                FROM sysobjects 
                                                WHERE name = 'User') 
                                                    CREATE TABLE [dbo].[User] ( 
                                                        [Id] BIGINT IDENTITY (1, 1) NOT NULL,
                                                        [Code] NVARCHAR (20) NOT NULL,
                                                        [Email] NVARCHAR (50) NOT NULL, 
                                                        [EmailConfirmed] NVARCHAR (10) DEFAULT ('False') NULL,
                                                        [EmailConfirmationCode] NVARCHAR (6) NULL, 
                                                        [CodeExpirationDate] DATETIME NULL,
                                                        [Password] NVARCHAR (MAX) NOT NULL,
                                                        [Phone] NVARCHAR (30) NOT NULL,
                                                        [Country] NVARCHAR (50) NOT NULL, 
                                                        [Adress] NVARCHAR (MAX) NOT NULL,
                                                        [PostalCode] NVARCHAR (10) NOT NULL, 
                                                        [Role] NVARCHAR (20) NOT NULL, 
                                                        [Photo] NVARCHAR (MAX) NULL,
                                                        [AddedOn] DATETIME DEFAULT (GETDATE()) NOT NULL,
                                                        PRIMARY KEY CLUSTERED ([Id] ASC) 
                                                );";

                using (SqlCommand command = new SqlCommand(sql, cnn))
                    command.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception e) {
                throw e;
            }
        }
        //Add new User record
        public static string AddUser(User user)
        {
            string msg = "0";
            try
            {
                CreateTable();
                Random Alea = new Random();
                
                user.Code = Alea.Next(10000, 999999).ToString();

                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    connection.Open();
                    if (CheckUserUnicityBy("Email",user.Email) == true)
                    {
                        string sql = @" INSERT INTO [User](Code, Email, Password, Phone, Country, Adress, PostalCode, Role, Photo, AddedOn) OUTPUT INSERTED.Id 
                                        VALUES (@Code, @Email, @Password, @Phone, @Country, @Adress, @PostalCode, @Role, @Photo, @AddedOn)";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Code", user.Code);
                            command.Parameters.AddWithValue("@Email", user.Email);
                            command.Parameters.AddWithValue("@Password", BLLUser.Encrypt(user.Password, user.Code));
                            command.Parameters.AddWithValue("@Phone", user.Phone);
                            command.Parameters.AddWithValue("@Country", user.Country);
                            command.Parameters.AddWithValue("@Adress", user.Adress);
                            command.Parameters.AddWithValue("@PostalCode", user.PostalCode);
                            command.Parameters.AddWithValue("@Role", user.Role);

                            if (String.IsNullOrEmpty(user.PhotoFileName))
                                command.Parameters.AddWithValue("@Photo", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@Photo", user.PhotoFileName);
                            
                            if (user.AddedOn == new DateTime())
                                command.Parameters.AddWithValue("@AddedOn", DateTime.Now);
                            else
                                command.Parameters.AddWithValue("@AddedOn", user.AddedOn);

                            long Id = (long)command.ExecuteScalar();

                            if (Id > 0)
                            {
                                msg = Id.ToString();
                            }
                        }
                    }
                    else
                    {
                        msg = "Utilisateur déjà existe !";
                    }
                    connection.Close();
                }
            }
            catch (Exception e) 
            { 
                msg = e.Message; 
            }

            return msg;
        }
        //Update the records of a particluar User
        public static string UpdateUser(User user)
        {
            string msg = "0";
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    connection.Open();

                    string sql = @" UPDATE [User] 
                                    SET Code=@Code,
                                        Email=@Email,
                                        [EmailConfirmed]=@EmailConfirmed,
                                        [CodeExpirationDate]=@CodeExpirationDate,
                                        Password=@Password,
                                        Phone=@Phone,
                                        Country=@Country,
                                        Adress=@Adress,
                                        PostalCode=@PostalCode,
                                        Role=@Role,
                                        Photo=@Photo 
                                    WHERE Id=@Id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        if (String.IsNullOrEmpty(user.Code))
                            command.Parameters.AddWithValue("@Code", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Code", user.Code);

                        if (String.IsNullOrEmpty(user.Email))
                            command.Parameters.AddWithValue("@Email", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Email", user.Email);

                        if (String.IsNullOrEmpty(user.EmailConfirmed))
                            command.Parameters.AddWithValue("@EmailConfirmed", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("EmailConfirmed", user.EmailConfirmed);

                        if (String.IsNullOrEmpty(user.EmailConfirmationCode))
                            command.Parameters.AddWithValue("@EmailConfirmationCode", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("EmailConfirmationCode", user.EmailConfirmationCode);    

                        if (user.CodeExpirationDate == new DateTime())
                            command.Parameters.AddWithValue("@CodeExpirationDate", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@CodeExpirationDate", user.CodeExpirationDate);

                        if (String.IsNullOrEmpty(user.Password))
                            command.Parameters.AddWithValue("@Password", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Password", BLLUser.Encrypt(user.Password, user.Code));

                        if (String.IsNullOrEmpty(user.Phone))
                            command.Parameters.AddWithValue("@Phone", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Phone", user.Phone);

                        if (String.IsNullOrEmpty(user.Country))
                            command.Parameters.AddWithValue("@Country", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Country", user.Country);

                        if (String.IsNullOrEmpty(user.Adress))
                            command.Parameters.AddWithValue("@Adress", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Adress", user.Adress);

                        if (String.IsNullOrEmpty(user.PostalCode))
                            command.Parameters.AddWithValue("@PostalCode", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@PostalCode", user.PostalCode);

                        if (String.IsNullOrEmpty(user.Role))
                            command.Parameters.AddWithValue("@Role", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Role", user.Role);

                        if (String.IsNullOrEmpty(user.PhotoFileName))
                            command.Parameters.AddWithValue("@Photo", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Photo", user.PhotoFileName);

                        command.Parameters.AddWithValue("@Id", user.Id);

                        if (command.ExecuteNonQuery() == 1)
                        {
                            msg = "1";
                        }
                        else
                        {
                            msg = "Erreur de mise à jour";
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e) 
            { 
                msg = e.Message;
            }

            return msg;
        }
        //Get All Users from DB
        public static List<User> GetAllUsers()
        {
            List<User> lstUser = new List<User>();
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    connection.Open();
                    string sql = @" SELECT * 
                                    FROM [User] 
                                    ORDER BY Id DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                User user = new User();

                                user.Id = long.Parse(dataReader["Id"].ToString());
                                user.Code = dataReader["Code"].ToString();
                                user.Email = dataReader["Email"].ToString();
                                user.EmailConfirmed = dataReader["EmailConfirmed"].ToString();
                                user.EmailConfirmationCode = dataReader["EmailConfirmationCode"].ToString();
                                user.CodeExpirationDate = dataReader["CodeExpirationDate"].ToString() == "" ? new DateTime() : DateTime.Parse(dataReader["CodeExpirationDate"].ToString());
                                user.Phone = dataReader["Phone"].ToString();
                                user.Country = dataReader["Country"].ToString();
                                user.Adress = dataReader["Adress"].ToString();
                                user.PostalCode = dataReader["PostalCode"].ToString();
                                user.Role = dataReader["Role"].ToString();
                                user.PhotoFileName = dataReader["Photo"].ToString();
                                user.AddedOn = dataReader["AddedOn"].ToString() == "" ? new DateTime() : DateTime.Parse(dataReader["AddedOn"].ToString());

                                lstUser.Add(user);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e) 
            {
                throw e;
            }

            return lstUser;
        }
        //Get User By
        public static User GetUserBy(string Field, string Value)
        {
            User user = new User();
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    connection.Open();
                    string sql = @" SELECT TOP 1 * 
                                    FROM [User] 
                                    WHERE [" + Field + @"]=@Value";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Value", Value);

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                user.Id = long.Parse(dataReader["Id"].ToString());
                                user.Code = dataReader["Code"].ToString();
                                user.Email = dataReader["Email"].ToString();
                                user.EmailConfirmed = dataReader["EmailConfirmed"].ToString();
                                user.EmailConfirmationCode = dataReader["EmailConfirmationCode"].ToString();
                                user.CodeExpirationDate = dataReader["CodeExpirationDate"].ToString() == "" ? new DateTime() : DateTime.Parse(dataReader["CodeExpirationDate"].ToString());
                                user.Password = BLLUser.Decrypt(dataReader["Password"].ToString(), user.Code);
                                user.Phone = dataReader["Phone"].ToString();
                                user.Country = dataReader["Country"].ToString();
                                user.Adress = dataReader["Adress"].ToString();
                                user.PostalCode = dataReader["PostalCode"].ToString();
                                user.Role = dataReader["Role"].ToString();
                                user.PhotoFileName = dataReader["Photo"].ToString();
                                user.AddedOn = dataReader["AddedOn"].ToString() == "" ? new DateTime() : DateTime.Parse(dataReader["AddedOn"].ToString());
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e) 
            {
                throw e;
            }

            return user;
        }
        //Get List of User By
        public static List<User> GetAllUsersBy(string Field, string Value)
        {
            List<User> lstUser = new List<User>();
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    connection.Open();
                    string sql = @" SELECT * 
                                    FROM [User] 
                                    WHERE [" + Field + @"]=@Field";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", Value);

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                User user = new User();

                                user.Id = long.Parse(dataReader["Id"].ToString());
                                user.Code = dataReader["Code"].ToString();
                                user.Email = dataReader["Email"].ToString();
                                user.EmailConfirmed = dataReader["EmailConfirmed"].ToString();
                                user.EmailConfirmationCode = dataReader["EmailConfirmationCode"].ToString();
                                user.CodeExpirationDate = dataReader["CodeExpirationDate"].ToString() == "" ? new DateTime() : DateTime.Parse(dataReader["CodeExpirationDate"].ToString());
                                user.Password = BLLUser.Decrypt(dataReader["Password"].ToString(), user.Code);
                                user.Phone = dataReader["Phone"].ToString();
                                user.Country = dataReader["Country"].ToString();
                                user.Adress = dataReader["Adress"].ToString();
                                user.PostalCode = dataReader["PostalCode"].ToString();
                                user.Role = dataReader["Role"].ToString();
                                user.PhotoFileName = dataReader["Photo"].ToString();
                                user.AddedOn = dataReader["AddedOn"].ToString() == "" ? new DateTime() : DateTime.Parse(dataReader["AddedOn"].ToString());

                                lstUser.Add(user);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e) 
            {
                throw e;
            }

            return lstUser;
        }
        //Delete the records of a particular User
        public static string DeleteUserBy(string Field, string Value)
        {
            string msg = "Erreur de suppression";
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    connection.Open();
                    string sql = @" DELETE 
                                    FROM [User] 
                                    WHERE "+ Field +"=@Value";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Value", Value);

                        if (command.ExecuteNonQuery() == 1)
                        {
                            msg = "1";
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e) 
            { 
                msg = e.Message; 
            }

            return msg;
        }
    }
}
