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



		public static BuildingNeed Calc(string itemName, int count)
		{
			//ItemPack target = new ItemPack("石墨烯", new Number(2, 3));
			ItemPack target = new ItemPack(itemName, count);



			BuildingNeed first = null;


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
				if (!Data.nameToRecipes.ContainsKey(buildingPack.itemPack.name))
				{
					result.Add(tmp[0].itemPack);
					tmp.RemoveAt(0);
				}
				else
				{
					var recipe = Data.nameToRecipes[buildingPack.itemPack.name];
					var time = recipe.time / Data.buildingEffective[recipe.building];
					var buildingCount = time * buildingPack.itemPack.count / recipe.target.count;

					var buildingNeed = new BuildingNeed()
					{
						building = recipe.building,
						itemName = buildingPack.itemPack.name,
						count = buildingCount,
						level = buildingPack.parent == null ? 1 : (buildingPack.parent.level + 1),
					};

					// 额外信息
					foreach (var recipeNeed in recipe.needs)
					{
						if (Data.oreNames.Contains(recipeNeed.name))
						{
							var needEff = recipeNeed.count * buildingCount / time;
							needEff.num *= 60;
							var extraInfo = string.Format("{0}{1}/min", recipeNeed.name, needEff.ToFloatString());
							buildingNeed.AppendInfo(extraInfo);
						}
						if (Data.liquidNames.Contains(recipeNeed.name))
						{
							var needEff = recipeNeed.count * buildingCount / time;
							var extraInfo = string.Format("{0}{1}/s", recipeNeed.name, needEff.ToFloatString());
							buildingNeed.AppendInfo(extraInfo);
						}
					}

					if (buildingPack.parent != null)
					{
						buildingPack.parent.childs.Add(buildingNeed);
						if (buildingPack.parent.extraInfo.Length != 0)
						{
							buildingPack.parent.ClearExtraInfo();
						}
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
			foreach (var eff in Data.buildingEffective)
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
			PrintBuildingNeed(first);
			Console.ReadLine();
			return first;
		}

		static void PrintBuildingNeed(BuildingNeed need)
		{
			Console.WriteLine(Tab(need.level) + "|--" + ResultString(need));
			foreach (var child in need.childs)
			{
				PrintBuildingNeed(child);
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

		public static string ResultString(BuildingNeed need)
		{
			var recipe = Data.nameToRecipes[need.itemName];
			Number effctive = Data.buildingEffective[recipe.building];
			string value = need.count + " " + need.building + "(" + need.itemName + ") " + recipe.ToSpeedString(effctive) + " " + need.extraInfo;
			return value;
		}
	}
}
