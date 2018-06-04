using System;
namespace g0nnaL4ugh.Finder
{
	public enum NodeType {Dir, File};

    public class Node
    {
		private NodeType type;
		public string fullPath;
		private string salt;

		public Node(NodeType type, string fullPath)
		{
			this.type = type;
			this.fullPath = fullPath;
        }

		public Node(NodeType type, string fullPath, string salt)
        {
            this.type = type;
            this.fullPath = fullPath;
            this.salt = salt;
        }
        
		public NodeType Type { get { return this.type; } }
		public string FullPath { get { return this.fullPath; } }
		public string Salt { get { return this.salt; } set { this.salt = value; } }
    }
}
