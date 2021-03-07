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

        // 重置配方列表
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

        // 重置具体的配方选项
	    public void ResetRecipeIndexChoose()
	    {
	        noPostEvent = true;
	        if (chosenRecipeTreeNode == null)
	        {
	            recipeIndexChoose.Items.Clear();
	            recipeIndexChoose.Text = "";
                RecipeContent.Text = "";
	            defaultRecipeCheckBox.Checked = false;
            }
	        else
	        {
	            recipeIndexChoose.Items.Clear();
	            var recipes = Data.GetRecipes(chosenRecipeTreeNode.itemName);
	            foreach (var recipe in recipes)
	            {
	                recipeIndexChoose.Items.Add(recipe.GetDisplayName());
	            }
	            recipeIndexChoose.SelectedIndex = chosenRecipeTreeNode.recipeIndex;
	            RecipeContent.Text = chosenRecipeTreeNode.recipe.ToString();
	            defaultRecipeCheckBox.Checked = Data.save.GetDefaultIndex(chosenRecipeTreeNode.itemName) == chosenRecipeTreeNode.recipeIndex;
            }
	        noPostEvent = false;
        }

        // 重置具体的建筑选项
        public void ResetBuildingIndexChoose()
	    {
	        noPostEvent = true;
	        if (chosenRecipeTreeNode == null)
	        {
	            buildingIndexChoose.Items.Clear();
	            buildingIndexChoose.Text = "";
                defaultBuildingCheckBox.Checked = false;
	        }
	        else
	        {
	            buildingIndexChoose.Items.Clear();
	            var buildings = Data.GetEffectiveBuildings(chosenRecipeTreeNode.building);
	            foreach (var building in buildings)
	            {
	                buildingIndexChoose.Items.Add(building.GetDisplayName());
	            }
	            buildingIndexChoose.SelectedIndex = chosenRecipeTreeNode.buildingIndex;
	            defaultBuildingCheckBox.Checked = Data.save.GetBuildingDefaultIndex(chosenRecipeTreeNode.building) == chosenRecipeTreeNode.buildingIndex;
            }
	        noPostEvent = false;
        }

        private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (RecipeList.SelectedItem == null || string.IsNullOrEmpty(RecipeList.SelectedItem.ToString()))
			{
				MessageBox.Show("请选择配方");
				Console.WriteLine("请选择配方");
				return;
			}
			Console.WriteLine(RecipeList.SelectedItem + " " + NumChoose.Value);

			ResultTreeView.Nodes.Clear();

		    Number num = (int) NumChoose.Value;
		    var itemName = RecipeList.SelectedItem.ToString();
			var arr = RecipeTreeNode.Calc(itemName, num, deviceToggle.Checked).ToArray();
			ResultTreeView.Nodes.AddRange(arr);
			ResultTreeView.ExpandAll();

			foreach (var recipeTreeNode in arr)
			{
				FoldGatherNode(recipeTreeNode);
			}
		    chosenRecipeTreeNode = null;
		    ResetRecipeIndexChoose();
		    ResetBuildingIndexChoose();
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
				chosenRecipeTreeNode = (RecipeTreeNode) e.Node;
			    ResetRecipeIndexChoose();
                ResetBuildingIndexChoose();
			}
		}

        private void NumChoose_ValueChanged(object sender, EventArgs e)
        {
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

        #region 配方详情

        private void recipeIndexChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (noPostEvent)
                return;

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
            defaultRecipeCheckBox.Checked = Data.save.GetDefaultIndex(chosenRecipeTreeNode.itemName) == chosenRecipeTreeNode.recipeIndex;
            ResetBuildingIndexChoose();
        }

        private void defaultRecipeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (chosenRecipeTreeNode == null || chosenRecipeTreeNode.recipe == null)
            {
                defaultRecipeCheckBox.Checked = false;
                return;
            }

            Console.WriteLine("defaultRecipeCheckBox_CheckedChanged Checked = " + defaultRecipeCheckBox.Checked);
            if (defaultRecipeCheckBox.Checked)
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

            defaultRecipeCheckBox.Checked = Data.save.GetDefaultIndex(chosenRecipeTreeNode.itemName) == chosenRecipeTreeNode.recipeIndex;
        }

	    private void buildingIndexChoose_SelectedIndexChanged(object sender, EventArgs e)
	    {
            if (noPostEvent)
                return;

	        if (buildingIndexChoose.SelectedIndex < 0)
	        {
	            Console.WriteLine(@"recipeIndexChoose.SelectedIndex < 0");
	            return;
	        }
	        if (chosenRecipeTreeNode == null)
	        {
	            Console.WriteLine(@"chosenRecipeTreeNode == null");
	            return;
	        }

	        chosenRecipeTreeNode.SetAndSaveBuildingIndex(buildingIndexChoose.SelectedIndex);
	        defaultBuildingCheckBox.Checked = Data.save.GetBuildingDefaultIndex(chosenRecipeTreeNode.building) == chosenRecipeTreeNode.buildingIndex;
        }

        private void defaultBuildingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (chosenRecipeTreeNode == null || chosenRecipeTreeNode.recipe == null || string.IsNullOrEmpty(chosenRecipeTreeNode.building))
            {
                defaultBuildingCheckBox.Checked = false;
                return;
            }
            Console.WriteLine("defaultBuildingCheckBox_CheckedChanged Checked = " + defaultRecipeCheckBox.Checked);
            if (defaultBuildingCheckBox.Checked)
            {
                Data.save.SetBuildingDefaultIndex(chosenRecipeTreeNode.building, chosenRecipeTreeNode.buildingIndex);
            }
        }

	    private static bool noPostEvent = false;

        #endregion

    }
}
