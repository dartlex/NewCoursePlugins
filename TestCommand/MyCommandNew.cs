using CoursePluginsAPI.cs;
using NewCoursePlugins.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestCommand
{
    public class MyCommandNew : IMyCommand

    {
        public void Execute(ProjectPageViewModel pageViewModel)
        {
            pageViewModel.Elements.Clear();
            MessageBox.Show("Все элементы удалены");

        }
    }
}
