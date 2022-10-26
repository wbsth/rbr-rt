using AdonisUI.Controls;
using Microsoft.Win32;
using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using rbr_rt.Utils;

namespace rbr_rt.ViewModels
{
    public class SetupExtractorViewModel:ObservableObject
    {
        public SetupExtractorViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFileExecute);
            ExtractCommand = new RelayCommand(ExtractExecute, () => !string.IsNullOrEmpty(ReplayPath));
            _setupExtractor = new SetupExtractor();
        }

        /// <summary> Utility to provide proper setup extracting </summary>
        private readonly SetupExtractor _setupExtractor;
        
        /// <summary> Keeps the name of the loaded replay </summary>
        private string _fileName;
        private string _setupPath;
        
        private string _replayPath;
        public string ReplayPath 
        {
            get => _replayPath;
            set
            {
                if (_replayPath != value)
                {
                    _replayPath = value;
                    OnPropertyChanged();
                    ExtractCommand.NotifyCanExecuteChanged();
                }
            }
        }
        
        #region COMMANDS
        
        public RelayCommand OpenFileCommand { get; }
        public RelayCommand ExtractCommand { get; }
        
        #endregion

        #region COMMANDS EXECUTES
        private void OpenFileExecute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".rpl",
                Filter = "RBR replay|*.rpl",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };
            if(openFileDialog.ShowDialog() == true)
            {
                ReplayPath = openFileDialog.FileName;
                _fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            }
        }

        private void ExtractExecute()
        {
            var setupFound = _setupExtractor.FindSetup(ReplayPath);
            if (setupFound)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Title = "Save setup file",
                    Filter = "RBR setup|*.lsp",
                    FileName = _fileName,
                    DefaultExt = ".lsp",
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    _setupPath = saveFileDialog.FileName;
                    if (_setupExtractor.SaveSetup(_setupPath))
                    {
                        MessageBox.Show("Setup extracted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error during setup extract", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No setup found! Is this a correct RBR replay file?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        #endregion

    }
}
