using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;

namespace NoRServer
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory = null;

        private static ISessionFactory SessionFactory => _sessionFactory ?? BuildSessionFactory();

        private static ISessionFactory BuildSessionFactory()
        {
            var config = new Configuration();

            config.Configure();
            config.AddAssembly("NoRServer");

            try
            {
                _sessionFactory = config.BuildSessionFactory();
            }
            catch (Exception e) 
            {
                throw;
            }
            return _sessionFactory;
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

    }
}