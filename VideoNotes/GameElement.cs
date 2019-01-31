using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoNotes
{

    /// <summary>
    /// Class to hold all info for a Game table from the database
    /// </summary>
    public class GameElement
    {

        string gameID;//The primary key and easy refernece
        string gameTitle;//Title of the game

        public GameElement(string id, string title)
        {
            gameID = id;
            gameTitle = title;
        }

        /// <summary>
        /// Getters and setters for all variables in this class
        /// </summary>
        public string ID
        {
            get { return gameID; }
            set { gameID = value; }
        }

        public string Title
        {
            get { return gameTitle; }
            set { gameTitle = value; }
        }
    }
}
