using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.Rest;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace PowerBiConnect.Controllers
{
    public class HomeController : Controller
    {
        readonly string aadAuthUri = "https://login.windows.net/common/oauth2/authorize/";
        readonly string redirectUri = "http://localhost/Home/Index";
        readonly string appId;
        readonly string appSecret;
        readonly string powerBiApiUri = "https://api.powerbi.com/";
        string code;
        string accessToken;
        Report report = new Report();

        public HomeController(IConfiguration configuration)
        {
            appId = Environment.GetEnvironmentVariable("POWERBICONNECT_APPID");
            appSecret = Environment.GetEnvironmentVariable("POWERBICONNECT_APPSECRET");
        }

        public ActionResult Index()
        {
            code = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("code");
            if (code == null)
            {
                GetAuthorizationCode();
                return View();
            }
            else
            {
                accessToken = GetAccessToken();

                report = GetReport();
                return View(new ReportViewModel(accessToken, report.Id, report.EmbedUrl));
            }  
        }

        public string GetAccessToken()
        {
            TokenCache tokenCache = new TokenCache();
            string authority = aadAuthUri;
            AuthenticationContext authContext = new AuthenticationContext(authority, tokenCache);
            ClientCredential clientCredential = new ClientCredential(appId, appSecret);

            return authContext.AcquireTokenByAuthorizationCodeAsync(code, new Uri(redirectUri), clientCredential).Result.AccessToken;
        }

        public void GetAuthorizationCode()
        {
            NameValueCollection parameters = new NameValueCollection
            {
                {"response_type", "code" },
                {"client_id", appId },
                {"redirect_uri" , redirectUri },
                {"resource" , "https://analysis.windows.net/powerbi/api" }
            };

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add(parameters);

            Response.Redirect(String.Format(aadAuthUri + "?{0}", queryString));
        }

        public Report GetReport()
        {
            PowerBIClient client = new PowerBIClient(new Uri(powerBiApiUri), new TokenCredentials(accessToken, "Bearer"));
            return client.Reports.GetReports().Value.FirstOrDefault();
        }
    }
}