using Nuke.Common;

namespace Targets;

partial class Build
{
    Target PublishBackEnd => _ => _
        .DependsOn(AddMigration);

    Target PublishFrontEnd => _ => _;
}
