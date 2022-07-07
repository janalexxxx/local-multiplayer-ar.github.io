<p align="center">
    <h1 align="center">Local Multiplayer AR</h1>
    <p align="center">How to setup a publicly accessible server using Uberspace & NodeJS</p>
</p>

# Part 01:
# Setting up Uberspace

## Creating a server on Uberspace

We are using Uberspace to host our server. Uberspace is simple to set up & provides us with a free & preconfigured Linux server for the first 30 days without adding any payment details.

Go to [Uberspace.de](https://dashboard.uberspace.de/register?lang=en) and create an account for your project. The process of setting up your account might take a few minutes.

When your account has been set up, go to your [Login Tab](https://dashboard.uberspace.de/dashboard/authentication) and set a password for "SSH Access to Uberspace". You will need this password to connect to the server. This can be the same password that you used for accessing your account. Although the text field might be filled already, the SSH password is not set automatically! Enter a password for SSH access and press "Go!". Now your SSH password has been set.

Then navigate to your [Datasheet Tab](https://dashboard.uberspace.de/dashboard/datasheet) and look up the informations that you need to connect to your server in the next step. 

You will need the following informations:
* Username 
* SSH Password (that you set in the previous step)
* Hostname (eg eltanin.uberspace.de)
* Server's web address ([username].uber.space)


## Connect to your Uberspace server

There are two fundamental ways that we can now connect to our server. Either we use a command line tool like PowerShell on Windows or Terminal on Mac. Or we use an SFTP application like [Cyberduck](https://cyberduck.io/). The command line can seem daunting due to its lack of a visual interface, but provides us the opportunity to directly use NodeJS on our server without the need of setting it up locally first. We can also use the Command Line to edit the settings of our server. An SFTP application has the advantage of a visual interface for the organization of all files on our server, but it lacks the opportunity to directly use NodeJS. Therefore we will need to use both to setup our server. We will use Cyberduck to manage all files and folders on our server and we will use the Command Line to change our server's settings and use NodeJS.


### Connect to your server via SFTP application

First download and install [Cyberduck](https://cyberduck.io/download/) on your computer. 
Then open the application and open a new connection by pressing the correlating button in the top bar. In the connection popup you now need to enter the following informations:

![Cyberduck SFTP Connection](/img/cyberduck-add-connection.png)

* Connection: SFTP (SSH File Transfer Protocol)
* Server: [username].uber.space
* Port: 22
* Username: [username]
* Password: [your-ssh-password]
* SSH Private Key: None

Then Press "Connect". 

If you encounter authentication problems, please double-check that you actually set an SSH password as explained in the previous step and that you used the right password and server-address to connect.

What you should see now is a list of folders like this:

![Uberspace Server Folder Structure](/img/cyberduck-uberspace-server-folders.png)

Hooray! 
You have just successfully connected to your own Linux server!

A server is simply a remote computer that you is publicly accessible via the internet using an IP address or a domain, which is basically nothing else than a name representing the server's IP address. Fundamentally your server simply consists of a bunch of folders and files - just like your local computer does.

The most important folder is the `html` folder. This is where we can put the data for our website or NodeJS server and make it publicly accessible.


### Create a folder for your NodeJS server

Open the `html` folder. Then right-click in it and select `Create a new folder`. This is going to be the folder in which all of our NodeJS code is going to be stored. You can name it however you like, just make sure to remember the name.

![Uberspace Create NodeJS Folder](/img/cyberduck-uberspace-create-nodejs-folder.png)

Your `html` folder should now look something like this:

![Uberspace With NodeJS Folder](/img/cyberduck-uberspace-with-nodejs-folder.png)

In the next step we need to switch to the Command Line to setup our NodeJS server and change the settings of our Uberspace to make it publicly accessible. You can leave Cyberduck open. We will return to it.


## Connect to your server via Command Line

Using Cyberduck you have already managed to connect to your server via SSH and you have seen how your server looks like. It is just a bunch of folders and files, that you can navigate in between.

Now we will do the same steps using the Terminal. We will connect to our server via SSH and then navigate to the folder for your NodeJS server (I called it nodejs-server), that you just created.

Therefore open the `Terminal` application on your Mac or `PowerShell` on Windows. Any other Command Line tool works as well. SSH is enabled by default on a Mac. If you have never used it before on Windows, you might need to enable it in your settings. Follow the first step of [This Tutorial](https://linoxide.com/how-use-ssh-commands-windows-10-command-prompt/) to install OpenSSH and enable an SSH connection on Windows.


### Establish an SSH connection

Enter the following command to establish a connection to your Uberspace server:

    $ ssh [username]@[hostname]

This is the command that I have to use for example:

    $ ssh argame@eltanin.uberspace.de

Then when prompted confirm the connection by entering `yes` & confirm by pressing `enter`.

Then when prompted enter your Uberspace SSH password & confirm by pressing `enter` once again.

Now you should see a Welcome message from Uberspace like this:

![Uberspace Welcome Message](/img/uberspace-welcome-message.png)

Hooray! 
You have just used the Command Line to connect to your server. You basically did the same thing that you did using Cyberduck before. Simply establishing an SSH connection to your server to access its folders and files. So you might be wondering: Why am I not seeing any folders and files here? How should I navigate to my NodeJS folder inside the `html` directory now? Well...


### Navigate your directory

The Command Line has no graphical interface at all. So there are also no folder icons that you can click on. You have to use text commands to see a list of files at the current directory and to navigate in between folders. 
These are the commands that you will need to navigate around:


#### List all visible files and folders at the current directory:

    $ ls
    

#### List all visible & hidden files and folders at the current directory:

    $ ls -a
    
    
#### Change directory

    $ cd path/of/directory
    
For example to change to the `/html/nodejs-server/` directory use:
    
    $ cd html/nodejs-server

If you want to move back up to the parent-directory use:

    $ cd ..
    
If you want to move back to the root directory use:

    $ cd ~
    
    
Tip #1: You can press `Tab` to use the auto-complete functionality and save some typing effort. 

Tip #2: The name of the current directory is always displayed on the left-hand side of the `$` sign. When I am in my `html/nodejs-server` directory, the Command Line will read `[argame@eltanin html/nodejs-server]$`. This way you always know where you are right now.


### Practice

Now practice your newly found skills of navigating in between folders on your server by moving a bit around and exploring the different folders on your server. When you feel proficient with this way of navigation, navigate inside of your `html/[servername]` directory.


# Part 02:
# Setup a simple NodeJS server

## Install NodeJS

Luckily NodeJS is already installed by default on your Uberspace. If you want to use NodeJS locally on your machine, you can download and install it from [here](https://nodejs.dev/download/). Using NodeJS outside of your Uberspace is not required in this course.

To check whether NodeJS is installed on your current system, execute the following command:

    $ node
    
To exit the NodeJS interface again type `.exit`.


## Open port

To connect to any server over the internet, the server needs to have a public IP-address or domain name & an open port to connect to. Our Uberspace server already has a public domain name by default, but is missing an open port. Without an open port, we can not connect to our server. Therefore we at first need to open an port, that our NodeJS server then can listen to requests on.

To open a port connect to your Uberspace via Command Line and input the following command:

    $ uberspace port add
    
After entering this command, you should receive a confirming message like `Port 42660 will be open for TCP and UDP traffic in a few minutes.`

In this case `42660` is our port number. This is the port number, that we will then use for NodeJS server in the following steps.


### List open ports

To get a list of currently open ports, execute the following command:

    $ $ uberspace port list
    

### Close open ports

In case you have too many open ports, you can also close ports again by executing the following command:

    $ uberspace port del YOUR_PORT_NUMBER


At the end of this step you should have one open port listed. Save the port number somewhere since you will need it in the next step.


## Create a server.js file

All that NodeJS requires to work after installation is a simple Javascript file, that you can then execute from the Command Line.

Therefore create a new file called `server.js` in your favourite text editor.

To this file add the following content:

```Javascript
const http = require('http');

// Set IP and port, that the server should listen on
const hostIP = '0.0.0.0';
const port = YOUR_PORT_NUMBER; // REPLACE with the port you opened in the previous step

// Define what the server should return to a client upon connection request
const server = http.createServer((request, result) => {
  result.statusCode = 200;
  // In this case it returns a simple text message
  result.setHeader('Content-Type', 'text/plain');
  result.end('Welcome to my NodeJS server');
});

// Start listening for requests on the defined port and IP
server.listen(port, hostIP, () => {
  // Receive a confirmation message when server starts listening
  console.log(`Server running at http://${hostIP}:${port}/`);
});
```

IMPORTANT: Replace the the port number with the port that you opened in the previous step!

IMPORTANT: When working on Uberspace the host's IP address is always `0.0.0.0`!

You can find more information about server status codes [here](https://sonihemant111.medium.com/http-response-status-codes-5970014a11bd), when interested.


## Start your NodeJS server

To start your very first NodeJS server you need to add this `server.js` file inside your `nodejs-server` folder.

Therefore connect to your Uberspace using Cyberduck, open your `nodejs-server` folder and drag'n'drop your `server.js` file into this folder. 

Then connect to your Uberspace using the Command Line, navigate inside your `nodejs-server` folder & start your NodeJS server by executing the following command:

    $ node server.js

If everything works, we should get a feedback like `Server running at http://0.0.0.0:42660/`

Now the server is up and running. Our server is listening to the right port. So let's fire up a web browser and navigate to our server by entering its web-address `[username].uber.space/nodejs-server`. And?

Nothing...

The browser can not establish a connection. Damn.

This is particularly confusing since, if we would start a local NodeJS server with the same file, we would actually see the desired result. So what is going on?

Well, our application is simply not visible yet to the outside. This is a security feature of Uberspace to prevent clients from accessing parts of your applications, that they shouldn't. We have to explicitly connect port and application with each other and publish this connection by setting up a Web Backend.

In the next step we are going to setup an Uberspace Web Backend and thereby make our server accessible from the outside.

In the meantime you can stop the execution of the server by pressing `Ctrl+C`.

Tip: You can also open multiple tabs in your Command Line and connect to your Uberspace in each of them. This way your server can keep running on one tab and you can simultaneously access your Uberspace in another tab. 


## Setup an Uberspace Web Backend 

To make your NodeJS server visible to the public, you have to open a Web Backend connecting the folder, in which your NodeJS server is stored with the open port you defined earlier. 

You can create a new Web Backend by executing the following command:

    $ uberspace web backend set /YOUR_DIRECTORY --http --port YOUR_PORT
    
For a NodeJS server stored in the `/nodejs-server` directory and communicating via port `42660` the command would be this:

    $ uberspace web backend set /nodejs-server --http --port 42660
    
To check whether you Web Backend was setup successfully, you can list all web backends by executing the following command:
    
    $ uberspace web backend list
    
If something went wrong, you can delete a web backend by executing the following command:
    
    $ uberspace web backend del /nodejs-server
    

More information regarding Uberspace Web Backends can be found [here](https://manual.uberspace.de/web-backends/).


## Start the server again and celebrate

Now everything should be setup properly. You can navigate back into your `nodejs-server` directory and start the server using `node server.js`.

Then open your browser and navigate to your server at `https://[username].uber.space/nodejs-server`. 

You should now see the welcome message you added to your server like this:

![NodeJS Welcome Message](/img/nodejs-welcome-message.png)

Congratulations! You just successfully set up your first NodeJS server and it is available to the public!



# Part 03:
# Improving your simple NodeJS server

Your first NodeJS server is up and running and that is amazing, BUT it will stop running as soon as you close your Command Line application or your computer shuts down. It is not running constantly in the background. So let's improve this by setting up a service daemon, that will take care of running the server in the background.

Uberspace uses `supervisord` to monitor services. A service, or daemon, is a program that starts automatically and is kept in the background. In case it quits or crashes, it is restarted by `supervisord`.


## Creating a service

Create a new file called `run-nodejs-server.ini`.

Add the following content to it:

```
[program:run-nodejs-server]
directory = /home/[username]/html/nodejs-server
command = node server.js
startsecs = 60
```

Then place this file in `~/etc/services.d` in your Uberspace.


## Starting a service

Afterwards, ask `supervisord` to look for new `.ini` files:

    $ supervisorctl reread
    
And then start your daemon:

    $ supervisorctl update
    

## Controlling a service

You can start a non-running service by executing the following command:

    $ supervisorctl start run-nodejs-server

You can stop a running service by executing the following command:

    $ supervisorctl stop run-nodejs-server
    

You can restart a service by executing the following command:

    $ supervisorctl restart run-nodejs-server
    

You can get the status of all your services by executing the following command:

    $ supervisorctl status


Congratulations! 
You successfully set up a publicly accessible NodeJS server for your game!
