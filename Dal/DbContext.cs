using Models;
using SqlSugar;
using System;

namespace Dal
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class DbContext
    {
        /// <summary>
        /// DbContext构造函数，在内部初始化Db=SqlSugarClient
        /// </summary>
        public DbContext()
        {
            string connectionString = null;

#if DEBUG
            connectionString = Utility.ConfigHelper.GetAppSetting("ConnectionStringDebug");
            //LogHelper.Logger.Debug(Utility.Security.AESEncrypt(Utility.ConfigHelper.GetAppSetting("ConnectionStringDebug")));
#else
            connectionString = Utility.Security.AESDecrypt(Utility.ConfigHelper.GetAppSetting("ConnectionString"));
            connectionString = Utility.ConfigHelper.GetAppSetting("ConnectionStringDebug");

#endif
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                //数据库类型，MySql = 0,SqlServer = 1,Sqlite = 2,Oracle = 3,PostgreSQL = 4,不配置:1，默认：1
                DbType = (DbType)int.Parse(Utility.ConfigHelper.GetAppSetting("DbType") ?? "1"),
                IsAutoCloseConnection = true,
                //IsAutoCloseConnection = false,
                IsShardSameThread = true,
                MoreSettings = new ConnMoreSettings() { IsWithNoLockQuery = true },
            });
        }

        /// <summary>
        /// 用来处理事务多表查询和复杂的操作
        /// </summary>
        public SqlSugarClient Db;


        /// <summary>
        /// 工序/工站
        /// </summary>
        public DbSet<Station> StationDb { get { return new DbSet<Station>(Db); } }

        /// <summary>
        /// 应用日志
        /// </summary>
        public DbSet<AppLog> AppLogDb { get { return new DbSet<AppLog>(Db); } }

        /// <summary>
        /// 设备点表
        /// </summary>
        public DbSet<DeviceNode> DeviceNodeDb { get { return new DbSet<DeviceNode>(Db); } }

        /// <summary>
        /// 设备
        /// </summary>
        public DbSet<Equipment> EquipmentDb { get { return new DbSet<Equipment>(Db); } }

        /// <summary>
        /// 模块
        /// </summary>
        public DbSet<Module> ModuleDb { get { return new DbSet<Module>(Db); } }

        /// <summary>
        /// 通知公告
        /// </summary>
        public DbSet<Notice> NoticeDb { get { return new DbSet<Notice>(Db); } }

        /// <summary>
        /// OPC服务器
        /// </summary>
        public DbSet<OpcServer> OpcServerDb { get { return new DbSet<OpcServer>(Db); } }

        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Role> RoleDb { get { return new DbSet<Role>(Db); } }

        /// <summary>
        /// 角色模块
        /// </summary>
        public DbSet<RoleModule> RoleModuleDb { get { return new DbSet<RoleModule>(Db); } }

        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<User> UserDb { get { return new DbSet<User>(Db); } }

        /// <summary>
        /// 用户角色
        /// </summary>
        public DbSet<UserRole> UserRoleDb { get { return new DbSet<UserRole>(Db); } }
    }

}
