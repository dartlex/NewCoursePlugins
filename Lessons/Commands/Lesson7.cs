using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
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
    public class Lesson7 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            FilteredElementCollector levelCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType();
            var levels = levelCollector.ToElements().Cast<Level>();
            Level firstLevel = levels.FirstOrDefault(l => l.Name == "Этаж 1");
            Level secondLevel = levels.FirstOrDefault(l => l.Name == "Этаж 2");

            FilteredElementCollector wallCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType();
            WallType wallType = wallCollector.ToElements().FirstOrDefault(x => x.Name == "ADSK_Бетон В25_200 мм") as WallType;
            List<Curve> profile = new List<Curve>()
            {
                Line.CreateBound(new XYZ(0,0,0), new XYZ(10,0,0)),
                Line.CreateBound(new XYZ(10,0,0), new XYZ(10,0,10)),
                Line.CreateBound(new XYZ(10,0,10), new XYZ(0,0,10)),
                Line.CreateBound(new XYZ(0,0,10), new XYZ(0,0,0)),
            };

            Line curve = Line.CreateBound(new XYZ(10, 10, 0), new XYZ(30, 50, 0));

            using (Transaction t = new Transaction(doc, "Создать стену по профилю"))
            {
                t.Start();
                Wall.Create(doc, profile, wallType.Id, firstLevel.Id, false);

                t.Commit();

                if (t.GetStatus() == TransactionStatus.Committed)
                {
                    t.Start("Создать стену по контуру");
                    Wall newWall = Wall.Create(doc, curve, wallType.Id, firstLevel.Id, 3000 / 304.8, 0, false, false);
                    newWall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(secondLevel.Id);
                    t.Commit();
                }
            }

            return Result.Succeeded;
        }
    }
}
