using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoRServer.Model;

namespace NoRServer.Manager
{
    interface IUserManager
    {
        void Add(User user);            //增

        void Remove(User user);         //删

        User GetInfoById(int id);       //查
        User GetInfoByName(string name);
        ICollection<User> GetAllUsers();

        void Update(User user);         //改

        bool VerifyUser(string name, string password);     //验证

        bool IsOnline(string name);          //是否在线

        void ChangeLoading(string name,bool isLoad);        //登录游戏
    }
}
