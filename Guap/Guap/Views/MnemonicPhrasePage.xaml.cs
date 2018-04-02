using Xamarin.Forms;

namespace Guap.Views
{
	public partial class MnemonicPhrasePage : ContentPage
	{
		public MnemonicPhrasePage ()
		{
            InitializeComponent ();
            this.FillGrid();
		}

        // TODO refactor this
	    public void FillGrid()
	    {
	        var wordlist =
	            "brass bus same payment express already energy direct type have venture afraid type have venture afraid"
	                .Split(' ');
	        int i = 1, j = 1;
	        foreach (var word in wordlist)
	        {
	            if (j == 5)
	            {
	                i++;
	                j = 1;
	            }

	            this.Grid.Children.Add(
	                new Label()
	                    {
	                        Text = word,
	                        HorizontalOptions = LayoutOptions.Center,
	                        VerticalOptions = LayoutOptions.Center,
	                        TextColor = Color.White,
	                        FontAttributes = FontAttributes.Bold
	                    },
	                j,
	                i);
	            j++;

	        }
	    }
	}
}