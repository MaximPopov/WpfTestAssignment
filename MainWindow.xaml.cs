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
using System.Windows.Navigation;
using System.IO;

namespace WpfTestAssignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetFilePath(GetDefaultFilePath());
        }

        private string GetDefaultFilePath()
        {
            return System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "input.txt");
        }

        private void SetFilePath(string filePath)
        {
            // Create a new file if necessary, so that FileWatcher has something to watch for.
            if (!File.Exists(filePath))
                File.AppendAllText(filePath, "");

            FilePath.Text = filePath;
            FilePath.ScrollToEnd();

            FileInputReader.FilePath = filePath;
            FileWatcher.FilePath = filePath;
            FileWatcher.Watch();
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            var currentPath = FilePath.Text;

            // Show "Save File" dialog
            var dialog = new Microsoft.Win32.SaveFileDialog();
            
            dialog.FileName = Path.GetFileName(currentPath);
            dialog.InitialDirectory = Path.GetDirectoryName(currentPath);

            var result = dialog.ShowDialog();
            if (result == true)
                SetFilePath(dialog.FileName);
        }
    }
}
