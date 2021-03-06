﻿using Billy.Common.Controls;
using Caliburn.Micro;
using System.Windows;

namespace Billy.UI.Wpf.Common.Services
{
    public class MaterialDesignWindowManager : WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            if (view is Window window)
            {
                if (isDialog)
                {
                    var owner = this.InferOwnerOf(window);

                    if (owner != null)
                    {
                        window.Owner = owner;
                    }
                }
            }
            else
            {
                window = new MaterialDesignWindow
                {
                    Content = view,
                    SizeToContent = SizeToContent.WidthAndHeight
                };

                window.SetValue(View.IsGeneratedProperty, true);

                var owner = this.InferOwnerOf(window);
                if (owner != null)
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Owner = owner;
                }
                else
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }

            return window;
        }
    }
}