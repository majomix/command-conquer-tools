using System;
using System.ComponentModel;
using Skeleton.ViewModel.Commands;

namespace Skeleton.ViewModel
{
    internal class CreateByParameterCommand : AbstractWorkerCommand
    {
        private OneTimeRunViewModel _oneTimeRunViewModel;

        public override void Execute(object parameter)
        {
            _oneTimeRunViewModel = (OneTimeRunViewModel)parameter;
            Worker.RunWorkerAsync();
        }

        protected override void DoWork(object sender, DoWorkEventArgs e)
        {
            _oneTimeRunViewModel.Create();
            _oneTimeRunViewModel.OnRequestClose(new EventArgs());
        }
    }
}
