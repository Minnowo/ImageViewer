﻿using System;
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

        public int UndoCount
        {
            get { return undos.Count; }
        }

        public int RedoCount
        {
            get { return redos.Count; }
        }

        public ImageBase CurrentBitmap
        {
            get
            {
                return currentBitmap;
            }
            set
            {
                currentBitmap = value;

                if (currentBitmap != null)
                {
                    Format = value.GetImageFormat();
                }
                else
                {
                    Format = ImgFormat.nil;
                }
            }
        }
        private ImageBase currentBitmap;

        public ImgFormat Format;

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

        public BitmapUndo(ImageBase bmp)
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

            CurrentBitmap = ImageBase.ProperCast(bmp, this.Format);
        }

        /// <summary>
        /// Sets the CurrentBitmap and does not dispose of the last bitmap.
        /// </summary>
        /// <param name="bmp">The new bitmap.</param>
        public void UpdateBitmapReferance(Bitmap bmp)
        {
            CurrentBitmap = ImageBase.ProperCast(bmp, this.Format);
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
                    //bitmapUndoHistoryData.Push(currentBitmap.DeepClone());
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

            switch (change)
            {
                // need to track history data
                case BitmapChanges.Cropped:
                case BitmapChanges.Resized:
                case BitmapChanges.Dithered:
                case BitmapChanges.SetGray:
                case BitmapChanges.TransparentFilled:
                    bitmapUndoHistoryData.Push(CurrentBitmap.DeepClone());
                    CurrentBitmap.UpdateImage(bitmapRedoHistoryData.Pop());
                    break;

                // changes are easily undone and do not need to be kept in memory
                case BitmapChanges.Inverted:
                    CurrentBitmap.InvertColor();
                    break;
                case BitmapChanges.RotatedLeft:
                    CurrentBitmap.RotateLeft90();
                    break;
                case BitmapChanges.RotatedRight:
                    CurrentBitmap.RotateRight90();
                    break;
                case BitmapChanges.FlippedHorizontal:
                    CurrentBitmap.FlipHorizontal();
                    break;
                case BitmapChanges.FlippedVirtical:
                    CurrentBitmap.FlipVertical();
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

            switch (change)
            {
                // need to track history data
                case BitmapChanges.Cropped:
                case BitmapChanges.Resized:
                case BitmapChanges.Dithered:
                case BitmapChanges.SetGray:
                case BitmapChanges.TransparentFilled:
                    bitmapRedoHistoryData.Push(CurrentBitmap.DeepClone());
                    CurrentBitmap.UpdateImage(bitmapUndoHistoryData.Pop());
                    break;

                // changes are easily undone and do not need to be kept in memory
                case BitmapChanges.Inverted:
                    CurrentBitmap.InvertColor();
                    break;
                case BitmapChanges.RotatedLeft:
                    CurrentBitmap.RotateRight90();
                    break;
                case BitmapChanges.RotatedRight:
                    CurrentBitmap.RotateLeft90();
                    break;
                case BitmapChanges.FlippedHorizontal:
                    CurrentBitmap.FlipHorizontal();
                    break;
                case BitmapChanges.FlippedVirtical:
                    CurrentBitmap.FlipVertical();
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
            if (CurrentBitmap != null)
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
    }
}
