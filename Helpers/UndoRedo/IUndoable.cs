using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Helpers.UndoRedo
{
    public interface IUndoable
    {
        void Undo();

        void Redo();
    }
}
