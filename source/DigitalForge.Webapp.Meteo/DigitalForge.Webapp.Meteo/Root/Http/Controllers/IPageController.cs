// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1.Http.Controllers
{
    public interface IPageController : IController
    {
        Task<ContentResult> Render(object model, CancellationToken cancellationToken);

        Task<object> Build(CancellationToken cancellationToken);
    }
}
