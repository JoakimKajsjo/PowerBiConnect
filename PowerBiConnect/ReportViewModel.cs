using Microsoft.PowerBI.Api.V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerBiConnect
{
    public class ReportViewModel
    {
        public string accessToken;
        public string reportId;
        public string embedUrl;
        public Report report;

        public ReportViewModel(string accessToken, string reportId, string embedUrl)
        {
            this.accessToken = accessToken;
            this.reportId = reportId;
            this.embedUrl = embedUrl;
        }
    }
}
