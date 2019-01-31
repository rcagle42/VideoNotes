using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;


namespace VideoNotes
{
    /// <summary>
    /// Interaction logic for the MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string cs = "Data Source=db.sqlite;";//chosen source for the database. easy way to reference it

        List<GameElement> gameList = new List<GameElement>();//holds the list of all games pulled from SQL database
        List<FileElement> fileList = new List<FileElement>();//holds the list of all files pulled from SQL database
        string ClipFolder = AppDomain.CurrentDomain.BaseDirectory + "Clips/";//folder where we look for clips. defaults to the clips folder in the debug
        bool isPlaying = false;//Simple flag to keep track of video status

        public MainWindow()
        {
            InitializeComponent();
            buildGameList();//builds the starting game list. Reduces calls to the database
            buildFileList();//builds the starting file list. Reduces calls to the database
            GameDropDown.SelectedIndex = 0;//default the index to 0 so the field doesn't start as blank
            ClipDropDown.SelectedIndex = 0;//default the index to 0 so the field doesn't start as blank
        }

        /// <summary>
        /// Event Listener for the Add Game Button on the main screen
        /// Opens a new window for the user to add a new game
        /// </summary>
        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {

            AddNewGameMain newWindow = new AddNewGameMain(gameList, this);//Make a new Add game Window. Passing the game list and itself
            newWindow.Show();//show the add game Window

        }

        /// <summary>
        /// Event Listener for the Add Clip Button on the main screen
        /// Opens a new window for the user to add a new file with clip info
        /// </summary>
        private void AddClip_Click(object sender, RoutedEventArgs e)
        {

            AddNewFileMain newWindow = new AddNewFileMain(fileList, gameList, this);//Makes a new add file window. Passing the current file list, game list and itself
            newWindow.Show();//show the add file window

        }

