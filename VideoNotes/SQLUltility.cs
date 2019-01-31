using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoNotes
{
    /// <summary>
    /// Static Global class to handle basic SQL Database functions
    /// </summary>
    static class SQLUltility
    {

        /// <summary>
        /// function that returns the requested Columns from the selected Table
        /// </summary>
        public static string SelectSQL(string table, string database, string[] columnArray)
        {
            string returnString = "";//make the returnString set it to blank to avoid null errors

            using (SQLiteConnection con = new SQLiteConnection(database))//open a connection to the requested database
            {
                con.Open();//open the connection
                      
                string stm = "select * from " + table ;//make the select command based on request table

                using (SQLiteCommand cmd = new SQLiteCommand(stm, con))//fire off the command
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())//load up the reader
                    {
                       
                        while (rdr.Read())
                        {

                            foreach(string column in columnArray)//for each requested column
                            {
                                returnString = returnString + "|" + rdr[column];//add it to the return string with a space
                            }
                            returnString = returnString + "_";//add a dash between items so I can easily parse through the string later
                        }
                    }
                }

                con.Close();//close connection
            }


            return returnString;//return the string
        }

        /// <summary>
        /// Function that returns the requested columns from the requested table 
        /// Where a certain condition is met
        /// </summary>
        public static string SelectWhereSQL(string table, string whereAt, string database, string[] columnArray)
        {
            string returnString = "";//make the returnString set it to blank to avoid null errors

            using (SQLiteConnection con = new SQLiteConnection(database))//open a connection to the requested database
            {
                con.Open();//open the connection

                string stm = "select * from " + table + " where " + whereAt;//make the select where command based on request table

                using (SQLiteCommand cmd = new SQLiteCommand(stm, con))//fire off the command
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())//load up the reader
                    {

                        while (rdr.Read())
                        {

                            foreach (string column in columnArray)//for each requested column
                            {
                                returnString = returnString + rdr[column];//add it to the return string with a space
                            }
                            returnString = returnString + "_";//add a dash between items so I can easily parse through the string later
                        }
                    }
                }

                con.Close();//close connection
            }


            return returnString;//return the string
        }

        /// <summary>
        /// Fucntion to add the passed columns to the passed table
        /// </summary>
        public static void addToTable(string table, string columns, string newColInfo, string database)
        {
            SQLiteConnection con = new SQLiteConnection(database);//open up a new connection
            con.Open();//open the connection
            //Set up the insert command with the passed info
            SQLiteCommand insertSQL = new SQLiteCommand("insert into " + table + " (" + columns + ") values (" + newColInfo + ");", con);
         
            try
            {
                insertSQL.ExecuteNonQuery();//Try to fire off the SQL command
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);//Throw out an error if something goes wrong
            }

            con.Close();//close the connection
        }

        /// <summary>
        /// Fucntion to update the column in a certain table to the passed data
        /// </summary>
        public static void updateTable(string table, string columns, string newInfo, string ID, string database)
        {
            SQLiteConnection con = new SQLiteConnection(database);//Open the connection
            con.Open();
            //build out the SQL command with the passed info
            SQLiteCommand insertSQL = new SQLiteCommand("update " + table + " set " + columns + "= '" + newInfo + "' where id = " + ID + ";", con);          
            try
            {
                insertSQL.ExecuteNonQuery();//Try to fire off the SQL command
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);//Throw out an error if something goes wrong
            }

            con.Close();//close the connection
        }

    }

   
}
