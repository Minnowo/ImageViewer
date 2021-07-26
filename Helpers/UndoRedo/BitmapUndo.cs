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

        /// <summary>
        /// Dispose of the CurrentBitmap and replace it with the given bitmap.
        /// </summary>
        /// <param name="bmp">The new bitmap.</param>
        public void ReplaceBitmap(Bitmap bmp)
        {
            if (CurrentBitmap != null)
                CurrentBitmap.Dispose();

            CurrentBitmap = bmp;
        }

        /// <summary>
        /// Sets the CurrentBitmap and does not dispose of the last bitmap.
        /// </summary>
        /// <param name="bmp">The new bitmap.</param>
        public void UpdateBitmapReferance(Bitmap bmp)
        {
            CurrentBitmap = bmp;
        }

        /// <summary>
        /// Tracks a change done to the bitmap. Depending on the change the CurrentBitmap will be copied and saved in the history.
        /// <para>This should be called BEFORE the change is applied to the bitmap. If the change has been tracked but not be applied the DisposeLastUndo or DisposeLastRedo should be called.</para>
        /// </summary>
        /// <param name="change">The change that is going to occure to the bitmap.</param>
        public void TrackChange(BitmapChanges change)
        {
            ClearRedos();
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

        /// <summary>
        /// Removes all history of Redos and disposes any bitmap history used for redos.
        /// </summary>
        public void ClearRedos()
        {
            redos.Clear();
            for (int i = 0; i < bitmapRedoHistoryData.Count; i++)
            {
                bitmapRedoHistoryData.Pop().Dispose();
            }
        }

        /// <summary>
        /// Removes the last redo and disposes of any data with it.
        /// </summary>
        public void DisposeLastRedo()
        {
            if (redos.Count < 1)
                return;

            BitmapChanges change = redos.Pop();

            // if the change being removed had any bitmap data stored dispose it
            switch (change)
            {
                case BitmapChanges.Cropped:
                case BitmapChanges.Resized:
                case BitmapChanges.SetGray:
                case BitmapChanges.TransparentFilled:
                    if (bitmapRedoHistoryData.Count < 1)
                        return;

                    bitmapRedoHistoryData.Pop().Dispose();
                    break;
            }
        }

        /// <summary>
        /// Redoes the last undo.
        /// </summary>
        public void Redo()
        {
            if (redos.Count < 1)
                return;

            BitmapChanges change = redos.Pop();
            undos.Push(change);

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
                    using (bmp = bitmapRedoHistoryData.Pop())
                    {
                        bitmapUndoHistoryData.Push(CurrentBitmap.CloneSafe());

                        if (ImageAnimator.CanAnimate(bmp) && change == BitmapChanges.SetGray)
                        {
                            Bitmap newBmp = ImageHelper.GrayscaleGif((Bitmap)CurrentBitmap);
                            ReplaceBitmap(newBmp);
                            OnUpdateReferences();
                            OnRedo(change);
                            return;
                        }

                        ImageHelper.UpdateBitmap(CurrentBitmap, bmp);
                    }
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

        /// <summary>
        /// Clears all the undos and disposes of any bitmap history kept.
        /// </summary>
        public void ClearUndos()
        {
            undos.Clear();
            for (int i = 0; i < bitmapUndoHistoryData.Count; i++)
            {
                bitmapUndoHistoryData.Pop().Dispose();
            }
        }

        /// <summary>
        /// Removes and disposes the last undo.
        /// </summary>
        public void DisposeLastUndo()
        {
            if (undos.Count < 1)
                return;

            BitmapChanges change = undos.Pop();

            // if the change being removed had any bitmap data stored dispose it
            switch (change)
            {
                case BitmapChanges.Cropped:
                case BitmapChanges.Resized:
                case BitmapChanges.SetGray:
                case BitmapChanges.TransparentFilled:
                    if (bitmapUndoHistoryData.Count < 1)
                        return;

                    bitmapUndoHistoryData.Pop().Dispose();
                    break;
            }
        }

        /// <summary>
        /// Undoes the last tracked change.
        /// </summary>
        public void Undo()
        {
            if (undos.Count < 1)
                return;

            BitmapChanges change = undos.Pop();
            redos.Push(change);

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
                    using (bmp = bitmapUndoHistoryData.Pop())
                    {
                        bitmapRedoHistoryData.Push(CurrentBitmap.CloneSafe());

                        if (ImageAnimator.CanAnimate(bmp) && change == BitmapChanges.SetGray)
                        {   // can't update a gif using pointers so we need to update the references
                            Bitmap newBmp = ImageHelper.GrayscaleGif((Bitmap)CurrentBitmap);
                            ReplaceBitmap(newBmp);
                            OnUpdateReferences();
                            OnUndo(change);
                            return;
                        }

                        ImageHelper.UpdateBitmap(CurrentBitmap, bmp);
                    }
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

        /// <summary>
        /// Clears and disposes of both the undos and redos.
        /// </summary>
        public void ClearHistory()
        {
            ClearUndos();
            ClearRedos();
        }

        /// <summary>
        /// Dispose of the undos, redos, and the current bitmap.
        /// </summary>
        public void Dispose()
        {
            ClearHistory();
            if(CurrentBitmap != null)
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
