namespace MovieHelperBot
{
    partial class MainForm
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgReport = new System.Windows.Forms.DataGridView();
            this.ChatId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Command = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tokenTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgReport)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 550);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(624, 26);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Status
            // 
            this.Status.ForeColor = System.Drawing.Color.Red;
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(54, 20);
            this.Status.Text = "Offline";
            // 
            // dgReport
            // 
            this.dgReport.AllowUserToAddRows = false;
            this.dgReport.AllowUserToDeleteRows = false;
            this.dgReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChatId,
            this.Username,
            this.Command,
            this.Date});
            this.dgReport.Location = new System.Drawing.Point(0, 12);
            this.dgReport.Name = "dgReport";
            this.dgReport.ReadOnly = true;
            this.dgReport.RowHeadersWidth = 51;
            this.dgReport.RowTemplate.Height = 24;
            this.dgReport.Size = new System.Drawing.Size(624, 365);
            this.dgReport.TabIndex = 1;
            // 
            // ChatId
            // 
            this.ChatId.HeaderText = "ChatId";
            this.ChatId.MinimumWidth = 6;
            this.ChatId.Name = "ChatId";
            this.ChatId.ReadOnly = true;
            // 
            // Username
            // 
            this.Username.HeaderText = "Username";
            this.Username.MinimumWidth = 6;
            this.Username.Name = "Username";
            this.Username.ReadOnly = true;
            // 
            // Command
            // 
            this.Command.HeaderText = "Command";
            this.Command.MinimumWidth = 6;
            this.Command.Name = "Command";
            this.Command.ReadOnly = true;
            // 
            // Date
            // 
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 6;
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            // 
            // tokenTextBox
            // 
            this.tokenTextBox.Location = new System.Drawing.Point(108, 419);
            this.tokenTextBox.Name = "tokenTextBox";
            this.tokenTextBox.Size = new System.Drawing.Size(418, 22);
            this.tokenTextBox.TabIndex = 2;
            this.tokenTextBox.Text = "1654786506:AAGs2SiIOoR0ScuO6U9K4PBW46fJy7zLV_A";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 424);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Token";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 576);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tokenTextBox);
            this.Controls.Add(this.dgReport);
            this.Controls.Add(this.statusStrip1);
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Movie Helper Bot";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel Status;
        private System.Windows.Forms.DataGridView dgReport;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChatId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Username;
        private System.Windows.Forms.DataGridViewTextBoxColumn Command;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.TextBox tokenTextBox;
        private System.Windows.Forms.Label label1;
    }
}

