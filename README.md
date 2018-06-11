
```
   ____  ___                    _                 ____ _     
  / ___|/ _ \ _ __  _ __   __ _| |    __ _ _   _ / ___| |__  
 | |  _| | | | '_ \| '_ \ / _` | |   / _` | | | | |  _| '_ \ 
 | |_| | |_| | | | | | | | (_| | |__| (_| | |_| | |_| | | | |
  \____|\___/|_| |_|_| |_|\__,_|_____\__,_|\__,_|\____|_| |_|

  > Sh4ll w3 pl4y a gam3 ?
                                                             
```
## Description
**g0nnaL4ugh** is a simple C# (mono) ransomware that encrypts all the data using the Rijndael algorithm.

[![forthebadge](https://forthebadge.com/images/badges/oooo-kill-em.svg)](https://forthebadge.com)

## What's inside ?

    This project gas 2 projects within it:

    1. `g0nnaL4ugh`: The first one has all the core functionalities as Crypto, Crawling directories,
Snitching the generated password, etc; and it will be a console-only program 
that performs the encryption.
    2. `LifeBuoy`: On the other hand, this is GUI-only project
that will receive the password (in Base64) as input and perform the decryption using the
modules in the main project.

    ![Cannot Run](images/LifeBuoy.png?raw=true)

## Folder structure

> To open the project you should load the first `g0nnaL4augh.sln`

    /g0nnaL4augh
    ├── g0nnaL4augh.sln               # Solution file you should open
    ├── README.md
    ├── /images                       # Images used in README.md
    ├── /keys                         # Place where gen_rsa_keys.py script drops the keys (public and private)
    ├── /scripts                      # Helpful scripts in python3
    │   ├── gen_rsa_keys.py           # Script that generates a RSA (2048 bits) keys (this keys would be to asymmetric encrypt the password generated in the Snitch module)
    └── /g0nnaL4augh                  # Source code of main project
        ├── Program.cs                # Console encryption application main entry point
        ├── /Crypto                   # Crypto operations (Encrypt & Decrypt)
        ├── /Finder                   # Crawl in directories and apply encrypt or decrypt
        ├── /Properties               # IDE generated
        ├── /Snitch                   # Password snitch (send random generated password through mail or async encrypt it)
        └── /LifeBuoy                 # Decryption GUI project (using main project modules [Crypto, Finder])
            ├── /gtk-gui              # IDE generated gui components and properties
            ├── /Properties           # IDE generated
            ├── MainWindow.cs         # Gtk Window logic (use of g0nnaL4ugh modules)
            └── Program.cs            # Decryption application main entry point (Initialize Gtk window)


## FAQ
* **I don't wanna use monodevelop**

    Monodevelop and `Visual Studio` are compatible, you can work in it too.

* **Why Rijndael instead of AES, if AES is the standard ?**

    In a nutshell they're the same, actually Rijndael was one of the two
    creators of AES. The only difference is that the block size in AES is fixed
    to 128, 192, or 256 bits; and Rijndael can be any multiple of 32 from
    128 bits to 256 bits.

* **Which block cipher mode should I use ?**

    There are many of them: ECB, CBC, OFB, CFB, CTR, XTS.
    Since we are goin to encrypt more than one block ECB is off the table. 
    CBC, OFB and CFB are quite simmilar, they use the output of each block to
    feed the key of the next block (in ECB blocks are independant), so it can be
    any of this 3. Maybe we can consider CTR, since it's benefit is with
    paralellism and encryption is cpu intensive. We will pass on XTS cos is more
    for random accesible data. ([Source Stackoverflow](https://stackoverflow.com/a/1220869/3412989))

* **How should I generate random strings ?**

    We can use `Random()` but the problem is that the default seed use `Environment.TickCount` ([https://referencesource.microsoft.com/#mscorlib/system/random.cs](https://referencesource.microsoft.com/#mscorlib/system/random.cs)) and it can be predictable as it's said in this article: [https://utkusen.com/blog/destroying-the-encryption-of-hidden-tear-ransomware.html](https://utkusen.com/blog/destroying-the-encryption-of-hidden-tear-ransomware.html).

    ([Github issue](https://github.com/BlackMathIT/Ransomware/issues/2))

* **How should I generate the IV's and should it be the same for all files ?**

    To have a better key handling and random-length capability for password, we
    are going to use PBKDF2. It's an algorithm that (in a nutshell) iterates
    over the password and salt, with SHA1 a # of times. From that iteration we
    can extract 2 things: the key (the new "password") and a new random IV
    (initialization vector) for each one of the files. With that we make our 
    cipher texts less likely to be broken or reversed.

    ([Stackoverflow](https://stackoverflow.com/a/2790721))

* **How are the setters and getters in C# ?**

    They're like normal g&s but you can automate this, using `Automatic properties` to avoid boilerplate code:

    ```c#
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    ```
    [Resource](http://csharp.net-tutorials.com/csharp-3.0/automatic-properties/)

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
