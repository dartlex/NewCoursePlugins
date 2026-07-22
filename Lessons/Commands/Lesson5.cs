using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lessons.Commands
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class Lesson5 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection choices = uidoc.Selection;
            Reference hasPickOne = choices.PickObject(ObjectType.Element);
            if (hasPickOne != null)
            {
                Element selectedElement = doc.GetElement(hasPickOne.ElementId);
                Parameter markParam = selectedElement.LookupParameter("Марка");

                Parameter commentParam = selectedElement.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);

                using (Transaction t = new Transaction(doc, "Задать комментарий"))
                {
                    t.Start();
                    commentParam.Set("Привет, я комментарий!");
                    t.Commit();
                }

                using (Transaction t = new Transaction(doc, "Задать этаж"))
                {
                    Guid adskFloor = new Guid("9eabf56c-a6cd-4b5c-a9d0-e9223e19ea3f");
                    Parameter floorADSKParam = selectedElement.get_Parameter(adskFloor);

                    ElementId levelId = selectedElement.LevelId;
                    string result = "Нет этажа";
                    if (levelId.IntegerValue != -1)
                    {
                        Level level = doc.GetElement(levelId) as Level;
                        result = level.Name;
                    }
                    t.Start();
                    floorADSKParam.Set(result);
                    t.Commit();
                }
                Parameter glassParam = selectedElement.LookupParameter("Glass");
                if (glassParam != null)
                {
                    FilteredElementCollector collector = new FilteredElementCollector(doc).WhereElementIsNotElementType();
                    collector.OfCategory(BuiltInCategory.OST_Materials);
                    Material material = collector.ToElements().Cast<Material>().FirstOrDefault(x => x.Name == "Green Glass");
                    using (Transaction t = new Transaction(doc, "Задать материал"))
                    {
                        t.Start();
                        glassParam.Set(material.Id);
                        t.Commit();
                    }
                }
                Element elementType = null;
                if (selectedElement is RoofBase roof)
                {
                    elementType = roof.RoofType;
                    TaskDialog.Show(elementType.Name,elementType.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_MARK).AsString());
                }
                else if (selectedElement is FamilyInstance instance)
                {
                    elementType = instance.Symbol;
                }
                if(elementType != null) TaskDialog.Show(selectedElement.Name, $"Значение марки: {markParam.AsString()}\nЗначение комментария: {commentParam.AsString()}");
                return Result.Succeeded;
            }
            return Result.Cancelled;
        }
    }
}
