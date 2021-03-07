using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DysonRecipeWin
{
	public class RecipeTreeNode : TreeNode
	{
		public static List<TreeNode> Calc(string itemName, Number count, bool isBuildingCount)
		{
			ClearByproduct();
			recipeRoot = CreateNode(itemName);
			if(isBuildingCount)
				recipeRoot.SetBuildingCount(count);
			else
				recipeRoot.SetItemNeedCount(count);
			CalcRecipeNode(recipeRoot);
			CalcByproductTreeNodes();
			ResetNodesToRoot();
			return root;
		}

		public static bool CalcRecipeNode(RecipeTreeNode treeNode)
		{
			List<RecipeTreeNodePack> packs = new List<RecipeTreeNodePack>();
			foreach (var need in treeNode.recipe.needs)
			{
				packs.Add(new RecipeTreeNodePack(){ itemName = need.name, parent = treeNode});
			}

            treeNode.Nodes.Clear();
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
				    var node = CreateNode(buildingPack.itemName);

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

		public static void CreateChildNodes(RecipeTreeNode node)
		{
			node.Nodes.Clear();
			if (node.recipe == null)
			{
				return;
			}

			foreach (var need in node.recipe.needs)
			{
				var child = CreateNode(need.name);
				CreateChildNodes(child);
				node.Nodes.Add(child);
			}
		}

		public static RecipeTreeNode CreateNode(string itemName)
		{
			if (!Data.nameToRecipes.ContainsKey(itemName))
			{
				return null;
			}

			return new RecipeTreeNode()
			{
				itemName = itemName,
				recipeIndex = Data.save.GetDefaultIndex(itemName),
			};
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

		public static void ResetNodesToRoot()
		{
			List<TreeNode> nodes = new List<TreeNode>();
			nodes.Add(recipeRoot);
			nodes.Add(byproductRoot);

			root.Clear();
			foreach (var node in nodes)
			{
				if (node != null)
				{
					root.Add(node);
				}
			}
		}

		public static void CalcByproductTreeNodes()
		{
			Dictionary<string, Number> dict = new Dictionary<string, Number>();
			List<RecipeTreeNode> tmps = new List<RecipeTreeNode>();
			tmps.Add(recipeRoot);
			while (tmps.Count != 0)
			{
				var first = tmps[0];

				foreach (var pair in first.byproductDict)
				{
					Number num;
					if (!dict.TryGetValue(pair.Key, out num))
					{
						num = 0;
						dict[pair.Key] = num;
					}
					dict[pair.Key] = num + pair.Value;
				}

				tmps.RemoveAt(0);
			}

			if (dict.Count == 0)
			{
				byproductRoot = null;
			}
			else
			{
				if (byproductRoot == null)
				{
					byproductRoot = new TreeNode("副产品");
				}
				else
				{
					byproductRoot.Nodes.Clear();
				}
				foreach (var pair in dict)
				{
					byproductRoot.Nodes.Add(new TreeNode(new ItemPack(pair.Key, pair.Value).ToString()));
				}
			}
		}

		public static List<TreeNode> root = new List<TreeNode>();
		public static RecipeTreeNode recipeRoot = null;
		public static TreeNode byproductRoot = null;


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
				var needCount = parent.buildingCount * parent.recipe.GetNeedCount(itemName) / parent.timeEffect;
				SetItemNeedCount(needCount);

				// 副产品
				ClearByproducts();
				foreach (var byproduct in recipe.byproducts)
				{
					var count = buildingCount * byproduct.count / timeEffect;
					AddByproduct(byproduct.name, count);
				}

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
			var time = recipe.time / Data.GetBuildingEffective(recipe.building, buildingIndex);
			buildingCount = itemNeedCount * time / recipe.target.count;
		}

		public void SetBuildingCount(Number buildingCount)
		{
			this.buildingCount = buildingCount;
		}

		public string GetResultString()
		{
			Number effctive = Data.GetBuildingEffective(recipe.building, buildingIndex);
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
		    CalcRecipeNode(this);
		}

		// 添加副产品，结果为正说明多余，结果为副说明缺少
		public void AddByproduct(string itemName, Number count)
		{
			Number num;
			if (byproductDict.TryGetValue(itemName, out num))
			{
				byproductDict[itemName] = num + count;
			}
			else
			{
				byproductDict[itemName] = count;
			}
		}

		public void ClearByproducts()
		{
			byproductDict.Clear();
		}

		public string itemName;
		public Number buildingCount;
		public Dictionary<string, Number> byproductDict = new Dictionary<string, Number>();
		public StringBuilder extraInfo = new StringBuilder();
		public int recipeIndex = 0;
		public int buildingIndex = 0;

		public string building
		{
			get { return recipe.building; }
		}

		public Recipe recipe
		{
			get
			{
				var rs = Data.GetRecipes(itemName);
				if (rs == null)
				{
					return null;
				}
				return rs[Math.Min(Math.Max(0, recipeIndex), rs.Count - 1)];
			}
		}

		public Number timeEffect
		{
			get { return recipe.time / Data.GetBuildingEffective(recipe.building, index); }
		}

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
	}

	public class RecipeTreeNodePack
	{
		public RecipeTreeNode parent;
		public string itemName;
	}
}