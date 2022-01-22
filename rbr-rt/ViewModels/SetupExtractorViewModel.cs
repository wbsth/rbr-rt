using AdonisUI.Controls;
using Microsoft.Win32;
using rbr_rt.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rbr_rt.ViewModels
{
    public class SetupExtractorViewModel:ObservableObject
    {
        public SetupExtractorViewModel()
        {
            OpenFileCommand = new RelayCommand(o => OpenFileExecute());
            ExtractCommand = new RelayCommand(o => ExtractExecute(), o=>!String.IsNullOrEmpty(ReplayPath));
        }

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
            }
        }

        private void ExtractExecute()
        {
            MessageBox.Show("Hello world!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            // try to extract setup
            // if success, display save file dialog
            // else, display false message
        }

    }
}
