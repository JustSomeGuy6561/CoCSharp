# CoCSharp
CSharp port/rewrite of Corruption Of Champions Mod

This is still largely a work in development, expect a full, nicer readme at some future date.

This project is separated into 3 layers: Backend, Frontend, and UI. This is designed to be modular - Modifications to the content (in the frontend) would not break the GUI layer, and different versions of the GUI could be implemented targeting different platforms. 
Currently, there is only one implementation of the GUI, for Windows Desktop. The current goal is to use Xamarin for MacOS and Xamarin for Android to bring support to those platforms. A low priority, but eventual goal is to also implement a Linux version using GTKSharp, (low priority because Linux doesn't have a nice universal Graphics Framework, and Mono doesn't have WPF/Winforms/Metro support.)

Obligitory Disclaimer: This work contains plain-text that is sexually explicit in nature. The intention of this repository is to expose the source code responsible for running this game, not exposing anything explicit. It is our belief that this is not in violation of Terms of Service, though we will strive to meet such requirements if it is deemed not to be the case. 

##Build:##

There are currently 3 projects in this solution, but only one will build an application. Backend and Frontend both build into Class Libraries, which basically just means they have no main method and must be used by other applications in order to function properly. 
Frontend and backend thus target .Net Standard, version 2.0, using C# version 7.3. 

WinDesktop targets .Net Framework version 4.7.1, but i believe 4.6.2 is actually allowed without any issues. Again, C# version 7.3.

This project requires the Weak Event NuGet package from Thomas Levesque, available here, https://www.nuget.org/packages/ThomasLevesque.WeakEvent/

These instructions are written assuming you are using Visual Studio. It is freely available, and for Mac OS and Windows. 
Your Visual Studio installation will need to support ".NET Desktop Development", and if you wish to create an iOS or Android version, you will also need "Mobile Development with .NET". Both options are directly in the installer, which you can access when downloading Visual Studio, or by Running Visual Studio Installer and modifiying the existing install, if already installed. 

You will need to build a new solution, with 3 projects in it: CoC.Backend, CoC.Frontend, and CoC.WinDesktop. CoC.Frontend and CoC.Backend are both Class Libraries, and CoC.WinDesktop is a Windows Application. Include all the Backend content in CoC.Backend, ditto for Frontend and WinDesktop. Make sure these target C# version 7.3.

At this point, you'll need to include the Weak Event package. Tools-> NuGet Package Manager -> Manage Packages For Solution...
Browse for 'ThomasLevesque.WeakEvent', and include in all projects. 

At this point, you should be ready to build and run, though you'll likely get a build error, which can be fixed below.

Quick error fix: 
Ensure that your app.xaml's build action is "Application Definition". right click on app.xaml, properties->Advanced->Build Action.
Otherwise, it will fail to compile properly. The build action for all the embedded images should also be "Resource" and should not copy to output directory (that just adds extra unneccessary files)

I can update this with more common troubleshooting errors as needed.

Note that this is still in development, expect errors basically everywhere. 
