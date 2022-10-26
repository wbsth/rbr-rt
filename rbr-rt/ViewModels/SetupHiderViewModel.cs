using System.IO;
using AdonisUI.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using rbr_rt.Utils;

namespace rbr_rt.ViewModels
{
    public class SetupHiderViewModel:ObservableObject
    {
        public SetupHiderViewModel()
        {
            OpenReplayFileCommand = new RelayCommand(OpenReplayFileExecute);
            OpenSetupFileCommand = new RelayCommand(OpenSetupFileExecute);
            ReplaceSetupCommand = new RelayCommand(ReplaceSetupExecute, () => ReplaceButtonIsActive);
            _setupHiderHelper = new SetupHiderHelper();
        }
        
        private readonly SetupHiderHelper _setupHiderHelper;
        private bool ReplaceButtonIsActive => !string.IsNullOrEmpty(ReplayPath) && !string.IsNullOrEmpty(SetupPath);

        private string _replayName;
        private string _replayPath;
        private string _setupPath;
        private string _outputPath;

        /// <summary> Input path of the replay to be processed </summary>
        public string ReplayPath
        {
            get => _replayPath;
            set
            {
                if (_replayPath != value)
                {
                    _replayPath = value;
                    OnPropertyChanged();
                    ReplaceSetupCommand.NotifyCanExecuteChanged();
                }
            }
        }

        /// <summary> Input path of the car setup to be 'injected' into replay </summary>
        public string SetupPath
        {
            get => _setupPath;
            set
            {
                if (_setupPath != value)
                {
                    _setupPath = value;
                    OnPropertyChanged();
                    ReplaceSetupCommand.NotifyCanExecuteChanged();
                }
            }
        }

        /// <summary> Output path, where prepared replay file should be saved</summary>
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


        # region COMMANDS
        
        public RelayCommand OpenReplayFileCommand { get; }
        public RelayCommand OpenSetupFileCommand { get; }
        public RelayCommand ReplaceSetupCommand { get; }
        
        #endregion

        # region COMMANDS EXECUTES
        private void OpenReplayFileExecute()
        {
            // open replay file (only .rpl should be accepted)
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".rpl",
                Filter = "RBR replay|*.rpl",
                InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory
            };
            
            // if user entered file, save its path, and its name (without extension)
            if (openFileDialog.ShowDialog() == true)
            {
                ReplayPath = openFileDialog.FileName;
                _replayName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            }
        }

        private void OpenSetupFileExecute()
        {            
            // open setup file (only .lsp should be accepted)
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".lsp",
                Filter = "RBR setup|*.lsp",
                InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory
            };
            
            // if used entered file, save its path
            if (openFileDialog.ShowDialog() == true)
            {
                SetupPath = openFileDialog.FileName;
            }
        }

        
        private void ReplaceSetupExecute()
        {
            // choose output file location
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = "Save setup file",
                Filter = "RBR replay|*.rpl",
                FileName = _replayName + "_copy",
                DefaultExt = ".rpl",
                InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory
            };
            
            // prepare output file
            if (saveFileDialog.ShowDialog() == true)
            {
                OutputPath = saveFileDialog.FileName;
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
        
        #endregion
    }
}
