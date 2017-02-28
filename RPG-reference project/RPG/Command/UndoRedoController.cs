using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPG.ViewModel
{
    // Holder styr på de kommandoer der er i undo/redo stacken. 
    // Dette er en singleton, så alle der benytter den bruger samme instans. Det er opnået med Singleton pattern der kræver statisk instans privat konstruktor og statisk GetInstance() metode.
    public class UndoRedoController
    {
        // Part of singleton pattern.
        private static UndoRedoController controller = new UndoRedoController();

        // Undo stack.
        private LinkedList<IUndoRedoCommand> undoStack = new LinkedList<IUndoRedoCommand>();
        // Redo stack.
        private LinkedList<IUndoRedoCommand> redoStack = new LinkedList<IUndoRedoCommand>();

        // Part of singleton pattern.
        private UndoRedoController() { }

        // Part of singleton pattern.
        public static UndoRedoController GetInstance() { return controller; }

        // Bruges til at tilføje commander.
        public void AddAndExecute(IUndoRedoCommand command){
            undoStack.AddFirst(command);
            if (undoStack.Count == 50)
                undoStack.RemoveLast();
            redoStack.Clear();
            command.Execute();
        }

        // Sørger for at undo kun kan kaldes når der er kommandoer i undo stacken.
        public bool CanUndo()
        {
            return undoStack.Any();
        }

        // Udfører undo hvis det kan lade sig gøre.
        public void Undo()
        {
            if (undoStack.Count() <= 0) throw new InvalidOperationException();
            IUndoRedoCommand command = undoStack.First();
            
            redoStack.AddFirst(command);
            command.UnExecute();

            if (undoStack.Any())
                undoStack.RemoveFirst();
        }

        // Sørger for at redo kun kan kaldes når der er kommandoer i redo stacken.
        public bool CanRedo()
        {
            return redoStack.Any();
        }

        // Udfører redo hvis det kan lade sig gøre.
        public void Redo()
        {
            if (redoStack.Count() <= 0) throw new InvalidOperationException();
            IUndoRedoCommand command = redoStack.First();
            undoStack.AddFirst(command);
            command.Execute();

            if (redoStack.Any())
                redoStack.RemoveFirst();
        }

        //Clear undo/redo stacks
        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }
    }
}
