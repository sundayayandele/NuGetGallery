// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Client;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.RuntimeModel;
using NuGet.Services.Entities;

namespace GalleryTools.Commands
{
    public sealed class BackfillTfmMetadataCommand : BackfillCommand<string>
    {
        protected override string MetadataFileName => "tfmMetadata.txt";
        
        protected override MetadataSourceType SourceType => MetadataSourceType.Entities;
        
        protected override string QueryIncludes => $"{nameof(Package.SupportedFrameworks)}";

        protected override int LimitTo => 100000;

        public static void Configure(CommandLineApplication config)
        {
            Configure<BackfillTfmMetadataCommand>(config);
        }

        protected override string ReadMetadata(IList<string> files, NuspecReader nuspecReader)
        {
            var supportedTFMs = string.Empty;
            if (_packageService == null)
            {
                return supportedTFMs;
            }

            var supportedFrameworks = _packageService.GetSupportedFrameworks(nuspecReader, files);
            foreach (var tfm in supportedFrameworks)
            {
                supportedTFMs += supportedTFMs == string.Empty
                    ? tfm.GetShortFolderName()
                    : $";{tfm.GetShortFolderName()}";
            }

            return supportedTFMs;
        }

        protected override bool ShouldWriteMetadata(string metadata) => true;

        protected override void ConfigureClassMap(PackageMetadataClassMap map)
        {
            map.Map(x => x.Metadata).Index(3);
        }

        protected override void UpdatePackage(Package package, string metadata)
        {
/*            if (metadata == null || metadata.Length == 0)
            {
                return;
            }
*/
//            package.SupportedFrameworks = metadata.Split(',')
//                .Select(f => new PackageFramework {Package = package, TargetFramework = f}).ToArray();
        }
    }
}
