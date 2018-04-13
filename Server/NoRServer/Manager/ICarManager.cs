using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoRServer.Manager
{
    interface ICarManager
    {
        bool BuyCar(string userName,string carName);
        Model.UserCar FindCar(string userName);
        bool HaveCar(string userName, string carName);
        void AddCar(Model.User user, string carName);
    }
}
