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
using Sensors.Sensors;

namespace Sensors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string typeText = string.Empty;
        public MainWindow()
        {
            KeyDown += OnKeyDownHandler;
            Sensor sensor = new Sensor();
            InitializeComponent();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Find_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Change_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            MessageBox.Show(selectedItem.Content.ToString());
        }

        private void TypeTextBox_TextChanged(object sender, TextChangedEventArgs changedEventArgs)
        {
            //typeText = TypeTextBox.Text;
            //MainTextBlock.Text = typeText;
        }

        private void MeasuredNameTextBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {

        }

        private void IntervalNameTextBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {

        }

        private void MeasuredValueNameTextBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {

        }

        private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MainTextBlock.Text = string.Empty;
                MainTextBlock.Text += "Type:" + TypeTextBox.Text + "\n";
                MainTextBlock.Text += "Measured Name:" + MeasuredNameTextBox.Text + "\n";
                MainTextBlock.Text += "Measured Value:" + MeasuredValueTextBox.Text + "\n";
                MainTextBlock.Text += "Interval:" + IntervalNameTextBox.Text + "\n";
            }
        }
    }
}
