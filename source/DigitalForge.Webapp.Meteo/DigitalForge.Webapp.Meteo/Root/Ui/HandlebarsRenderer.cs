// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Hosting;

using WebApplication1.Root.Interfaces;

using HandlebarsDotNet;

namespace WebApplication1.Root.Ui
{
    public class HandlebarsRenderer : ITemplateRenderer
    {
        private readonly object _lockObject;
        private bool _isInitialized;

        private readonly IHostEnvironment _hostEnvironment;

        private Dictionary<string, Func<object, string>> _registeredTemplates;

        public HandlebarsRenderer(IHostEnvironment hostEnvironment)
        {
            this._lockObject = new object();
            this._isInitialized = false;

            this._hostEnvironment = hostEnvironment;
            this._registeredTemplates = new Dictionary<string, Func<object, string>>();

            this.Initialize();
        }

        async Task<string> ITemplateRenderer.Render(string name, object model, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return this._registeredTemplates[name](model);
        }

        private void Initialize()
        {
            if(!this._isInitialized)
            {
                lock(this._lockObject)
                {
                    if(!this._isInitialized)
                    {
                        this.DoInitialize();
                        this._isInitialized = true;
                    }
                }
            }
        }

        private void DoInitialize()
        {
            Directory.GetFiles($"{this._hostEnvironment.ContentRootPath.TrimEnd('/')}/Templates/Partials", @"*.html")
                .ToList()
                .ForEach(x =>
                {
                    var file = new FileInfo(x);

                    this.RegisterPartial(file.Name.Replace(".html", string.Empty), File.ReadAllText(x));
                });

            Directory.GetFiles($"{this._hostEnvironment.ContentRootPath.TrimEnd('/')}/Templates/Pages", @"*.html")
                .ToList()
                .ForEach(x =>
                {
                    var file = new FileInfo(x);

                    this.CompileTemplate(file.Name.Replace(".html", string.Empty), File.ReadAllText(x));
                });
        }

        private void CompileTemplate(string name, string template)
        {
            var _template = Handlebars.Compile(template);

            this._registeredTemplates.Add(name, _template);
        }

        private void RegisterPartial(string name, string partial)
        {
            Handlebars.RegisterTemplate(name, Handlebars.Compile(new StreamReader(this.CreateStreamFromString(partial))));
        }

        private Stream CreateStreamFromString(string partial)
        {
            var stream = new MemoryStream();

            using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            {
                writer.Write(partial);
                writer.Flush();

                writer.BaseStream.Position = 0;
            }

            return stream;
        }
    }
}
