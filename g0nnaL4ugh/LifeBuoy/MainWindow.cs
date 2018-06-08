using System;
using Gtk;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
		progressbar1.Visible = false;
    }
    
    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
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
