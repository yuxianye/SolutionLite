using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    [RunInstaller(true)]
    public partial class SolutionInstaller : System.Configuration.Install.Installer
    {
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;

        public SolutionInstaller()
        {
            InitializeComponent();
            // 创建ServiceProcessInstaller对象和ServiceInstaller对象            
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 设定ServiceProcessInstaller对象的帐号、用户名和密码等信息            
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            //this.serviceProcessInstaller.Password = null;
            //this.serviceProcessInstaller.Username = null;
            // 设定服务的名称            
            this.serviceInstaller.ServiceName = "慧远数采服务";
            serviceInstaller.Description = "慧远数采服务";
            serviceInstaller.DisplayName = "慧远数采服务";
            //设定服务启动的方式            
            this.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.Installers.AddRange(new System.Configuration.Install.Installer[] { this.serviceProcessInstaller, this.serviceInstaller });
        }

    }
}
