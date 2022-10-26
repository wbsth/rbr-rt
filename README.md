# RBR Replay Tools

WPF GUI app that provides some file manipulation for Richard Burns Rally replays.

![Alt text](/doc/sample.png "Optional Title")

## Features

* extracting setups from replay file
* replacing replay setup with another one (to hide your precious work ;) )

## Usage
Download newest version from [release tab](https://github.com/wbsth/rbr-rt/releases), unzip *rbr-rt.zip*, then run *rbr-rt.exe*

### Using setup extractor
Simply load replay you want to extract replay from, and hit 'Extract'. Then, specify where setup file should be saved to.

### Using setup hider
First, load two files:
* Replay in which you want to hide the car setup. (*'Replay file'*)

* Car setup that you want to be 'injected' into replay (e.x. default setup)

Then hit *Replace*, and save output replay file to desired location.

**Warning**:
* setup you want to inject must be saved using ingame editor

* if you want to open output replay file using RBR-HU plugin, you must also copy .ini file from original replay, and name it accordingly. (So they have same name - e.x. 'Replay.rpl' and 'Replay.ini') 

## Prerequisites

* .NET 6.0
