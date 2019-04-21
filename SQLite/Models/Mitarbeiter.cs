using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace SQLite.Models
{
    public class Mitarbeiter : INotifyPropertyChanged
    {
        public Mitarbeiter(int ID_mitarbeiter, string vorname, string nachname, int ID_team)
        {
            ID_Mitarbeiter = ID_mitarbeiter;
            Vorname = vorname;
            Nachname = nachname;
            ID_Team = ID_team;
        }
        public int ID_Mitarbeiter { get; set; }
        private string prop_Vorname;
        public string Vorname
        {
            get { return prop_Vorname; }
            set
            {
                if (prop_Vorname != null)
                {
                    //TEST
                    using (SQLiteConnection connection = new SQLiteConnection(Properties.Settings.Default.connString))
                    {
                        connection.Open();
                        SQLiteCommand cmd = new SQLiteCommand("update Mitarbeiter set FirstName = :Vorname where ID_Mitarbeiter=:id", connection);
                        cmd.Parameters.Add("id", DbType.String).Value = ID_Mitarbeiter;
                        cmd.Parameters.Add("Vorname", DbType.String).Value = value;
                        cmd.ExecuteNonQuery();
                    }
                    //TEST
                }
                prop_Vorname = value;
                NotifyPropertyChanged();
            }
        }
        private string prop_Nachname;
        public string Nachname
        {
            get { return prop_Nachname; }
            set
            {
                if (prop_Nachname != null)
                {
                    //TEST
                    using (SQLiteConnection connection = new SQLiteConnection(Properties.Settings.Default.connString))
                    {
                        connection.Open();
                        SQLiteCommand cmd = new SQLiteCommand("update Mitarbeiter set LastName = :Nachname where ID_Mitarbeiter=:id", connection);
                        cmd.Parameters.Add("id", DbType.String).Value = ID_Mitarbeiter;
                        cmd.Parameters.Add("Nachname", DbType.String).Value = value;
                        cmd.ExecuteNonQuery();
                    }
                    //TEST
                }
                prop_Nachname = value;
                NotifyPropertyChanged();
            }
        }
        private int prop_ID_Team;
        public int ID_Team
        {
            get { return prop_ID_Team; }
            set
            {
                if (prop_ID_Team > 0)
                {
                    //TEST
                    using (SQLiteConnection connection = new SQLiteConnection(Properties.Settings.Default.connString))
                    {
                        connection.Open();
                        SQLiteCommand cmd = new SQLiteCommand("update Mitarbeiter set ID_Team = :ID_Team where ID_Mitarbeiter=:id", connection);
                        cmd.Parameters.Add("id", DbType.String).Value = ID_Mitarbeiter;
                        cmd.Parameters.Add("ID_Team", DbType.Int32).Value = value;
                        cmd.ExecuteNonQuery();
                    }
                    //TEST
                }
                prop_ID_Team = value;
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

        //private string _Nachname;
        //{
        //    get { return _Nachname; }
        //    set
        //    {
        //        _Nachname = value;
        //        OnPropertyChanged("Nachname");
        //    }
        //}

        //public void OnPropertyChanged(String propertyName)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}
        //public event PropertyChangedEventHandler PropertyChanged;
    }
}
