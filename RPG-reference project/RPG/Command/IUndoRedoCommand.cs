using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPG.ViewModel
{
    public interface IUndoRedoCommand
    {
        void Execute();
        void UnExecute();
    }
}
