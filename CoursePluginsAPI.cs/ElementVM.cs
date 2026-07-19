using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCoursePlugins.Models
{
    public class ElementVM
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public ElementVM(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}
