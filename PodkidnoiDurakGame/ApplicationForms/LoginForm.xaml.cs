using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApplicationForms
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public event Action<string> OnStartWithAI;
        public event Action<string> OnStartWithNetUser;
        public event Action OnAboutShow;
        public event Action OnExit;

        private void StartWithAI_Click(object sender, RoutedEventArgs e)
        {
            string nickname = Nickname.Text;
            if (nickname == "")
            {
                MessageBox.Show("Nickname can't be empty");
                return;
            }

            if (OnStartWithAI != null) OnStartWithAI(nickname);
            this.Close();
        }

        private void StartWithNetUser_Click(object sender, RoutedEventArgs e)
        {
            string nickname = Nickname.Text;
            if (nickname == "")
            {
                MessageBox.Show("Nickname can't be empty");
                return;
            }

            if (OnStartWithNetUser != null) OnStartWithNetUser(nickname);
            // Uncomment when this feature will be installed
            //this.Close();
        }

        private void OpenAboutWindow_Click(object sender, RoutedEventArgs e)
        {
            if (OnAboutShow != null) OnAboutShow();
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            if (OnExit != null) OnExit();
            this.Close();
        }
        
    }
}
