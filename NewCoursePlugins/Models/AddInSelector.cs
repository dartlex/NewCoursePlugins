using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NewCoursePlugins.Models
{
    public class AddInSelector
    {
        public string AssemblyPath { get; set; }

        public string ClassName{ get; set; }

        public string ButtonName { get; set; }
    }
}
