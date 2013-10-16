﻿using System.Windows;
using System.Windows.Controls;

namespace BillsManager.View.Controls
{
    public class CloseWindowButton : Button
    {
        protected override void OnClick()
        {
            base.OnClick();

            Window.GetWindow(this).Close();
        }
    }
}
