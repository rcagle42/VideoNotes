using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VideoNotes
{
    /// <summary>
    /// Interaction logic for AddNewGameMain.xaml
    /// </summary>
    public partial class AddNewGameMain : Window
    {
        List<GameElement> currentGameList;//current game list for reference
        string cs = "Data Source=db.sqlite;";//easy reference for the selected database
        MainWindow parentWindow;

        public AddNewGameMain(List<GameElement> currentGames, MainWindow parentWindow)
        {
            InitializeComponent();
            currentGameList = currentGames;
            this.parentWindow = parentWindow;
        }

        /// <summary>
        /// Event Listener for the Done Button the Add New Game page
        /// </summary>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            string newTitle;
            int lastID;
            
            Int32.TryParse(currentGameList[currentGameList.Count-1].ID, out lastID);//parse out the ID from the last game in the list
       
            int newID = lastID + 1;//Add one to the the last ID
            newTitle = "'"+newID+"'" + "," + "'" +NewGameTitle.Text +"'";//make the new game info string
      
            SQLUltility.addToTable("games", "id, title", newTitle, cs);//add the new game to the table

            parentWindow.updateGameList();//update the game list
            this.Close();
        }

        /// <summary>
        /// Event Listener for the Cancel Button the Add New Game page
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
