﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taggy.Model
{
    /// <summary>
    /// File reference that includes various descriptive attributes.
    /// </summary>
    public class FileReference
    {
        private string filePath = "";
        private TagCluster tagCluster = new TagCluster();

        public FileReference(string filePath, TagCluster tags)
        {
            FilePath = filePath;
            TagCluster = tags;
        }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public string FilePathUntagged
        {
            get
            {
                var atIndex = filePath.IndexOf('@');
                if (atIndex >= 0)
                {
                    var filePathUntagged = filePath.Substring(0, atIndex).TrimEnd();
                    return filePathUntagged;
                }

                return filePath;
            }
        }

        public TagCluster TagCluster 
        {
            get { return tagCluster; }
            set { tagCluster = value; }
        }
    }
}
