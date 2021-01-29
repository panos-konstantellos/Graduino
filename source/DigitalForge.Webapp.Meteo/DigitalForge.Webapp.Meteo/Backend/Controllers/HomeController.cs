﻿// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

using WebApplication1.Http.Controllers;

namespace WebApplication1.Controllers
{
    public class HomeController : PageControllerBase, IPageController
    {
        public HomeController(IHostEnvironment hostEnvironment) : base(hostEnvironment)
        {
        }

        protected override async Task<object> DoBuild(CancellationToken cancellationToken)
        {
            dynamic model = await base.DoBuild(cancellationToken);

            model.title = @"Meteo.gr";

            return model;
        }
    }
}