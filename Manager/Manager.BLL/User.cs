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
        public Message Login(string username, string password)
        {
            Logger.Instance(typeof(User)).Info("username:{0}登录系统.", username);
            Message result = new Message();
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
                    result.Append = user;
                    return result;
                }
            }
            //登录失败
            result.Status = false;
            result.Context = "登录失败,用户名或密码错误.";
            return result;
        }
    }
}
