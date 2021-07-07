using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Helpers.UndoRedo
{
    public enum BitmapChanges 
    {
        Inverted,
        SetGray,
        Cropped,
        Resized,
        Dithered,
        TransparentFilled,
        RotatedLeft,
        RotatedRight,
        FlippedHorizontal,
        FlippedVirtical
    }

    public class BitmapUndo : IDisposable, IUndoable
    {
        public delegate void UndoEvent(BitmapChanges change);
        public static event UndoEvent UndoHappened;

        public delegate void RedoEvent(BitmapChanges change);
        public static event RedoEvent RedoHappened;

        public delegate void UpdateReferencesEvent();
        public static event UpdateReferencesEvent UpdateReferences;

        public int UndoCount
        {
            get { return undos.Count; }
        }

        public int RedoCount
        {
            get { return redos.Count; }
        }

        public Bitmap CurrentBitmap;

        private Stack<BitmapChanges> undos;
        private Stack<BitmapChanges> redos;
        private Stack<Bitmap> bitmapUndoHistoryData;
        private Stack<Bitmap> bitmapRedoHistoryData;

        public BitmapUndo()
        {
            undos = new Stack<BitmapChanges>();
            redos = new Stack<BitmapChanges>();
            bitmapUndoHistoryData = new Stack<Bitmap>();
            bitmapRedoHistoryData = new Stack<Bitmap>();
        }

        public BitmapUndo(Bitmap bmp)
        {
            undos = new Stack<BitmapChanges>();
            redos = new Stack<BitmapChanges>();
            bitmapUndoHistoryData = new Stack<Bitmap>();
            bitmapRedoHistoryData = new Stack<Bitmap>();

            CurrentBitmap = bmp;
        }

        public void PrintUndos()
        {
            Console.Clear();
            Console.WriteLine("UNDOS [ ");
            foreach(BitmapChanges c in undos)
            {
                Console.Write($"{c} ");
            }
            Console.WriteLine("]");

            Console.WriteLine("REDOS [ ");
            foreach (BitmapChanges c in redos)
            {
                Console.Write($"{c} ");
            }
            Console.WriteLine("]");
        }

        public void ReplaceBitmap(Bitmap bmp)
        {
            if (CurrentBitmap != null)
                CurrentBitmap.Dispose();

            CurrentBitmap = bmp;
        }

        public void UpdateBitmapReferance(Bitmap bmp)
        {
            CurrentBitmap = bmp;
        }

        public void TrackChange(BitmapChanges change)
        {
            ClearRedos();
            undos.Push(change);
            PrintUndos();
            switch (change)
            {
                // need to track history data
                case BitmapChanges.Cropped:
                case BitmapChanges.Dithered:
                case BitmapChanges.Resized:
                case BitmapChanges.SetGray:
                case BitmapChanges.TransparentFilled:
                    bitmapUndoHistoryData.Push(CurrentBitmap.CloneSafe());
                    break;

                // changes are easily undone and do not need to be kept in memory
                case BitmapChanges.Inverted:
                case BitmapChanges.RotatedLeft:
                case BitmapChanges.RotatedRight:
                case BitmapChanges.FlippedHorizontal:
                case BitmapChanges.FlippedVirtical:
                    break;
            }
        }

        public void ClearRedos()
        {
            redos.Clear();
            for (int i = 0; i < bitmapRedoHistoryData.Count; i++)
            {
                bitmapRedoHistoryData.Pop().Dispose();
            }
        }

        public void DisposeLastRedo()
        {
            if (undos.Count < 1)
                return;

            redos.Pop();

            if (bitmapRedoHistoryData.Count < 1)
                return;

            bitmapUndoHistoryData.Pop().Dispose();
        }

        public void Redo()
        {
            Console.WriteLine("redoing");
            if (redos.Count < 1)
                return;

            BitmapChanges change = redos.Pop();
            undos.Push(change);
            PrintUndos();
            Bitmap bmp;
            switch (change)
            {
                // need to track history data
                case BitmapChanges.Cropped:
                case BitmapChanges.Resized:
                    bmp = bitmapRedoHistoryData.Pop();
                    bitmapUndoHistoryData.Push(CurrentBitmap.CloneSafe());
                    ReplaceBitmap(bmp);
                    OnUpdateReferences();
                    break;
                case BitmapChanges.Dithered:
                case BitmapChanges.SetGray:
                case BitmapChanges.TransparentFilled:
                    bmp = bitmapRedoHistoryData.Pop();
                    bitmapUndoHistoryData.Push(bmp);
                    
                    if (ImageAnimator.CanAnimate(bmp) && change == BitmapChanges.SetGray)
                    {
                        Bitmap newBmp = ImageHelper.GrayscaleGif((Bitmap)CurrentBitmap);
                        ReplaceBitmap(newBmp);
                        OnUpdateReferences();
                        OnRedo(change);
                        return;
                    }

                    ImageHelper.UpdateBitmap(CurrentBitmap, bmp);
                    
                    break;

                // changes are easily undone and do not need to be kept in memory
                case BitmapChanges.Inverted:
                    if (ImageAnimator.CanAnimate(CurrentBitmap))
                    {
                        Bitmap newBmp = ImageHelper.InvertGif((Bitmap)CurrentBitmap);
                        ReplaceBitmap(newBmp);
                        OnUpdateReferences();
                        OnRedo(change);
                        return;
                    }

                    ImageHelper.InvertBitmapSafe(CurrentBitmap);
                    break;
                case BitmapChanges.RotatedLeft:
                    CurrentBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone); 
                    break;
                case BitmapChanges.RotatedRight:
                    CurrentBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case BitmapChanges.FlippedHorizontal:
                    CurrentBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case BitmapChanges.FlippedVirtical:
                    CurrentBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY); 
                    break;
            }
            OnRedo(change);
        }

        public void ClearUndos()
        {
            undos.Clear();
            for (int i = 0; i < bitmapUndoHistoryData.Count; i++)
            {
                bitmapUndoHistoryData.Pop().Dispose();
            }
        }

        public void DisposeLastUndo()
        {
            if (undos.Count < 1)
                return;

            undos.Pop();

            if (bitmapUndoHistoryData.Count < 1)
                return;

            bitmapUndoHistoryData.Pop().Dispose();
        }

        public void Undo()
        {
            Console.WriteLine("undoing");
            if (undos.Count < 1)
                return;

            BitmapChanges change = undos.Pop();
            redos.Push(change);
            PrintUndos();
            Bitmap bmp;
            switch (change)
            {
                // need to track history data
                case BitmapChanges.Cropped:
                case BitmapChanges.Resized:
                    bmp = bitmapUndoHistoryData.Pop();
                    bitmapRedoHistoryData.Push(CurrentBitmap.CloneSafe());
                    ReplaceBitmap(bmp);
                    OnUpdateReferences();
                    break;
                case BitmapChanges.Dithered:
                case BitmapChanges.SetGray:
                case BitmapChanges.TransparentFilled:
                    bmp = bitmapUndoHistoryData.Pop();
                    bitmapRedoHistoryData.Push(bmp);

                    if (ImageAnimator.CanAnimate(bmp) && change == BitmapChanges.SetGray)
                    {   // can't update a gif using pointers so we need to update the references
                        Bitmap newBmp = ImageHelper.GrayscaleGif((Bitmap)CurrentBitmap);
                        ReplaceBitmap(newBmp);
                        OnUpdateReferences();
                        OnUndo(change);
                        return;
                    }

                    ImageHelper.UpdateBitmap(CurrentBitmap, bmp);
                    
                    break;

                // changes are easily undone and do not need to be kept in memory
                case BitmapChanges.Inverted:
                    if (ImageAnimator.CanAnimate(CurrentBitmap))
                    {   // can't update a gif using pointers so we need to update the references
                        Bitmap newBmp = ImageHelper.InvertGif((Bitmap)CurrentBitmap);
                        ReplaceBitmap(newBmp);
                        OnUpdateReferences();
                        OnUndo(change);
                        return;
                    }

                    ImageHelper.InvertBitmapSafe(CurrentBitmap);
                    break;
                case BitmapChanges.RotatedLeft:
                    CurrentBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case BitmapChanges.RotatedRight:
                    CurrentBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case BitmapChanges.FlippedHorizontal:
                    CurrentBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case BitmapChanges.FlippedVirtical:
                    CurrentBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
            }
            OnUndo(change);
        }

        public void ClearHistory()
        {
            undos.Clear();
            redos.Clear();

            for(int i = 0; i < bitmapUndoHistoryData.Count; i++)
            {
                bitmapUndoHistoryData.Pop().Dispose();
            }

            for (int i = 0; i < bitmapRedoHistoryData.Count; i++)
            {
                bitmapRedoHistoryData.Pop().Dispose();
            }
        }

        public void Dispose()
        {
            ClearHistory();
            CurrentBitmap.Dispose();
            CurrentBitmap = null;
        }

        private void OnUndo(BitmapChanges change)
        {
            if (UndoHappened != null)
                UndoHappened(change);
        }

        private void OnRedo(BitmapChanges change)
        {
            if (RedoHappened != null)
                RedoHappened(change);
        }

        private void OnUpdateReferences()
        {
            if (UpdateReferences != null)
                UpdateReferences();
        }
    }
}
