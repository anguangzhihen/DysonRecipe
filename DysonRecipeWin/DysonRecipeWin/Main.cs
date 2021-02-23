using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml.ConditionalFormatting;

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
			var need = Program.Calc(RecipeList.SelectedItem.ToString(), (int)NumChoose.Value);
			ResultTreeView.Nodes.Add(GetTreeNode(need));
		}

		TreeNode GetTreeNode(BuildingNeed need)
		{
			List<TreeNode> treeChilds = new List<TreeNode>();
			foreach (var child in need.childs)
			{
				treeChilds.Add(GetTreeNode(child));
			}
			var recipe = Program.nameToRecipes[need.itemName];
			Number effctive = Program.buildingEffective[recipe.building];
			string value = need.building + "(" + need.itemName + ") " + need.count + " " + recipe.ToSpeedString(effctive);
			TreeNode now = new TreeNode(value, treeChilds.ToArray());
			return now;
		}


		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{

		}
	}
}
