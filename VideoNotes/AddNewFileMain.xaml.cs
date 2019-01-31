using Microsoft.Win32;
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
    /// Interaction logic for AddNewFileMain.xaml
    /// </summary>
    public partial class AddNewFileMain : Window
    {
        List<FileElement> currentFileList;//current list of all files
        List<GameElement> currentGameList;//current list of all games
        string cs = "Data Source=db.sqlite;";//quick reference for the database
        MainWindow parentWindow;//The main window, used to update the lists
        string selectedFile;//Used to hold what video file they have selected

        public AddNewFileMain(List<FileElement> currentFiles, List<GameElement> currentGames, MainWindow parentWindow)
        {
            InitializeComponent();
            currentFileList = currentFiles;//set the lists to passed over values
            currentGameList = currentGames;

            foreach (GameElement info in currentGameList) {//go through all current games
                
                GameDropDown.Items.Add(info.Title);//add them to the drop down list on the Add File Page

                }
            this.parentWindow = parentWindow;
        }

        /// <summary>
        /// Event handle for the Done button on Add new File page
        /// </summary>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
            {
            string newTitle;
            int newID;
            newID = currentFileList.Count + 1;//set the new ID to the the current count + 1 to keep track

            if (selectedFile != null)//if they have selected a file 
            {
                
                //set up the new file info from the UI
                newTitle = "'" + newID + "'," + "'" + currentGameList[GameDropDown.SelectedIndex].ID + "'," + "'" + NewClipTitle.Text + "'," + "'" + NewClipDes.Text + "'," + "'" + selectedFile + "'";

                //Call the add to table funciton to add the new file to the table
                SQLUltility.addToTable("files", "id, game_id, title, description, filename", newTitle, cs);

                parentWindow.updateGameList();//update the gamelist 
                NoClip.Visibility = Visibility.Hidden;//make sure the "No Clip" error is hidden
                this.Close();
            }
            else
            {
                NoClip.Visibility = Visibility.Visible;//if no clip selected throw up the error
            }
        }

        /// <summary>
        /// Event handle for the Cancel button on Add new File page
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();//Close the page
        }


        /// <summary>
        /// Event handle for the Browse button on Add new File page
        /// </summary>
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
        
                //open up a file browser to select the clip
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                NewClipFile.Content = openFileDialog.SafeFileName;//set on screen UI to selected file
                selectedFile = openFileDialog.SafeFileName;//set the variable to what the user selected
                }
            
        }

    }
}
