using Common.Function;
using Common.Logger;
using Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.BLL
{
    public class User : BaseBLL<Manager.Model.User>
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="userType">用户类型</param>
        /// <returns>登录结果</returns>
        public Message Login(string username, string password, string userType)
        {
            Logger.Instance(typeof(User)).Info("username:{0}登录系统.", username);
            Message result = new Message();
            try
            {
                //通过username到数据库中查询用户信息
                List<Manager.Model.User> users = base.Search(d => d.User_Username == username && d.User_IsDel == false);
                //判断是否取到用户
                if (users.Count > 0)
                {
                    //取到用户，取出第一个
                    Manager.Model.User user = users.First();
                    //对比密码
                    if (user.User_Password.Equals(Md5.GetEncryptionValue(password)))
                    {
                        //登录成功
                        Logger.Instance(typeof(User)).Info("用户登录成功.");
                        result.Status = true;
                        result.Context = "登录成功.";
                        result.Append = user.User_Id;
                        return result;
                    }
                }
                //登录失败
                result.Status = false;
                result.Context = "登录失败,用户名或密码错误.";
                return result;
            }
            catch (Exception e)
            {
                Logger.Instance(typeof(User)).Error(e.Message);
                result.Status = false;
                result.Context = "登录失败,未知异常.";
                return result;
            }
        }

        /// <summary>
        /// 通过id获取用户信息
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns>用户信息</returns>
        public Manager.Model.User GetUser(Guid id)
        {
            Logger.Instance(typeof(User)).Info("获取User:{0}的信息.", id.ToString());
            try
            {
                //通过user id查询用户
                List<Manager.Model.User> users = base.Search(d => d.User_Id == id && d.User_IsDel == false);
                if (users.Count > 0)
                {
                    return users.First();
                }
                return null;
            }
            catch (Exception e)
            {
                Logger.Instance(typeof(User)).Error(e.Message);
                return null;
            }
        }
    }
}
