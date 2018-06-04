
```
   ____  ___                    _                 ____ _     
  / ___|/ _ \ _ __  _ __   __ _| |    __ _ _   _ / ___| |__  
 | |  _| | | | '_ \| '_ \ / _` | |   / _` | | | | |  _| '_ \ 
 | |_| | |_| | | | | | | | (_| | |__| (_| | |_| | |_| | | | |
  \____|\___/|_| |_|_| |_|\__,_|_____\__,_|\__,_|\____|_| |_|
                                                             
```
## Description
**g0nnaL4ugh** is a simple C# (mono) ransomware that encrypts all the data using the Rijndael algorithm.

[![forthebadge](https://forthebadge.com/images/badges/oooo-kill-em.svg)](https://forthebadge.com)


## FAQ

* How are the setters and getters in C# ?

They're like normal g&s but you can automate this, using `Automatic properties` to avoid boilerplate code:

```
private string name;

public string Name
{
    get { return name; }
    set { name = value; }
}
```
[Resource](http://csharp.net-tutorials.com/csharp-3.0/automatic-properties/)


* Why Rijndael instead of AES, if AES is the standard ?

* Which block cipher mode should I use ?

* How should I generate random strings ?

    We can use `Random()` but the problem is that the default seed use `Environment.TickCount` ([https://referencesource.microsoft.com/#mscorlib/system/random.cs](https://referencesource.microsoft.com/#mscorlib/system/random.cs)) and it can be predictable as it's said in this article: [https://utkusen.com/blog/destroying-the-encryption-of-hidden-tear-ransomware.html](https://utkusen.com/blog/destroying-the-encryption-of-hidden-tear-ransomware.html).

    ((Github issue)[https://github.com/BlackMathIT/Ransomware/issues/2])

* How should I generate the IV's and should it be the same for all files ?

    ([Stackoverflow](https://stackoverflow.com/a/2790721))

## Monodevelop errors (v7.5)

1. When I just start monodevelop on a project or just generate one, it closes silently.

You can do 3 things (all together or maybe one):
	* Change the line endings for your operating system, for example linux / mac: `Edit > Preferences > Code Formatting > Text file > Line endings > Unix / Mac`
	* Turn off the Version Control: `Edit > Preferences > Version Control > Disable Version Control globally`

2. When hit the play button it says: `Debugger operation failed. ApplicationName='/usr/lib/gnome-terminal/gnome-terminal-server', CommandLine='--app-id mono.develop.id1231cca092074c52b27483ce4c4b004c', CurrentDirectory='', Native error= Cannot find the specified file`:

![Cannot Run](images/cannot_run.png?raw=true)

You need to do two things:

* Unset some global variable called `GNOME_DESKTOP_SESSION_ID` ([Stackoverflow](https://stackoverflow.com/a/23233374)):
```
unset GNOME_DESKTOP_SESSION_ID
```
Then run `monodevelop`
In order to persist this solution, we should modify the startup bash for `monodevelop`, We can find it by running `which monodevelop`.
Then append the `unset` statement just under the `shebang`.

* Install `xterm`.
	* If you are in fedora:
		```
		sudo dnf install xterm
		```
	* If your are in ubuntu:
		```
		sudo apt-get install xterm
		```


## Bypass Window security

We can sign the executable with some fake credentials using this tool: ((https://github.com/HackerFantastic/Public/blob/master/tools/bypassavp.sh)[https://github.com/HackerFantastic/Public/blob/master/tools/bypassavp.sh]).