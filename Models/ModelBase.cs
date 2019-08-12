using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Models
{
    /// <summary>
    /// 模型的基类，所有模型都需要继承自此基类.
    /// </summary>
    public abstract class ModelBase : Prism.Mvvm.BindableBase, IDisposable, IDataErrorInfo, ICloneable
    {
        #region IDisposable
        private bool _disposed;

        /// <summary>
        /// 释放对象，用于外部调用
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放当前对象时释放资源
        /// </summary>
        ~ModelBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// 重写以实现释放对象的逻辑
        /// </summary>
        /// <param name="disposing">是否要释放对象</param>
        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                Disposing();
            }
            _disposed = true;
        }

        /// <summary>
        /// 重写以实现释放派生类资源的逻辑
        /// </summary>
        protected virtual void Disposing()
        {

        }
        #endregion

        #region IDataErrorInfo
        [SugarColumn(IsIgnore = true)]
        public string this[string columnName]
        {
            get
            {
                var vc = new ValidationContext(this, null, null);
                vc.MemberName = columnName;
                var res = new List<ValidationResult>();
                var result = Validator.TryValidateProperty(this.GetType().GetProperty(columnName).GetValue(this, null), vc, res);
                if (res.Count > 0)
                {
                    foreach (var vr in res)
                    {

                        if (dicError.ContainsKey(columnName))
                        {
                            dicError[columnName] = (vr == ValidationResult.Success) ? false : true;
                        }
                        else
                        {
                            dicError.Add(columnName, (vr == ValidationResult.Success) ? false : true);
                        }
                    }
                    return string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
                }
                if (dicError.ContainsKey(columnName))
                {
                    dicError[columnName] = false;
                }
                else
                {
                    dicError.Add(columnName, false);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 错误
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string Error { get; }

        /// <summary>
        /// 错误字典
        /// </summary>
        private Dictionary<string, bool> dicError = new Dictionary<string, bool>();

        private bool isValidated;

        /// <summary>
        /// 是否通过验证，false验证失败 true验证通过
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool IsValidated
        {
            get
            {
                foreach (var v in dicError)
                {
                    //有错误那么验证失败，返回false
                    if (v.Value)
                    {
                        return false;
                    }
                }
                return true;
            }
            set
            {
                SetProperty(ref isValidated, value);
            }
        }

        #endregion

        #region ICloneable
        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
        #endregion
    }

    public abstract class ModelDalBase : ModelBase
    {
        #region GUID主键
        private Guid id;
        /// <summary>
        /// Desc:GUID主键
        /// Default:newid()
        /// Nullable:False
        /// </summary>           
        public Guid Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }
        #endregion

        #region 创建时间
        private DateTime? createdTime;
        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:True
        /// </summary>           
        public DateTime? CreatedTime
        {
            get { return createdTime; }
            set { SetProperty(ref createdTime, value); }
        }
        #endregion

        #region 创建人
        private string creatorUser;
        /// <summary>
        /// Desc:创建人
        /// Default:
        /// Nullable:True
        /// </summary>    
        public string CreatorUser
        {
            get { return creatorUser; }
            set { SetProperty(ref creatorUser, value); }
        }
        #endregion

        #region 最后更新时间
        private DateTime? lastUpdatedTime;
        /// <summary>
        /// Desc:最后更新时间
        /// Default:DateTime.Now
        /// Nullable:True
        /// </summary>           
        public DateTime? LastUpdatedTime
        {
            get { return lastUpdatedTime; }
            set { SetProperty(ref lastUpdatedTime, value); }
        }
        #endregion

        #region 最后更新人
        private string lastUpdatorUser;
        /// <summary>
        /// Desc:最后更新人
        /// Default:
        /// Nullable:True
        /// </summary>       
        public string LastUpdatorUser
        {
            get { return lastUpdatorUser; }
            set { SetProperty(ref lastUpdatorUser, value); }
        }
        #endregion

        #region 备注
        private string remark;
        /// <summary>
        /// Desc:备注
        /// Default:
        /// Nullable:True
        /// </summary>       
        public string Remark
        {
            get { return remark; }
            set { SetProperty(ref remark, value); }
        }
        #endregion

    }
}
