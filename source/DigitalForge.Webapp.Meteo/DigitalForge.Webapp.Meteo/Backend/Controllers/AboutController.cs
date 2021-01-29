// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

using WebApplication1.Http.Controllers;

namespace DigitalForge.Webapp.Meteo.Backend.Controllers
{
    public class AboutController : PageControllerBase, IPageController
    {
        public AboutController(IHostEnvironment hostEnvironment) : base(hostEnvironment)
        {
            this.PageTemplate = "about";
        }

        protected async override Task<object> DoBuild(CancellationToken cancellationToken)
        {
            dynamic model = await base.DoBuild(cancellationToken);

            model.Title = @"Graduational Project - Meteo";
            model.Description = @"People of all ages every day need to be informed about the weather conditions that will prevail in the coming days. There are also groups of workers that the weather conditions will determine whether they have the ability to work or not, for example sailors, farmers, construction workers, etc. Just this we need to cover with this thesis. Every reader has access to the website and learn about the weather conditions.";

            return model;
        }
    }
}
