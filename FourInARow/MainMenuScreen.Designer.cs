namespace FourInARow
{
    partial class MainMenuScreen
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
            this.newGameButton = new System.Windows.Forms.Button();
            this.bestTimeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // newGameButton
            // 
            this.newGameButton.Location = new System.Drawing.Point(105, 60);
            this.newGameButton.Name = "newGameButton";
            this.newGameButton.Size = new System.Drawing.Size(75, 23);
            this.newGameButton.TabIndex = 0;
            this.newGameButton.Text = "New game";
            this.newGameButton.UseVisualStyleBackColor = true;
            this.newGameButton.Click += new System.EventHandler(this.newGameButton_Click);
            // 
            // bestTimeButton
            // 
            this.bestTimeButton.Location = new System.Drawing.Point(105, 101);
            this.bestTimeButton.Name = "bestTimeButton";
            this.bestTimeButton.Size = new System.Drawing.Size(75, 23);
            this.bestTimeButton.TabIndex = 1;
            this.bestTimeButton.Text = "Best Time";
            this.bestTimeButton.UseVisualStyleBackColor = true;
            this.bestTimeButton.Click += new System.EventHandler(this.resultsButton_Click);
            // 
            // MainMenuScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 227);
            this.Controls.Add(this.bestTimeButton);
            this.Controls.Add(this.newGameButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainMenuScreen";
            this.Text = "Four in a Row";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button newGameButton;
        private System.Windows.Forms.Button bestTimeButton;
    }
}