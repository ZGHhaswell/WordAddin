using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordMonitorApp.DependProperties
{
    public static class DialogResultHelper
    {
        public static readonly DependencyProperty DialogResultProperty =
        DependencyProperty.RegisterAttached(
            "DialogResult",
            typeof(bool?),
            typeof(DialogResultHelper),
            new PropertyMetadata(DialogResultChanged));

        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
                window.DialogResult = e.NewValue as bool?;
        }

        public static void SetDialogResult(DependencyObject target, bool? value)
        {
          target.SetValue(DialogResultProperty, value);
        }
    }
}
