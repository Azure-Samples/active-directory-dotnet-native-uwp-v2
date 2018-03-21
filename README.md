---
services: active-directory
platforms: dotnet
author: jmprieur
level: 200
client: UWP 
service: Microsoft Graph
endpoint: AAD V2
---
![Build badge](https://identitydivision.visualstudio.com/_apis/public/build/definitions/a7934fdd-dcde-4492-a406-7fad6ac00e17/485/badge)

# Universal Windows Platform application signing in users with Microsoft and calling the Microsoft Graph

| [Getting Started](https://apps.dev.microsoft.com/portal/register-app)| [Library](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki) | [Docs](https://aka.ms/aadv2) | [Support](README.md#community-help-and-support) 
| --- | --- | --- | --- |

This simple sample demonstrates how to use the [Microsoft Authentication Library (MSAL) for .NET](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) to get an access token and call the Microsoft Graph (using OAuth 2.0 against the Azure AD v2.0 endpoint) for a Universal Windows Platform (UWP) application.

![Topology](ReadmeFiles/Topology.png)
## Steps to run

You can get full explaination about this sample, and build it from scratch by going to [Windows desktop .NET guided walkthrough](https://docs.microsoft.com/azure/active-directory/develop/guidedsetups/active-directory-mobileanddesktopapp-windowsdesktop-intro).
You would have to change a few things (see below, build from scratch)

If you just want to quickly run it, use the following instructions:

1. Register an Azure AD v2.0 (converged) app. 
    - Navigate to the [App Registration Portal](https://identity.microsoft.com). 
    - Go to the the `My Apps` page, click `Add an App`, and name your app.  
    - Set a platform by clicking `Add Platform`, select `Native`.
    - Copy to the clipboard your Application Id

2. Clone the code.
  ```
  git clone https://github.com/Azure-Samples/active-directory-dotnet-native-uwp-v2.git
  ```

3. In the `App.xaml.cs` file, set your application/client id copied from the App Registration Portal.

    ``private static string ClientId = "[Application Id pasted from the application registration portal]"``

4. (Optionally): Enable Windows Integrated Authentication when using a federated Azure AD tenant
Out of the box, this sample is not configured to work with Windows Integrated Authentication (WIA) when used with a federated Azure Active Directory domain. To work with WIA the application manifest must enable additional capabilities. These are not configured by default for this sample because applications requesting the Enterprise Authentication or Shared User Certificates capabilities require a higher level of verification to be accepted into the Windows Store, and not all developers may wish to perform the higher level of verification.
To enable Windows Integrated Authentication, in Package.appxmanifest, in the Capabilities tab, enable:
    - Enterprise Authentication
    - Private Networks (Client & Server)
    - Shared User Certificates
Also, in the constructor of the `App` in `App.xaml.cs`, add the following line of code: ```App.PublicClientApp.UseCorporateNetwork = true;```

5. Run the application from Visual Studio (Debug | Start without Debugging), directly on the local machine, or after deploying to a device or an emulator.

> Note that if your organization requires Multiple Factor Authentication (MFA), and you try to use the PIN, the certificate will be proposed, but the PIN window won't be presented. This is a known issue with WIA. As a workaround, you might want to use phone authentication (proposed as alternative ways of doing MFA)


## Steps to build from scratch
Follow the instructions given in [Windows desktop .NET guided walkthrough](https://docs.microsoft.com/azure/active-directory/develop/guidedsetups/active-directory-mobileanddesktopapp-windowsdesktop-intro), but:
- Instead of creating a WPF project, you will need to create a **UWP** project
- Instead of using a Label in the `MainWindow.xaml`, you will need to use a **TextBock** (as Labels are not supported in the UWP platform). Intead of the Label Content property, use the TextBlock's **Text** property
- With UWP applications, you don't need to add a cache as it's already managed by MSAL.Net

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
- [Recommended pattern to acquire a token in public client applications](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/AcquireTokenSilentAsync-using-a-cached-token#recommended-call-pattern-in-public-client-applications)
- [Acquiring tokens ineractively in public client applications](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Acquiring-tokens-interactively) 
- [Customizing Token cache serialization](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/token-cache-serialization)
