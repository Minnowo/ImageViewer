using System.ComponentModel;

// ReSharper disable once CheckNamespace
namespace ImageViewer.Helpers.Dithering
{
  [Description("Bayer-2")]
  [Browsable(false)]
  public class Bayer2 : OrderedDithering
  {
    #region Constructors

    public Bayer2()
      : base(new byte[,]
             {
               { 0, 2 },
               { 3, 1 }
             })
    { }

    #endregion
  }
}
