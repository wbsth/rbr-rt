using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdonisUI.Controls;
using Microsoft.Win32;
using rbr_rt.Core;
using rbr_rt.Utils;

namespace rbr_rt.ViewModels
{
    public class SetupHiderViewModel:ObservableObject
    {
        public SetupHiderViewModel()
        {
            OpenReplayFileCommand = new RelayCommand(o => OpenReplayFileExecute());
            OpenSetupFileCommand = new RelayCommand(o => OpenSetupFileExecute());
            ReplaceSetupCommand = new RelayCommand(o => ReplaceSetupExecute(), o => ReplaceButtonIsActive);
            _setupHiderHelper = new SetupHiderHelper();
        }

        private readonly SetupHiderHelper _setupHiderHelper;
        private bool ReplaceButtonIsActive => !string.IsNullOrEmpty(ReplayPath) && !string.IsNullOrEmpty(SetupPath);

        private string _replayName;
        private string _replayPath;
        private string _setupPath;
        private string _outputPath;

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

        public string SetupPath
        {
            get => _setupPath;
            set
            {
                if (_setupPath != value)
                {
                    _setupPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string OutputPath
        {
            get => _outputPath;
            set
            {
                if (_outputPath != value)
                {
                    _outputPath = value;
                    OnPropertyChanged();
                }
            }
        }


        public RelayCommand OpenReplayFileCommand { get; }
        public RelayCommand OpenSetupFileCommand { get; }
        public RelayCommand ReplaceSetupCommand { get; }

        private void OpenReplayFileExecute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".rpl",
                Filter = "RBR replay|*.rpl",
                InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory
            };
            if (openFileDialog.ShowDialog() == true)
            {
                ReplayPath = openFileDialog.FileName;
                _replayName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            }
        }

        private void OpenSetupFileExecute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".lsp",
                Filter = "RBR setup|*.lsp",
                InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory
            };
            if (openFileDialog.ShowDialog() == true)
            {
                SetupPath = openFileDialog.FileName;
            }
        }

        private void ReplaceSetupExecute()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = "Save setup file",
                Filter = "RBR replay|*.rpl",
                FileName = _replayName + "_copy",
                DefaultExt = ".rpl",
                InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                _outputPath = saveFileDialog.FileName;
                if (_setupHiderHelper.HideSetup(ReplayPath, SetupPath, OutputPath))
                {
                    MessageBox.Show("Setup replaced", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Error during setup replacement", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
