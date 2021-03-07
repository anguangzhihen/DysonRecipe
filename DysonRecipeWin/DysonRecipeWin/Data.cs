using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
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
	        oreNames.Add("硫酸");

            List<Recipe>  recipes = new List<Recipe>();
	        foreach (var oreName in oreNames)
	        {
		        var recipe = new Recipe();
		        recipe.target = new ItemPack(oreName, 1);
		        recipe.isGather = true;
		        recipe.building = "采集";
		        recipe.time = 1;
		        recipe.displayName = oreName + "（采集）";
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
	                    var targetRow = sheet.Cells[i, j++].Value.GetStringOrEmpty().Split('|');
	                    string displayName = sheet.Cells[i, j++].Value.GetStringOrEmpty();
                        List<string[]> needs= new List<string[]>();
                        var needsRow = sheet.Cells[i, j++].Value.GetStringOrEmpty().Split('|');
                        foreach (var row in needsRow)
                        {
                            needs.Add(row.Split(':'));
                        }
	                    string building = sheet.Cells[i, j++].Value.GetStringOrEmpty();
	                    string time = sheet.Cells[i, j++].Value.GetStringOrEmpty();
	                    string level = sheet.Cells[i, j++].Value.GetStringOrEmpty();
						bool isLoop = false;
						bool.TryParse(sheet.Cells[i, j++].Value.GetStringOrEmpty(), out isLoop);
						for (int k = 0; k < targetRow.Length; k++)
	                    {
		                    var row = targetRow[k];
		                    var targetStrs = row.Split(':');
							var target = new ItemPack(targetStrs[0], new Number(targetStrs[1]));

		                    if (k == 0)
		                    {
			                    if (!itemNames.Contains(targetStrs[0]))
			                    {
				                    itemNames.Add(targetStrs[0]);
								}
							}

		                    Recipe recipe = new Recipe();
		                    recipe.target = target;
							recipe.displayName = displayName;
		                    recipe.isLoop = isLoop;
		                    foreach (var row2 in targetRow)
		                    {
								var targetStrs2 = row2.Split(':');
			                    if (targetStrs2[0] != targetStrs[0])
			                    {
									recipe.byproducts.Add(new ItemPack(targetStrs2[0], new Number(targetStrs2[1])));
								}
							}
							foreach (var need in needs)
		                    {
			                    recipe.needs.Add(new ItemPack(need[0], new Number(need[1])));
		                    }
		                    recipe.building = building;
		                    recipe.time = new Number(time);
		                    recipe.level = int.Parse(level);
		                    recipes.Add(recipe);
						}
                    }
                    catch (Exception e)
                    {
	                    MessageBox.Show($@"Excel格式错误 {i}行 {j - 1}列，请确认数据正确后再使用");
                        Console.WriteLine($@"Excel格式错误 {i}行 {j - 1}列 路径：{RECIPES_FILE_PATH}");
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

	        buildingEffective.Clear();
			RegisterBuilding(new Building(){ name = "制造台", displayName = "制造台 Ⅰ", effective = new Number(3, 4)});
			RegisterBuilding(new Building(){ name = "制造台", displayName = "制造台 Ⅱ", effective = 1});
			RegisterBuilding(new Building(){ name = "制造台", displayName = "制造台 Ⅲ", effective = new Number(3, 2)});
			RegisterBuilding(new Building(){ name = "电弧熔炉", effective = 1});
			RegisterBuilding(new Building(){ name = "化工厂", effective = 1});
			RegisterBuilding(new Building(){ name = "微型粒子对撞器", effective = 1});
			RegisterBuilding(new Building(){ name = "矩阵研究站", effective = 1});
			RegisterBuilding(new Building(){ name = "原油精炼机", effective = 1});
			RegisterBuilding(new Building(){ name = "采集", effective = 1});


			nameToRecipes = new Dictionary<string, List<Recipe>>();
	        foreach (var recipe in recipes)
	        {
		        if (!nameToRecipes.ContainsKey(recipe.target.name))
		        {
			        nameToRecipes[recipe.target.name] = new List<Recipe>();
		        }
				nameToRecipes[recipe.target.name].Add(recipe);
	        }

	        foreach (var nameToRecipe in nameToRecipes)
	        {
		        if (!nameToRecipe.Value[0].couldDefault)
		        {
			        int index = 0;
			        foreach (var recipe in nameToRecipe.Value)
			        {
				        if (recipe.couldDefault)
				        {
					        break;
				        }
				        index++;
			        }
			        if (index == nameToRecipe.Value.Count)
			        {
				        MessageBox.Show("没有找到非循环的配方，请确认数据正确后再使用");
				        throw new Exception("没有找到非循环的配方，请确认数据正确后再使用");
			        }
					else
			        {
				        save.SetDefaultIndex(nameToRecipe.Key, index);
			        }
				}
			}
        }

	    public static Recipe GetRecipe(string itemName)
	    {
		    var recipes = nameToRecipes[itemName];
			return recipes[0];
	    }

	    public static List<Recipe> GetRecipes(string itemName)
	    {
		    List<Recipe> result;
		    if (nameToRecipes.TryGetValue(itemName, out result))
		    {
			    return result;
		    }
		    return null;
	    }

	    public static void RegisterBuilding(Building building)
	    {
		    List<Building> list = null;
		    if (!buildingEffective.TryGetValue(building.name, out list))
		    {
			    list = new List<Building>();
			    buildingEffective[building.name] = list;
		    }
			list.Add(building);
		}

	    public static Number GetBuildingEffective(string buildingName, int index = 0)
	    {
		    List<Building> list = null;
		    if (!buildingEffective.TryGetValue(buildingName, out list))
		    {
			    return 1;
		    }
		    return list[Math.Max(Math.Min(index, list.Count - 1), 0)].effective;
	    }

        public static List<Building> GetEffectiveBuildings(string buildingName)
        {
		    List<Building> list = null;
            buildingEffective.TryGetValue(buildingName, out list);
            return list;
        }

	    public static void SaveFile()
	    {
		    var str = JsonConvert.SerializeObject(_save, Formatting.Indented);
			File.WriteAllText(SAVE_FILE_PATH, str);
	    }

	    public static Save save
		{
		    get
		    {
			    if (_save == null)
			    {
				    try
				    {
					    if (File.Exists(SAVE_FILE_PATH))
					    {
						    _save = JsonConvert.DeserializeObject<Save>(File.ReadAllText(SAVE_FILE_PATH));
					    }
					}
				    catch (Exception e)
				    {
					    Console.WriteLine(e);
				    }

				    if (_save == null)
				    {
					    _save = new Save();
						_save.SetBuildingDefaultIndex("制造台", 1);
				    }
			    }
			    return _save;
		    }
	    }

	    private static Save _save;

		public static List<string> itemNames = new List<string>();
	    public static List<string> oreNames = new List<string>();
	    public static Dictionary<string, List<Building>> buildingEffective = new Dictionary<string, List<Building>>();
	    public static Dictionary<string, List<Recipe>> nameToRecipes;

		public static string RECIPES_FILE_PATH = Directory.GetCurrentDirectory() + "/data/Recipes.xlsx";
		public static string SAVE_FILE_PATH = Directory.GetCurrentDirectory() + "/save.json";

    }

	public static class Ext
	{
		public static string GetStringOrEmpty(this object obj)
		{
			if (obj == null)
			{
				return "";
			}
			return obj.ToString();
		}
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

	    public override string ToString()
	    {
			StringBuilder sb = new StringBuilder();
		    //sb.Append(GetDisplayName());
		    //sb.Append("：");
		    sb.Append(target);
	        foreach (var byproduct in byproducts)
	        {
	            sb.Append(" + ");
	            sb.Append(byproduct);
	        }

		    sb.Append(" <= ");
		    bool first = true;
		    foreach (var need in needs)
		    {
			    if (first)
			    {
				    first = false;
			    }
			    else
			    {
				    sb.Append(" + ");
			    }
			    sb.Append(need);
		    }
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

	    public string GetDisplayName()
	    {
		    if (string.IsNullOrEmpty(displayName))
		    {
			    return target.name;
		    }
		    return displayName;
	    }

	    public bool IsBuilding()
	    {
		    return level >= 10;
	    }

	    public bool IsComponent()
	    {
		    return !IsBuilding();
	    }

	    public bool couldDefault
	    {
		    get
		    {
			    if (isLoop)
			    {
				    return false;
			    }
			    foreach (var need in needs)
			    {
				    if (need.name == target.name)
				    {
					    return false;
				    }
			    }
			    return true;
		    }
	    }

		public ItemPack target;
        public List<ItemPack> needs = new List<ItemPack>();
	    public List<ItemPack> byproducts = new List<ItemPack>();
		public Number time;
        public string building;
        public int level;
	    public bool isGather = false;	// 采集
	    public string displayName;
	    public bool isLoop = false;
    }

	public class Building
	{
		public string GetDisplayName()
		{
			if (string.IsNullOrEmpty(displayName))
			{
				return name;
			}
			return displayName;
		}

		public string name;
		public string displayName;
		public Number effective;
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
            return count + " " + name;
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
		public int GetBuildingDefaultIndex(string itemName)
		{
			return GetDefaultIndex(itemName + "_建筑");
		}

		public void SetBuildingDefaultIndex(string itemName, int index)
		{
			SetDefaultIndex(itemName + "_建筑", index);
		}

		public int GetDefaultIndex(string itemName)
		{
			int result = 0;
			nameToDefaultIndex.TryGetValue(itemName, out result);
			return result;
		}

		public void SetDefaultIndex(string itemName, int index)
		{
			if (index == 0)
			{
				if (nameToDefaultIndex.ContainsKey(itemName))
				{
					nameToDefaultIndex.Remove(itemName);
				}
			}
			else
			{
				nameToDefaultIndex[itemName] = index;
			}
			Data.SaveFile();
		}

		public Dictionary<string, int> nameToDefaultIndex = new Dictionary<string, int>();
	}
}
