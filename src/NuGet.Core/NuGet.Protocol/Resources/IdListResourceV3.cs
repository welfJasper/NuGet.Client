// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class IdListResourceV3 : IdListResource
    {
        private readonly string _source;

        private static readonly Dictionary<string, HashSet<string>> _packageIds = new Dictionary<string, HashSet<string>>
        {
            {
                "https://www.myget.org/F/jver-sandbox/api/v3/index.json",
                new HashSet<string>()
            },
            {
                "https://dotnet.myget.org/F/rx/api/v3/index.json",
                new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "System.Reactive",
                    "System.Interactive",
                    "System.Reactive.Core",
                    "System.Reactive.Linq",
                    "System.Interactive.Async",
                    "System.Reactive.Providers",
                    "Microsoft.Reactive.Testing",
                    "System.Reactive.Interfaces",
                    "System.Interactive.Providers",
                    "System.Reactive.Experimental",
                    "System.Reactive.Windows.Forms",
                    "System.Reactive.WindowsRuntime",
                    "System.Reactive.PlatformServices",
                    "System.Reactive.Runtime.Remoting",
                    "System.Reactive.Windows.Threading",
                    "System.Interactive.Async.Providers",
                    "System.Reactive.Observable.Aliases",
                }
            },
            {
                "https://knapcode.pkgs.visualstudio.com/_packaging/knapcode-nugetprotocol/nuget/v3/index.json",
                new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "K.Np.A",
                    "K.Np.B",
                    "K.Np.FullMetadata",
                    "K.Np.Unlisted",                
                }
            }
        };

        public IdListResourceV3(string source)
        {
            _source = source;
        }

        public override Task<bool> HasIdAsync(string id, CancellationToken token)
        {
            if (!_packageIds.TryGetValue(_source, out var ids))
            {
                return Task.FromResult(true);
            }

            var hasId = ids.Contains(id);
            return Task.FromResult(hasId);
        }
    }
}
