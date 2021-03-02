using System;
using System.Windows.Forms;

namespace DysonRecipeWin
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();

			this.RecipeList.Items.Clear();

			foreach (var recipe in Data.recipes)
			{
				if (RecipeList.SelectedItem == null || string.IsNullOrEmpty(RecipeList.SelectedItem.ToString()))
				{
					RecipeList.SelectedItem = recipe.target.name;
				}
				this.RecipeList.Items.Add(recipe.target.name);
			}
			ResultTreeView.Nodes.Clear();

			RecipeContent.Text = "";
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
                num *= Data.GetRecipe(itemName, 0, 0).target.count;
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
					recipeIndexChoose.Items.Add(recipe.displayName);
				}
				recipeIndexChoose.SelectedIndex = chosenRecipeTreeNode.recipeIndex;

				RecipeContent.Text = chosenRecipeTreeNode.recipe.ToString();
			}
		}

        private void NumChoose_ValueChanged(object sender, EventArgs e)
        {
        }

		private void recipeIndexChoose_SelectedIndexChanged(object sender, EventArgs e)
		{
			Console.WriteLine("sender: " + sender + " e: " + e);
			if (recipeIndexChoose.SelectedIndex < 0)
			{
				Console.WriteLine("recipeIndexChoose.SelectedIndex < 0");
				return;
			}
			if (chosenRecipeTreeNode == null)
			{
				Console.WriteLine("chosenRecipeTreeNode == null");
				return;
			}

			chosenRecipeTreeNode.SetAndSaveRecipeIndex(recipeIndexChoose.SelectedIndex);
			RecipeContent.Text = chosenRecipeTreeNode.recipe.ToString();
		}

		private RecipeTreeNode chosenRecipeTreeNode = null;

		private void RecipeContent_Click(object sender, EventArgs e)
		{

		}
	}
}
