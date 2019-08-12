using System;

namespace Dal.CreateModel
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("DB First start！");
                Dal.DbContext dbContext = new DbContext();
                Console.WriteLine("DB First Create Class File...！");
                string directoryPath = Utility.ConstValue.AppPath + @"..\Models\DbModel";
                Console.WriteLine($"Class File Directory Path:{directoryPath}");
                //dbContext.Db.DbFirst.IsCreateAttribute(true).IsCreateDefaultValue(true).CreateClassFile(directoryPath);
                dbContext.Db.DbFirst.CreateClassFile(directoryPath);
                Console.WriteLine("DB First Complete！");
                Console.WriteLine("Press 'o' key open directory！Press other key close this console.");
                while (true)
                {
                    System.Threading.Thread.Sleep(500);
                    var v = Console.ReadKey();
                    if (v.Key == ConsoleKey.O)
                    {
                        System.Diagnostics.Process.Start("explorer", directoryPath);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
