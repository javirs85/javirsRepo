using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SkiaSharp;
using SkiaSharp.Views.Forms;
using gameTools;

namespace GameController.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapViewer : ContentView
	{
		public MapViewer ()
		{
			InitializeComponent ();
            SKCanvasView canvas = new SKCanvasView();
            canvas.PaintSurface += Canvas_PaintSurface;
            Content = canvas;
		}

        private void Canvas_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 25
            };
            paint.IsAntialias = true;

            canvas.DrawCircle(info.Width / 2, info.Height / 2, 100, paint);

            paint.Style = SKPaintStyle.Fill;
            paint.Color = SKColors.Blue;
            canvas.DrawCircle(info.Width / 3, info.Height / 3, 100, paint);

        }

        internal void setPuzzle(Puzzle p)
        {
            p.UpdateInTheUIRequired += (o, e) => {
                ;
            };
        }
    }
}