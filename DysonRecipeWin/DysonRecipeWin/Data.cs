using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace DysonRecipeWin
{
    public class Data
    {
        public static void Load()
        {
	        oreNames.Clear();
	        oreNames.Add("铁矿");
	        oreNames.Add("铜矿");
	        oreNames.Add("硅石");
	        oreNames.Add("钛石");
	        oreNames.Add("石矿");
	        oreNames.Add("煤矿");
	        oreNames.Add("水");
	        oreNames.Add("原油");

			recipes.Clear();
	        foreach (var oreName in oreNames)
	        {
		        var recipe = new Recipe();
		        recipe.target = new ItemPack(oreName, 1);
		        recipe.isGather = true;
		        recipe.building = "采集";
		        recipe.time = 1;
		        recipes.Add(recipe);
	        }
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

	        buildingEffective = new Dictionary<string, Number>()
	        {
		        { "电弧熔炉", 1 },
		        { "制造台", new Number(1, 1)},
		        { "化工厂", 1 },
		        { "微型粒子对撞器", 1 },
		        { "矩阵研究站", 1 },
				{ "原油精炼机", 1 },
				{ "采集", 1 },
			};

	        nameToRecipes = new Dictionary<string, List<Recipe>>();
	        foreach (var recipe in Data.recipes)
	        {
		        if (!nameToRecipes.ContainsKey(recipe.target.name))
		        {
			        nameToRecipes[recipe.target.name] = new List<Recipe>();
		        }
				nameToRecipes[recipe.target.name].Add(recipe);
	        }
        }

		public static Save save = new Save();

		public static List<Recipe> recipes = new List<Recipe>();
	    public static List<string> oreNames = new List<string>();
	    public static Dictionary<string, Number> buildingEffective;
	    public static Dictionary<string, List<Recipe>> nameToRecipes;

		public static string RECIPES_FILE_PATH = Directory.GetCurrentDirectory() + "/data/Recipes.xlsx";

    }

    public class Recipe
    {
        public string ToSpeedString(Number effective)
        {
            var newTime = time / effective;
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            sb.Append(target.ToSpeedString(newTime, false));
            sb.Append(" ← ");
            bool first = true;
            foreach (var need in needs)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(" ");
                }

                sb.Append(need.ToSpeedString(newTime));
            }
            sb.Append(" ]");
            return sb.ToString();
        }

	    public Number GetNeedCount(string itemName)
	    {
		    foreach (var need in needs)
		    {
			    if (need.name == itemName)
			    {
				    return need.count;
			    }
		    }
			Console.WriteLine("没有找到itemName");
		    return 0;
	    }

        public ItemPack target;
        public List<ItemPack> needs = new List<ItemPack>();
        public Number time;
        public string building;
        public int level;
	    public bool isGather = false;	// 采集
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

        public string ToSpeedString(Number time, bool needName = true)
        {
            if (!needName)
            {
                return string.Format("{0:0.##}", CalcSpeed(time));
            }

            return string.Format("{0:0.##}{1}", CalcSpeed(time), name);
        }

        public string name;
        public Number count;
    }

	public class Save
	{
		
	}

	public class NodeSave
	{
		public int depth;
		public int index;
		public int recipeIndex;
	}
}
