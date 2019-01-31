using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoNotes
{
    /// <summary>
    /// Class to hold all info for a File table from the database
    /// </summary>
    public class FileElement
    {
        string id;//Primary key for the file
        string gameID;//What game the flie is attached to this file
        string title;//the title of the file
        string description;//the description of the file
        string filename;//the filename of clip attched to this file

        public FileElement(string id, string gameID, string title, string description, string filename)
        {
            this.id = id;
            this.gameID = gameID;
            this.title = title;
            this.description = description;
            this.filename = filename;
        }

        /// <summary>
        /// Getters and Setters for all variables in this class
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string GameID
        {
            get { return gameID; }
            set { gameID = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }
    }
}
