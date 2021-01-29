// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebApplication1.Adapters;
using WebApplication1.Root;
using WebApplication1.Root.Interfaces;

using Newtonsoft.Json;

namespace WebApplication1.Http.Controllers
{
    public abstract class PageControllerBase : ControllerBase, IPageController
    {
        private readonly IHostEnvironment _hostEnvironment;

        private bool ShowModel
        {
            get
            {
                return this.ControllerContext.HttpContext.Request.Query.ContainsKey("model")
                    && this.ControllerContext.HttpContext.Request.Query["model"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase);
            }
        }

        protected virtual string PageTemplate { get; set; }

        public PageControllerBase(IHostEnvironment hostEnvironment)
        {
            this.PageTemplate = @"home";

            this._hostEnvironment = hostEnvironment;
        }

        async Task<object> IPageController.Build(CancellationToken cancellationToken)
        {
            return await this.DoBuild(cancellationToken);
        }

        async Task<ContentResult> IPageController.Render(object model, CancellationToken cancellationToken)
        {
            if (this.ShowModel)
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(model),
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };

            return new ContentResult
            {
                Content = await this.DoRender(model, cancellationToken),
                ContentType = "text/html",
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpGet]
        public async Task<ContentResult> Index(CancellationToken cancellationToken)
        {
            var controller = (this as IPageController);

            var model = await controller.Build(cancellationToken);
            return await controller.Render(model, cancellationToken);
        }

        protected virtual async Task<object> DoBuild(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            
            var method = new HttpMethod(this.ControllerContext.HttpContext.Request.Method) ?? HttpMethod.Get;

            var model = new DynamicExpandoObject() as dynamic;

            model.DigitalForgeApplication = new ApplicationAdapter().Build(new ApplicationAdapterOption
            {
                Method = method,
                CurrentUrl = this.GetUrl()
            });

            return model;
        }

        protected virtual async Task<string> DoRender(object model, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var renderer = new WebApplication1.Root.Ui.HandlebarsRenderer(this._hostEnvironment) as ITemplateRenderer;

            var masterPage = await renderer.Render("master", model, cancellationToken);
            var headerPage = await renderer.Render("header", model, cancellationToken);
            var footerPage = await renderer.Render("footer", model, cancellationToken);
            var page = await renderer.Render(this.PageTemplate, model, cancellationToken);

            return masterPage
                .Replace("[[header]]", headerPage)
                .Replace("[[page]]", page)
                .Replace("[[footer]]", footerPage);
        }

        private Uri GetUrl()
        {
            var request = this.ControllerContext.HttpContext.Request;

            var uriBuilder = new UriBuilder();

            var host = request.Headers.TryGetValue("X-Forwarded-Host", out var _host)
                ? new HostString(_host.ToString().NullIfEmpty() ?? request.Host.ToString())
                : request.Host;

            var scheme = request.Headers.TryGetValue("X-Forwarded-Proto", out var _proto)
                ? _proto.ToString().NullIfEmpty() ?? request.Scheme
                : request.Scheme;

            uriBuilder.Scheme = scheme;
            uriBuilder.Host = host.Host;
            uriBuilder.Port = host.Port ?? 80;
            uriBuilder.Path = request.Path.HasValue ? request.Path.Value : string.Empty;
            uriBuilder.Query = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;

            return uriBuilder.Uri;
        }
    }
}
