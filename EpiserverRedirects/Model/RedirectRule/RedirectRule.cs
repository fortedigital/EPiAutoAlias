using System;
using EPiServer.Data;
using EPiServer.Data.Dynamic;
using EPiServer.Security;

namespace Forte.EpiserverRedirects.Model.RedirectRule
{
    public enum RedirectRuleType { ExactMatch = 1, Regex = 2, Wildcard = 3}
    public enum RedirectType {Permanent = 1, Temporary = 2}
    public enum RedirectOrigin { System = 1, Manual = 2, Import = 3}
    
    [EPiServerDataStore(AutomaticallyRemapStore = true)]
    public class RedirectRule : IDynamicData
    {
        public void FromManual()
        {
            CreatedOn = DateTime.UtcNow;
            CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name;
            RedirectOrigin = RedirectOrigin.Manual;
        }

        public static RedirectRule NewFromManual(string oldPattern, string newPattern, RedirectType redirectType,
            RedirectRuleType redirectRuleType, bool isActive, string notes)
        {
            return new RedirectRule
            {
                RedirectOrigin = RedirectOrigin.Manual,
                OldPattern = oldPattern,
                NewPattern = newPattern,
                RedirectType = redirectType,
                RedirectRuleType = redirectRuleType,
                IsActive = isActive,
                CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name,
                CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                Notes = notes
            };
        }
        
        public static RedirectRule NewFromSystem(string oldPattern, string newPattern, RedirectType redirectType,
            RedirectRuleType redirectRuleType, string notes)
        {
            return new RedirectRule
            {
                RedirectOrigin = RedirectOrigin.System,
                OldPattern = oldPattern,
                NewPattern = newPattern,
                RedirectType = redirectType,
                RedirectRuleType = redirectRuleType,
                IsActive = true,
                CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name,
                CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                Notes = notes
            };
        }

        public static RedirectRule NewFromSystem(string oldPattern, int contentId, RedirectType redirectType,
            RedirectRuleType redirectRuleType, string notes)
        {
            return new RedirectRule
            {
                RedirectOrigin = RedirectOrigin.System,
                OldPattern = oldPattern,
                ContentId = contentId,
                RedirectType = redirectType,
                RedirectRuleType = redirectRuleType,
                IsActive = true,
                CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name,
                CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                Notes = notes,
            };
        }

        public static RedirectRule NewFromImport(string oldPattern, string newPattern, RedirectType redirectType,
            RedirectRuleType redirectRuleType, bool isActive, string notes)
        {
            return new RedirectRule
            {
                RedirectOrigin = RedirectOrigin.Import,
                OldPattern = oldPattern,
                NewPattern = newPattern,
                RedirectType = redirectType,
                RedirectRuleType = redirectRuleType,
                IsActive = isActive,
                CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name,
                Notes = notes
            };
        }

        public static RedirectRule NewFromImport(string oldPattern, int contentId, RedirectType redirectType,
            RedirectRuleType redirectRuleType, bool isActive, string notes)
        {
            return new RedirectRule
            {
                RedirectOrigin = RedirectOrigin.Import,
                OldPattern = oldPattern,
                ContentId = contentId,
                RedirectType = redirectType,
                RedirectRuleType = redirectRuleType,
                IsActive = isActive,
                CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                CreatedBy = PrincipalInfo.CurrentPrincipal.Identity.Name,
                Notes = notes
            };
        }

        public Identity Id { get; set; }

        public int? ContentId { get; set; }
        public string OldPattern{ get; set; }
        public string NewPattern { get; set; }

        public RedirectRuleType RedirectRuleType { get; set; }
        
        public RedirectType RedirectType { get; set; }
        
        public RedirectOrigin RedirectOrigin { get; set; }
        public DateTime CreatedOn { get; set; }
        
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string Notes { get; set; }
    }

}