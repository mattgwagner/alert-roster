using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace alert_roster.web.Models
{
    public class Authentication
    {
        public const String ReadOnlyRole = "ReadOnly";

        public const String ReadWriteRole = "ReadWrite";

        private static String ReadOnlyPassord { get { return ConfigurationManager.AppSettings["Password.ReadOnly"]; } }

        private static String ReadWritePassword { get { return ConfigurationManager.AppSettings["Password.ReadWrite"]; } }

        public static void Authenticate(String password)
        {
            if (ReadOnlyPassord == password)
            {
                FormsAuthentication.SetAuthCookie(Authentication.ReadOnlyRole, true);
            }
            else if (ReadWritePassword == password)
            {
                FormsAuthentication.SetAuthCookie(Authentication.ReadWriteRole, true);
            }
        }
    }
}