using System;
using g0nnaL4ugh;
using g0nnaL4ugh.Crypto;
using Gtk;

public partial class MainWindow : Gtk.Window
{
	private bool isProcessing = false;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
		progressbar1.Visible = false;
		wrong.Visible = false;
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
		wrong.Visible = false;
		if (this.IsValidEntry(entry2.Text))
		{
			this.BeginDecryption();
		}else {
			wrong.Visible = true;
		}
	}

    private bool IsValidEntry(string text)
	{
		try
		{
			Convert.FromBase64String(entry2.Text);
		}
		catch(Exception)
		{
			return false;
		}
		return !string.IsNullOrEmpty(text);
	}

    private void BeginDecryption()
	{
		rescue.Sensitive = false;
        entry2.Sensitive = false;
        progressbar1.Visible = true;
        this.BeginProgressAnimation();
        this.isProcessing = true;

		Spider spider;
		string[] paths = { "/home/giwiro/Playground/ransomware" };
		Decrypter decrypter = new Decrypter();
		byte[] pwd = Convert.FromBase64String(entry2.Text);
		Console.WriteLine(pwd.Length);
		spider = new Spider(paths, decrypter, pwd);
        // spider.Spread();
	}

    private void BeginProgressAnimation()
	{
	}
}
