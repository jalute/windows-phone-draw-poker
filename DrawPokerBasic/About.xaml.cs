using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Reflection;
using Microsoft.Phone.Tasks;

namespace DrawPokerBasic
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();

            // Version v = Assembly.GetExecutingAssembly().GetName().Version;  <-- doesn't work
            //string versionString = v.Major + "." + v.Minor + "." + v.Build + "." + v.Revision;

            string[] fullNameArray = Assembly.GetExecutingAssembly().FullName.Split(',');
            string versionString = fullNameArray[1];

            txtVersion.Text = versionString;
        }

        private void EMailLink_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "Video Poker Basic feedback";
            emailComposeTask.Body = "";
            emailComposeTask.To = "feedback@jalute.com";

            emailComposeTask.Show();
        }
    }
}