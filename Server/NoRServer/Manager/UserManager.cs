using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoRServer.Model;
using NHibernate;
using NHibernate.Criterion;

namespace NoRServer.Manager
{
    class UserManager : IUserManager
    {
        private static UserManager instance = null;
        public static UserManager Get => instance ?? (instance = new UserManager());

        public void Add(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }

        public ICollection<User> GetAllUsers()
        {
            using(ISession session = NHibernateHelper.OpenSession())
            {
                return session.CreateCriteria(typeof(User)).List<User>();
            }
        }

        public User GetInfoById(int id)
        {
            using(ISession session = NHibernateHelper.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    User user = session.Get<User>(id);
                    transaction.Commit();
                    return user;
                }
            }
        }

        public User GetInfoByName(string name)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                //ICriteria criteria = session.CreateCriteria(typeof(User));
                //criteria.Add(Restrictions.Eq("Username", name));
                //User user = criteria.UniqueResult<User>();
                //return user;
                return session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("Username", name)).UniqueResult<User>();
                
            }
        }

        public bool IsOnline(string userName)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using(ITransaction tc = session.BeginTransaction())
                {
                    User user = session.CreateCriteria(typeof(User))
                        .Add(Restrictions.Eq("Username", userName))
                        .UniqueResult<User>();
                    tc.Commit();
                    return user.IsOnline;
                }
            }
        }

        public void ChangeLoading(string userName,bool isOnline)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using(ITransaction tc = session.BeginTransaction())
                {
                    User user = session.CreateCriteria(typeof(User))
                        .Add(Restrictions.Eq("Username", userName))
                        .UniqueResult<User>();
                    if (user == null)
                        return;
                    user.IsOnline = isOnline;
                    session.Update(user);
                    tc.Commit();

                }
            }
        }

        public void Remove(User user)
        {
            using(ISession session = NHibernateHelper.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(user);
                    transaction.Commit();
                }
            }
        }

        public void Update(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(user);
                    transaction.Commit();
                }
            }
        }

        public bool VerifyUser(string name,string password)
        {
            using(ISession session = NHibernateHelper.OpenSession())
            {
                User user = session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Username", name))
                    .Add(Restrictions.Eq("Password", password))
                    .UniqueResult<User>();
                return user != null;                    
            }
        }

        public int GetCoinByName(string name)
        {
            using(ISession session = NHibernateHelper.OpenSession())
            {
                User user = session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Username", name))
                    .UniqueResult<User>();
                return user.Coins;
            }
        }

        public void ChangeCoin(string name,int count)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                User user = session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Username", name))
                    .UniqueResult<User>();
                user.Coins += count;
                using(ITransaction ts = session.BeginTransaction())
                {
                    session.Update(user);
                    ts.Commit();
                }
            }
        }
    }
}
