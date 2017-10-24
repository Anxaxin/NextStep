namespace NextStepMVC
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web;

    public static class Settings
    {
        private static readonly Dictionary<string, SiteSettings> Domains = new Dictionary<string, SiteSettings>();

        static Settings()
        {
            using (var ef = new CMSDbEntities())
            {
                var query = from b in ef.PageSettings.Include("PageSettingDomains")
                            orderby b.Id
                            select b;
                foreach (var item in query)
                {
                    Domains.Add(item.PrimaryPageUri, new SiteSettings { PageSettingId = item.Id, CultureName = item.Culture, Alias = item.PrimaryPageUri, NextStepUrl = false, RedirectToMaster = false, MasterUrl = item.PrimaryPageUri });
                    foreach (var alias in item.PageSettingDomains)
                    {
                        Domains.Add(alias.Domain, new SiteSettings { PageSettingId = alias.PageSettingId, CultureName = alias.Culture, Alias = alias.Domain, NextStepUrl = alias.NextStepAlias, RedirectToMaster = true, MasterUrl = item.PrimaryPageUri });
                    }
                }
            }
        }

        public static SiteSettings GetCurrentDomain()
        {
            var host = HttpContext.Current.Request.Url.Host;
            SiteSettings s;
            Domains.TryGetValue(host, out s);
            return s;
        }

        public static CultureInfo GetCurrentCulture()
        {
            var host = HttpContext.Current.Request.Url.Host;
            SiteSettings s;
            if (!Domains.TryGetValue(host, out s))
            {
                s = Domains["nextstep.sima.dk"];
            }

            return CultureInfo.GetCultureInfo(s.CultureName);
        }
    }
}