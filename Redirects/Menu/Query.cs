using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Shell.Services.Rest;
using Forte.Redirects.Model.RedirectRule;
using Forte.Redirects.Model.RedirectType;

namespace Forte.Redirects.Menu
{
    [ModelBinder(typeof(QueryModelBinder))]
    public class Query
    {
        public string oldPattern { get; set; } //oldUrlSearch
        public string newPattern { get; set; } //newUrlSearch
        public RedirectType? redirectType{ get; set; } //priority search
        public RedirectRuleType? redirectRuleType{ get; set; } //typeSearch
        public IEnumerable<SortColumn> SortColumns { get; set; }
        public ItemRange Range { get; set; }
    }

    public static class QueryExtension
    {
        public static IEnumerable<RedirectRule> Get(this IQueryable<RedirectRule> redirectRules, Query query = null)
        {
            if (query == null)
                return redirectRules.AsEnumerable();

            if (!string.IsNullOrEmpty(query.oldPattern))
                redirectRules = redirectRules.Where(rr => rr.OldPattern.Contains(query.oldPattern));

            if (!string.IsNullOrEmpty(query.newPattern))
                redirectRules = redirectRules.Where(rr => rr.NewPattern.Contains(query.newPattern));

            if (query.redirectType != null)
                redirectRules = redirectRules.Where(rr => rr.RedirectType == query.redirectType);

            if (query.redirectRuleType != null)
                redirectRules = redirectRules.Where(rr => rr.RedirectRuleType == query.redirectRuleType);

            if (query.SortColumns != null)
                redirectRules = redirectRules.OrderBy(query.SortColumns);
            
            return redirectRules
                .ApplyRange(query.Range)
                .Items
                .AsEnumerable();
        }
    }
}