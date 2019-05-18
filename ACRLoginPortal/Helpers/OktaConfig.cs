using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACRLoginPortal.Helpers
{
    public class OktaConfig
    {
        public string Okta_OrgUri { set; get; } 
        public string Okta_AuthServer { set; get; }
        public string Okta_APIToken { set; get; }
        public string Okta_Scope { get; set; }
        public string Okta_ClientId { get; set; }
        public string Okta_ClientSecret { get; set; }
        public string SMTP_Server { get; set; }
        public string SMTP_Port { get; set; }
        public string SMTP_Username { get; set; }
        public string SMTP_Password { get; set; }
        public bool SMTP_EnableSSl { get; set; }
    }
}
