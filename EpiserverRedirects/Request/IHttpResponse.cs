namespace Forte.EpiserverRedirects.Request
{
    public interface IHttpResponse
    {
        void Redirect(string location, int statusCode);
    }
}