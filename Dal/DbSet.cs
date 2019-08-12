using SqlSugar;
using System.Collections.Generic;

namespace Dal
{
    /// <summary>
    /// 数据集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbSet<T> : SimpleClient<T> where T : class, new()
    {
        public DbSet(SqlSugarClient context) : base(context)
        {

        }

        //SimpleClient中的方法满足不了你，你可以扩展自已的方法
        public List<T> GetByIds(dynamic[] ids)
        {
            return Context.Queryable<T>().In(ids).ToList();
        }
    }

}
