# Pitter Desktop Client for Windows

This repository contains the code in production or beta for the Pitter Desktop App, which integrated into the platform at https://pitter.us/

Any form of code posted in this repository is for keeping track of changes, and allowing users to overview our code to see what runs in the background. Production updates are pulled straight from this repository.

Functions:
  - Singular File Upload
  - Multi-file upload from clipboard via compressed archive
  - Screenshot Capture of fullscreen, window or specified region
  - Selection Overlay (Position of Cursor, Dimensions of Region)
  - Panel Integration
  - Diagnostics Utility
  - File Synchronization
  - Settings Synchronization
  - Processor efficiency (Application will limit itself to one core, on the lowest priority possible)


### Dependencies

Below is the list of required software that may or may not be included with this repository.

* [.NET Framework 4.5] - Core Language Support
* [Newtonsoft.JSON] - JSON Parsing Library

### Installation

While it is recommended that you install pitter via the installer at https://pitter.us/, you may install it manually by creating the path `%appdata%\pitter` and adding the binary and dependencies in this folder.


### Development

Contibution to the Pitter project may not be performed direcctly, however input is appreciated and accepted via the issue tracker related to this repository.


   [.NET Framework 4.5]: <https://www.microsoft.com/net>
   [Newtonsoft.JSON]: <http://www.newtonsoft.com/json>
 
