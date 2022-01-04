namespace Skeleton.ViewModel.Commands
{
    internal class ExtractByParameterCommand : AbstractParameterCommand
    {
        protected override void DoSpecificWork()
        {
            myOneTimeRunViewModel.Extract();
        }
    }
}