using System;
using System.Collections.Generic;
using System.Text;

namespace TDQ.Models
{
    public class Group
    {
        public string ImageFilePath { get; set; }
        public string Filename { get; set; }
        public string Name { get; set; }
        public string[] EmailList { get; set; }
        public int GroupNo { get; set; }
    }
}
