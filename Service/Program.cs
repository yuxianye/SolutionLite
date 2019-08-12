using System;
using System.ServiceProcess;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

#if DEBUG
                //调试时直接运行服务
                SolutionService solutionService = new SolutionService();
                var result = solutionService.StartService().Result;
                LogHelper.Logger.Info($"服务开启结果（调试）：{result}");
                Console.ReadKey();

#else
                LogHelper.Logger.Info($"服务启动...");
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new SolutionService()
                };
                ServiceBase.Run(ServicesToRun);
                LogHelper.Logger.Info($"服务启动完成");
#endif
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"服务程序启动错误{ex.Message}", ex);
                Console.ReadKey();
            }
        }
    }
}
