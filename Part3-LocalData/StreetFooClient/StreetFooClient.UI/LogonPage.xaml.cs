﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace StreetFooClient.UI
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LogonPage : StreetFooClient.UI.Common.LayoutAwarePage
    {
        public LogonPage()
        {
            this.InitializeComponent();

            // initialize the model...
            this.Username = string.Empty;
            this.Password = string.Empty;
        }

        public string Username
        {
            get
            {
                return this.GetDataModelValue<string>();
            }
            set
            {
                this.SetDataModelValue(value);
            }
        }

        public string Password
        {
            get
            {
                return this.GetDataModelValue<string>();
            }
            set
            {
                this.SetDataModelValue(value);
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void HandleLogonClick(object sender, RoutedEventArgs e)
        {
            // in a proper implementation, it's better to setup an MVVM and have this handled
            // in the view-model...

            // get a proxy - a more sophisticated approach is to use an IoC container here
            // like TinyIoC...
            ILogonServiceProxy proxy = new LogonServiceProxy();
            this.EnterBusy();
            proxy.Logon(this.Username, this.Password, (result) =>
            {
                // show...
                if (result.IsOk)
                {
                    // tell the service that we're logged on...
                    AppRuntime.Logon(this.Username, result.LogonToken);

                    // go to the "reports" page... but careful, as we may be on a different thread...
                    this.SafeNavigate(typeof(ReportsPage));
                }
                else
                    this.ShowAlert(result.Error);

                // stop...
                this.ExitBusy();

            });
        }

        private void HandleRegisterClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage));
        }
    }
}
