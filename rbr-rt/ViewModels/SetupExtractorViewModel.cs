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
    }
}
