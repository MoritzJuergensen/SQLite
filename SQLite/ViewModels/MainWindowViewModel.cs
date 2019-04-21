using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SQLite;
using SQLite.Models;
using System.Windows.Input;
using System.Data;
using System.Linq;
using System.Windows;
using System.Reflection;

namespace SQLite.ViewModels
{

    class MainWindowViewModel : INotifyPropertyChanged
    {
        public Mitarbeiter MySelectedMitarbeiter { get; set; }
        public Team MySelectedTeam { get; set; }
        public ObservableCollection<Mitarbeiter> OC_Mitarbeiter { get; set; }
        public ObservableCollection<Team> OC_Team { get; set; }
        public MainWindowViewModel()
        {
            _canExecute = true;
            OC_Team = new ObservableCollection<Team>();
            OC_Mitarbeiter = new ObservableCollection<Mitarbeiter>();
            FetchData();
            OC_Mitarbeiter.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler
                (CollectionChangedMethod);
            OC_Team.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler
                (CollectionChangedMethod);
        }

        #region ICommands
        private ICommand _clickCommand;
        private ICommand _clickCommand2;
        private ICommand _clickCommand3;
        public ICommand Click_NewRow
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(param => CreateNewRow(param), CanExecuteAttachmentChecked()));
            }
        }
        private void CreateNewRow(object param)
        {
            string Parameter = (string) param;
            if (Parameter == "Mitarbeiter")
            {
                OC_Mitarbeiter.Add(new Mitarbeiter(99999, "?", "?", 1));
            }
            else if (Parameter == "Team")
            {
                OC_Team.Add(new Team(99999, "?"));
            }
            else
            {
                MessageBox.Show("falscher Parameter");
            }
        }
        public ICommand Click_Information
        {
            get
            {
                return _clickCommand2 ?? (_clickCommand2 = new CommandHandler(param => DisplayInformation(param), CanExecuteAttachmentChecked()));
            }
        }
        private void DisplayInformation(object param)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            MessageBox.Show("Build: " + version.ToString());
        }

        public ICommand DeleteEntry
        {
            get
            {
                return _clickCommand3 ?? (_clickCommand2 = new CommandHandler(param => DeleteEntryVoid(param), CanExecuteAttachmentChecked()));
            }
        }

        private void DeleteEntryVoid(object param)
        {
            string Parameter = (string)param;
            if (Parameter == "Mitarbeiter")
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(Properties.Settings.Default.connString))
                    {
                        connection.Open();
                        SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Mitarbeiter WHERE ID_Mitarbeiter= '" + MySelectedMitarbeiter.ID_Mitarbeiter + "'", connection);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                OC_Mitarbeiter.Remove(OC_Mitarbeiter.Where(i => i.ID_Mitarbeiter == MySelectedMitarbeiter.ID_Mitarbeiter).Single());
            }
            else if (Parameter == "Team")
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(Properties.Settings.Default.connString))
                    {
                        connection.Open();
                        SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Teams WHERE ID_Team= '" + MySelectedTeam.ID_Team + "'", connection);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                OC_Team.Remove(OC_Team.Where(i => i.ID_Team == MySelectedTeam.ID_Team).Single());
            }
            else
            {
                MessageBox.Show("falscher Parameter");
            }
        }

        private bool CanExecuteAttachmentChecked()
        {
            return true;
        }

#endregion
        private bool _canExecute;


        public event PropertyChangedEventHandler PropertyChanged;

        void FetchData()
        {
            MessageBox.Show(Properties.Settings.Default.connString);
            using (SQLiteConnection connection = new SQLiteConnection(Properties.Settings.Default.connString))
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT ID_Mitarbeiter, FirstName, LastName, ID_Team FROM Mitarbeiter", connection);
                SQLiteCommand cmd_Team = new SQLiteCommand("SELECT * FROM Teams", connection);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                { 
                    while (reader.Read())
                    {
                        int ID_Mitarbeiter = reader.GetInt32(0);
                        string Vorname = reader.GetString(1);
                        string Nachname = reader.GetString(2);
                        int ID_Team = reader.GetInt32(3);
                        OC_Mitarbeiter.Add(new Mitarbeiter(ID_Mitarbeiter, Vorname, Nachname, ID_Team));
                    }
                }
                using (SQLiteDataReader reader = cmd_Team.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ID_Team = reader.GetInt32(0);
                        string Name = reader.GetString(1);
                        OC_Team.Add(new Team(ID_Team, Name));
                    }
                }
            }
        }

        private void Test_SelectionChanged()
        {
        }

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            //different kind of changes that may have occurred in collection
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (sender.ToString().Contains("Mitarbeiter"))
                { 
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(Properties.Settings.Default.connString))
                    {
                        connection.Open();
                        SQLiteCommand cmd = new SQLiteCommand("INSERT INTO [Mitarbeiter] (FirstName, LastName, ID_Team) VALUES (@FirstName,@LastName,@ID_Team)", connection);
                        cmd.Parameters.Add("@FirstName", DbType.String).Value = "?";
                        cmd.Parameters.Add("@LastName", DbType.String).Value = "?";
                        cmd.Parameters.Add("@ID_Team", DbType.Int32).Value = "1";
                        cmd.ExecuteNonQuery();
                        SQLiteCommand Command = new SQLiteCommand(connection);
                        Command.CommandText = "select last_insert_rowid()";
                        Int64 LastRowID64 = (Int64)Command.ExecuteScalar();
                        int LastRowID = (int)LastRowID64;
                        var item = OC_Mitarbeiter.FirstOrDefault(i => i.ID_Mitarbeiter == 99999);
                        if (item != null)
                        {
                            item.ID_Mitarbeiter = LastRowID;
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                }
                else
                {
                    if (sender.ToString().Contains("Team"))
                    {
                        try
                        {
                            using (SQLiteConnection connection = new SQLiteConnection(Properties.Settings.Default.connString))
                            {
                                connection.Open();
                                SQLiteCommand cmd = new SQLiteCommand("INSERT INTO [Teams] (Name) VALUES (@Name)", connection);
                                cmd.Parameters.Add("@Name", DbType.String).Value = "?";
                                cmd.ExecuteNonQuery();
                                SQLiteCommand Command = new SQLiteCommand(connection);
                                Command.CommandText = "select last_insert_rowid()";
                                Int64 LastRowID64 = (Int64)Command.ExecuteScalar();
                                int LastRowID = (int)LastRowID64;
                                var item = OC_Team.FirstOrDefault(i => i.ID_Team == 99999);
                                if (item != null)
                                {
                                    item.ID_Team = LastRowID;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                        }
                    }
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
            }

        }
    }

    public class CommandHandler : ICommand
    {
        private Action<object> _action;
        private bool _canExecute;
        public CommandHandler(Action<object> action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
