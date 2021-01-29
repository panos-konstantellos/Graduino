// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Arduino.OpenWeather.Threading
{
    public interface IBackgroundJob
    {
        void Initialize(string options);

        Task ExecuteAsync(CancellationToken cancellationToken);
    }

    public interface IBackgroundJob<T> : IBackgroundJob
        where T : class, new()
    {
        void Initialize(T options);
    }
}