// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Arduino.OpenWeather
{
    internal class Client : IDisposable
    {
        private readonly string _apiKey;

        public Client(string apiKey)
        {
            this._apiKey = apiKey;
        }

        public async Task<WeatherDto> GetCurrentWeather(string latitude, string longtitude,
            CancellationToken cancellationToken)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(
                    $@"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longtitude}&appid={this._apiKey}&units=metric",
                    cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }

                return JsonConvert.DeserializeObject<WeatherDto>(await response.Content.ReadAsStringAsync());
            }
        }

        public void Dispose()
        {
        }
    }
}