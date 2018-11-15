using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GBCore;

namespace GameController
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DebugPage : ContentPage
	{
		public DebugPage ()
		{
			InitializeComponent ();

            try
            {
                GameItems.newMessageToUI += GameItems_newMessageToUI;
            }catch (Exception ex)
            {
                ;
            }
		}

        private void GameItems_newMessageToUI(object sender, string e)
        {
            Device.BeginInvokeOnMainThread(() => {
                DebugContainer.Children.Add(new Label() { Text = e });
            });
        }
    }
}