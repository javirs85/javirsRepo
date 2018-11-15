using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameController
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GBColors : ResourceDictionary
	{
		public GBColors ()
		{
			InitializeComponent ();
		}
	}
}