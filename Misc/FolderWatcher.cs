using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ImageViewer.Helpers;
using ImageViewer.Settings;

namespace ImageViewer.Misc
{
    public class FolderWatcher : IDisposable
    {
        public int CurrentFileIndex = -1;
        public string CurrentDirectory
        {
            get
            {
                return directory;
            }
        }

        private List<string> files;
        private List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();
        private System.Windows.Forms.Timer resortTimer = new System.Windows.Forms.Timer() { Interval = InternalSettings.Folder_Watcher_Resort_Timer_Limit };
        private string directory;

        public FolderWatcher(string path)
        {
            resortTimer.Tick += ResortTimer_Tick;
            directory = path;
            
            if (!Directory.Exists(path))
            {
                CreateWatchers("C:\\", false);
                files = new List<string>();
                return;
            }

            CreateWatchers(path, true);
            SetFiles(path);
        }

        private void ResortTimer_Tick(object sender, EventArgs e)
        {
            files = files.OrderByNatural(f => f).ToList();
            resortTimer.Stop();
        }

        private void FolderChanged(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                // no need to remove from list we will check if the file exists when fetching
                case WatcherChangeTypes.Deleted:
                    files.Remove(e.Name);
                    break;

                // using a timer because i don't want to waste cpu resorting the files if lots of files 
                // are being created / copied
                case WatcherChangeTypes.Created:
                case WatcherChangeTypes.Renamed:
                    files.Add(e.Name);

                    if (!resortTimer.Enabled)
                    {
                        resortTimer.Start();
                    }
                    break;
            }
        }

        private void SetFiles(string path)
        {
            files = Directory.EnumerateFiles(path).OrderByNatural(e => e).
                Where(e => InternalSettings.Readable_Image_Formats.Contains(Helper.GetFilenameExtension(e))).ToList();
        }

        public void RefreshAllFiles()
        {
            files.Clear();
            SetFiles(directory);
        }

        private void CreateWatchers(string path, bool enabled = true)
        {
            foreach (string f in InternalSettings.Readable_Image_Formats_Dialog_Options)
            {
                FileSystemWatcher w = new FileSystemWatcher();
                w.Path = path;
                w.IncludeSubdirectories = false;
                w.Filter = f;
                w.Created += FolderChanged;
                w.Renamed += FolderChanged;
                w.Deleted += FolderChanged;
                w.EnableRaisingEvents = enabled;
                watchers.Add(w);
            }
        }

        private void UpdateWatchers(string path, bool enable = true)
        {
            foreach (FileSystemWatcher fsw in watchers)
            {
                fsw.Path = path;
                fsw.EnableRaisingEvents = enable;
            }
        }

        public void UpdateDirectory(string path)
        {
            directory = path;

            if (!Directory.Exists(path))
            {
                UpdateWatchers(path, false);
                files.Clear();
                return;
            }

            UpdateWatchers(path, true);
            RefreshAllFiles();
        }

        public void UpdateIndex(string path)
        {
            CurrentFileIndex = files.IndexOf(path);
        }

        public string GetNextFile()
        {
            if (files.Count < 1 || files.Count <= CurrentFileIndex + 1)
                return string.Empty;

            CurrentFileIndex++;
            return files[CurrentFileIndex];
        }

        public string GetPreviousFile()
        {
            if (files.Count < 1 || CurrentFileIndex - 1 < 0)
                return string.Empty;

            CurrentFileIndex--;
            return files[CurrentFileIndex];
        }

        public bool GetNextValidFile(out string path)
        {
            if (files.Count < 1) 
            {
                path = string.Empty;
                return false; 
            }

            while (true)
            {
                if(files.Count <= CurrentFileIndex + 1)
                {
                    path = string.Empty;
                    return false;
                }

                CurrentFileIndex++;

                // the path doesn't exist so remove it from the list
                if (!File.Exists(files[CurrentFileIndex]))
                {
                    files.RemoveAt(CurrentFileIndex);
                    CurrentFileIndex--; // reset the counter because we need to check the same index
                    continue;
                }

                path = files[CurrentFileIndex];
                return true;
            }
        }

        public bool GetPreviousValidFile(out string path)
        {
            if (files.Count < 1)
            {
                path = string.Empty;
                return false;
            }

            while (true)
            {
                if (CurrentFileIndex - 1 < 0)
                {
                    path = string.Empty;
                    return false;
                }

                CurrentFileIndex--;

                // the path doesn't exist so remove it from the list
                if (!File.Exists(files[CurrentFileIndex]))
                {
                    files.RemoveAt(CurrentFileIndex);
                    CurrentFileIndex++; // reset the counter because we need to check the same index
                    continue;
                }

                path = files[CurrentFileIndex];
                return true;
            }
        }

        public void Dispose()
        {
            foreach(FileSystemWatcher fsw in watchers)
            {
                fsw.Created -= FolderChanged;
                fsw.Renamed -= FolderChanged;
                fsw.Deleted -= FolderChanged;
                fsw.Dispose();
            }

            this.watchers.Clear();
            this.files.Clear();
        }
    }
}
