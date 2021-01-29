// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1.Root.Interfaces
{
    interface ITemplateRenderer
    {
        Task<string> Render(string name, object model, CancellationToken cancellationToken);
    }
}
