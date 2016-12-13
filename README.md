# Framework Class Library Extension (FCL Extension)

**THIS LIBRARY WILL BECOME DEPRECATED AND IS SUCCEEDED BY SEPARATE NEW LIBRARIES**:

- [.NET Standard Library Extension](https://github.com/Whathecode/.NET-Standard-Library-Extension): Core classes which target .NET Standard.
- [.NET Core Library Extension](https://github.com/Whathecode/NET-Core-Library-Extension): Core classes which target .NET Core, supporting additional functionality not available on all .NET Standard platforms.
- The remaining functionality (e.g., Interop, PresentationFramework helpers for MVVM) will be ported to separate libraries once I find the time.

The Framework Class Library Extension project contains highly reuseable classes I find to be missing in .NET's FCL. Since some of the implementations I blog about start to use a lot of code from this library, I decided to make it public so people can just access the entire library instead of small separate code samples. It was becoming a burden posting all dependent code for my code samples.

Assemblies from the original FCL are followed as much as possible:

- Whathecode.System extends on System, System.Core and WindowsBase.
- Whathecode.PresentationFramework extends on PresentationCore and PresentationFramework.

Namespaces from the original FCL are followed as much as possible:

- Helper classes are contained within the corresponding namespace. E.g. A helper class for System.Diagnostics.Process will be located in Whathecode.System.Diagnostics.ProcessHelper.
- Whole reuseable helper APIs are located in the relevant namespace. E.g. An abstract input controller is located in Whathecode.System.Windows.Input.InputController.
- Linq extensions are located in Whathecode.System.Linq.
- Extension methods are placed in an Extension namespace in the relevant namespace. E.g. general extension are located in Whathecode.System.Extensions, while extension methods when doing reflection are located in Whathecode.System.Reflection.Extensions.
- Unit tests are placed in a Tests namespace in the relevant namespace. MSTest is used. These tests also serve as examples.
- Aspects are placed in an Aspects namespace in the relevant namespace. In order to use them PostSharp is required, but no other code is dependent on those projects.

The library also contains unproven toy classes, which I still have to experiment with to judge their usefulness. They are located within *.Experimental namespaces.

Some classes do runtime code compilation which was made possible thanks to the excellent [RunSharp library](http://runsharp.hg.sourceforge.net/hgweb/runsharp/runsharp/summary) of Stefan Simik. It really simplifies Reflection.Emit.

Copyright (c) 2011 Steven Jeuris
The library is distributed under the terms of the MIT license (http://opensource.org/licenses/mit-license). More information can be found in "LICENSE.txt"
