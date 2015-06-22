using System;
using System.Windows.Forms;

namespace FourInARow
{
    public partial class MainMenuScreen : Form
    {
        public delegate void RunGameScreen();
        public delegate void RunBestResultScreen();

        public event RunGameScreen GoOn;
        public event RunBestResultScreen weNeedToGo;
        public MainMenuScreen()
        {
            InitializeComponent();
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            GoOn();
        }

        private void resultsButton_Click(object sender, EventArgs e)
        {
            weNeedToGo();
        }
    }
}
