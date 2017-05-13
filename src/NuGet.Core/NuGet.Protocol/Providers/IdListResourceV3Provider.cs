// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class IdListResourceV3Provider : ResourceProvider
    {
        public IdListResourceV3Provider()
            : base(typeof(IdListResource), nameof(IdListResourceV3Provider))
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            IdListResource resource = null;

            var serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(token);

            if (serviceIndex != null)
            {
                // TODO: detect ID list resource. For now, assume the V3 source it.
                resource = new IdListResourceV3(source.PackageSource.Source);
            }

            return new Tuple<bool, INuGetResource>(resource != null, resource);
        }
    }
}
