using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EncryptionMd5
    {
        public static String md5(String key)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5");
        }
    }
}
