using Models;
using SqlSugar;
using System;

namespace Dal.CreateDb
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Code First start！");
                SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = Utility.ConfigHelper.GetAppSetting("ConnectionString"),
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                });
                Console.WriteLine($"Code First CreateDb...！");
                Console.WriteLine($"ConnectionString:{db.CurrentConnectionConfig.ConnectionString}");
                db.CodeFirst.InitTables(typeof(AppLog));
                db.CodeFirst.InitTables(typeof(DeviceNode));
                db.CodeFirst.InitTables(typeof(Equipment));
                db.CodeFirst.InitTables(typeof(Module));
                db.CodeFirst.InitTables(typeof(Notice));
                db.CodeFirst.InitTables(typeof(OpcServer));
                db.CodeFirst.InitTables(typeof(Role));
                db.CodeFirst.InitTables(typeof(RoleModule));
                db.CodeFirst.InitTables(typeof(User));
                db.CodeFirst.InitTables(typeof(UserRole));

                Console.WriteLine("Code First Complete！");
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Code First Error！");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
