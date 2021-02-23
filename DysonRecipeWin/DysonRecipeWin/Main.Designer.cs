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
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("节点0");
			this.RecipeList = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.NumChoose = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.ResultTreeView = new System.Windows.Forms.TreeView();
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.NumChoose)).BeginInit();
			this.SuspendLayout();
			// 
			// RecipeList
			// 
			this.RecipeList.Font = new System.Drawing.Font("宋体", 12F);
			this.RecipeList.FormattingEnabled = true;
			this.RecipeList.ItemHeight = 16;
			this.RecipeList.Items.AddRange(new object[] {
            "21"});
			this.RecipeList.Location = new System.Drawing.Point(15, 44);
			this.RecipeList.Name = "RecipeList";
			this.RecipeList.Size = new System.Drawing.Size(115, 212);
			this.RecipeList.TabIndex = 0;
			this.RecipeList.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("宋体", 12F);
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "选择配方";
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("宋体", 12F);
			this.button1.Location = new System.Drawing.Point(300, 44);
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
			this.NumChoose.Location = new System.Drawing.Point(155, 44);
			this.NumChoose.Name = "NumChoose";
			this.NumChoose.Size = new System.Drawing.Size(120, 26);
			this.NumChoose.TabIndex = 3;
			this.NumChoose.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("宋体", 12F);
			this.label2.Location = new System.Drawing.Point(152, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "选择数量";
			// 
			// ResultTreeView
			// 
			this.ResultTreeView.Location = new System.Drawing.Point(423, 44);
			this.ResultTreeView.Name = "ResultTreeView";
			treeNode1.Name = "节点0";
			treeNode1.Text = "节点0";
			this.ResultTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
			this.ResultTreeView.Size = new System.Drawing.Size(672, 394);
			this.ResultTreeView.TabIndex = 5;
			this.ResultTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("宋体", 12F);
			this.label3.Location = new System.Drawing.Point(420, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "结果";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1120, 453);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.ResultTreeView);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.NumChoose);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.RecipeList);
			this.Name = "Main";
			this.Text = "暗光的戴森球量化工具";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.NumChoose)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox RecipeList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.NumericUpDown NumChoose;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TreeView ResultTreeView;
		private System.Windows.Forms.Label label3;
	}
}

