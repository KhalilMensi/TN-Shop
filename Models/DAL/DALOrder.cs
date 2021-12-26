using AngularAspCore.Models.Entity;
using System.Data;
using System.Data.SqlClient;

namespace AngularAspCore.Models.DAL
{
    public class DALOrder
    {
        private static bool CheckOrderUnicityBy(string Field, string Value)
        {
            try
            {
                using (SqlConnection Cnn = DBConnection.GetConnection())
                {
                    string StrSQL = @"  SELECT TOP 1 * 
                                        FROM [Order] 
                                        WHERE " + Field + " = @Value";

                    SqlCommand Cmd = new SqlCommand(StrSQL, Cnn);
                    Cmd.Parameters.AddWithValue("@Value", Value);
                    using (SqlDataReader Reader = Cmd.ExecuteReader())
                    {
                        if (Reader.Read())
                            return false;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }
        public static void createTable()
        {
            try
            {
                SqlConnection cnn = DBConnection.GetConnection();
                cnn.Open();
                string sql = @"IF NOT EXISTS (  SELECT * 
                                                FROM sysobjects 
                                                WHERE name = 'Order') 
                                                    CREATE TABLE [dbo].[Order] ( 
                                                        [Id] BIGINT IDENTITY (1, 1) NOT NULL,
                                                        [IdUser] BIGINT NOT NULL,
                                                        [OrderRef] NVARCHAR (50) NOT NULL, 
                                                        [OrderDate] DATETIME NOT NULL,
                                                        [AmountHT] FLOAT NOT NULL, 
                                                        [AmountTTC] FLOAT NOT NULL,
                                                        [DiscountPercent] INT NOT NULL,
                                                        [PayementRef] NVARCHAR (30) NOT NULL,
                                                        [PayementDate] DATETIME NULL, 
                                                        [PayementMethode] NVARCHAR (30) NULL,
                                                        [InvoiceDate] DATETIME NULL, 
                                                        [InvoiceFirstName] NVARCHAR (30) NULL, 
                                                        [InvoiceLastName] NVARCHAR (30) NULL,
                                                        [InvoiceCity] NVARCHAR (30) NULL, 
                                                        [InvoiceCountry] NVARCHAR (30) NULL,
                                                        [InvoiceAddress] NVARCHAR (MAX) NULL, 
                                                        [InvoiceEmail] NVARCHAR (50) NULL,
                                                        [InvoicePhone] NVARCHAR (30) NULL, 
                                                        [InvoiceState] NVARCHAR (30) NULL,
                                                        PRIMARY KEY CLUSTERED ([Id] ASC),
                                                        CONSTRAINT [FK_Order_User] FOREIGN KEY ([IdUser]) REFERENCES [dbo].[User] ([Id])
                                                );";

                using (SqlCommand command = new SqlCommand(sql, cnn))
                    command.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static string addOrder(Order order)
        {
            string msg = "0";
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    connection.Open();

                    string sql = @" INSERT INTO [Order](IdUser, OrderRef, OrderDate, AmountHT, AmountTTC, DiscountPercent, PayementRef, PayementDate, PayementMethode, InvoiceDate, InvoiceFirstName, InvoiceLastName, InvoiceCity, InvoiceCountry, InvoiceAddress, InvoiceEmail, InvoicePhone, State) OUTPUT INSERTED.Id 
                                VALUES (@IdUser, @OrderRef, @OrderDate, @AmountHT, @AmountTTC, @DiscountPercent, @PayementRef, @PayementDate, @PayementMethode, @InvoiceDate, @InvoiceFirstName, @InvoiceLastName, @InvoiceCity, @InvoiceCountry, @InvoiceAddress, @InvoiceEmail, @InvoicePhone, @State)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (String.IsNullOrEmpty(order.IdUser.ToString()))
                            command.Parameters.AddWithValue("@IdUser", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@IdUser", order.IdUser);
                        if (String.IsNullOrEmpty(order.OrderRef))
                            command.Parameters.AddWithValue("@OrderRef", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@OrderRef", order.OrderRef);
                        if (order.OrderDate == new DateTime())
                            command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                        else
                            command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        if (String.IsNullOrEmpty(order.AmountHT.ToString()))
                            command.Parameters.AddWithValue("@AmountHT", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@AmountHT", order.AmountHT);
                        if (String.IsNullOrEmpty(order.AmountTTC.ToString()))
                            command.Parameters.AddWithValue("@AmountTTC", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@AmountTTC", order.AmountTTC);
                        if (String.IsNullOrEmpty(order.DiscountPercent.ToString()))
                            command.Parameters.AddWithValue("@DiscountPercent", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@DiscountPercent", order.DiscountPercent);
                        if (String.IsNullOrEmpty(order.PayementRef))
                            command.Parameters.AddWithValue("@PayementRef", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@PayementRef", order.PayementRef);
                        if (order.PayementDate == new DateTime())
                            command.Parameters.AddWithValue("@PayementDate", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@PayementDate", order.PayementDate);
                        if (String.IsNullOrEmpty(order.PayementMethode))
                            command.Parameters.AddWithValue("@PayementMethode", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@PayementMethode", order.PayementMethode);
                        if (order.InvoiceDate == new DateTime())
                            command.Parameters.AddWithValue("@InvoiceDate", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceDate", order.InvoiceDate);
                        if (String.IsNullOrEmpty(order.InvoiceFirstName))
                            command.Parameters.AddWithValue("@InvoiceFirstName", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceFirstName", order.InvoiceFirstName);
                        if (String.IsNullOrEmpty(order.InvoiceLastName))
                            command.Parameters.AddWithValue("@InvoiceLastName", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceLastName", order.InvoiceLastName);
                        if (String.IsNullOrEmpty(order.InvoiceCity))
                            command.Parameters.AddWithValue("@InvoiceCity", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceCity", order.InvoiceCity);
                        if (String.IsNullOrEmpty(order.InvoiceCountry))
                            command.Parameters.AddWithValue("@InvoiceCountry", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceCountry", order.InvoiceCountry);
                        if (String.IsNullOrEmpty(order.InvoiceAddress))
                            command.Parameters.AddWithValue("@InvoiceAddress", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceAddress", order.InvoiceAddress);
                        if (String.IsNullOrEmpty(order.InvoiceEmail))
                            command.Parameters.AddWithValue("@InvoiceEmail", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceEmail", order.InvoiceEmail);
                        if (String.IsNullOrEmpty(order.InvoicePhone))
                            command.Parameters.AddWithValue("@InvoicePhone", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoicePhone", order.InvoicePhone);
                        if (String.IsNullOrEmpty(order.State))
                            command.Parameters.AddWithValue("@State", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@State", order.State);

                        long Id = (long)command.ExecuteScalar();

                        if (Id > 0)
                        {
                            msg = Id.ToString();

                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            return msg;
        }
        public static string updateOrder(Order order)
        {
            string msg = "0";
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    connection.Open();

                    string sql = @" UPDATE [Order] 
                                    SET IdUser=@IdUser,
                                        OrderRef=@OrderRef,
                                        OrderDate=@OrderDate,
                                        AmountHT=@AmountHT,
                                        AmountTTC=@AmountTTC,
                                        DiscountPercent=@DiscountPercent,
                                        PayementRef=@PayementRef,
                                        PayementDate=@PayementDate,
                                        PayementMethode=@PayementMethode,
                                        InvoiceDate=@InvoiceDate,
                                        InvoiceFirstName=@InvoiceFirstName,
                                        InvoiceLastName=@InvoiceLastName,
                                        InvoiceCity=@InvoiceCity,
                                        InvoiceCountry=@InvoiceCountry,
                                        InvoiceAddress=@InvoiceAddress,
                                        InvoiceEmail=@InvoiceEmail,
                                        InvoicePhone=@InvoicePhone, 
                                        State=@State 
                                    WHERE Id=@Id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        if (String.IsNullOrEmpty(order.IdUser.ToString()))
                            command.Parameters.AddWithValue("@IdUser", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@IdUser", order.IdUser);
                        if (String.IsNullOrEmpty(order.OrderRef))
                            command.Parameters.AddWithValue("@OrderRef", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@OrderRef", order.OrderRef);
                        if (order.OrderDate == new DateTime())
                            command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                        else
                            command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        if (String.IsNullOrEmpty(order.AmountHT.ToString()))
                            command.Parameters.AddWithValue("@AmountHT", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@AmountHT", order.AmountHT);
                        if (String.IsNullOrEmpty(order.AmountTTC.ToString()))
                            command.Parameters.AddWithValue("@AmountTTC", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@AmountTTC", order.AmountTTC);
                        if (String.IsNullOrEmpty(order.DiscountPercent.ToString()))
                            command.Parameters.AddWithValue("@DiscountPercent", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@DiscountPercent", order.DiscountPercent);
                        if (String.IsNullOrEmpty(order.PayementRef))
                            command.Parameters.AddWithValue("@PayementRef", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@PayementRef", order.PayementRef);
                        if (order.PayementDate == new DateTime())
                            command.Parameters.AddWithValue("@PayementDate", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@PayementDate", order.PayementDate);
                        if (String.IsNullOrEmpty(order.PayementMethode))
                            command.Parameters.AddWithValue("@PayementMethode", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@PayementMethode", order.PayementMethode);
                        if (order.InvoiceDate == new DateTime())
                            command.Parameters.AddWithValue("@InvoiceDate", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceDate", order.InvoiceDate);
                        if (String.IsNullOrEmpty(order.InvoiceFirstName))
                            command.Parameters.AddWithValue("@InvoiceFirstName", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceFirstName", order.InvoiceFirstName);
                        if (String.IsNullOrEmpty(order.InvoiceLastName))
                            command.Parameters.AddWithValue("@InvoiceLastName", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceLastName", order.InvoiceLastName);
                        if (String.IsNullOrEmpty(order.InvoiceCity))
                            command.Parameters.AddWithValue("@InvoiceCity", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceCity", order.InvoiceCity);
                        if (String.IsNullOrEmpty(order.InvoiceCountry))
                            command.Parameters.AddWithValue("@InvoiceCountry", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceCountry", order.InvoiceCountry);
                        if (String.IsNullOrEmpty(order.InvoiceAddress))
                            command.Parameters.AddWithValue("@InvoiceAddress", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceAddress", order.InvoiceAddress);
                        if (String.IsNullOrEmpty(order.InvoiceEmail))
                            command.Parameters.AddWithValue("@InvoiceEmail", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoiceEmail", order.InvoiceEmail);
                        if (String.IsNullOrEmpty(order.InvoicePhone))
                            command.Parameters.AddWithValue("@InvoicePhone", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@InvoicePhone", order.InvoicePhone);
                        if (String.IsNullOrEmpty(order.State))
                            command.Parameters.AddWithValue("@State", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@State", order.State);

                        command.Parameters.AddWithValue("@Id", order.Id);

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
        public static List<Order> getAllOrders()
        {
            List<Order> lstOrder = new List<Order>();
            try
            {
                using (SqlConnection connection = DBConnection.GetConnection())
                {
                    connection.Open();
                    string sql = @" SELECT * 
                                    FROM [Order] 
                                    ORDER BY Id DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Order order = new Order();
                                order.Id = Convert.ToInt64(dataReader["Id"]);
                                order.IdUser = Convert.ToInt64(dataReader["IdUser"]);
                                order.OrderRef = dataReader["OrderRef"].ToString();
                                order.OrderDate = Convert.ToDateTime(dataReader["OrderDate"]);
                                order.AmountHT = float.Parse(dataReader["AmountHT"].ToString());
                                order.AmountTTC = float.Parse(dataReader["AmountTTC"].ToString());
                                order.DiscountPercent = Convert.ToInt32(dataReader["DiscountPercent"]);
                                order.PayementRef = dataReader["PayementRef"].ToString();

                               
                                lstOrder.Add(order);
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

            return lstOrder;
        }
    }
}
