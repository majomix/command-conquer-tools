using Skeleton.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Skeleton.ViewModel
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        private int myCurrentProgress = 100;
        private string myLoadedFilePath;
        private string myCurrentFile;
        private bool myHasError;

        public SkeletonEditor Model { get; protected set; }
        public string LoadedFilePath
        {
            get { return myLoadedFilePath; }
            set
            {
                if (myLoadedFilePath != value)
                {
                    myLoadedFilePath = value;
                    OnPropertyChanged("LoadedFilePath");
                }
            }
        }
        public string CurrentFile
        {
            get { return myCurrentFile; }
            protected set
            {
                if (myCurrentFile != value)
                {
                    myCurrentFile = value;
                    OnPropertyChanged("CurrentFile");
                }
            }
        }
        public int CurrentProgress
        {
            get { return myCurrentProgress; }
            protected set
            {
                if (myCurrentProgress != value)
                {
                    myCurrentProgress = value;
                    OnPropertyChanged("CurrentProgress");
                }
            }
        }
        public bool HasError
        {
            get { return myHasError; }
            set
            {
                if (myHasError != value)
                {
                    myHasError = value;
                    OnPropertyChanged("HasError");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestClose;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void OnRequestClose(EventArgs e)
        {
            RequestClose(this, e);
        }

        public void LoadStructure()
        {
            using (var file = File.Open(LoadedFilePath, FileMode.Open))
            using (var reader = new SkeletonBinaryReader(file))
            {
                Model.LoadFileStructure(reader);
                OnPropertyChanged("Model");
            }
        }

        public void ExtractWithPredicate(string directory, Func<FileTableEntry, bool> function)
        {
            using (var file = File.Open(LoadedFilePath, FileMode.Open))
            using (var reader = new SkeletonBinaryReader(file))
            {
                var filtered = Model.MegaFile.FileTableEntries.Where(function).ToList();
                for (var index = 0; index < filtered.Count; index++)
                {
                    var entry = filtered[index];
                    Model.ExtractFile(entry, directory, reader);
                }
            }
        }

        public void CreateNewMegaFile(string directory, string targetPath)
        {
            var entries = new List<FileTableEntry>();
            var names = new List<string>();

            foreach (string file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                var fileNameOnly = file.Split(new[] { directory + @"\" }, StringSplitOptions.RemoveEmptyEntries)[0];

                if (!string.IsNullOrWhiteSpace(fileNameOnly))
                {
                    names.Add(fileNameOnly);

                    var currentEntry = new FileTableEntry
                    {
                        FileNameTableIndex = (short)(names.Count - 1),
                        Import = file
                    };

                    entries.Add(currentEntry);
                }
            }

            using (var outputFileStream = File.Open(targetPath, FileMode.Create))
            {
                using (var writer = new SkeletonBinaryWriter(outputFileStream))
                {
                    Model.WriteStubMegaFile(writer, names, entries);

                    foreach (var entry in Model.MegaFile.FileTableEntries)
                    {
                        Model.WriteDataEntry(writer, entry, names[entry.FileNameTableIndex]);
                    }

                    Model.FinalizeMegaFile(writer);
                }
            }
        }

        public void ResolveNewFiles(string directory)
        {
            foreach (string file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                string[] tokens = file.Split(new string[] { directory + @"\" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string token in tokens)
                {
                    if (!String.IsNullOrWhiteSpace(token))
                    {
                    }
                }
            }
        }

        public void SaveStructure(string path)
        {
            using (SkeletonBinaryReader reader = new SkeletonBinaryReader(File.Open(LoadedFilePath, FileMode.Open)))
            {
                using (SkeletonBinaryWriter writer = new SkeletonBinaryWriter(File.Open(path, FileMode.Create)))
                {
                    //foreach (Entry entry in entries)
                    //{
                    Model.SaveDataEntry(reader, writer);
                    //CurrentProgress = (int)(currentSize * 100.0 / totalSize);
                    //CurrentFile = entry.Name;
                    //}

                    Model.SaveIndex(writer);
                }
            }

            OnPropertyChanged("Model");
        }

        public string GenerateRandomName()
        {
            Random generator = new Random();
            return Path.ChangeExtension(LoadedFilePath, @".tmp_" + generator.Next().ToString());
        }
    }
}