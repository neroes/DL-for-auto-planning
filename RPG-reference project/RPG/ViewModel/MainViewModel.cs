using RPGModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace RPG.ViewModel
{

    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // Manages undo/redo.
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();

        public Map map { get; set; }
        public PlayerEntity player { get; set; }
        public ObservableCollection<MonsterEntity> monsters { get; set; }
        public ObservableCollection<ItemEntity> items { get; set; }
        public ObservableCollection<TriggerEntity> triggers { get; set; }
        public TriggerEntity nextLevelTrigger { get; set; }

        //The monster that the ui has focus on
        public MonsterEntity monsterFocus { get; set; }

        public String statusMessage { get; set; }

        // Commands bound to UI
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public static ICommand StartGameCommand  { get; private set; }
        public ICommand SaveGameCommand { get; private set; }

        public ICommand ArrowKeyActionCommand { get; private set; }


        public int level = 0;

        public int ItemsUsed = 0;

        public MainViewModel()
        {
            //Initialise monsters
            monsters = new ObservableCollection<MonsterEntity>();
            //Initialise items
            items = new ObservableCollection<ItemEntity>();
            //Initialise trigger
            triggers = new ObservableCollection<TriggerEntity>();
            
            // Commands that the UI can call is bound to the methods they call
            UndoCommand = new RelayCommand(Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(Redo, undoRedoController.CanRedo);

            StartGameCommand = new RelayCommand<int>(LoadLevel, SaveExists);
            SaveGameCommand = new RelayCommand<int>(Save);

            ArrowKeyActionCommand = new RelayCommand<Entity.Direction>(ArrowKeyAction);

            LoadLevel(level);

            //Initialise save method last
            SaveGame.initialise(this);
        }

        public void ArrowKeyAction(Entity.Direction direction)
        {
            System.Drawing.Point point = Entity.getCoordinatesFromDirection(player, direction);

            //Add the action to the undo/redo stack and execute it
            if (map.isWalkable(point.X, point.Y))
            {
                undoRedoController.AddAndExecute(new ArrowKeyActionCommand(this, direction));
                Update();
            } 
        }

        public void Undo()
        {
            undoRedoController.Undo();
            NotifyPropertyChanged("");
        }
        public void Redo()
        {
            undoRedoController.Redo();
            Update();
        }
        public void Save(int pos)
        {
            //Set the savegame position to use
            SaveGame.setSavePosition(pos);
            //Set up a backgroundworker
            BackgroundWorker saveWorker = new BackgroundWorker();
            saveWorker.WorkerReportsProgress = false;
            saveWorker.WorkerSupportsCancellation = false;
            saveWorker.DoWork += new DoWorkEventHandler(saveWorker_DoWork);
            saveWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(saveWorker_DoWork_RunWorkerCompleted);

            if (!saveWorker.IsBusy)
            {
                saveWorker.RunWorkerAsync();
                statusMessage = "Saving...";
                NotifyPropertyChanged("statusMessage");
            }
        }

        // This event handler takes care of saving the game in a new thread 
        private void saveWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            SaveGame.Execute();
        }

        // This event handler deals with the results of the background operation. 
        private void saveWorker_DoWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                statusMessage = "Error: " + e.Error.Message;
            }
            else
            {
                statusMessage = "Saved!";
            }
            NotifyPropertyChanged("statusMessage");
        }

        //Update things that need to be updated after an action has taken place, including AI actions
        public void Update()
        {
            if (player.isDead())
            {
                LoadLevel(Int32.MaxValue);
            }
            for (int i = 0; i < monsters.Count; i++)
            {
                //Remove dead monsters
                if (monsters[i].isDead())
                    monsters.RemoveAt(i);
                //Update remaining
                else
                    monsters[i].update(map, monsters, player);
            }
            for (int i = 0; i < items.Count; i++ )// removing used items
            {
                if (items[i].used) { items.RemoveAt(i); }
            }
            statusMessage = "";
            NotifyPropertyChanged("");
            player.position.Changed += position_Changed;
        }

        public void LoadLevel(int load)
        {
            //Clear everything
            undoRedoController.Clear();
            

            Initialise col = new Initialise(load, LoadNextLevel);
            Collections collection = col.Ini();

            map = collection.map;
            monsters = collection.monsters;
            items = collection.items;
            player = collection.player;
            nextLevelTrigger = collection.nextLevelTrigger;
            triggers = collection.triggers;
            level = collection.level;

            NotifyPropertyChanged("");
        }

        //Load the next level
        public void LoadNextLevel()
        {
            level++;
            LoadLevel(level);
        }

        public bool SaveExists(int save)
        {
            if (save >= 0) { return true; }
            else
            {
                string filename = ("savegames\\save " + -save + ".xml");
                return File.Exists(filename);
            }
        }
        public GeometryCollection view
        {
            get { if (FogofWar()) { return new GeometryCollection { new EllipseGeometry(player.Center, 100, 100), new RectangleGeometry(new Rect(0, 0, 800, 800)) }; } else return null; }
        }
        public bool FogofWar()
        {
            string filename = ("Data\\level " + level + ".xml");
            return (level > 1 && File.Exists(filename));
        }


        public new event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        void position_Changed(object sender, EventArgs e)
        {
            NotifyPropertyChanged("");
        }
    }
}