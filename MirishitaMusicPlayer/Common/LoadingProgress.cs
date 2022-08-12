using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Common
{
    internal class LoadingProgress : IProgress<int>
    {
        private readonly int _totalValue;
        private readonly bool _relative;
        private readonly ProgressBar _progressBar;

        private int accumulatedValue;

        public LoadingProgress(int totalValue, bool relative, ProgressBar progressBar)
        {
            _totalValue = totalValue;
            _relative = relative;
            _progressBar = progressBar;
        }

        public void Report(int value)
        {
            if (_progressBar.IsHandleCreated)
                _progressBar.Invoke(() => _progressBar.Value = (int)((float)accumulatedValue / _totalValue * 100.0f));

            if (!_relative && accumulatedValue < _totalValue)
                accumulatedValue += value;
            else
                accumulatedValue = value;
        }
    }
}
