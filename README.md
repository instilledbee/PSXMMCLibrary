[![Build Status](https://travis-ci.org/instilledbee/PSXMMCLibrary.svg?branch=master)](https://travis-ci.org/instilledbee/PSXMMCLibrary)

# PSXMMCLibrary
A managed .NET library for parsing Playstation (PSX) memory card files.

# About
This project has mainly served as a learning tool for parsing Playstation memory card files. In its current state it is by no means feature-complete, and is considered a work in progress. This project is made public for the benefit of other developers to use and improve upon.

For an example on how to use this library, check out its sibling project [PSXCardReader.NET](https://github.com/instilledbee/PSXCardReader.NET)

# Usage
You may integrate the source directly to your existing .NET application solution, or build a DLL out of the solution and reference it in your project. (Interop with non-.NET languages has not been tested)

# Project Structure
* `PSXMMCLibrary` contains the important parsing logic for blocks and directory frames.
* `PSXMMCLibrary.Models` contains the POCOs that represent parsed data.
* `PSXMMCLibrary.Tests` contains unit tests for the parsers.

# Current Features
* Can open and parse ePSXe (*.mcr) memory card format.
* Able to parse header block and its directory frames (metadata about save blocks).
* Able to parse individual memory card blocks.
* Unit tests available to maintain code stability.

# Planned Features
* Checksumming of parsed data (including unit tests)
* Add methods to modify save data (including unit tests)
* Properly parse memory card icons (the methods are available, but apparently they return invalid icon data)
* Support other PSX memory card formats (e.g. DexDrive saves)
