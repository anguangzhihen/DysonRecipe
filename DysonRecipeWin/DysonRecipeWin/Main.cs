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
                num *= Data.nameToRecipes[itemName].target.count;
            }
			var arr = RecipeTreeNode.Calc(itemName, num).ToArray();
			ResultTreeView.Nodes.AddRange(arr);
			ResultTreeView.ExpandAll();

			foreach (var recipeTreeNode in arr)
			{
				FoldGatherNode(recipeTreeNode);
			}
		}

		void FoldGatherNode(RecipeTreeNode node)
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
			Console.WriteLine(sender + " " + e.ToString());
		}

        private void NumChoose_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
