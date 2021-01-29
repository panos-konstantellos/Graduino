# Arduino Common IO for C#

This project aims to provide a cross platform dotnet library to communicate with arduino devices via SerialPort.

*Linux target can also use LibC as an alternative to the native c# SerialPort implementation.*

This project contains also two console applications for running and testing the common library.

## Device Detection
This project provides a cross platform device detection mechanism.

``var ports = new DeviceDetector().GetPorts(); // example output: ["/dev/ttyACM0"]``

## Getting Started

- Install the latest version of the .NET Core SDK from this page <https://www.microsoft.com/net/download/core>
- Clone the repository using the command `git clone https://github.com/ntellos13/Graduino.git` and checkout the `master` branch.
- Next, navigate to `.\source\Arduino.Common.IO`.
- To run
    - Arduino.Listener
        - Edit the configuration file `.\source\Arduino.Common.IO\Arduino.Listener\appsettings.json`
        - Call `dotnet run --project ./Arduino.Listener/`
    - Arduino.OpenWeather
        - Edit the configuration file `.\source\Arduino.Common.IO\Arduino.OpenWeather\appsettings.json`
        - Call `dotnet run --project ./Arduino.OpenWeather/`
