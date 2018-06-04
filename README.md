
## Description
**g0nnaL4ugh** is a simple C# (mono) ransomware that encrypts all the data using the Rijndael algorithm.

[![forthebadge](https://forthebadge.com/images/badges/oooo-kill-em.svg)](https://forthebadge.com)


## FAQ

* Why Rijndael instead of AES, if AES is the standard ?

* Which block cipher mode should I use ?

* Should I use the same IV for all files ?


## Monodevelop errors (v7.5)

* When I just start monodevelop on a project or just generate one, it closes silently.

You can do 3 things (all together or maybe one):
	* Change the line endings for your operating system, for example linux / mac: `Edit > Preferences > Code Formatting > Text file > Line endings > Unix / Mac`
	* Turn off the Version Control: `Edit > Preferences > Version Control > Disable Version Control globally`

* When hit the play button it says: `Debugger operation failed. ApplicationName='/usr/lib/gnome-terminal/gnome-terminal-server', CommandLine='--app-id mono.develop.id1231cca092074c52b27483ce4c4b004c', CurrentDirectory='', Native error= Cannot find the specified file`:

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
		If you are in fedora:
		```
		sudo dnf install xterm
		```
		If your are in ubuntu:
		```
		sudo apt-get install xterm
		```


