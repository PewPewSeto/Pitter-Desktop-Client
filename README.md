### Welcome to the Pitter Desktop WebApp
The Desktop app is our primary application for using pitter, while providing bindings between the web and you using the Awesomium framework.

### Features
 - File Uploading (up to 100MB)
 - Screenshotting (Selection, Fullscreen and Current window)
 - Clipboard Upload
 - Custom Keybinds
 - Uploading Statistics
 - API 
 - DPAPI Encryption
 - Settings Synchronization

### Directory Structure
Pitter has two unique directory structures that are required for operation.

By default, pitter will intall into **%appdata%\Pitter**, and your files will be saved to the Pitter folder in your documents.

### External Usage
The Pitter Team is aware that there are other utilities that offer their own screen capture system, but allow uploading to different APIs.

If you have said application, you may upload your file using the settings below:

> URL: https://api.pitter.us/scalar.php
> Method: POST
> Required File Variable Name: sendfile ($_FILE['sendfile'])
> Max File Upload Size: 100MB
> Cached Retrieval Point: https://c.pitter.us/ + file
> Non-cached Retrieval Point: https://i.pitter.us/ + file
> Required Variables:
>> username - base64 encoded email
>> password - base64 encoded password
>>command - "upload"
>>filename - the base filename of the file you are uploading
