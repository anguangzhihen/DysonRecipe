using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DysonRecipeWin
{
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Data.Load();
			Calc("石墨烯", 1);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Main());
		}

		public static Dictionary<string, Number> buildingEffective;
		public static Dictionary<string, Recipe> nameToRecipes;

		public static BuildingNeed Calc(string itemName, int count)
		{
			//ItemPack target = new ItemPack("石墨烯", new Number(2, 3));
			ItemPack target = new ItemPack(itemName, count);

			buildingEffective = new Dictionary<string, Number>()
			{
				{ "电弧熔炉", 1 },
				{ "制造台", new Number(1, 1)},
				{ "化工厂", 1 },
				{ "微型粒子对撞器", 1 },
			};

			BuildingNeed first = null;

			nameToRecipes = new Dictionary<string, Recipe>();
			foreach (var recipe in Data.recipes)
			{
				if (!nameToRecipes.ContainsKey(recipe.target.name))
				{
					nameToRecipes[recipe.target.name] = recipe;
				}
			}

			List<BuildingTargetPack> tmp = new List<BuildingTargetPack>();
			List<ItemPack> result = new List<ItemPack>();
			tmp.Add(new BuildingTargetPack() { itemPack = target });

			int tmpCount = 0;
			while (tmp.Count > 0)
			{
				tmpCount++;
				if (tmpCount > 1000)
				{
					Console.WriteLine("失败，配方产生了循环");
					return null;
				}
				var buildingPack = tmp[0];
				if (!nameToRecipes.ContainsKey(buildingPack.itemPack.name))
				{
					result.Add(tmp[0].itemPack);
					tmp.RemoveAt(0);
				}
				else
				{
					var recipe = nameToRecipes[buildingPack.itemPack.name];
					var buildingCount = recipe.time / buildingEffective[recipe.building];
					buildingCount = buildingCount * buildingPack.itemPack.count / recipe.target.count;

					var buildingNeed = new BuildingNeed()
					{
						building = recipe.building,
						itemName = buildingPack.itemPack.name,
						count = buildingCount,
						level = buildingPack.parent == null ? 1 : (buildingPack.parent.level + 1),
					};
					if (buildingPack.parent != null)
					{
						buildingPack.parent.childs.Add(buildingNeed);
					}
					if (first == null)
					{
						first = buildingNeed;
					}

					foreach (var need in recipe.needs)
					{
						var tmpNeed = need; ;
						tmpNeed.count = tmpNeed.count * buildingPack.itemPack.count;
						tmpNeed.count /= recipe.target.count;
						tmp.Add(new BuildingTargetPack() { itemPack = tmpNeed, parent = buildingNeed });
					}
					tmp.RemoveAt(0);
				}
			}

			Console.WriteLine("目标：" + target + "个每秒");
			Console.WriteLine();
			Console.WriteLine("当前设备效率：");
			foreach (var eff in buildingEffective)
			{
				Console.WriteLine(string.Format(Tab(1) + "{0} {1:0%}", eff.Key, eff.Value.ToFloat()));
			}
			Console.WriteLine();
			Console.WriteLine("需求矿石：");
			foreach (var itemPack in result)
			{
				var tmpItemPack = itemPack;
				tmpItemPack.count *= 60;
				Console.WriteLine(Tab(1) + tmpItemPack + "个每分");
			}
			Console.WriteLine();
			Console.WriteLine("需求设施：");
			PrintBuildingNeed(first, nameToRecipes, buildingEffective);
			Console.ReadLine();
			return first;
		}

		static void PrintBuildingNeed(BuildingNeed need, Dictionary<string, Recipe> nameToRecipes, Dictionary<string, Number> buildingEffective)
		{
			var recipe = nameToRecipes[need.itemName];
			Number effctive = buildingEffective[recipe.building];
			Console.WriteLine(Tab(need.level) + "|--" + need.building + "(" + need.itemName + ") " + need.count + " " + recipe.ToSpeedString(effctive));
			foreach (var child in need.childs)
			{
				PrintBuildingNeed(child, nameToRecipes, buildingEffective);
			}
		}

		public static string Tab(int count)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < count; i++)
			{
				sb.Append("    ");
			}
			return sb.ToString();
		}
	}
}
