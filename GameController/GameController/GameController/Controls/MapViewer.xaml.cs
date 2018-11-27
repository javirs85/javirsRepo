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

        List<Point> options = new List<Point>();
        List<int> selected = new List<int>();
        List<int> solution = null;

        SKCanvasView canvas = null;


        public MapViewer ()
		{
			InitializeComponent ();
            canvas = new SKCanvasView();
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
                Color = SKColor.Parse("#f4f4f4"),
                StrokeWidth = 25
            };
            paint.IsAntialias = true;

            options = new List<Point>()
            {
                new Point(info.Width / 4, info.Height / 4),
                new Point(info.Width / 4*3, info.Height / 4),
                new Point(info.Width / 4, info.Height / 4*3),
                new Point(info.Width / 4*3, info.Height / 4*3),
            };

            if (solution != null)
            {
                paint.Color = SKColors.Red;
                canvas.DrawCircle((float)options[solution[0] - 1].X, (float)options[solution[0] - 1].Y, 50, paint);

                paint.Color = SKColor.Parse("#eaeaea");
                canvas.DrawCircle((float)options[solution[1] - 1].X, (float)options[solution[1] - 1].Y, 50, paint);

                paint.Color = SKColor.Parse("#d6d4d4");
                canvas.DrawCircle((float)options[solution[2] - 1].X, (float)options[solution[2] - 1].Y, 50, paint);

                paint.Color = SKColor.Parse("#bcbaba");
                canvas.DrawCircle((float)options[solution[3] - 1].X, (float)options[solution[3] - 1].Y, 50, paint);
            }
        }

        internal void setPuzzle(Puzzle p)
        {
           
            p.UpdateInTheUIRequired += (o, e) => {
                if (solution == null)
                    if (p.Solution != null)
                    {
                        solution = p.Solution.ToCharArray().Select(x => Int32.Parse(x.ToString())).ToList();
                    }
                Device.BeginInvokeOnMainThread(() =>
                {
                    canvas.InvalidateSurface();
                });
            };
        }
    }
}