﻿using System;
using System.Linq;
using System.Threading.Tasks;
using EPiServer;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Model.RedirectRule;
using Forte.EpiserverRedirects.Redirect;

namespace Forte.EpiserverRedirects.Resolver
{
    [Obsolete]
    public class WildcardResolver : BaseRuleResolver, IRedirectRuleResolver
    {
        private readonly IQueryable<RedirectRule> _redirectRuleResolverRepository;

        public WildcardResolver(IQueryable<RedirectRule> redirectRuleResolverRepository, IContentLoader contentLoader) : base(contentLoader)
        {
            _redirectRuleResolverRepository = redirectRuleResolverRepository;
        }

        public Task<IRedirect> ResolveRedirectRuleAsync(UrlPath oldPath)
        {
            return Task.Run(() =>
            {
                var rule = _redirectRuleResolverRepository
                    .Where(r => r.IsActive && r.RedirectRuleType == RedirectRuleType.Wildcard)
                    .OrderBy(r => r.Priority)
                    .FirstOrDefault();

                return ResolveRule(rule, r => new WildcardRedirect(r));
            });
        }
        
    }
}
