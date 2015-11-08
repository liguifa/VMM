namespace Manager.DAL
{
    ﻿using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Linq.Expressions;
    using System.Data;
    using System.Diagnostics;
    using Manager.Model;
    using System.Data.Entity.Infrastructure;

    #region public class BaseDAL+数据层库父类
    /// <summary>
    /// 数据层库父类
    /// </summary>
    public class BaseDAL<T> where T : class
    {
        VMMEntities db = new VMMEntities();

        #region public int Add(T model)+数据库增加
        /// <summary>
        /// 数据库增加
        /// </summary>
        /// <param name="model">要增加的实体对象</param>
        /// <returns></returns>
        public int Add(T model)
        {
            db.Set<T>().Add(model);
            return db.SaveChanges();
        }
        #endregion

        #region public int Del(T model)+根据id删除
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="model">包含主键实体对象</param>
        /// <returns></returns>
        public int Del(T model)
        {
            db.Set<T>().Attach(model);
            db.Set<T>().Remove(model);
            return db.SaveChanges();
        }
        #endregion

        #region public int DelWhere(Expression<Func<T, bool>> where)+根据条件删除
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="where">删除条件Lambda表达式</param>
        /// <returns></returns>
        public int DelWhere(Expression<Func<T, bool>> where)
        {
            List<T> list = db.Set<T>().Where(where).ToList();
            list.ForEach(d => db.Set<T>().Remove(d));
            return db.SaveChanges();
        }
        #endregion

        #region public int Modify(T model, params string[] proNames)+数据层修改
        /// <summary>
        /// 数据层修改
        /// </summary>
        /// <param name="model">要修改的实体对象</param>
        /// <param name="proNames">要修改的属性名称</param>
        /// <returns></returns>
        public int Modify(T model, params string[] proNames)
        {
            DbEntityEntry entry = db.Entry<T>(model);
            entry.State = EntityState.Unchanged;
            foreach (string proName in proNames)
            {
                entry.Property(proName).IsModified = true;
            }

            return db.SaveChanges();
        }
        #endregion

        #region public List<T> Search(Expression<Func<T, bool>> whereLambda)+普通查询
        /// <summary>
        /// 普通查询
        /// </summary>
        /// <param name="whereLambda">查询条件Lambda表达式</param>
        /// <returns></returns>
        public List<T> Search(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().AsNoTracking().Where(whereLambda).ToList();
        }
        #endregion

        #region public List<T> Search<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda)+排序查询
        /// <summary>
        /// 排序查询
        /// </summary>
        /// <typeparam name="TKey">泛型类对象</typeparam>
        /// <param name="whereLambda">查询条件Lambda表达式</param>
        /// <param name="orderLambda">排序条件Lambda表达式</param>
        /// <returns></returns>
        public List<T> Search<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda)
        {
            return db.Set<T>().Where(whereLambda).OrderBy(orderLambda).ToList();
        }
        #endregion

        #region public List<T> Search<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, int pageIndex, int pageSize)+分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TKey">泛型类对象</typeparam>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">排序条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        public List<T> Search<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            return db.Set<T>().Where(whereLambda).OrderByDescending(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        #endregion

        #region public long SearchCount()+查询元素总个数
        /// <summary>
        /// 查询元素总个数
        /// </summary>
        /// <returns></returns>
        public long SearchCount(Expression<Func<T, bool>> whereLambad)
        {
            return db.Set<T>().Count(whereLambad);
        }
        #endregion
    }
    #endregion
}