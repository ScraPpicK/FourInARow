using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FourInARow
{
    public partial class BestTimeScreen : Form
    {
        List<Result> results;
        public delegate void closeAllScreens();
        public delegate void getBack();
        public event closeAllScreens thisIsTheEnd;
        public event getBack backToMainMenu;

        public BestTimeScreen()
        {
            InitializeComponent();
            this.FormClosed += closeAll;
        }

        /// <summary>
        /// Event that occurs when form is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeAll(object sender, FormClosedEventArgs e)
        {
            thisIsTheEnd();
        }

        public BestTimeScreen(List<Result> res)
        {
            results = res;
            InitializeComponent();
            this.FormClosed += closeAll;

            resultsDataGridView.ColumnCount = 2;
            resultsDataGridView.RowCount = 4;
            resultsDataGridView.Columns[1].Width = 150;

            if(results.Count != 0)
                for (int i = 0; i < results.Count; i++)
                {
                    resultsDataGridView[0, i].Value = results[i].player;
                    resultsDataGridView[1, i].Value = results[i].result;
                }
            else
                for (int i = 0; i < 4; i++)
                {
                    resultsDataGridView[0, i].Value = "-";
                    resultsDataGridView[1, i].Value = "-";
                }
        }

        private void BestTimeScreen_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            backToMainMenu();
        }
    }
}
