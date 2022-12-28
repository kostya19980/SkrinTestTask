using SkrinTestTask.Models;
using System;
using System.Data.SqlClient;

namespace SkrinTestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            // Читаем и десериализуем Xml
            XmlSerializer<Orders> xmlSerializer = new XmlSerializer<Orders>();
            var model = xmlSerializer.Deserialize("Orders.xml");
            // Подключаемся к БД
            DbConnection db = new DbConnection("server=.\\SQLEXPRESS01;database=TestDB;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True");
            foreach (var order in model.orders)
            {
                // Добавляем пользователя в БД
                string[] words = order.User.FIO.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@FirstName", words[1]),
                    new SqlParameter("@LastName", words[0]),
                    new SqlParameter("@Patronymic", words[2]),
                    new SqlParameter("@Email", order.User.Email)
                };
                int userId = db.InsertData("IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email) " +
                    "BEGIN SELECT UserId FROM Users WHERE Email = @Email END " +
                    "ELSE BEGIN INSERT INTO Users (FirstName, LastName, Patronymic, Email) output INSERTED.UserId VALUES (@FirstName, @LastName, @Patronymic, @Email) END", sqlParameters);
                // Добавляем общую информацию о заказе
                sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@OrderId", order.Id),
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@TotalSum", order.TotalSum),
                    new SqlParameter("@OrderDate", order.RegDate)
                };
                int OrderId = db.InsertData("IF NOT EXISTS (SELECT 1 FROM Orders WHERE OrderId = @OrderId) " +
                    "BEGIN INSERT INTO Orders (OrderId, UserId,TotalSum, OrderDate) output INSERTED.OrderId VALUES (@OrderId, @UserId, @TotalSum, @OrderDate) END", sqlParameters);
                // Если заказа еще нет в БД
                if(OrderId != 0)
                {
                    // Добавляем продукты и детали о заказе в БД
                    foreach (var product in order.Products)
                    {
                        // Продукт
                        sqlParameters = new SqlParameter[]
                        {
                            new SqlParameter("@Price", product.Price),
                            new SqlParameter("@Title", product.Name)
                        };
                        int productId = db.InsertData("IF EXISTS (SELECT 1 FROM Products WHERE Title = @Title) " +
                            "BEGIN SELECT ProductId FROM Products WHERE Title = @Title END " +
                            "ELSE BEGIN INSERT INTO Products (Price, Title) output INSERTED.ProductId VALUES (@Price, @Title) END", sqlParameters);
                        // Детали заказа
                        sqlParameters = new SqlParameter[]
                        {
                            new SqlParameter("@OrderId", order.Id),
                            new SqlParameter("@ProductId", productId),
                            new SqlParameter("@Quantity", product.Quantity)
                        };
                        db.InsertData("INSERT INTO OrderDetails (OrderId, ProductId, Quantity) output INSERTED.OrderId VALUES (@OrderId, @ProductId, @Quantity)", sqlParameters);
                    }
                }
            }
        }
    }
}
