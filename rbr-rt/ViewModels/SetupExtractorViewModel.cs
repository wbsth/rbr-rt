using AdonisUI.Controls;
using Microsoft.Win32;
using rbr_rt.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rbr_rt.Utils;

namespace rbr_rt.ViewModels
{
    public class SetupExtractorViewModel:ObservableObject
    {
        public SetupExtractorViewModel()
        {
            OpenFileCommand = new RelayCommand(o => OpenFileExecute());
            ExtractCommand = new RelayCommand(o => ExtractExecute(), o=>!String.IsNullOrEmpty(ReplayPath));
            _setupExtractor = new SetupExtractor();
        }

        private string fileName;
        private SetupExtractor _setupExtractor;

        private string _replayPath;
        private string _setupPath;
        public string ReplayPath 
        {
            get => _replayPath;
            set
            {
                if (_replayPath != value)
                {
                    _replayPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand OpenFileCommand { get; set; }
        public RelayCommand ExtractCommand { get; set; }


        private void OpenFileExecute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".rpl",
                Filter = "RBR replay|*.rpl",
                InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory
            };
            if(openFileDialog.ShowDialog() == true)
            {
                ReplayPath = openFileDialog.FileName;
                fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
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
                    FileName = fileName,
                    DefaultExt = ".lsp",
                    InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory
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

    }
}
