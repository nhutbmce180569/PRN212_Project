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
using System.Windows.Shapes;
using FinalProject.Models;

namespace FinalProject.Views.Admin.Employee
{
    /// <summary>
    /// Interaction logic for UpdateEmployee.xaml
    /// </summary>
    public partial class UpdateEmployee : Window
    {
        public UpdateEmployee(FinalProject.Models.Employee employee)
        {
            InitializeComponent();
            txtGender.Items.Add("Male");
            txtGender.Items.Add("Female");
            txtGender.Items.Add("Other");
            txtFullName.Text = employee.FullName;
            txtBirthday.SelectedDate = employee.Birthday;
            txtGender.SelectedIndex = txtGender.Items.IndexOf(employee.Gender.Trim());
            txtEmail.Text = employee.Email;
            txtPassword.Text = employee.Password;
            txtPhoneNumber.Text = employee.PhoneNumber;
            txtCreateDate.SelectedDate = employee.CreatedDate;
            txtStatus.Items.Add("Active");
            txtStatus.Items.Add("Inactive");
            txtStatus.SelectedIndex = txtStatus.Items.IndexOf(employee.Status.Trim());
            using (var context = new FstoreContext())
            {
                var roleList = context.Roles.Where(r => r.RoleId != 1).ToList();
                txtRoleId.ItemsSource = roleList;
                txtRoleId.SelectedValuePath = "RoleId";
                txtRoleId.DisplayMemberPath = "Name";
                txtRoleId.SelectedValue = employee.RoleId;
            }          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Windows[0].Opacity = 1;
            Application.Current.Windows[0].IsHitTestVisible = true;

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
