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

        public List<UserClass> LoginList()
        {
            XElement rootElement = XElement.Load("test.xml");

            GetOutline(0, rootElement);

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