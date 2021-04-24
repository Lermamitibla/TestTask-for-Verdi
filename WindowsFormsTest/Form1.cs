using System;
using System.Globalization;
using System.Windows.Forms;

namespace WindowsFormsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

            var shedule = new WorkingShedule(startTimeTextBox.Text, endTimeTextBox.Text,
                lunchTimeStartTextBox.Text, lunchTimeEndTextBox.Text, cellDurationTextBox.Text);


            shedule.SaveToJsonFile(SaveResultPathTextBox.Text);
            MessageBox.Show(shedule.Status);
        }

        private void SelectFolderButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            SaveResultPathTextBox.Text = folderBrowserDialog1.SelectedPath;
        }

    }
}
