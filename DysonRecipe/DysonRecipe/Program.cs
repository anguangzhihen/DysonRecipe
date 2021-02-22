using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonRecipe
{
	public enum ItemType
	{
		IronOre,
		Iron,
		CopperOre,
		Copper,
		Magnet,
		MagnetCoil,

		// building
		Furnace,
		Manufacture
	}

	public class Item
	{
		public Item(string name, ItemType itemType)
		{
			this.name = name;
			this.itemType = itemType;
		}

		public string name;
		public ItemType itemType;
	}

	public struct Number
	{
		public int num;
		public int dividedNum;

		public Number(int num, int dividedNum)
		{
			this.num = num;
			this.dividedNum = dividedNum;
			ProcessResult();
		}

		void ProcessResult()
		{
			var tmp = GetLargestCommonDivisor(num, dividedNum);
			num /= tmp;
			dividedNum /= tmp;
		}

		static int GetLargestCommonDivisor(int n1, int n2)
		{
			int max = n1 > n2 ? n1 : n2;
			int min = n1 < n2 ? n1 : n2;
			int remainder;
			while (min != 0)
			{
				remainder = max % min;
				max = min;
				min = remainder;
			}
			return max;
		}

		static int GetLeastCommonMutiple(int n1, int n2)
		{
			return n1 * n2 / GetLargestCommonDivisor(n1, n2);
		}

		public static Number operator +(Number n1, Number n2)
		{
			var divNum = GetLeastCommonMutiple(n1.dividedNum, n2.dividedNum);
			var num = n1.num * divNum / n1.dividedNum + n2.num * divNum / n2.dividedNum;
			var number = new Number(num, divNum);
			number.ProcessResult();
			return number;
		}

		public static Number operator *(Number n1, Number n2)
		{
			var number = new Number(n1.num * n2.num, n1.dividedNum * n2.dividedNum);
			number.ProcessResult();
			return number;
		}

		public static Number operator /(Number n1, Number n2)
		{
			var number = new Number(n1.num * n2.dividedNum, n1.dividedNum * n2.num);
			number.ProcessResult();
			return number;
		}

		public static implicit operator Number(int num)
		{
			return new Number(num, 1);
		}

		public override string ToString()
		{
			if (dividedNum == 1)
				return num.ToString();
			return num + "/" + dividedNum;
		}

		public float ToFloat()
		{
			return (float)num / dividedNum;
		}
	}

	public struct ItemPack
	{
		public ItemPack(ItemType itemType, Number count)
		{
			this.itemType = itemType;
			this.count = count;
		}

		public float CalcSpeed(Number time)
		{
			return (count / time).ToFloat();
		}

		public override string ToString()
		{
			return Program.GetItemName(itemType) + " " + count;
		}

		public string ToSpeedString(Number time)
		{
			return string.Format("{0} {1:0.00}个每秒", Program.GetItemName(itemType), CalcSpeed(time));
		}

		public ItemType itemType;
		public Number count;
	}

	public class Recipe
	{
		public ItemPack target;
		public List<ItemPack> needs;
		public Number time;
		public ItemType building;

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
	}

	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Calc();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.WriteLine("失败，计算出错");
				Console.ReadLine();
			}
		}

		static void Calc()
		{
			ItemPack target = new ItemPack(ItemType.MagnetCoil, 2);

			items = new Dictionary<ItemType, Item>();
			items.Add(ItemType.Copper, new Item("铜板", ItemType.Copper));
			items.Add(ItemType.CopperOre, new Item("铜矿", ItemType.CopperOre));
			items.Add(ItemType.IronOre, new Item("铁矿", ItemType.IronOre));
			items.Add(ItemType.Iron, new Item("铁板", ItemType.Iron));
			items.Add(ItemType.Magnet, new Item("磁铁", ItemType.Magnet));
			items.Add(ItemType.MagnetCoil, new Item("磁铁线圈", ItemType.MagnetCoil));
			items.Add(ItemType.Furnace, new Item("熔炉", ItemType.Furnace));
			items.Add(ItemType.Manufacture, new Item("制作台", ItemType.Manufacture));

			var buildingEffective = new Dictionary<ItemType, Number>()
			{
				{ ItemType.Furnace, 1 },
				{ ItemType.Manufacture, new Number(0, 1)},	//
			};

			Dictionary<ItemType, Dictionary<ItemType, Number>> buildingNeed = new Dictionary<ItemType, Dictionary<ItemType, Number>>();
			List<Recipe> recipes = new List<Recipe>();
			recipes.Add(new Recipe()
			{
				target = new ItemPack(ItemType.Iron, 1),
				needs = new List<ItemPack>() {new ItemPack(ItemType.IronOre, 1) },
				time = 1,
				building = ItemType.Furnace,
			});
			recipes.Add(new Recipe()
			{
				target = new ItemPack(ItemType.Copper, 1),
				needs = new List<ItemPack>() { new ItemPack(ItemType.CopperOre, 1) },
				time = 1,
				building = ItemType.Furnace,
			});
			recipes.Add(new Recipe()
			{
				target = new ItemPack(ItemType.Magnet, 1),
				needs = new List<ItemPack>() { new ItemPack(ItemType.IronOre, 1) },
				time = new Number(3, 2),
				building = ItemType.Furnace,
			});
			recipes.Add(new Recipe()
			{
				target = new ItemPack(ItemType.MagnetCoil, 2),
				needs = new List<ItemPack>()
				{
					new ItemPack(ItemType.Magnet, 2),
					new ItemPack(ItemType.Copper, 1)
				},
				time = 1,
				building = ItemType.Manufacture,
			});

			Dictionary<ItemType, Recipe> itemTypeToRecipes = new Dictionary<ItemType, Recipe>();
			foreach (var recipe in recipes)
			{
				if (!itemTypeToRecipes.ContainsKey(recipe.target.itemType))
				{
					itemTypeToRecipes[recipe.target.itemType] = recipe;
				}
			}

			List<ItemPack> tmp = new List<ItemPack>();
			List<ItemPack> result = new List<ItemPack>();
			tmp.Add(target);

			int tmpCount = 0;
			while (tmp.Count > 0)
			{
				tmpCount++;
				if (tmpCount > 1000)
				{
					Console.WriteLine("失败，配方产生了循环");
					return;
				}
				var itemPack = tmp[0];
				if (!itemTypeToRecipes.ContainsKey(itemPack.itemType))
				{
					result.Add(tmp[0]);
					tmp.RemoveAt(0);
				}
				else
				{
					var recipe = itemTypeToRecipes[itemPack.itemType];
					foreach (var need in recipe.needs)
					{
						var tmpNeed = need;;
						tmpNeed.count = tmpNeed.count * itemPack.count;
						tmpNeed.count /= recipe.target.count;
						tmp.Add(tmpNeed);
					}
					Number buildingCount;
					Dictionary<ItemType, Number> dict;
					if (!buildingNeed.TryGetValue(recipe.building, out dict))
					{
						buildingNeed[recipe.building] = new Dictionary<ItemType, Number>();
						dict = buildingNeed[recipe.building];
					}
					if (!dict.TryGetValue(recipe.target.itemType, out buildingCount))
					{
						buildingCount = new Number(0, 1);
					}
					var plusCount = recipe.time / buildingEffective[recipe.building];
					plusCount = plusCount * itemPack.count / recipe.target.count;
					dict[recipe.target.itemType] = buildingCount + plusCount;

					tmp.RemoveAt(0);
				}
			}

			Console.WriteLine("目标：" + target + "个每秒");
			Console.WriteLine();
			Console.WriteLine("当前设备效率：");
			foreach (var eff in buildingEffective)
			{
				Console.WriteLine(string.Format("\t{0} {1:0%}", GetItemName(eff.Key), eff.Value.ToFloat()));
			}
			Console.WriteLine();
			Console.WriteLine("需求矿石：");
			foreach (var itemPack in result)
			{
				var tmpItemPack = itemPack;
				tmpItemPack.count *= 60;
				Console.WriteLine("\t" + tmpItemPack + "个每分");
			}
			Console.WriteLine();
			Console.WriteLine("需求设施：");
			foreach (var pair in buildingNeed)
			{
				foreach (var pair2 in pair.Value)
				{
					var recipe = itemTypeToRecipes[pair2.Key];
					Number effctive = buildingEffective[recipe.building];
					Console.WriteLine("\t" + Program.GetItemName(pair.Key) + "(" + Program.GetItemName(pair2.Key) + ") " + pair2.Value + " " + recipe.ToSpeedString(effctive) );
				}
			}
			Console.ReadLine();
		}

		public static string GetItemName(ItemType itemType)
		{
			if (items == null)
			{
				return itemType.ToString();
			}
			Item item = null;
			if (items.TryGetValue(itemType, out item))
			{
				return item.name;
			}
			return itemType.ToString();
		}

		private static Dictionary<ItemType, Item> items;
	}
}
