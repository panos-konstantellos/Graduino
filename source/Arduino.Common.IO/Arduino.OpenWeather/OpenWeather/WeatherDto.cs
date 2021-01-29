// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Arduino.OpenWeather
{
    [DataContract]
    public class Coord
    {
        [DataMember(Name = "lon"), JsonProperty("lon")]
        public double Lon { get; set; }

        [DataMember(Name = "lat"), JsonProperty("lat")]
        public double Lat { get; set; }
    }

    [DataContract]
    public class Weather
    {
        [DataMember(Name = "id"), JsonProperty("id")]
        public int Id { get; set; }

        [DataMember(Name = "main"), JsonProperty("main")]
        public string Main { get; set; }

        [DataMember(Name = "description"), JsonProperty("description")]
        public string Description { get; set; }

        [DataMember(Name = "icon"), JsonProperty("icon")]
        public string Icon { get; set; }
    }

    [DataContract]
    public class Main
    {
        [DataMember(Name = "temp"), JsonProperty("temp")]
        public double Temp { get; set; }

        [DataMember(Name = "feels_like"), JsonProperty("feels_like")]
        public double FeelsLike { get; set; }

        [DataMember(Name = "temp_min"), JsonProperty("temp_min")]
        public double TempMin { get; set; }

        [DataMember(Name = "temp_max"), JsonProperty("temp_max")]
        public double TempMax { get; set; }

        [DataMember(Name = "pressure"), JsonProperty("pressure")]
        public int Pressure { get; set; }

        [DataMember(Name = "humidity"), JsonProperty("humidity")]
        public int Humidity { get; set; }
    }

    [DataContract]
    public class Wind
    {
        [DataMember(Name = "speed"), JsonProperty("speed")]
        public double Speed { get; set; }

        [DataMember(Name = "deg"), JsonProperty("deg")]
        public int Deg { get; set; }
    }

    [DataContract]
    public class Clouds
    {
        [DataMember(Name = "all"), JsonProperty("all")]
        public int All { get; set; }
    }

    [DataContract]
    public class Sys
    {
        [DataMember(Name = "type"), JsonProperty("type")]
        public int Type { get; set; }

        [DataMember(Name = "id"), JsonProperty("id")]
        public int Id { get; set; }

        [DataMember(Name = "country"), JsonProperty("country")]
        public string Country { get; set; }

        [DataMember(Name = "sunrise"), JsonProperty("sunrise")]
        public int Sunrise { get; set; }

        [DataMember(Name = "sunset"), JsonProperty("sunset")]
        public int Sunset { get; set; }
    }

    [DataContract]
    public class WeatherDto
    {
        [DataMember(Name = "coord"), JsonProperty("coord")]
        public Coord Coord { get; set; }

        [DataMember(Name = "weather"), JsonProperty("weather")]
        public List<Weather> Weather { get; set; }

        [DataMember(Name = "base"), JsonProperty("base")]
        public string Base { get; set; }

        [DataMember(Name = "main"), JsonProperty("main")]
        public Main Main { get; set; }

        [DataMember(Name = "visibility"), JsonProperty("visibility")]
        public int Visibility { get; set; }

        [DataMember(Name = "wind"), JsonProperty("wind")]
        public Wind Wind { get; set; }

        [DataMember(Name = "clouds"), JsonProperty("clouds")]
        public Clouds Clouds { get; set; }

        [DataMember(Name = "dt"), JsonProperty("dt")]
        public int Dt { get; set; }

        [DataMember(Name = "sys"), JsonProperty("sys")]
        public Sys Sys { get; set; }

        [DataMember(Name = "timezone"), JsonProperty("timezone")]
        public int Timezone { get; set; }

        [DataMember(Name = "id"), JsonProperty("id")]
        public int Id { get; set; }

        [DataMember(Name = "name"), JsonProperty("name")]
        public string Name { get; set; }

        [DataMember(Name = "cod"), JsonProperty("cod")]
        public int Cod { get; set; }
    }
}