using Microsoft.Identity.Client;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace active_directory_dotnet_native_uwp_v2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Set the API Endpoint to Graph 'me' endpoint
      
        //Set the scope for API call to user.read
        string[] scopes = new string[] { "user.read" };

        // Below are the clientId (Application Id) of your app registration and the tenant information. 
        // You have to replace:
        // - the content of ClientID with the Application Id for your app registration
        // - The content of Tenant by the information about the accounts allowed to sign-in in your application:
        //   - For Work or School account in your org, use your tenant ID, or domain
        //   - for any Work or School accounts, use organizations
        //   - for any Work or School accounts, or Microsoft personal account, use common
        //   - for Microsoft Personal account, use consumers
        private const string ClientId = "[Enter client ID of the app as obtained from Azure Portal, e.g. 4a1aa1d5-c567-49d0-ad0b-cd957a47f842]";
        public IPublicClientApplication PublicClientApp { get; } 

        public MainPage()
        {
            this.InitializeComponent();


            // To change from Microsoft public cloud to a national cloud, use another value of AzureCloudInstance
            PublicClientApp = PublicClientApplicationBuilder.Create(ClientId)
                .WithAuthority("https://login.microsoftonline.com/common")
                .WithLogging((level, message, containsPii) =>
                {
                    Debug.WriteLine($"MSAL: {level} {message} ");
                }, LogLevel.Warning, enablePiiLogging: false, enableDefaultPlatformLogging: true)
                .WithUseCorporateNetwork(false)
                .WithRedirectUri(Constant.PublicClientRedirectUri)
                .Build();                
        }

        /// <summary>
        /// Call AcquireTokenAsync - to acquire a token requiring user to sign-in
        /// </summary>
        private async void CallGraphButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationResult authResult = null;
            ResultText.Text = string.Empty;
            TokenInfoText.Text = string.Empty;

            // It's good practice to not do work on the UI thread, so use ConfigureAwait(false) whenever possible.            
            IEnumerable<IAccount> accounts = await PublicClientApp.GetAccountsAsync().ConfigureAwait(false);
            IAccount firstAccount = accounts.FirstOrDefault();

            try
            {
                authResult = await PublicClientApp.AcquireTokenSilent(scopes, firstAccount)
                                                  .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    authResult = await PublicClientApp.AcquireTokenInteractive(scopes)
                                                      .ExecuteAsync()
                                                      .ConfigureAwait(false);
                }
                catch (MsalException msalex)
                {
                    await DisplayMessageAsync($"Error Acquiring Token:{System.Environment.NewLine}{msalex}");
                }
            }
            catch (Exception ex)
            {
                await DisplayMessageAsync($"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}");
                return;
            }

            if (authResult != null)
            {
                var graphServiceClient = GetGraphServiceClient(authResult.AccessToken);
                
                User graphUser = await graphServiceClient.Me.Request().GetAsync();

                // Go back to the UI thread to make changes to the UI
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ResultText.Text = "Display Name: " + graphUser.DisplayName + "\nBusiness Phone: " + graphUser.BusinessPhones.FirstOrDefault()
                                      + "\nGiven Name: " + graphUser.GivenName + "\nid: " + graphUser.Id
                                      + "\nUser Principal Name: " + graphUser.UserPrincipalName;
                    DisplayBasicTokenInfo(authResult);
                    this.SignOutButton.Visibility = Visibility.Visible;
                });
            }
        }

        /// <summary>
        /// Instantiating GraphServiceClient using the access token.
        /// </summary>
        /// <param name="token">The token</param>
        /// <returns>GraphServiceClient</returns>
        private GraphServiceClient GetGraphServiceClient(string token)
        {
            GraphServiceClient graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return Task.FromResult(0);
            }));
            return graphServiceClient;
        }

        /// <summary>
        /// Sign out the current user
        /// </summary>
        private async void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<IAccount> accounts = await PublicClientApp.GetAccountsAsync().ConfigureAwait(false);
            IAccount firstAccount = accounts.FirstOrDefault();

            try
            {
                await PublicClientApp.RemoveAsync(firstAccount).ConfigureAwait(false);
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ResultText.Text = "User has signed-out";
                    this.CallGraphButton.Visibility = Visibility.Visible;
                    this.SignOutButton.Visibility = Visibility.Collapsed;
                });
            }
            catch (MsalException ex)
            {
                ResultText.Text = $"Error signing-out user: {ex.Message}";
            }
        }

        /// <summary>
        /// Display basic information contained in the token. Needs to be called from the UI thead.
        /// </summary>
        private void DisplayBasicTokenInfo(AuthenticationResult authResult)
        {
            TokenInfoText.Text = "";
            if (authResult != null)
            {
                TokenInfoText.Text += $"User Name: {authResult.Account.Username}" + Environment.NewLine;
                TokenInfoText.Text += $"Token Expires: {authResult.ExpiresOn.ToLocalTime()}" + Environment.NewLine;
            }
        }

        /// <summary>
        /// Displays a message in the ResultText. Can be called from any thread.
        /// </summary>
        private async Task DisplayMessageAsync(string message)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                   () =>
                   {
                       ResultText.Text = message;
                   });
        }
    }
}
