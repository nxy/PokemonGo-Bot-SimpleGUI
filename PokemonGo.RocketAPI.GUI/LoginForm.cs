using PokemonGo.RocketAPI.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokemonGo.RocketAPI.GUI
{
    public partial class LoginForm : Form
    {
        public AuthType auth;
        public bool loginSelected = false;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            boxUsername.Text = "";
            comboLoginMethod.SelectedIndex = 0;
            this.ActiveControl = boxUsername;

            setLoginFormInformation();
        }

        private void setLoginFormInformation()
        {
            if (UserSettings.Default.RememberMe)
            {
                cbRememberMe.Checked = true;

                if (isPtcPasswordSet())
                {
                    comboLoginMethod.SelectedIndex = 0;
                    setPtcUsernameAndPassword();
                }
                else if (isGooglecPasswordSet())
                {
                    comboLoginMethod.SelectedIndex = 1;
                    setGoogleMailAndPassword();
                }
            }
        }

        private bool isPtcPasswordSet()
        {
            return !(string.IsNullOrWhiteSpace(UserSettings.Default.PtcPassword));
        }

        private bool isGooglecPasswordSet()
        {
            return !(string.IsNullOrWhiteSpace(UserSettings.Default.GooglePassword));
        }

        private void setPtcUsernameAndPassword()
        {
            lbUsername.Text = "Username";
            boxUsername.Text = UserSettings.Default.PtcUsername;
            boxPassword.Text = UserSettings.Default.PtcPassword;
            this.ActiveControl = btnLogin;
        }

        private void setGoogleMailAndPassword()
        {
            lbUsername.Text = "Email";
            boxUsername.Text = UserSettings.Default.GoogleMail;
            boxPassword.Text = UserSettings.Default.GooglePassword;
            this.ActiveControl = btnLogin;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(boxUsername.Text))
            {
                if (!string.IsNullOrWhiteSpace(boxPassword.Text))
                {
                    if(comboLoginMethod.SelectedIndex == 0)
                    {
                        UserSettings.Default.PtcUsername = boxUsername.Text;
                        UserSettings.Default.PtcPassword = boxPassword.Text;
                    }
                    else
                    {
                        UserSettings.Default.GoogleMail = boxUsername.Text;
                        UserSettings.Default.GooglePassword = boxPassword.Text;
                    }

                    if (cbRememberMe.Checked)
                    {
                        UserSettings.Default.RememberMe = true;
                        UserSettings.Default.Save();
                    }
                    else
                    {
                        UserSettings.Default.PtcUsername = "";
                        UserSettings.Default.PtcPassword = "";
                        UserSettings.Default.GoogleMail = "";
                        UserSettings.Default.GooglePassword = "";
                        UserSettings.Default.RememberMe = false;
                        UserSettings.Default.Save();
                    }
                    
                    //Properties.Settings.Default.
                    loginSelected = true;
                    auth = (AuthType) Enum.Parse(typeof(AuthType), comboLoginMethod.SelectedItem.ToString());
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Password textbox cannot be empty", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Username textbox cannot be empty", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComboLoginMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (UserSettings.Default.RememberMe)
            {
                cbRememberMe.Checked = true;
            }

            if (comboBox.SelectedIndex == 0)
            {
                if (UserSettings.Default.RememberMe && isPtcPasswordSet())
                    setPtcUsernameAndPassword();
                else
                {
                    lbUsername.Text = "Username";
                    boxUsername.Text = "";
                    boxPassword.Text = "";
                    this.ActiveControl = boxUsername;
                }
            }
            else
            {
                if (UserSettings.Default.RememberMe && isGooglecPasswordSet())
                    setGoogleMailAndPassword();
                else
                {
                    lbUsername.Text = "Email";
                    boxUsername.Text = "";
                    boxPassword.Text = "";
                    this.ActiveControl = boxUsername;
                }
            }
        }

        private void boxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin_Click(null, null);
        }
    }
}
