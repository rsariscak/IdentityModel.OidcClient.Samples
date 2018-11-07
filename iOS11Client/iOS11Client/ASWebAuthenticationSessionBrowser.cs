using Foundation;
using System.Threading.Tasks;
using IdentityModel.OidcClient.Browser;
using AuthenticationServices;

namespace iOS11Client
{
    public class ASWebAuthenticationSessionBrowser : IBrowser
    {
        ASWebAuthenticationSession _af;

        public Task<BrowserResult> InvokeAsync(BrowserOptions options)
        {
            var wait = new TaskCompletionSource<BrowserResult>();

            _af = new ASWebAuthenticationSession(
                new NSUrl(options.StartUrl),
                options.EndUrl,
                (callbackUrl, error) =>
                {
                    if (error != null)
                    {
                        var errorResult = new BrowserResult
                        {
                            ResultType = BrowserResultType.UserCancel,
                            Error = error.ToString()
                        };

                        wait.SetResult(errorResult);
                    }
                    else
                    {
                        var result = new BrowserResult
                        {
                            ResultType = BrowserResultType.Success,
                            Response = callbackUrl.AbsoluteString
                        };

                        wait.SetResult(result);
                    }
                });

            _af.Start();
            return wait.Task;
        }
    }
}