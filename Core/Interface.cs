using System;

namespace Core
{
    /// <summary>
    /// 表示审计属性
    /// </summary>
    public interface IAudited : ICreatedAudited, ICreatedTime, IUpdateAudited
    {
    }

    /// <summary>
    /// 给信息添加 创建时间、创建者 属性
    /// </summary>
    public interface ICreatedAudited : ICreatedTime
    {
        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        string CreatorUser { get; set; }
    }

    /// <summary>
    /// 表示实体将包含创建时间
    /// </summary>
    public interface ICreatedTime
    {
        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        DateTime CreatedTime { get; set; }
    }

    /// <summary>
    /// 表示实体将包含更新者，更新时间属性
    /// </summary>
    public interface IUpdateAudited
    {
        /// <summary>
        /// 获取或设置 最后更新时间
        /// </summary>
        DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        string LastUpdatorUser { get; set; }
    }

    /// <summary>
    /// 表示实体将包含备注属性
    /// </summary>
    public interface IRemark
    {
        /// <summary>
        /// 获取或设置 备注
        /// </summary>
        string IRemark { get; set; }
    }

}
