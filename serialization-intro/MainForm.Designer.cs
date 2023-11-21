namespace serialization_intro
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            listBox = new ListBox();
            labelName = new Label();
            textBoxDescription = new TextBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            buttonFromJson = new Button();
            buttonToJson = new Button();
            buttonFromCsv = new Button();
            buttonToCsv = new Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 8;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(listBox, 0, 0);
            tableLayoutPanel1.Controls.Add(labelName, 2, 0);
            tableLayoutPanel1.Controls.Add(textBoxDescription, 2, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 8);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 9;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // listBox
            // 
            listBox.Dock = DockStyle.Fill;
            listBox.FormattingEnabled = true;
            listBox.ItemHeight = 25;
            listBox.Location = new Point(11, 11);
            listBox.Margin = new Padding(10);
            listBox.Name = "listBox";
            tableLayoutPanel1.SetRowSpan(listBox, 9);
            listBox.Size = new Size(230, 428);
            listBox.TabIndex = 0;
            // 
            // labelName
            // 
            labelName.BackColor = Color.FromArgb(0, 102, 203);
            tableLayoutPanel1.SetColumnSpan(labelName, 5);
            labelName.Dock = DockStyle.Fill;
            labelName.ForeColor = Color.White;
            labelName.Location = new Point(296, 1);
            labelName.Name = "labelName";
            labelName.Size = new Size(448, 40);
            labelName.TabIndex = 1;
            labelName.Text = "The Rock";
            labelName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBoxDescription
            // 
            textBoxDescription.BackColor = Color.FromArgb(0, 0, 192);
            tableLayoutPanel1.SetColumnSpan(textBoxDescription, 5);
            textBoxDescription.Dock = DockStyle.Fill;
            textBoxDescription.ForeColor = Color.White;
            textBoxDescription.Location = new Point(296, 45);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.ReadOnly = true;
            textBoxDescription.Size = new Size(448, 74);
            textBoxDescription.TabIndex = 2;
            textBoxDescription.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel1.SetColumnSpan(tableLayoutPanel2, 7);
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(buttonFromJson, 0, 0);
            tableLayoutPanel2.Controls.Add(buttonToJson, 1, 0);
            tableLayoutPanel2.Controls.Add(buttonFromCsv, 2, 0);
            tableLayoutPanel2.Controls.Add(buttonToCsv, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(255, 372);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(541, 74);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // buttonFromJson
            // 
            buttonFromJson.Dock = DockStyle.Fill;
            buttonFromJson.Location = new Point(3, 3);
            buttonFromJson.Name = "buttonFromJson";
            buttonFromJson.Size = new Size(124, 68);
            buttonFromJson.TabIndex = 0;
            buttonFromJson.Text = "From Json";
            buttonFromJson.UseVisualStyleBackColor = true;
            // 
            // buttonToJson
            // 
            buttonToJson.Dock = DockStyle.Fill;
            buttonToJson.Location = new Point(133, 3);
            buttonToJson.Name = "buttonToJson";
            buttonToJson.Size = new Size(124, 68);
            buttonToJson.TabIndex = 0;
            buttonToJson.Text = "To Json";
            buttonToJson.UseVisualStyleBackColor = true;
            // 
            // buttonFromCsv
            // 
            buttonFromCsv.Dock = DockStyle.Fill;
            buttonFromCsv.Location = new Point(263, 3);
            buttonFromCsv.Name = "buttonFromCsv";
            buttonFromCsv.Size = new Size(124, 68);
            buttonFromCsv.TabIndex = 0;
            buttonFromCsv.Text = "From Csv";
            buttonFromCsv.UseVisualStyleBackColor = true;
            // 
            // buttonToCsv
            // 
            buttonToCsv.Dock = DockStyle.Fill;
            buttonToCsv.Location = new Point(393, 3);
            buttonToCsv.Name = "buttonToCsv";
            buttonToCsv.Size = new Size(124, 68);
            buttonToCsv.TabIndex = 0;
            buttonToCsv.Text = "To Csv";
            buttonToCsv.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 0, 192);
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Roster";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private ListBox listBox;
        private Label labelName;
        private TextBox textBoxDescription;
        private TableLayoutPanel tableLayoutPanel2;
        private Button buttonToCsv;
        private Button buttonFromCsv;
        private Button buttonToJson;
        private Button buttonFromJson;
    }
}
