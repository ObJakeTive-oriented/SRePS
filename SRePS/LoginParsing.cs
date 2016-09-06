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
        private List<UserClass> users = new List<UserClass>();
        public string[] userNames;

        public LoginParsing()
        {
            XElement rootElement = XElement.Load("test.xml");

            GetOutline(0, rootElement);

            int count = 0;
            userNames = new string[users.Count];
            foreach (UserClass u in users)
            {
                userNames[count] = u.user;
                count++;
            }
        }

        public List<UserClass> LoginList()
        {
            return users;
        }

        private void GetOutline(int indentLevel, XElement element)
        {
            if (element.Attribute("user") != null)
            {
                UserClass userDetails = new UserClass();
                userDetails.user = (string)element.Element("name");
                userDetails.password = (string)element.Element("password");
                userDetails.type = (string)element.Element("type");

                users.Add(userDetails);
            }

            foreach (XElement childElement in element.Elements())
            {
                GetOutline(indentLevel + 1, childElement);
            }
            return;
        }
    }

    public class UserClass
    {
        public string user;
        public string password;
        public string type;
    }
}