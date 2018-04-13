using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;

namespace NoRServer.Manager
{
    public class PriceManager : IPriceManager
    {
        private static PriceManager instance = null;
        public static PriceManager Get => instance ?? (instance = new PriceManager());

        public int GetPrice(string carName)
        {
            using(ISession session = NHibernateHelper.OpenSession())
            {
                Model.CarPrice car = session.CreateCriteria(typeof(Model.CarPrice))
                    .Add(Restrictions.Eq("CarName", carName))
                    .UniqueResult<Model.CarPrice>();

                if (car == null)
                    return -1;

                return car.Price;
            }
        }
    }
}
