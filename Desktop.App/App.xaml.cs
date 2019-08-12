using MahApps.Metro;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace Desktop.App
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
            try
            {
                Uri uri = null;
                //uri = new Uri(@"pack://application:,,,/Desktop.Resource;component/DefaultResources.xaml", UriKind.RelativeOrAbsolute);
                uri = new Uri(AppPath + @"DefaultResources.xaml", UriKind.RelativeOrAbsolute);
                ResourceDictionary res = new ResourceDictionary { Source = uri };
                Application.Current.Resources.MergedDictionaries.Add(res);

                var accent = ThemeManager.GetAccent(Utility.ConfigHelper.GetAppSetting("Accent"));
                var appTheme = ThemeManager.GetAppTheme(Utility.ConfigHelper.GetAppSetting("Theme"));
                ThemeManager.ChangeAppStyle(Application.Current, accent, appTheme);
            }
            catch (Exception ex)
            {
                string msg = "应用程序初始化错误！请联系管理员！";
                LogHelper.Logger.Error(msg, ex);
                MessageBox.Show(msg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Mutex used to allow only one instance.
        /// </summary>
        private Mutex _mutex;

        /// <summary>
        /// Name of mutex to use. Should be unique for all applications.
        /// </summary>
        public const string MutexName = "SolutionLite.DesktopApp";

        /// <summary>
        /// Sets the foreground window.
        /// </summary>
        /// <param name="hWnd">Window handle to bring to front.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// AppPath(包含末尾的斜线)，应用程序的exe路径
        /// </summary>
        public static readonly string AppPath = Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.LastIndexOf('\\') + 1);

        /// <summary>
        /// Handler that closes the mutex.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void CloseMutexHandler(object sender, EventArgs e)
        {
            _mutex?.Close();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.Register<Dal.DbContext.IDalModule, Dal.DalModule>();
            //containerRegistry.Register<Dal.DbContext>();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
            //return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            //Assembly assembly = Assembly.Load("ModuleA");
            //Type v = assembly.GetType("ModuleA.ModuleAModule");
            //moduleCatalog.AddModule(new ModuleInfo(v, "ContentRegion", InitializationMode.OnDemand));
        }


        protected override void ConfigureViewModelLocator()
        {
            ////base.ConfigureViewModelLocator();
            //ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            //{
            //    var viewName = viewType.FullName;
            //    var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            //    var viewModelName = $"{viewName}Model, {viewAssemblyName}";
            //    return Type.GetType(viewModelName);
            //});

        }

        protected override Window CreateShell()
        {
            // Try to grab mutex
            bool createdNew;
            _mutex = new Mutex(true, MutexName, out createdNew);
            if (!createdNew)
            {
                // Bring other instance to front and exit.
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        //MessageBox.Show("当前程序已启动!");
                        LogHelper.Logger.Info("当前程序已启动!");
                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
                Application.Current.Shutdown();
                return new Window();
            }
            else
            {
                Exit += CloseMutexHandler;
                // Add Event handler to exit event.
                return Container.Resolve<MainWindow>();
            }

        }
    }
}
