using System;
using System.Collections.Generic;
using System.IO;

namespace g0nnaL4ugh
{
	public enum OperatingSystem { Win, OSX, Unix };

	public class PathUtil
	{
		private string home;
		private List<string> start;
		private string desktop;

		public PathUtil()
		{
			var operatingSystem = PathUtil.DetectOS();
			string user = PathUtil.GetCurrentUser();
			switch (operatingSystem)
			{
				case OperatingSystem.OSX:
					home = Path.Combine("/Users", user);
					break;
				case OperatingSystem.Unix:
					home = Path.Combine("/home", user);
#if DEBUG
					start = new List<string>() {
						Path.Combine(home, "Playground/ransomware")
					};
#else
					start = new List<string>() {
                        // home
                    };
#endif
					break;
				case OperatingSystem.Win:
					home = Path.Combine(@"C:\Users", user);
					break;
			}
            // Common values
			desktop = Path.Combine(home, "Desktop");
        }

        public string Home { get { return this.home; } }

		public List<string> Start { get { return this.start; } }

		public string Desktop { get { return this.desktop; } }

        private static string GetCurrentUser()
		{         
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;         
		}

		public static OperatingSystem? DetectOS()
		{
			var os = Environment.OSVersion;
            PlatformID pid = os.Platform;
			switch (pid)
			{
				case PlatformID.MacOSX:
					return OperatingSystem.OSX;
				case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
					return OperatingSystem.Win;
                case PlatformID.Unix:
					return OperatingSystem.Unix;
                default:
					return null;
			}
		}
    }
}
