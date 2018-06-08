using System;
using Gtk;

public partial class MainWindow : Gtk.Window
{
	private bool isProcessing = false;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
		progressbar1.Visible = false;
    }
    
    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
		if (!this.isProcessing)
		{
			Application.Quit();
		}      
        a.RetVal = true;
    }

	protected void OnRescueBtnClicked(object sender, EventArgs e)
	{
		if (this.IsValidEntry(entry2.Text))
		{
			rescue.Sensitive = false;
            entry2.Sensitive = false;
			progressbar1.Visible = true;
			this.BeginProgressAnimation();
			this.isProcessing = true;
		}
	}

    private bool IsValidEntry(string text)
	{
		return !string.IsNullOrEmpty(text);
	}

    private void BeginProgressAnimation()
	{
	}
}
