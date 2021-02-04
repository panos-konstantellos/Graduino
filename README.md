# Graduino
Graduino is a Bachelor's Degree final project for the [University of West Attica](http://www.ice.uniwa.gr).
Live demo is available [here](https://meteo.devnt.gr/)

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

Graduino consist of multiple projects
- **`Arduino.Common.IO`**: Arduino Common IO for C#, a cross platform dotnet library to communicate with arduino devices via SerialPort
- **`DigitalForge.ApplicationServer.Meteo`**: An application server that exposes REST API for Weather Data operations, capable to scale horizontally.
- **`DigitalForge.Webapp.Meteo`**: A simple frontend to display current weather data
- **`DigitalForge.Arduino.Sketch`**: An arduino sketch to collect weather data from various sensors
- **`Fritzing`**: Schematic and PCB for an arduino with various weather sensors attached.

## Build Status
![.NET](https://github.com/ntellos13/Graduino/workflows/.NET/badge.svg)

## Getting Started

- Install the latest version of the .NET Core SDK from this page <https://www.microsoft.com/net/download/core>
- Clone the repository using the command `git clone https://github.com/ntellos13/Graduino.git` and checkout the `master` branch.
- Build and run any project in source ( see README.md inside the desired project )

## License
Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.

Licensed under the GNU General Public License v3.0 or later. See [LICENSE.md](./LICENSE.md) in the project root for license information.

## Code of Conduct
See [CODE-OF-CONDUCT.md](./CODE-OF-CONDUCT.md)
