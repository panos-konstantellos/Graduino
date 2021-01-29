// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Net.Http;

using WebApplication1.Root;

using Newtonsoft.Json;

namespace WebApplication1.Adapters
{
    public class ApplicationAdapterOption
    {
        public HttpMethod Method { get; set; }

        public Uri CurrentUrl { get; set; }
    }

    public class ApplicationAdapter
    {
        public object Build(ApplicationAdapterOption options)
        {
            dynamic model = new DynamicExpandoObject();

            model.ApplicationGuid = Guid.NewGuid().ToString();
            model.CurrentUrl = new
            {
                Method = options.Method.ToString(),
                Protocol = options.CurrentUrl.Scheme.ToString(),
                Host = options.CurrentUrl.Host.ToString(),
                Port = options.CurrentUrl.Port.ToString(),
                Path = !string.IsNullOrEmpty(options.CurrentUrl.Query)
                    ? options.CurrentUrl.PathAndQuery.Replace(options.CurrentUrl.Query, string.Empty).Replace("?", string.Empty)
                    : options.CurrentUrl.PathAndQuery.Replace("?", string.Empty),
                QueryString = options.CurrentUrl.Query,
                FullUrl = options.CurrentUrl.ToString(),
                BaseUrl = $"{options.CurrentUrl.Scheme}://{options.CurrentUrl.Host}:{options.CurrentUrl.Port}/"
            };

            model.DigitalForgeJavascriptOptions = new JavascriptAdapter().Build(new JavascriptAdapterOptions
            {
                Method = options.Method,
                CurrentUrl = options.CurrentUrl,
                Api = new ApiOptions
                {
                    BaseUrl = new Uri(model.CurrentUrl.BaseUrl),
                    DefaultMethod = options.Method
                }
            });

            return model;
        }
    }

    public sealed class JavascriptAdapterOptions
    {
        public ApiOptions Api { get; set; }

        public HttpMethod Method { get; set; }

        public Uri CurrentUrl { get; set; }
    }

    public sealed class ApiOptions
    {
        public HttpMethod DefaultMethod { get; set; }

        public Uri BaseUrl { get; set; }
    }

    public class JavascriptAdapter
    {
        public object Build(JavascriptAdapterOptions options)
        {
            dynamic model = new DynamicExpandoObject();

            model.LanguageId = 1;
            model.Api = new
            {
                DefaultMethod = options.Api.DefaultMethod.ToString(),
                BaseUrl = options.Api.BaseUrl,
                Protocol = options.Api.BaseUrl.Scheme.ToString(),
                Host = options.Api.BaseUrl.Host.ToString(),
                Port = options.Api.BaseUrl.Port.ToString()
            };
            model.CurrentUrl = new
            {
                Method = options.Method.ToString(),
                Protocol = options.CurrentUrl.Scheme.ToString(),
                Host = options.CurrentUrl.Host.ToString(),
                Port = options.CurrentUrl.Port.ToString(),
                Path = !string.IsNullOrEmpty(options.CurrentUrl.Query)
                    ? options.CurrentUrl.PathAndQuery.Replace(options.CurrentUrl.Query, string.Empty).Replace("?", string.Empty)
                    : options.CurrentUrl.PathAndQuery.Replace("?", string.Empty),
                QueryString = options.CurrentUrl.Query,
                FullUrl = options.CurrentUrl.ToString(),
                BaseUrl = $"{options.CurrentUrl.Scheme}://{options.CurrentUrl.Host}:{options.CurrentUrl.Port}/"
            };

            return string.Format("<script>var DigitalForgeOptions = {0}; </script>", JsonConvert.SerializeObject(model));
        }
    }
}
 