using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lessons.Commands
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class Lesson6 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            FilteredElementCollector levelCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType();
            Level firstLevel = levelCollector.ToElements().Cast<Level>().FirstOrDefault(l => l.Name == "Level 1");

            if (firstLevel == null)
            {
                TaskDialog.Show("Ошибка", "В проекте отсутствует уровень с таким именем");
                return Result.Failed;
            }

            FilteredElementCollector columnCollector = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralColumns);
            FamilySymbol columnType = columnCollector.FirstElement() as FamilySymbol;

            if (columnType == null)
            {
                TaskDialog.Show("Ошибка", "В проект не загружено ни одно семейство Архитектурных колонн (категория OST_Columns). Загрузите семейство через вкладку Вставить.");
                return Result.Failed;
            }

            using (Transaction t = new Transaction(doc, "Create column"))
            {
                t.Start();
                if (!columnType.IsActive) columnType.Activate();
                XYZ origin = new XYZ(0, 0, 0);
                doc.Create.NewFamilyInstance(origin, columnType, firstLevel, StructuralType.NonStructural);
                t.Commit();
            }

            return Result.Succeeded;
        }
    }
}
