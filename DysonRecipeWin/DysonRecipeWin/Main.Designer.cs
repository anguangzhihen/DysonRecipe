namespace DysonRecipeWin
{
	partial class Main
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("节点0");
			this.RecipeList = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.NumChoose = new System.Windows.Forms.NumericUpDown();
			this.ResultTreeView = new System.Windows.Forms.TreeView();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.deviceToggle = new System.Windows.Forms.RadioButton();
			this.recipeIndexChoose = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.RecipeContent = new System.Windows.Forms.Label();
			this.componentToggle = new System.Windows.Forms.RadioButton();
			this.buildingToggle = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.defaultRecipeToggle = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.NumChoose)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// RecipeList
			// 
			this.RecipeList.Font = new System.Drawing.Font("宋体", 12F);
			this.RecipeList.FormattingEnabled = true;
			this.RecipeList.ItemHeight = 16;
			this.RecipeList.Items.AddRange(new object[] {
            "21"});
			this.RecipeList.Location = new System.Drawing.Point(6, 43);
			this.RecipeList.Name = "RecipeList";
			this.RecipeList.Size = new System.Drawing.Size(148, 212);
			this.RecipeList.TabIndex = 0;
			this.RecipeList.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("宋体", 12F);
			this.label1.Location = new System.Drawing.Point(12, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "生产需求：";
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("宋体", 12F);
			this.button1.Location = new System.Drawing.Point(15, 444);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "生成";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// NumChoose
			// 
			this.NumChoose.Font = new System.Drawing.Font("宋体", 12F);
			this.NumChoose.Location = new System.Drawing.Point(6, 72);
			this.NumChoose.Name = "NumChoose";
			this.NumChoose.Size = new System.Drawing.Size(148, 26);
			this.NumChoose.TabIndex = 3;
			this.NumChoose.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.NumChoose.ValueChanged += new System.EventHandler(this.NumChoose_ValueChanged);
			// 
			// ResultTreeView
			// 
			this.ResultTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ResultTreeView.Font = new System.Drawing.Font("宋体", 10F);
			this.ResultTreeView.Location = new System.Drawing.Point(200, 44);
			this.ResultTreeView.Name = "ResultTreeView";
			treeNode3.Name = "节点0";
			treeNode3.Text = "节点0";
			this.ResultTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
			this.ResultTreeView.Size = new System.Drawing.Size(561, 394);
			this.ResultTreeView.TabIndex = 5;
			this.ResultTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("宋体", 12F);
			this.label3.Location = new System.Drawing.Point(197, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "结果：";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("宋体", 10F);
			this.label4.Location = new System.Drawing.Point(404, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(357, 14);
			this.label4.TabIndex = 7;
			this.label4.Text = "【格式】需求数 设备(配方) [ 输出效率 ← 输入效率 ]";
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Checked = true;
			this.radioButton1.Font = new System.Drawing.Font("宋体", 12F);
			this.radioButton1.Location = new System.Drawing.Point(6, 20);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(90, 20);
			this.radioButton1.TabIndex = 8;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "产品数量";
			this.radioButton1.UseVisualStyleBackColor = true;
			// 
			// deviceToggle
			// 
			this.deviceToggle.AutoSize = true;
			this.deviceToggle.Font = new System.Drawing.Font("宋体", 12F);
			this.deviceToggle.Location = new System.Drawing.Point(6, 46);
			this.deviceToggle.Name = "deviceToggle";
			this.deviceToggle.Size = new System.Drawing.Size(90, 20);
			this.deviceToggle.TabIndex = 9;
			this.deviceToggle.Text = "设备数量";
			this.deviceToggle.UseVisualStyleBackColor = true;
			// 
			// recipeIndexChoose
			// 
			this.recipeIndexChoose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.recipeIndexChoose.FormattingEnabled = true;
			this.recipeIndexChoose.Location = new System.Drawing.Point(289, 451);
			this.recipeIndexChoose.Name = "recipeIndexChoose";
			this.recipeIndexChoose.Size = new System.Drawing.Size(121, 20);
			this.recipeIndexChoose.TabIndex = 10;
			this.recipeIndexChoose.SelectedIndexChanged += new System.EventHandler(this.recipeIndexChoose_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("宋体", 12F);
			this.label2.Location = new System.Drawing.Point(197, 451);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.TabIndex = 11;
			this.label2.Text = "选择配方：";
			// 
			// RecipeContent
			// 
			this.RecipeContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.RecipeContent.AutoSize = true;
			this.RecipeContent.Font = new System.Drawing.Font("宋体", 10F);
			this.RecipeContent.Location = new System.Drawing.Point(286, 483);
			this.RecipeContent.Name = "RecipeContent";
			this.RecipeContent.Size = new System.Drawing.Size(392, 14);
			this.RecipeContent.TabIndex = 12;
			this.RecipeContent.Text = "星际物流运输船 1 = 钛合金 10 + 处理器 10 + 加力推进器 2";
			this.RecipeContent.Click += new System.EventHandler(this.RecipeContent_Click);
			// 
			// componentToggle
			// 
			this.componentToggle.AutoSize = true;
			this.componentToggle.Checked = true;
			this.componentToggle.Font = new System.Drawing.Font("宋体", 12F);
			this.componentToggle.Location = new System.Drawing.Point(6, 17);
			this.componentToggle.Name = "componentToggle";
			this.componentToggle.Size = new System.Drawing.Size(58, 20);
			this.componentToggle.TabIndex = 13;
			this.componentToggle.TabStop = true;
			this.componentToggle.Text = "组件";
			this.componentToggle.UseVisualStyleBackColor = true;
			this.componentToggle.CheckedChanged += new System.EventHandler(this.componentToggle_CheckedChanged);
			// 
			// buildingToggle
			// 
			this.buildingToggle.AutoSize = true;
			this.buildingToggle.Font = new System.Drawing.Font("宋体", 12F);
			this.buildingToggle.Location = new System.Drawing.Point(70, 17);
			this.buildingToggle.Name = "buildingToggle";
			this.buildingToggle.Size = new System.Drawing.Size(58, 20);
			this.buildingToggle.TabIndex = 14;
			this.buildingToggle.Text = "建筑";
			this.buildingToggle.UseVisualStyleBackColor = true;
			this.buildingToggle.CheckedChanged += new System.EventHandler(this.buildingToggle_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.buildingToggle);
			this.groupBox1.Controls.Add(this.componentToggle);
			this.groupBox1.Controls.Add(this.RecipeList);
			this.groupBox1.Location = new System.Drawing.Point(15, 35);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(160, 280);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.radioButton1);
			this.groupBox2.Controls.Add(this.deviceToggle);
			this.groupBox2.Controls.Add(this.NumChoose);
			this.groupBox2.Location = new System.Drawing.Point(15, 321);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(160, 117);
			this.groupBox2.TabIndex = 16;
			this.groupBox2.TabStop = false;
			// 
			// defaultRecipeToggle
			// 
			this.defaultRecipeToggle.AutoSize = true;
			this.defaultRecipeToggle.Location = new System.Drawing.Point(433, 453);
			this.defaultRecipeToggle.Name = "defaultRecipeToggle";
			this.defaultRecipeToggle.Size = new System.Drawing.Size(47, 16);
			this.defaultRecipeToggle.TabIndex = 17;
			this.defaultRecipeToggle.TabStop = true;
			this.defaultRecipeToggle.Text = "默认";
			this.defaultRecipeToggle.UseVisualStyleBackColor = true;
			this.defaultRecipeToggle.CheckedChanged += new System.EventHandler(this.defaultRecipeToggle_CheckedChanged);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(777, 531);
			this.Controls.Add(this.defaultRecipeToggle);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.RecipeContent);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.recipeIndexChoose);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.ResultTreeView);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Name = "Main";
			this.Text = "暗光的戴森球量化工具";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.NumChoose)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox RecipeList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.NumericUpDown NumChoose;
		private System.Windows.Forms.TreeView ResultTreeView;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton deviceToggle;
		private System.Windows.Forms.ComboBox recipeIndexChoose;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label RecipeContent;
		private System.Windows.Forms.RadioButton componentToggle;
		private System.Windows.Forms.RadioButton buildingToggle;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton defaultRecipeToggle;
	}
}

