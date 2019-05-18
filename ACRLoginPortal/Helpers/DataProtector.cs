using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACRLoginPortal.Helpers
{
    public class DataProtector
    {
        IDataProtector _protector;

        public DataProtector(IDataProtectionProvider provider, string purpose)
        {
            _protector = provider.CreateProtector(purpose);
        }

        public string ProtectStr(string input)
        {
            string key = _protector.Protect(input);
            return key;
        }

        public string UnprotectStr(string key)
        {
            try
            {
                return _protector.Unprotect(key);
            }
            catch
            {
                return "";
            }
        }
    }
}
