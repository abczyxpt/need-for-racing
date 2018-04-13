using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;

namespace NoRServer.Manager
{
    public class CarManager : ICarManager
    {
        private static CarManager instance = null;
        public static CarManager Get => instance ?? (instance = new CarManager());

        public bool BuyCar(string userName, string carName)
        {
            Model.User user = UserManager.Get.GetInfoByName(userName);
            //安全判断
            if (user == null)
                return false;
            //判断钱是否够
            int carPrice = PriceManager.Get.GetPrice(carName);
            LogInit.Log.Info("汽车价格 : " + carPrice + "玩家金币" + user.Coins);
            if (user.Coins < carPrice)
                return false;
            
            using(ISession session = NHibernateHelper.OpenSession())
            {
                Model.UserCar userCar = FindCar(userName);
                if (userCar == null)
                {
                    AddCar(user, carName);
                    userCar = FindCar(userName);
                }
                using(ITransaction tc = session.BeginTransaction())
                {
                    switch (carName)
                    {
                        case "SportCar":
                            //1.扣钱
                            UserManager.Get.ChangeCoin(user.Username, -carPrice);
                            //2.买成功
                            userCar.SportCar = true;
                            break;
                    }
                    session.Update(userCar);
                    tc.Commit();
                }
            }

            return true;
        }

        public Model.UserCar FindCar(string userName)
        {
            using(ISession session = NHibernateHelper.OpenSession())
            {
                Model.User user = UserManager.Get.GetInfoByName(userName);
                LogInit.Log.Info("寻找汽车 " + user.Id);
                return session.CreateCriteria(typeof(Model.UserCar))
                    .Add(Restrictions.Eq("UserId", user.Id))
                    .UniqueResult<Model.UserCar>();
            }
        }

        public bool HaveCar(string userName, string carName)
        {
            Model.UserCar userCar = FindCar(userName);
            LogInit.Log.Info("寻找汽车反馈" + (userCar == null));
            if (userCar == null)
                return false;

            bool isHave = false;
            switch (carName)
            {
                case "SportCar":
                    isHave = userCar.SportCar;
                    break;
                default:
                    break;
            }
            return isHave;
        }

        public void AddCar(Model.User user,string carName)
        {
            using(ISession session = NHibernateHelper.OpenSession())
            {
                using(ITransaction tc = session.BeginTransaction())
                {
                    Model.UserCar userCar = new Model.UserCar
                    {
                        UserId = user.Id,
                        SportCar = true,
                    };
                    LogInit.Log.Info("添加：" + userCar.UserId + " " + userCar.SportCar);
                    session.Save(userCar);
                    tc.Commit();
                }
            }
        }
    }
}
