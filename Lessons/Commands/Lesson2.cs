using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lessons.Commands
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    internal class Lesson2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            FilteredElementCollector collectorWalls = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType();
            IList<Element> walls = collectorWalls.ToElements();

            FilteredElementCollector collectorWallType = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType();
            IList<Element> wallsTypes = collectorWallType.ToElements();

            FilteredElementCollector collectorRoofs = new FilteredElementCollector(doc).OfClass(typeof(RoofBase)).WhereElementIsNotElementType();
            var roofs = collectorRoofs.ToElements().Cast<RoofBase>();
            RoofBase firstRoof = roofs.FirstOrDefault(x => x.LevelId.IntegerValue != -1);

            Level level = doc.GetElement(firstRoof.LevelId) as Level;
            MessageBox.Show(level.Name);


            RoofType roofType = firstRoof.RoofType;

            MessageBox.Show(roofType.Name);
            //MessageBox.Show($"Всего стен: {walls.Count}\nВсего типов стен: {wallsTypes.Count}");
            //MessageBox.Show($"Всего крыш: {roofs.Count}");
            return Result.Succeeded;
        }
    }
}
