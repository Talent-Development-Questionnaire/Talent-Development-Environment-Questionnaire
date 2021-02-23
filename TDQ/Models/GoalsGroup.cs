using System;
using System.Collections.Generic;
using System.Text;

namespace TDQ.Models
{
    public class GoalsGroup
    {
        public string Filepath { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string ImageFilePath { get; set; }
        public string Color { get; set; }
        public Group Group { get; set; }
    }
}
