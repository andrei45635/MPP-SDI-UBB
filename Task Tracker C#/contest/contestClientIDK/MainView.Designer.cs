namespace contestClientIDK
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.tasks = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.filterTasksView = new System.Windows.Forms.DataGridView();
            this.tasksView1 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.registerTask2CB = new System.Windows.Forms.ComboBox();
            this.registerTask1CB = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.registerAgeTB = new System.Windows.Forms.TextBox();
            this.registerNameTB = new System.Windows.Forms.TextBox();
            this.taskTypeCB = new System.Windows.Forms.ComboBox();
            this.ageGroupCB = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tasks.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filterTasksView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tasksView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1121, 558);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Logout";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tasks
            // 
            this.tasks.Controls.Add(this.tabPage1);
            this.tasks.Controls.Add(this.tabPage2);
            this.tasks.Location = new System.Drawing.Point(-1, 1);
            this.tasks.Name = "tasks";
            this.tasks.SelectedIndex = 0;
            this.tasks.Size = new System.Drawing.Size(1051, 609);
            this.tasks.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.filterTasksView);
            this.tabPage1.Controls.Add(this.tasksView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1043, 583);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // filterTasksView
            // 
            this.filterTasksView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.filterTasksView.Location = new System.Drawing.Point(479, 0);
            this.filterTasksView.Name = "filterTasksView";
            this.filterTasksView.Size = new System.Drawing.Size(564, 583);
            this.filterTasksView.TabIndex = 1;
            // 
            // tasksView1
            // 
            this.tasksView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tasksView1.Location = new System.Drawing.Point(3, 0);
            this.tasksView1.Name = "tasksView1";
            this.tasksView1.Size = new System.Drawing.Size(470, 580);
            this.tasksView1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.registerTask2CB);
            this.tabPage2.Controls.Add(this.registerTask1CB);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.registerAgeTB);
            this.tabPage2.Controls.Add(this.registerNameTB);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1043, 583);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(717, 83);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Register";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(423, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Task 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(423, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Task 1";
            // 
            // registerTask2CB
            // 
            this.registerTask2CB.FormattingEnabled = true;
            this.registerTask2CB.Location = new System.Drawing.Point(426, 120);
            this.registerTask2CB.Name = "registerTask2CB";
            this.registerTask2CB.Size = new System.Drawing.Size(121, 21);
            this.registerTask2CB.TabIndex = 5;
            // 
            // registerTask1CB
            // 
            this.registerTask1CB.FormattingEnabled = true;
            this.registerTask1CB.Location = new System.Drawing.Point(426, 49);
            this.registerTask1CB.Name = "registerTask1CB";
            this.registerTask1CB.Size = new System.Drawing.Size(121, 21);
            this.registerTask1CB.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Age";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(111, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // registerAgeTB
            // 
            this.registerAgeTB.Location = new System.Drawing.Point(114, 120);
            this.registerAgeTB.Name = "registerAgeTB";
            this.registerAgeTB.Size = new System.Drawing.Size(157, 20);
            this.registerAgeTB.TabIndex = 1;
            // 
            // registerNameTB
            // 
            this.registerNameTB.Location = new System.Drawing.Point(114, 50);
            this.registerNameTB.Name = "registerNameTB";
            this.registerNameTB.Size = new System.Drawing.Size(157, 20);
            this.registerNameTB.TabIndex = 0;
            // 
            // taskTypeCB
            // 
            this.taskTypeCB.FormattingEnabled = true;
            this.taskTypeCB.Location = new System.Drawing.Point(1102, 174);
            this.taskTypeCB.Name = "taskTypeCB";
            this.taskTypeCB.Size = new System.Drawing.Size(121, 21);
            this.taskTypeCB.TabIndex = 2;
            // 
            // ageGroupCB
            // 
            this.ageGroupCB.FormattingEnabled = true;
            this.ageGroupCB.Location = new System.Drawing.Point(1102, 235);
            this.ageGroupCB.Name = "ageGroupCB";
            this.ageGroupCB.Size = new System.Drawing.Size(121, 21);
            this.ageGroupCB.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1127, 286);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Filter";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1247, 611);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ageGroupCB);
            this.Controls.Add(this.taskTypeCB);
            this.Controls.Add(this.tasks);
            this.Controls.Add(this.button1);
            this.Name = "MainView";
            this.Text = "MainView";
            this.Load += new System.EventHandler(this.MainView_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ContestWindowFormClosing);
            this.tasks.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.filterTasksView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tasksView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tasks;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView tasksView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView filterTasksView;
        private System.Windows.Forms.ComboBox taskTypeCB;
        private System.Windows.Forms.ComboBox ageGroupCB;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox registerTask2CB;
        private System.Windows.Forms.ComboBox registerTask1CB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox registerAgeTB;
        private System.Windows.Forms.TextBox registerNameTB;
    }
}