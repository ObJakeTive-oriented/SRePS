using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SRePS
{
    public class LoginParsing
    {
        public List<UserClass> UserList (XmlDocument doc)
        {
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/users/user");

            List<UserClass> users = new List<UserClass>();

            foreach (XmlNode node in nodes)
            {
                UserClass user = new UserClass();
                user.user = node.SelectSingleNode("user").InnerText;
                user.password = node.SelectSingleNode("password").InnerText;
                user.type = node.SelectSingleNode("type").InnerText;

                users.Add(user);
            }
            return users;
        }
    }

    class UserClass
    {
        public string user;
        public string password;
        public string type;
    }
}