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
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;

namespace WpfTestAssignment
{
    /// <summary>
    /// Interaction logic for FileInputReader.xaml
    /// </summary>
    public partial class FileInputReader : UserControl
    {
        private DispatcherTimer _autoSaveTimer;
        private int _savedPosition;
        private string _filePath;

        public FileInputReader()
        {
            InitializeComponent();

            Dispatcher.ShutdownStarted += new EventHandler(Dispatcher_ShutdownStarted);

            _autoSaveTimer = new DispatcherTimer();
            _autoSaveTimer.Tick += new EventHandler(Timer_Tick);
            _autoSaveTimer.Start();

            // Must be set after timer initialization.
            AutoSaveIntervalInSeconds = 5;
        }

        public int AutoSaveIntervalInSeconds 
        {
            get { return Convert.ToInt32(_autoSaveTimer.Interval.TotalSeconds); }
            set 
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", value, "Auto save interbval must be a positive integer.");
                _autoSaveTimer.Interval = new TimeSpan(0, 0, value); 
            }
        }

        public string FilePath 
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                InitializeInput();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Save();
        }

        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (_savedPosition >= TextInput.Text.Length)
                return;

            // Write text since the last saved position.

            var text = TextInput.Text.Substring(_savedPosition, TextInput.Text.Length - _savedPosition);

            if (Write(text))
            {
                _savedPosition = TextInput.Text.Length;
            }
        }

        private bool Write(string text)
        {
            try
            {
                File.AppendAllText(FilePath, text);

                return true;
            }
            catch (Exception ex)
            {
                // Display error in some ad hoc control.
                return false;
            }
        }

        private void InitializeInput()
        {
            TextInput.Clear();
            TextInput.IsReadOnly = false;

            _savedPosition = 0;
        }

        private void DisplayError(Exception ex)
        {
            TextInput.Text = ex.Message;
            TextInput.IsReadOnly = true;
        }

        private void TextInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (var change in e.Changes)
            {
                // Adjust saved position if changes are made before it.
                // These changes will not be saved.
                if (change.Offset < _savedPosition)
                    _savedPosition += change.AddedLength - change.RemovedLength;
            }
        }
    }
}
