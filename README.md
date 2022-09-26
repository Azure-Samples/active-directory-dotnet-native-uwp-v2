---
services: active-directory
platforms: dotnet
author: Shama-K
level: 200
client: UWP
service: Microsoft Graph
endpoint: Microsoft identity platform
page_type: sample				  								 
languages:
- csharp
page_type: sample
products:
- azure
- azure-active-directory
- windows
- windows-uwp
- office-ms-graph
description: "This sample demonstrates how to use the Microsoft Authentication Library for .NET to get an access token and call the Microsoft Graph from a UWP app."
urlFragment: uwp-signing-in-graph-aad
---

# Universal Windows Platform application signing in users with Microsoft and calling the Microsoft Graph

![Build badge](https://identitydivision.visualstudio.com/_apis/public/build/definitions/a7934fdd-dcde-4492-a406-7fad6ac00e17/485/badge)

| [Getting Started](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-uwp)| [Library](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki) | [Docs](https://aka.ms/aadv2) | [Support](README.md#community-help-and-support)
| --- | --- | --- | --- |

## About this sample

### Table of contents
- [Universal Windows Platform application signing in users with Microsoft and calling the Microsoft Graph](#universal-windows-platform-application-signing-in-users-with-microsoft-and-calling-the-microsoft-graph)
  - [About this sample](#about-this-sample)
    - [Table of contents](#table-of-contents)
    - [Overview](#overview)
  - [Steps to run](#steps-to-run)
  - [How to run this sample](#how-to-run-this-sample)
    - [Step 1: Clone or download this repository](#step-1-clone-or-download-this-repository)
    - [Step 2: Register the sample application with your Azure Active Directory tenant](#step-2-register-the-sample-application-with-your-azure-active-directory-tenant)
      - [Choose the Azure AD tenant where you want to create your applications](#choose-the-azure-ad-tenant-where-you-want-to-create-your-applications)
      - [Register the UWP App (UWP-App-calling-MSGraph)](#register-the-uwp-app-uwp-app-calling-msgraph)
    - [Step 3: Configure the  UWP App (UWP-App-calling-MSGraph) to use your app registration](#step-3-configure-the--uwp-app-uwp-app-calling-msgraph-to-use-your-app-registration)
    - [Step 4: Run the sample](#step-4-run-the-sample)
  - [Community help and support](#community-help-and-support)
  - [Contributing](#contributing)
  - [More information](#more-information)

### Overview

This sample demonstrates how to use the [Microsoft Authentication Library (MSAL) for .NET](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) to get an access token and call the Microsoft Graph using the MS Graph SDK from a Universal Windows Platform (UWP) application.

1. The .NET client UWP application uses the Microsoft Authentication Library (MSAL) to sign-in a user and obtain a JWT access token from Azure Active Directory (Azure AD).
2. The access token is then used as a bearer token to call  Microsoft Graph and fetch the signed-in user's details.

> Looking for previous versions of this code sample? Check out the tags on the [releases](../../releases) GitHub page.

![Topology](ReadmeFiles/Topology.png)

## Steps to run

You can get full explanation about this sample, and build it from scratch by going to [Call the Microsoft Graph API from a Universal Windows Platform (UWP) application](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-uwp).
You would have to change a few things (see below, build from scratch)

## How to run this sample

To run this sample, you'll need:

- [Visual Studio 2022](https://aka.ms/vsdownload)
- An Internet connection
- Windows SDK minimum version 10.0.17663.0
- An Azure Active Directory (Azure AD) tenant. For more information on how to get an Azure AD tenant, see [How to get an Azure AD tenant](https://azure.microsoft.com/documentation/articles/active-directory-howto-tenant/)
- A user account in your Azure AD tenant. This sample will not work with a Microsoft account (formerly Windows Live account). Therefore, if you signed in to the [Azure portal](https://portal.azure.com) with a Microsoft account and have never created a user account in your directory before, you need to do that now.
- AzureAD Powershell Module if opting to use the automatic set up in Step 2 (available at the [Powershell Gallery](https://www.powershellgallery.com/packages/AzureAD/))

### Step 1: Clone or download this repository

From your shell or command line:

```Shell
git clone https://github.com/Azure-Samples/active-directory-dotnet-native-uwp-v2.git
```

or download and extract the repository .zip file.

> Given that the names of the referenced NuGet packages are quite long, you might want to clone it in a folder close to the root of your hard drive, to avoid file size limitations on Windows.

### Step 2: Register the sample application with your Azure Active Directory tenant

#### Choose the Azure AD tenant where you want to create your applications

As a first step you'll need to:

1. Sign in to the [Azure portal](https://portal.azure.com) using either a work or school account or a personal Microsoft account.
1. If your account is present in more than one Azure AD tenant, select your profile at the top right corner in the menu on top of the page, and then **switch directory**.
   Change your portal session to the desired Azure AD tenant.															 

#### Register the UWP App (UWP-App-calling-MSGraph)

1. Navigate to the Microsoft identity platform for developers [App registrations](https://go.microsoft.com/fwlink/?linkid=2083908) page.
1. Select **New registration**.
1. In the **Register an application page** that appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `UWP-App-calling-MSGraph`.
   - Under **Supported account types**, select **Accounts in any organizational directory and personal Microsoft accounts (e.g. Skype, Xbox, Outlook.com)**.
1. Select **Register** to create the application.
1. In the app's registration screen, find and note the **Application (client) ID**. You use this value in your app's configuration file(s) later in your code.
1. The redirect URI is tied to the application's identity. To find it, execute the [following code](https://github.com/Azure-Samples/active-directory-dotnet-native-uwp-v2/blob/5f798a47a35fff4f862d142561891845765ae836/Native_UWP_V2/MainPage.xaml.cs#L93) inside your app:

```csharp
 // returns smth like S-1-15-2-2601115387-131721061-1180486061-1362788748-631273777-3164314714-2766189824
string sid = Windows.Security.Authentication.Web.WebAuthenticationBroker.GetCurrentApplicationCallbackUri().Host.ToUpper();

// This is redirect uri you need to register in the app registration portal. The app config does not need it.
string redirectUri = $"ms-appx-web://microsoft.aad.brokerplugin/{sid}";
```

1. In the app's registration screen, select **Authentication** in the menu.
   - If you don't have a platform added, select **Add a platform** and select the **Public client (mobile & desktop)** option.
   - In the **Redirect URIs** | add the redirect URI of your app (see above).

1. Select **Save** to save your changes.
1. In the app's registration screen, click on the **API permissions** blade in the left to open the page where we add access to the APIs that your application needs.
   - Click the **Add a permission** button and then,
   - Ensure that the **Microsoft APIs** tab is selected.
   - In the *Commonly used Microsoft APIs* section, click on **Microsoft Graph**
   - In the **Delegated permissions** section, select **User.Read** in the list. Use the search box if necessary.
   - Click on the **Add permissions** button at the bottom.

### Step 3: Configure the  UWP App (UWP-App-calling-MSGraph) to use your app registration

Open the project in your IDE (like Visual Studio) to configure the code.
>In the steps below, "ClientID" is the same as "Application ID" or "AppId".

1. Open the `Native_UWP_V2\MainPage.xaml.cs` file
1. Find the below [line](https://github.com/Azure-Samples/active-directory-dotnet-native-uwp-v2/blob/5f798a47a35fff4f862d142561891845765ae836/Native_UWP_V2/MainPage.xaml.cs#L35)
   ```csharp
   private const string ClientId = "4a1aa1d5-c567-49d0-ad0b-cd957a47f842"
   ``` 
   and replace the existing value with the application ID (clientId) of the  `UWP-App-calling-MSGraph` application copied from the Azure portal.
1. Another option is to modify [this line](https://github.com/Azure-Samples/active-directory-dotnet-native-uwp-v2/blob/5f798a47a35fff4f862d142561891845765ae836/Native_UWP_V2/MainPage.xaml.cs#L37) which is currently set to **"Common"** that means that an user from any Tenant can log-in into the application. To restrict the log-in to current tenant, you should change the value to tenant Id or tenant name as explained inside comment for the line.  

### Step 4: Run the sample

1. Run the application from Visual Studio (Debug | Start without Debugging), directly on the local machine, or after deploying to a device or an emulator.

> [Consider taking a moment to share your experience with us.](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR73pcsbpbxNJuZCMKN0lURpUOTFTSkg3WUVGTjZENFVYMDRRQjdXUkUzQyQlQCN0PWcu)


## Community help and support

We use [Stack Overflow](http://stackoverflow.com/questions/tagged/msal) with the community to provide support. We highly recommend you ask your questions on Stack Overflow first and browse existing issues to see if someone has asked your question before. Make sure that your questions or comments are tagged with [msal.dotnet].

If you find and bug in the sample please raise the issue on [GitHub Issues](../../issues).

If you find a bug in msal.Net, please raise the issue on [MSAL.NET GitHub Issues](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/issues).

To provide a recommendation, visit our [User Voice page](https://feedback.azure.com/forums/169401-azure-active-directory).

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## More information

For more information see MSAL.NET's conceptual documentation:

- [Quickstart: Register an application with the Microsoft identity platform (Preview)](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app)
- [Quickstart: Configure a client application to access web APIs (Preview)](https://docs.microsoft.com/azure/active-directory/develop/quickstart-configure-app-access-web-apis)
- [Recommended pattern to acquire a token in public client applications](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/AcquireTokenSilentAsync-using-a-cached-token#recommended-call-pattern-in-public-client-applications)
- [Acquiring tokens interactively in public client applications](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Acquiring-tokens-interactively)
