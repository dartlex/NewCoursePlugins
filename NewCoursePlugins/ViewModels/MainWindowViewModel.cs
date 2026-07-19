using CoursePluginsAPI.cs;
using NewCoursePlugins.Models;
using NewCoursePlugins.Views;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml.Serialization;

namespace NewCoursePlugins.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public List<CommandVM> Commands { get; private set; }

        public ProjectPage ProjectPage { get; }

        private ProjectPageViewModel _projectPageViewModel;

        public MainWindowViewModel()
        {
            Commands = new List<CommandVM>()
            {
                new CommandVM("Кнопка 1", new DelegateCommand(Button1)),
                new CommandVM("Кнопка 2", new DelegateCommand(Button2)),
                new CommandVM("Создать элемент", new DelegateCommand(Button3)),
                new CommandVM("Удалить элемент", new DelegateCommand(Button4)),
            };

            _projectPageViewModel = new ProjectPageViewModel();
            ProjectPage = new ProjectPage();
            ProjectPage.DataContext = _projectPageViewModel;
            LoadCommands();
        }

        private void Button1()
        {
            MessageBox.Show(nameof(Button1));
        }

        private void Button2()
        {
            MessageBox.Show(nameof(Button2));
        }

        private void Button3()
        {
            _projectPageViewModel.AddElement(new ElementVM("Новый элемент", "Просто новый элемент для теста"));
        }

        private void Button4()
        {
            if (_projectPageViewModel != null) _projectPageViewModel.RemoveElement(_projectPageViewModel.SelectedElement.Id);
        }

        private void LoadCommands()
        {
            string commandSelectorsFolderPath = "C://Users//Иван//source//repos//dartlex//NewCoursePlugins//Selectors";

            string selectorPatter = "*.myaddin";

            string[] files = Directory.GetFiles(commandSelectorsFolderPath, selectorPatter);

            foreach (string file in files)
            {
                string fileContent = File.ReadAllText(file);
                XmlSerializer serializer = new XmlSerializer(typeof(AddInSelector));

                using (StreamReader sr = new StreamReader(file))
                {
                    var selector = serializer.Deserialize(sr) as AddInSelector;

                    var assembly = Assembly.LoadFrom(selector.AssemblyPath);

                    IMyCommand command = assembly.CreateInstance(selector.ClassName) as IMyCommand;

                    DelegateCommand delegateCommand = new DelegateCommand(() => { command.Execute(_projectPageViewModel); });

                    Commands.Add(new CommandVM(selector.ButtonName, delegateCommand));
                }
            }
        }
    }
}
