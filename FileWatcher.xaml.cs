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
    /// Interaction logic for FileWatcher.xaml
    /// </summary>
    public partial class FileWatcher : UserControl
    {
        private string _filePath;
        private FileSystemWatcher _watcher;

        public FileWatcher()
        {
            InitializeComponent();

            _watcher = new FileSystemWatcher();
            _watcher.Changed += new FileSystemEventHandler(OnFileChanged);

            MaxDisplayedContentLength = 5000;
        }

        /// <summary>
        /// Maximum number of characters displayed in file content text box.
        /// </summary>
        public int MaxDisplayedContentLength { get; set; }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            ReadAndDisplayContents();
        }

        private void ReadAndDisplayContents()
        {
            var contents = File.ReadAllText(_filePath);

            DisplayContents(contents);
        }

        private delegate void DisplayContentsDelegate(string contents);

        private void DisplayContents(string contents)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new DisplayContentsDelegate(DisplayContents), contents);
                return;
            }

            if (contents.Length > MaxDisplayedContentLength)
                contents = contents.Substring(contents.Length - MaxDisplayedContentLength, MaxDisplayedContentLength);

            FileContents.Text = contents;
            FileContents.ScrollToEnd();
        }

        public string FilePath 
        {
            get { return _filePath; }
            set
            {
                _filePath = value;

                ReadAndDisplayContents();

                _watcher.Path = Path.GetDirectoryName(_filePath);
                _watcher.Filter = Path.GetFileName(_filePath);
            }
        }

        public void Watch()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
        }
    }
}
