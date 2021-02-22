using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace DysonRecipe
{
    public class Data
    {
        public static void Load()
        {
            recipes.Clear();
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(RECIPES_FILE_PATH)))
            {
                ExcelWorksheet sheet = excelPackage.Workbook.Worksheets[1];

                int nullValueCount = 0;
                for (int i = 2; i < sheet.Cells.Rows; i++)
                {
                    int j = 1;

                    try
                    {
                        var value = sheet.Cells[i, 1].Value;
                        if (value == null || string.IsNullOrEmpty(value.ToString()))
                        {
                            nullValueCount++;
                            if (nullValueCount >= 10)
                            {
                                break;
                            }
                            continue;
                        }
                        nullValueCount = 0;
                        string name = sheet.Cells[i, j++].Value.ToString();
                        string count = sheet.Cells[i, j++].Value.ToString();
                        List<string[]> needs= new List<string[]>();
                        var needsRow = sheet.Cells[i, j++].Value.ToString().Split('|');
                        foreach (var row in needsRow)
                        {
                            needs.Add(row.Split(':'));
                        }
                        Recipe recipe = new Recipe();
                        recipe.target = new ItemPack(name, new Number(count));
                        foreach (var need in needs)
                        {
                            recipe.needs.Add(new ItemPack(need[0], new Number(need[1])));
                        }
                        string building = sheet.Cells[i, j++].Value.ToString();
                        recipe.building = building;
                        string time = sheet.Cells[i, j++].Value.ToString();
                        recipe.time = new Number(time);
                        string level = sheet.Cells[i, j++].Value.ToString();
                        recipe.level = int.Parse(level);
                        recipes.Add(recipe);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(string.Format("Excel格式错误 {0}行 {1}列 路径：" + RECIPES_FILE_PATH, i, j-1));
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }

        public static List<Recipe> recipes = new List<Recipe>();

        public static string RECIPES_FILE_PATH = Directory.GetCurrentDirectory() + "/data/Recipes.xlsx";

    }

    public class Recipe
    {
        public string ToSpeedString(Number effective)
        {
            var newTime = time / effective;
            StringBuilder sb = new StringBuilder();
            sb.Append("[ 输出：");
            sb.Append(target.ToSpeedString(newTime));
            sb.Append(" 输入：");
            bool first = true;
            foreach (var need in needs)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(" | ");
                }

                sb.Append(need.ToSpeedString(newTime));
            }
            sb.Append("]");
            return sb.ToString();
        }

        public ItemPack target;
        public List<ItemPack> needs = new List<ItemPack>();
        public Number time;
        public string building;
        public int level;
    }

    public struct ItemPack
    {
        public ItemPack(string name, Number count)
        {
            this.name = name;
            this.count = count;
        }

        public float CalcSpeed(Number time)
        {
            return (count / time).ToFloat();
        }

        public override string ToString()
        {
            return name + " " + count;
        }

        public string ToSpeedString(Number time)
        {
            return string.Format("{0} {1:0.00}个每秒", name, CalcSpeed(time));
        }

        public string name;
        public Number count;
    }
}
