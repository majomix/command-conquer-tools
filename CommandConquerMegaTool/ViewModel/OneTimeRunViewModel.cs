using Skeleton.Model;
using Skeleton.ViewModel.Commands;
using NDesk.Options;
using System;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Skeleton.ViewModel
{
    public enum EditorAction
    {
        None,
        Create,
        Export,
        Import
    }

    internal class OneTimeRunViewModel : BaseViewModel
    {
        private string _targetDirectory;
        public ICommand ExtractByParameterCommand { get; }
        public ICommand ImportByParameterCommand { get; }
        public ICommand CreateByParameterCommand { get; }

        public EditorAction Action { get; set; }

        public OneTimeRunViewModel()
        {
            ParseCommandLine();
            Model = new SkeletonEditor();

            ImportByParameterCommand = new ImportByParameterCommand();
            ExtractByParameterCommand = new ExtractByParameterCommand();
            CreateByParameterCommand = new CreateByParameterCommand();
        }

        public void ParseCommandLine()
        {
            OptionSet options = new OptionSet()
                .Add("export", value => Action = EditorAction.Export)
                .Add("import", value => Action = EditorAction.Import)
                .Add("create", value => Action = EditorAction.Create)
                .Add("meg=", value => LoadedFilePath = CreateFullPath(value, true))
                .Add("target=", value => LoadedFilePath = CreateFullPath(value, false))
                .Add("dir=", value => _targetDirectory = CreateFullPath(value, false));

            options.Parse(Environment.GetCommandLineArgs());
        }

        public void Extract()
        {
            if (_targetDirectory != null && LoadedFilePath != null)
            {
                LoadStructure();
                ExtractWithPredicate(_targetDirectory, _ => true);
            }
        }

        public void Import()
        {
        }

        public void Create()
        {
            if (LoadedFilePath != null && Directory.Exists(_targetDirectory))
            {
                CreateNewMegaFile(_targetDirectory, LoadedFilePath);
            }
        }

        private string CreateFullPath(string path, bool checkForFileExistence)
        {
            if (string.IsNullOrEmpty(path)) return null;

            if (path.Contains(':') && CheckForExistence(path, checkForFileExistence))
            {
                return path;
            }

            string resultPath = Directory.GetCurrentDirectory() + @"\" + path.Replace('/', '\\');
            return CheckForExistence(resultPath, checkForFileExistence) ? resultPath : null;
        }

        private bool CheckForExistence(string path, bool checkForFile)
        {
            if (checkForFile == true)
            {
                return File.Exists(path);
            }
            return true;
        }
    }
}
