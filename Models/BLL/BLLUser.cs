using AngularAspCore.Extensions;
using AngularAspCore.Models.DAL;
using AngularAspCore.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace AngularAspCore.Models.BLL
{
    public class BLLUser
    {
        //Add new User record
        public static string AddUser(User user)
        {
            return DALUser.AddUser(user);
        }
        //Update the records of a particluar User
        public static string UpdateUser(User user)
        {
            return DALUser.UpdateUser(user);
        }

        //Get All Users from DB
        public static List<User> GetAllUsers()
        {
            return DALUser.GetAllUsers();
        }
        //Get User By
        public static User GetUserBy(string Field, string Value)
        {
            return DALUser.GetUserBy(Field, Value);
        }
        //Get List of User By
        public static List<User> GetAllUsersBy(string Field, string Value)
        {
            return DALUser.GetAllUsersBy(Field, Value);
        }
        //Delete the records of a particular User
        public static string DeleteUserBy(string Field, string Value)
        {
            return DALUser.DeleteUserBy(Field, Value);
        }
        //Copy Files to Server
        public static JsonResponse CopyFilesToServer(User user, IWebHostEnvironment hostingEnvironment)
        {
            JsonResponse CopyFilesToServer = new JsonResponse();
            CopyFilesToServer.success = false;
            CopyFilesToServer.message = "l'opération n'a pas réussi";

            try
            {
                if (user.Photo != null)
                {
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "/img/Users");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    var filePath = Path.Combine(uploads, user.PhotoFileName);
                    user.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    CopyFilesToServer.success = true;
                    CopyFilesToServer.message = "Opération réussie";
                }
            }
            catch (Exception ex)
            {
                CopyFilesToServer.message = "échec de l'opération : " + ex.Message;
            }
            return CopyFilesToServer;
        }

        #region OPERATIONS

        // Generate password
        private static Random random = new Random();
        public static string RandomPassword()
        {
            const string chars = "azertyuiopqsdfghjklmwxcvbnABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%&";
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //Methode to Encrypt the Password 
        public static string Encrypt(string clearText, string EncryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        //Methode to Decrypt the Password
        public static string Decrypt(string cipherText, string EncryptionKey)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        //Generate Confirmation Code
        public static JsonResponse GenerateConfirmationCode(User user)
        {
            JsonResponse GenerateConfirmationCode = new JsonResponse();
            GenerateConfirmationCode.success = false;
            GenerateConfirmationCode.message = "Une erreur est survenue, veuillez réessayer plus tard";
            Random Alea = new Random();
            user.EmailConfirmationCode = Alea.Next(100000, 9999999).ToString();
            user.CodeExpirationDate = DateTime.Now.AddHours(2);
            
            if (UpdateUser(user) == "1")
            {
                string Message = "";

                Message += "<p>Bonjour ,</p>";

                Message += "<p>Vous n'avez pas encore activé votre compte dans notre boutique en ligne ?</p>" +
                "<p>Vous devez confirmer votre adresse e-mail " + user.Email + " pour pouvoir continuer ,</p>" +
                "<p>veuillez utiliser le code secret ci-dessous</p>" +
                "<p></p>" +
                "<p><b>Code secret: " + user.EmailConfirmationCode + "</b></p>" +
                "<p></p>" +
                "<p>Bien à vous ,</p>" +
                 "<p>MonBoutique</p>";

                JsonResponse sendMail = SendMail(user.Email, "Vérification Identité", Message);
                if (sendMail.success)
                {
                    GenerateConfirmationCode.success = true;
                    GenerateConfirmationCode.message = "Un nouveau code a été généré avec succès, veuillez vérifier votre boîte mail";
                }
            }
            return GenerateConfirmationCode;
        }
        //Send Mail
        public static JsonResponse SendMail(string To, string Subject, string Body)
        {
            JsonResponse sendmail = new JsonResponse();
            sendmail.success = false;
            sendmail.message = "Une erreur s'est produite veuillez réessayer ultérieurement";
            MailMessage mail = new MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress("email", "Title", System.Text.Encoding.UTF8);
            mail.Subject = Subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = Body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("******", "*******");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            try
            {
                client.Send(mail);
                sendmail.success = true;
                sendmail.message = "L'e-mail a été envoyé avec succès";
            }
            catch (Exception ex)
            {
                sendmail.success = false;
                sendmail.message = ex.Message;
            }
            return sendmail;
        }
        
        #endregion
    }
}
