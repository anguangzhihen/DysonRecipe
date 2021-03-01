using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DysonRecipeWin
{
	public class RecipeTreeNode : TreeNode
	{
		public static List<RecipeTreeNode> Calc(string itemName, Number count)
		{
			ClearByproduct();
			recipeRoot = new RecipeTreeNode(itemName);
			recipeRoot.SetItemNeedCount(count);
			CalcLeftNode(recipeRoot);
			ClearAndAddRecipeNodesToRoot(recipeRoot, byproductRoot);
			return root;
		}

		public static bool CalcLeftNode(RecipeTreeNode treeNode)
		{
			List<RecipeTreeNodePack> packs = new List<RecipeTreeNodePack>();
			foreach (var need in treeNode.recipe.needs)
			{
				packs.Add(new RecipeTreeNodePack(){ itemName = need.name, parent = treeNode});
			}

			int tmpCount = 0;
			while (packs.Count > 0)
			{
				tmpCount++;
				if (tmpCount > 10000)
				{
					treeNode.Nodes.Clear();
					return false;
				}
				var buildingPack = packs[0];
				if (!Data.nameToRecipes.ContainsKey(buildingPack.itemName))
				{
					packs.RemoveAt(0);
				}
				else
				{
					var node = new RecipeTreeNode()
					{
						itemName = buildingPack.itemName,
					};

					// 节点生成
					buildingPack.parent.Nodes.Add(node);

					foreach (var need in node.recipe.needs)
					{
						packs.Add(new RecipeTreeNodePack() { itemName = need.name, parent = node });
					}
					packs.RemoveAt(0);
				}
			}

			treeNode.Calc();

			return true;
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

		public static void ClearByproduct()
		{
			byproductRoot = null;
		}

		public static void ClearAndAddTextInfoToRoot(string text)
		{
			root.Clear();
			root.Add(new RecipeTreeNode(){ Text = text });
		}

		public static void ClearAndAddRecipeNodesToRoot(params RecipeTreeNode[] nodes)
		{
			root.Clear();
			foreach (var node in nodes)
			{
				if (node != null)
				{
					root.Add(node);
				}
			}
		}

		public static List<RecipeTreeNode> root = new List<RecipeTreeNode>();
		public static RecipeTreeNode recipeRoot = null;
		public static RecipeTreeNode byproductRoot = null;

		public RecipeTreeNode()
		{
		}

		public RecipeTreeNode(string itemName)
		{
			this.itemName = itemName;
		}

		public void Calc()
		{
			// 计算过程
			if (Parent != null && Parent is RecipeTreeNode)
			{
				var parent = (RecipeTreeNode)Parent;

				// 计算需求数
				var parentRecipe = parent.recipe;
				var parentTime = parentRecipe.time / Data.buildingEffective[parentRecipe.building];
				var needCount = parent.buildingCount * parentRecipe.GetNeedCount(itemName) / parentTime;

				SetItemNeedCount(needCount);

				// 额外信息
				ClearExtraInfo();
				foreach (var recipeNeed in recipe.needs)
				{
					if (Data.oreNames.Contains(recipeNeed.name))
					{
						var needEff = recipeNeed.count * buildingCount / recipe.time;
						needEff.num *= 60;
						var extraInfo = string.Format("{0}{1}/min", recipeNeed.name, needEff.ToFloatString());
						AppendInfo(extraInfo);
					}
				}
			}

			if (recipe.isGather)
			{
				Text = GetGatherResultString();
			}
			else
			{
				Text = GetResultString();
			}

			foreach (var node in Nodes)
			{
				if (node is RecipeTreeNode)
				{
					((RecipeTreeNode)node).Calc();
				}
			}
		}

		public void SetItemNeedCount(Number itemNeedCount)
		{
			var time = recipe.time / Data.buildingEffective[recipe.building];
			buildingCount = itemNeedCount * time / recipe.target.count;
		}

		public string GetResultString()
		{
			Number effctive = Data.buildingEffective[recipe.building];
			string value = buildingCount.ToFloatString() + " " + building + "(" + itemName + ") " + recipe.ToSpeedString(effctive) + " " + extraInfo;
			return value;
		}

		public string GetGatherResultString()
		{
			return string.Format("采集：{0}{1}/min", itemName, (buildingCount * 60).ToFloatString());
		}

		public void AppendInfo(string info)
		{
			if (extraInfo.Length != 0)
			{
				extraInfo.Append(" ");
			}
			else
			{
				extraInfo.Append("采集：");
			}
			extraInfo.Append(info);
		}

		public void ClearExtraInfo()
		{
			extraInfo.Clear();
		}

		public void SetAndSaveRecipeIndex(int recipeIndex)
		{
			this.recipeIndex = recipeIndex;
			//Data.save.SetNodeIndex(itemName, depth, index, recipeIndex);
			Calc();
		}

		public string itemName;

		public Number buildingCount;

		public string building
		{
			get { return recipe.building; }
		}

		public Recipe recipe
		{
			get { return Data.GetRecipes(itemName)[recipeIndex]; }
		}

		public int recipeIndex = 0;

		public int index
		{
			get
			{
				if (Parent == null || !(Parent is RecipeTreeNode))
				{
					return 0;
				}
				return Parent.Nodes.IndexOf(this);
			}
		}

		public int depth
		{
			get
			{
				if (Parent == null || !(Parent is RecipeTreeNode))
				{
					return 0;
				}
				return ((RecipeTreeNode) Parent).depth + 1;
			}
		}

		public StringBuilder extraInfo = new StringBuilder();
	}

	public class RecipeTreeNodePack
	{
		public RecipeTreeNode parent;
		public string itemName;
	}
}