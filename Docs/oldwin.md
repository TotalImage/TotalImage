Windows 7 support ended on **January 14th, 2020**. Windows 8.1 support ended on **January 10th, 2023**.

Although these older versions of Windows are not officially supported by .NET, it can still be installed on them but .NET-based applications may or may not work correctly. Microsoft will not provide any fixes for .NET issues specific to these unsupported versions of Windows.

With the above in mind, we provide **minimal** support for users of TotalImage on Windows 7 or Windows 8.1. We test it to see if it still runs, but not additional functionality tests are performed on these versions, and we may or may not be able to fix problems
spefic to these versions of Windows. If one day Microsoft decides to start actively preventing installation of newer .NET releases on Windows 7 or 8.1, we will end our support for them as well, as we want to take advantage of new features and other improvements in newer releases of .NET.

If you want to use TotalImage on Windows 7 or 8.1, we recommend you:
* install all available Windows updates to ensure the best possible security and functionality
* install the latest Microsoft Visual C++ Redistributables, otherwise you will get an error when trying to run TotalImage
* avoid using dark mode in TotalImage, because it does not work properly on these versions of Windows and we can't fix it
