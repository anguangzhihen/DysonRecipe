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
		public string name;
		public ItemType itemTyep;
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

		public Number Plus(Number other)
		{
			var divNum = GetLeastCommonMutiple(dividedNum, other.dividedNum);
			num = num * divNum / dividedNum + other.num * divNum / other.dividedNum;
			dividedNum = divNum;
			ProcessResult();
			return this;
		}

		public Number Mul(Number other)
		{
			num *= other.num;
			dividedNum *= other.dividedNum;
			ProcessResult();
			return this;
		}

		public Number Div(Number other)
		{
			num *= other.dividedNum;
			dividedNum *= other.num;
			ProcessResult();
			return this;
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
	}

	public struct ItemPack
	{
		public ItemPack(ItemType itemType, Number count)
		{
			this.itemType = itemType;
			this.count = count;
		}

		public override string ToString()
		{
			return itemType + " " + count;
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
	}

	class Program
	{
		static void Main(string[] args)
		{
			ItemPack target = new ItemPack(ItemType.MagnetCoil, 2);

			var buildingEffective = new Dictionary<ItemType, float>()
			{
				{ ItemType.Furnace, 1f },
				{ ItemType.Manufacture, 1f},
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
					try
					{
						var recipe = itemTypeToRecipes[itemPack.itemType];
						foreach (var need in recipe.needs)
						{
							var tmpNeed = need;;
							tmpNeed.count.Mul(itemPack.count);
							//tmpNeed.count.Mul(recipe.time);
							tmpNeed.count.Div(recipe.target.count);
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
						var plusCount = recipe.time;
						plusCount.Mul(itemPack.count);
						plusCount.Div(recipe.target.count);
						dict[recipe.target.itemType] = buildingCount.Plus(plusCount);

						tmp.RemoveAt(0);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						Console.WriteLine("失败，计算出错");
					}
				}
			}

			Console.WriteLine("目标：" + target + "psec");
			Console.WriteLine("需求矿石：");
			foreach (var itemPack in result)
			{
				var tmpItemPack = itemPack;
				tmpItemPack.count.Mul(60);
				Console.WriteLine(tmpItemPack + "pmin");
			}
			Console.WriteLine("需求设施：");
			foreach (var pair in buildingNeed)
			{
				foreach (var pair2 in pair.Value)
				{
					Console.WriteLine(pair.Key + " " + pair2.Key + " " + pair2.Value);
				}
			}
			Console.ReadLine();
		}
	}
}
