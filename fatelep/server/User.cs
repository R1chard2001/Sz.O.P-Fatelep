using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace server
{
    internal class User
    {
        public static List<User> UserList = new List<User>();
        public User(string Username, string Password, int IsAdmin)
        {
            this.Name = Username;
            this.Password = Password;
            this.IsAdmin = IsAdmin;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private int isAdmin;
        public int IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; }
        }


        public static User LoginTry(string username, string password)
        {
            return UserList.Find(u => u.Name == username && u.Password == password);
        }
        public static void LoadUsers(string Filename)
        {
            UserList.Clear();
            XDocument xml = XDocument.Load(Filename);
            foreach (var user in xml.Descendants("user"))
            {
                User newUser = new User(
                    (string)user.Attribute("name"), 
                    (string)user.Attribute("password"),
                    (int)user.Attribute("is_admin")
                    );
                UserList.Add(newUser);
            }
        }
        public static void SaveUsers(string Filename)
        {
            XElement root = new XElement("users");

            foreach (User u in UserList)
            {
                root.Add(
                    new XElement(
                        "user",
                        new XAttribute((XName)"name", u.Name),
                        new XAttribute((XName)"password", u.Password),
                        new XAttribute((XName)"is_admin", u.IsAdmin)
                        )
                    );
            }
            XDocument xml = new XDocument(root);
            xml.Save(Filename);
        }
    }
}
