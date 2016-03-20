using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ApplicationForms
{
    /// <summary>
    /// Логика взаимодействия для ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        private string _result;

        public ResultWindow()
        {
            InitializeComponent();
        }

        public ResultWindow(string result)
        {
            InitializeComponent();
            _result = result;
            ResultText.Text = _result;
        }

        public event Action OnStartWithAI;
        public event Action OnStartWithNetUser;
        public event Action OnAboutShow;
        public event Action OnExit;

        private void StartWithAI_Click(object sender, RoutedEventArgs e)
        {
            if (OnStartWithAI != null) OnStartWithAI();
            this.Close();
        }

        private void StartWithNetUser_Click(object sender, RoutedEventArgs e)
        {
            if (OnStartWithNetUser != null) OnStartWithNetUser();
            // Uncomment when this feature will be installed
            //this.Close();
        }

        private void OpenAboutWindow_Click(object sender, RoutedEventArgs e)
        {
            if (OnAboutShow != null) OnAboutShow();
        }

    }
}
