using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileCoreServices;
using UIKit;

namespace P42.Uno.SandboxedStorage
{
    static class FilePickerExtensions
    {
        internal static UIViewController GetActiveViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;

            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            return viewController;
        }
    }
}
