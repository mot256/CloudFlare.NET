﻿namespace CloudFlare.NET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods on <see cref="HttpClient"/> to wrap the Zone APIs
    /// </summary>
    /// <seealso href="https://api.cloudflare.com/#zone"/>
    public static class HttpClientZoneExtensions
    {
        /// <summary>
        /// Gets the base address of the CloudFlare API.
        /// </summary>
        public static Uri ZonesUri { get; } = new Uri(CloudFlareConstants.BaseUri, "zones");

        /// <summary>
        /// Gets the zones for the account specified by the <paramref name="auth"/> details.
        /// </summary>
        public static async Task<IReadOnlyList<Zone>> GetZonesAsync(
            this HttpClient client,
            CancellationToken cancellationToken,
            CloudFlareAuth auth)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (auth == null)
                throw new ArgumentNullException(nameof(auth));

            var request = new HttpRequestMessage(HttpMethod.Get, ZonesUri);
            request.AddAuth(auth);

            HttpResponseMessage response =
                await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false);

            return (await response
                .GetResultAsync<IReadOnlyList<Zone>>(cancellationToken)
                .ConfigureAwait(false))
                .Result;
        }
    }
}