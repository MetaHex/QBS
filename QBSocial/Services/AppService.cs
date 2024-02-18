using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace QBSocial.Services
{
    public class AppService
    {
        private AppSettings AppSettings { get; set; }
        public AppService(
            IOptions<AppSettings> settings)
        {

            AppSettings = settings?.Value;
        }


        public string GetImgStorageUrl()
        {
            return AppSettings.StorageUrl;
        }
        // Consider using IWebAssemblyHostEnvironment HostEnvironment. See FTLoginDisplay.razor
        public bool IsProdEnvironment()
        {
            return AppSettings.IsProd;
        }
    }
}
