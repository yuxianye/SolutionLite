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
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                });
                Console.WriteLine($"Code First CreateDb...！");
                Console.WriteLine($"ConnectionString:{db.CurrentConnectionConfig.ConnectionString}");

                //db.CodeFirst.InitTables(typeof(User), typeof(Msg102604), typeof(Msg102608), typeof(Msg102614)
                //   , typeof(Msg261001), typeof(Msg261003), typeof(Msg261005), typeof(Msg261006), typeof(Msg261007)
                //   , typeof(Msg261011), typeof(Msg261012), typeof(Msg261013));
                //db.CodeFirst.InitTables(typeof(User));

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
