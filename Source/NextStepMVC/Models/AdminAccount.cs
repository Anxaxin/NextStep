namespace NextStepMVC.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Microsoft.Ajax.Utilities;

    public static class AdminAccount
    {
        public static bool LogOn(string user, string pass, int id)
        {
            using (var ef = new CMSDbEntities())
            {
                var query = from b in ef.AdminLogins
                            where b.PageSettingId == id
                            where b.user.Equals(user)
                            where b.pass.Equals(pass)
                            select b;

                if (query.Count() == 1)
                {
                    return true;
                }

                return false;
            }
        }

        public static void LogOff()
        {
            HttpContext.Current.Session.Clear();
        }

        public static void AddUser(string userName, string passWord, int pageId)
        {
            using (var ef = new CMSDbEntities())
            {
                AdminLogin al = new AdminLogin();
                al.PageSettingId = pageId;
                al.pass = passWord;
                al.user = userName;

                ef.AdminLogins.Add(al);
                ef.SaveChanges();
            }
        }

        public static void DeleteUser(int id)
        {
            using (var ef = new CMSDbEntities())
            {
                var user = ef.AdminLogins.Find(id);
                if (user != null)
                { 
                    ef.AdminLogins.Remove(user);
                    ef.SaveChanges();
                }
            }
        }

        public static void EditUser(int id, string passWord)
        {
            using (var ef = new CMSDbEntities())
            {
                var user = ef.AdminLogins.Find(id);
                if (user != null)
                {
                    user.pass = passWord;
                    ef.SaveChanges();
                }
            }
        }
    }
}