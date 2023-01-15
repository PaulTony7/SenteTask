using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace SenteTask
{
    static class Input
    {
        public static bool LoadCompany(string path, out List<User> users, out User owner)
        {
            users = new List<User>();
            owner = null;
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode root = doc.FirstChild;
            if(!root.HasChildNodes)
                return false;

            //Load owner
            XmlNode ownerNode = root.FirstChild;
            owner = new User(uint.Parse(ownerNode.Attributes["id"].Value), null);
            users.Add(owner);
            if(!ownerNode.HasChildNodes)
                return true;
            
            owner.subordinates = new List<User>();
            foreach(XmlNode childNode in ownerNode.ChildNodes)
                owner.subordinates.Add(LoadUser(childNode, owner, users));
            return true;
        }
        
        static User LoadUser(XmlNode userNode, User supervisor, List<User> users)
        {

            User user = new User(
                uint.Parse(userNode.Attributes["id"].Value),
                supervisor
            );
            users.Add(user);
            user.subordinates = new List<User>();
            if(!userNode.HasChildNodes)
                return user;
            foreach(XmlNode childNode in userNode.ChildNodes)
                user.subordinates.Add(LoadUser(childNode, user, users));
            return user;

        }

        public static bool LoadTransfers(string path, out List<Transfer> transfers, List<User> users)
        {
            transfers = new List<Transfer>();
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode root = doc.FirstChild;
            if(!root.HasChildNodes)
                return false;
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                uint id = uint.Parse(root.ChildNodes[i].Attributes["od"].Value);
                User user = users.First(x => x.id == id);
                if(user == null)
                    return false;
                transfers.Add(new Transfer(
                    user,
                    uint.Parse(root.ChildNodes[i].Attributes["kwota"].Value)
                ));
            }
            return true;
        }
    }
}