namespace NextStepMVC
{
    public class SiteSettings
    {
        public string CultureName { get; set; }

        public string Alias { get; set; }

        public int PageSettingId { get; set; }

        public bool NextStepUrl { get; set; }

        public bool RedirectToMaster { get; set; }

        public string MasterUrl { get; set; }
    }
}