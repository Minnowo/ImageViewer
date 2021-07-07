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
        }

        public BitmapUndo(Bitmap bmp)
        {
            undos = new Stack<BitmapChanges>();
            redos = new Stack<BitmapChanges>();
            bitmapUndoHistoryData = new Stack<Bitmap>();
            bitmapRedoHistoryData = new Stack<Bitmap>();

            CurrentBitmap = bmp;
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
            redos.Clear();
            undos.Push(change);

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

        public void Redo()
        {
            if (redos.Count < 1)
                return;

            BitmapChanges change = redos.Pop();
            undos.Push(change);

            switch (change)
            {
                // need to track history data
                case BitmapChanges.Cropped:
                case BitmapChanges.Resized:
                    break;
                case BitmapChanges.Dithered:
                case BitmapChanges.SetGray:
                case BitmapChanges.TransparentFilled:
                    Bitmap bmp = bitmapRedoHistoryData.Pop();
                    bitmapUndoHistoryData.Push(bmp);
                    
                    if (ImageAnimator.CanAnimate(bmp) && change == BitmapChanges.SetGray)
                    {
                        Bitmap newBmp = ImageHelper.GrayscaleGif((Bitmap)CurrentBitmap);
                        ReplaceBitmap(newBmp);
                        OnUpdateReferences();
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
                    CurrentBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
                case BitmapChanges.FlippedVirtical:
                    CurrentBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX); 
                    break;
            }
            OnRedo(change);
        }

        public void DisposeLastUndo()
        {
            if (undos.Count < 1)
                return;

            BitmapChanges change = undos.Pop();

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

            switch (change)
            {
                // need to track history data
                case BitmapChanges.Cropped:
                case BitmapChanges.Resized:
                    break;
                case BitmapChanges.Dithered:
                case BitmapChanges.SetGray:
                case BitmapChanges.TransparentFilled:
                    Bitmap bmp = bitmapUndoHistoryData.Pop();
                    bitmapRedoHistoryData.Push(bmp);

                    if (ImageAnimator.CanAnimate(bmp) && change == BitmapChanges.SetGray)
                    {
                        Bitmap newBmp = ImageHelper.GrayscaleGif((Bitmap)CurrentBitmap);
                        ReplaceBitmap(newBmp);
                        OnUpdateReferences();
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
        }

        public void Dispose()
        {
            ClearHistory();
            CurrentBitmap.Dispose();
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
