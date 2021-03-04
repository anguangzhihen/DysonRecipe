using System;
using System.Windows.Forms;

namespace DysonRecipeWin
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
			ResetRecipeList(r => r.IsComponent());
			ResultTreeView.Nodes.Clear();
			RecipeContent.Text = "";
		}

		public void ResetRecipeList(Func<Recipe, bool> filter)
		{
			this.RecipeList.Items.Clear();
			for (int i = Data.itemNames.Count - 1; i >= 0; i--)
			{
				var itemName = Data.itemNames[i];
				var recipe = Data.GetRecipe(itemName);
				if (recipe == null)
				{
					Console.WriteLine(@"recipe == null");
					continue;
				}
				if (filter != null && !filter(recipe))
				{
					continue;
				}

				if (RecipeList.SelectedItem == null || string.IsNullOrEmpty(RecipeList.SelectedItem.ToString()))
				{
					RecipeList.SelectedItem = recipe.target.name;
				}
				this.RecipeList.Items.Add(recipe.target.name);
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (RecipeList.SelectedItem == null || string.IsNullOrEmpty(RecipeList.SelectedItem.ToString()))
			{
				Console.WriteLine("请选择配方");
				return;
			}
			Console.WriteLine(RecipeList.SelectedItem + " " + NumChoose.Value);

			ResultTreeView.Nodes.Clear();

		    Number num = (int) NumChoose.Value;
		    var itemName = RecipeList.SelectedItem.ToString();
            if (deviceToggle.Checked)
            {
                num *= Data.GetRecipe(itemName).target.count;
            }
			var arr = RecipeTreeNode.Calc(itemName, num).ToArray();
			ResultTreeView.Nodes.AddRange(arr);
			ResultTreeView.ExpandAll();

			foreach (var recipeTreeNode in arr)
			{
				FoldGatherNode(recipeTreeNode);
			}
		}

		void FoldGatherNode(TreeNode node)
		{
			bool hasGather = false;
			foreach (var child in node.Nodes)
			{
				if (child is RecipeTreeNode && ((RecipeTreeNode) child).recipe.isGather)
				{
					hasGather = true;
					break;
				}
			}
			if(hasGather)
				node.Toggle();

			foreach (var child in node.Nodes)
			{
				if (child is RecipeTreeNode)
				{
					FoldGatherNode(((RecipeTreeNode)child));
				}
			}
		}


		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node is RecipeTreeNode)
			{
				recipeIndexChoose.Items.Clear();
				chosenRecipeTreeNode = (RecipeTreeNode) e.Node;
				var recipes = Data.GetRecipes(chosenRecipeTreeNode.itemName);
				foreach (var recipe in recipes)
				{
					recipeIndexChoose.Items.Add(recipe.GetDisplayName());
				}
				recipeIndexChoose.SelectedIndex = chosenRecipeTreeNode.recipeIndex;

				RecipeContent.Text = chosenRecipeTreeNode.recipe.ToString();
				defaultRecipeToggle.Checked = Data.save.GetDefaultIndex(chosenRecipeTreeNode.itemName) == chosenRecipeTreeNode.recipeIndex;
			}
		}

        private void NumChoose_ValueChanged(object sender, EventArgs e)
        {
        }

		private void recipeIndexChoose_SelectedIndexChanged(object sender, EventArgs e)
		{
			Console.WriteLine($@"sender: {sender} e: {e}");
			if (recipeIndexChoose.SelectedIndex < 0)
			{
				Console.WriteLine(@"recipeIndexChoose.SelectedIndex < 0");
				return;
			}
			if (chosenRecipeTreeNode == null)
			{
				Console.WriteLine(@"chosenRecipeTreeNode == null");
				return;
			}

			chosenRecipeTreeNode.SetAndSaveRecipeIndex(recipeIndexChoose.SelectedIndex);
			RecipeContent.Text = chosenRecipeTreeNode.recipe.ToString();
			defaultRecipeToggle.Checked = Data.save.GetDefaultIndex(chosenRecipeTreeNode.itemName) == chosenRecipeTreeNode.recipeIndex;
		}

		private RecipeTreeNode chosenRecipeTreeNode = null;

		private void RecipeContent_Click(object sender, EventArgs e)
		{

		}

		private void buildingToggle_CheckedChanged(object sender, EventArgs e)
		{
			if (buildingToggle.Checked)
			{
				ResetRecipeList(r => r.IsBuilding());
			}
		}

		private void componentToggle_CheckedChanged(object sender, EventArgs e)
		{
			if (componentToggle.Checked)
			{
				ResetRecipeList(r => r.IsComponent());
			}
		}

		private void defaultRecipeToggle_CheckedChanged(object sender, EventArgs e)
		{
			Console.WriteLine("defaultRecipeToggle.Checked = " + defaultRecipeToggle.Checked);
			if (defaultRecipeToggle.Checked)
			{
				if (!chosenRecipeTreeNode.recipe.couldDefault)
				{
					MessageBox.Show("无法选择该默认配方，因为产生了配方循环");
				}
				else
				{
					Data.save.SetDefaultIndex(chosenRecipeTreeNode.itemName, chosenRecipeTreeNode.recipeIndex);
				}
			}

			defaultRecipeToggle.Checked = Data.save.GetDefaultIndex(chosenRecipeTreeNode.itemName) == chosenRecipeTreeNode.recipeIndex;
		}
	}
}
