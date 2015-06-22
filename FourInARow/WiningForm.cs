using System;
using System.Windows.Forms;

namespace FourInARow
{
    public partial class WiningForm : Form
    {
        public delegate void restartGame();
        public delegate void mainMenu();
        public event restartGame newGame;
        public event mainMenu weNeedGoToMainMenu;
        public WiningForm()
        {
            InitializeComponent();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            newGame();
        }

        private void mainMenuButton_Click(object sender, EventArgs e)
        {
            weNeedGoToMainMenu();
        }
    }
}
