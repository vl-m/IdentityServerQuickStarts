using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerApp
{
    public class SensitiveData
    {
        // Configure Google https://identityserver4.readthedocs.io/en/release/quickstarts/4_external_authentication.html
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }
    }
}
