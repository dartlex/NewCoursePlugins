using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePluginsAPI.cs
{
    public interface IMyCommand
    {
        void Execute(ProjectPageViewModel pageViewModel);
    }
}
