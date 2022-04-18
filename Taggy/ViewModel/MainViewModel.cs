﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Taggy.Model;

namespace Taggy.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields

        private string location;
        private ObservableCollection<Tag> tags = new ObservableCollection<Tag>();
        private Tag selectedTag;
        private TagCloudViewModel tagCloud = new TagCloudViewModel();
        private ObservableCollection<FileReference> fileReferences = new ObservableCollection<FileReference>();        
        private ObservableCollection<FileReference> fileReferencesForSelectedTags = new ObservableCollection<FileReference>();
        private ObservableCollection<Resource> resources = new ObservableCollection<Resource>();

        #endregion

        #region Constructors

        public MainViewModel()
        {
            Location = "S:\\Books";
        }

        #endregion

        #region Properties

        public string Location
        {
            get { return location; }
            set
            {
                if (location == value)
                    return;
                location = value; 
                OnPropertyChanged(nameof(Location));
            }
        }

        public ObservableCollection<Tag> Tags
        {
            get { return tags; }
            set 
            {
                if (tags == value)
                    return;
                tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }

        public Tag SelectedTag
        {
            get { return selectedTag; }
            set
            {
                if (selectedTag == value)
                    return;
                selectedTag = value;
                OnPropertyChanged(nameof(SelectedTag));
            }
        }

        public TagCloudViewModel TagCloud
        {
            get { return tagCloud; }
            set
            {
                if (tagCloud == value)
                    return;
                tagCloud = value;
                OnPropertyChanged(nameof(TagCloud));
            }
        }

        public ObservableCollection<FileReference> FileReferences
        {
            get { return fileReferences; }
            set
            {
                if (fileReferences == value)
                    return;
                fileReferences = value;
                OnPropertyChanged(nameof(FileReferences));
                UpdateFileReferencesForSelectedTag();
            }
        }

        public ObservableCollection<FileReference> FileReferencesForSelectedTags
        {
            get { return fileReferencesForSelectedTags; }
            set
            {
                if (fileReferencesForSelectedTags == value)
                    return;
                fileReferencesForSelectedTags = value;
                OnPropertyChanged(nameof(FileReferencesForSelectedTags));
            }
        }

        public ObservableCollection<Resource> Resources
        {
            get { return resources; }
            set
            {
                if (resources == value)
                    return;
                resources = value;
                OnPropertyChanged(nameof(Resources));
            }
        }

        #endregion

        #region Actions

        public void Load()
        {
            var resourceConfigFilePath = GetResourceConfigFilePath();
            if (File.Exists(resourceConfigFilePath))
            {
                var resourceConfig = new ResourceConfig();
                resourceConfig.LoadFile(resourceConfigFilePath);
                Resources = new ObservableCollection<Resource>(resourceConfig.Resources);
            }
        }

        public void Save()
        {
            var resourceConfigFilePath = GetResourceConfigFilePath();
            var resourceConfig = new ResourceConfig();
            resourceConfig.Resources = Resources;
            resourceConfig.Save(resourceConfigFilePath);
        }

        public void Reindex()
        {
            var fileReferences = FileReferenceBrowser.Browse(Location);
            this.FileReferences = new ObservableCollection<FileReference>(fileReferences);

            var concatenatedTags = fileReferences.SelectMany(f => f.TagCluster.Items);
            var distinctTags = concatenatedTags.Distinct().OrderBy(t => t.Name + "#" + t.Value);
            Tags = new ObservableCollection<Tag>(distinctTags);
            foreach(var tag in tags)
            {
                var tagCloudItem = new TagCloudItemViewModel();
                tagCloudItem.Tag = tag;
                tagCloudItem.Weight = concatenatedTags.Count(t => t == tag);
                tagCloud.Items.Add(tagCloudItem);
            }
        }

        private static string GetResourceConfigFilePath()
        {
            return "Resources.xml";
        }

        #endregion

        #region Other Methods

        private void UpdateFileReferencesForSelectedTag()
        {
            FileReferencesForSelectedTags = new ObservableCollection<FileReference>(FileReferences);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
