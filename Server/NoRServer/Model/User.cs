using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoRServer.Model
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Registerdate { get; set; }
        public virtual bool IsOnline { get; set; }

        public override string ToString()
        {
            if (this == null)
                return null;
            return "\nId = "+Id + " Username = " + Username + " Password = " + Password + " Registerdate = " + Registerdate+"\n";
        }
    }
}
