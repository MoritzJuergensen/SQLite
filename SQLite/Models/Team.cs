using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace SQLite.Models
{
    public class Team : INotifyPropertyChanged
    {
        public Team(int ID_team, string name)
        {
            ID_Team = ID_team;
            Name = name;
        }
        public int ID_Team { get; set; }
        private string prop_Name;

        public string Name
        {
            get { return prop_Name; }
            set
            {
                if (prop_Name != null)
                {
                    //TEST
                    using (SQLiteConnection connection = new SQLiteConnection(Properties.Settings.Default.connString))
                    {
                        connection.Open();
                        SQLiteCommand cmd = new SQLiteCommand("update Teams set Name = :Name where ID_Team=:id", connection);
                        cmd.Parameters.Add("id", DbType.String).Value = ID_Team;
                        cmd.Parameters.Add("Name", DbType.String).Value = value;
                        cmd.ExecuteNonQuery();
                    }
                    //TEST
                }

                prop_Name = value;
                NotifyPropertyChanged();
            }

        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsSelected { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
