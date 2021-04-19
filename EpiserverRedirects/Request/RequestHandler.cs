using System;
using System.Threading.Tasks;
using EPiServer.Web.Routing;
using Forte.EpiserverRedirects.Model;
using Forte.EpiserverRedirects.Redirect;
using Forte.EpiserverRedirects.Resolver;

namespace Forte.EpiserverRedirects.Request
{
    public class RequestHandler
    {
        private readonly IRedirectRuleResolver _redirectRuleResolver;
        private readonly IResponseStatusCodeResolver _responseStatusCodeResolver;
        private readonly IUrlResolver _urlResolver;

        public RequestHandler(IRedirectRuleResolver redirectRuleResolver,
            IResponseStatusCodeResolver responseStatusCodeResolver,
            IUrlResolver urlResolver)
        {
            _redirectRuleResolver = redirectRuleResolver;
            _responseStatusCodeResolver = responseStatusCodeResolver;
            _urlResolver = urlResolver;
        }

        public async Task Invoke(Uri request, IHttpResponse response)
        {
            var requestPath = UrlPath.FromUri(request);

            var redirectRule = await _redirectRuleResolver.ResolveRedirectRuleAsync(requestPath);

            if (redirectRule is NullRedirectRule)
            {
                var requestPathEncoded = UrlPath.FromUrlPathEncode(requestPath);
                
                redirectRule = await _redirectRuleResolver.ResolveRedirectRuleAsync(requestPathEncoded);
            }

            redirectRule?.Execute(request, response, _urlResolver, _responseStatusCodeResolver);
        }
    }
}