        /// <summary>
        /// Event Listener for the Update Button on the main screen
        /// Writes out whatever is written in the description box to the current selected item
        /// </summary>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndexCurrent;//variable to hold what file index was selected
            selectedIndexCurrent = ClipDropDown.SelectedIndex;//Set the index to the current selection
            if (ClipDropDown.SelectedIndex != -1)//Check to make sure it is not null
            {
                string text = ClipDropDown.SelectedItem.ToString();//convert the selected item to a string

                foreach (FileElement info in fileList)//Check each file in filelist
                {

                    if (info.Title.Equals(text))//If the list matches the selected title
                    {
                        SQLUltility.updateTable("files", "description", ClipDes.Text, info.ID, cs);//update the current selection with the new description
                        updateFileList();//update the file list to show the changes
                        ClipDropDown.SelectedIndex = selectedIndexCurrent;//reset the dropdown to the previous selected item
                        break;
                    }
                }

            }

        }

        /// <summary>
        /// Event Listener for the Play Button on the main screen
        /// Toggles the video between play and pause
        /// </summary>
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)//If the current flag is set to true
            {

                ClipPlayer.Pause();//pause the video
                isPlaying = false;
                PlayButton.Visibility = Visibility.Hidden;//Hide the play button graphic
                PauseButton.Visibility = Visibility.Visible;//show the pause button graphic
            }
            else
            {
                ClipPlayer.Play();//play the video
                isPlaying = true;
                ClipPlayer.Position = TimeSpan.FromSeconds(0);//Reset the clip
                PlayButton.Visibility = Visibility.Visible;//Show the play button graphic
                PauseButton.Visibility = Visibility.Hidden;//hide the pause button graphic
            }

        }

        /// <summary>
        /// Event Listener for the File Nav Button on the main screen
        /// Opens up a folder browser to allow the user to select where clips are stored 
        /// </summary>
        private void FileNavButton_Click(object sender, RoutedEventArgs e)
        {

            FolderBrowserDialog folderDlg = new FolderBrowserDialog();//open a new folder nav window
            System.Windows.Forms.DialogResult result = folderDlg.ShowDialog();//save the result out
      
            if (result == System.Windows.Forms.DialogResult.OK)//if the user hit ok on the ui
            {
                ClipFolder = folderDlg.SelectedPath;//set the video source to the new selected folder
            }


        }

        /// <summary>
        /// Event Listener that fires when the game drop down changes
        /// </summary>
        private void GameDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GameDropDown.SelectedIndex != -1)//as long as your not on a null selection
            {
                string text = GameDropDown.SelectedItem.ToString();//store the selected item to text
                string ID = Regex.Match(text, @"\d+").Value;//grab the ID out of the string

                ClipDropDown.Items.Clear();//clear all the currently added files in the cilp list

                foreach (FileElement info in fileList)//check all files
                {
                    if (info.GameID.Equals(ID))//if they are tied to this ID
                    {
                        ClipDropDown.Items.Add(info.Title);//add it to the list
                    }
                }

            }
        }

        /// <summary>
        /// Event Listener that fires when the file drop down changes
        /// </summary>
        private void ClipDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClipDropDown.SelectedIndex != -1)//as long as your not on a null selection
            {
                string text = ClipDropDown.SelectedItem.ToString();//store the selected item out to a string

                foreach (FileElement info in fileList)//check through all the files
                {

                    if (info.Title.Equals(text))//If you find one that matches
                    {
                        ClipDes.Text = info.Description;//set the main page description to the selected items description

                        ClipPlayer.Source = new Uri(ClipFolder + "/" + info.FileName, UriKind.RelativeOrAbsolute);//set the source of the media player to the selected item

                        ClipPlayer.Play();//play the video clip
                        ClipPlayer.ScrubbingEnabled = true;//allow scrubbing
                        ClipPlayer.Position = TimeSpan.FromSeconds(2);//move the video forward two seconds. This allows for a really basic thumbnail
                        ClipPlayer.Pause();//pause the video
                    }
                }
            }
        }


        /// <summary>
        /// Build game list function that is used to 
        /// grab all current games from the database and store them for use later
        /// </summary>
        private void buildGameList()
        {
            string[] gameColumns = { "id", "title" };//what Columns I am wanting from the table
            string[] gameInfo =
            SQLUltility.SelectSQL("games", cs, gameColumns).Split('_');//parsing the data from the SQL data base into a string array

            foreach (string info in gameInfo)//for every item returned by selectsql
            {
                if (info.Length > 0)//as long as it is not blank
                {
                    String[] splitInfo = info.Split('|');//Split the data up between the ID and Name
                    GameElement newElement = new GameElement(splitInfo[1], splitInfo[2]);//make a new element with the ID and name
                    gameList.Add(newElement);//add it to the game list
                }
            }

            foreach (GameElement info in gameList)//for every game loaded from the database
            {
                GameDropDown.Items.Add("ID: " + info.ID + " " + info.Title);//add it to the dropdown list   
            }
        }

        /// <summary>
        /// Build file list function that is used to 
        /// grab all current files from the database and store them for use later
        /// </summary>
        private void buildFileList()
        {
            string[] fileColumns = { "id", "game_id", "title", "description", "filename" };//what Columns I am wanting from the table
            string[] fileInfo =
            SQLUltility.SelectSQL("files", cs, fileColumns).Split('_');//getting the requested columns from the table

            foreach (string info in fileInfo)//for every item returned by selectsql
            {
                if (info.Length > 0)//as long as it is not blank
                {
                    String[] splitInfo = info.Split('|');//split the data from the SQL database up for easy parsing
                    FileElement newElement = new FileElement(splitInfo[1], splitInfo[2], splitInfo[3], splitInfo[4], splitInfo[5]);//make a new element with requested info
                    fileList.Add(newElement);//add it to filelist for later use
                }
            }

            //This section makes sure the dropdown list for the files is in line with the selected game on update
            if (GameDropDown.SelectedIndex != -1)//as long the drop down is not null
            {
                string text = GameDropDown.SelectedItem.ToString();//grab the selected item into text
                string ID = Regex.Match(text, @"\d+").Value;//Grab the ID out of the string

                ClipDropDown.Items.Clear();//clear the file dropdown list

                foreach (FileElement info in fileList)//check all the files
                {
                    if (info.GameID.Equals(ID))//if they match the current ID
                    {
                        ClipDropDown.Items.Add(info.Title);//add it to the drop down list
                    }
                }

            }

        }

        /// <summary>
        /// updateGameList is used to grab info about the
        /// games in the database and add them to the correct list
        /// </summary>
        public void updateGameList()
        {
            GameDropDown.Items.Clear();//clear the current game drop down
            gameList.Clear();//clear the current game list
            buildGameList();//build the new game list
            updateFileList();//update the file list to make sure they match
        }

        /// <summary>
        /// updateFileList is used to grab info about the
        /// files in the database and add them to the correct list
        /// </summary>
        public void updateFileList()
        {
            ClipDropDown.Items.Clear();//Clear the current file drop down
            fileList.Clear();//clear the current file list
            buildFileList();//build the new file list

        }
    }
}
