Cube.Images
====

[![NuGet](https://img.shields.io/nuget/v/Cube.Images.svg)](https://www.nuget.org/packages/Cube.Images/)
[![AppVeyor](https://ci.appveyor.com/api/projects/status/ffsk5mc9i8o9iy72?svg=true)](https://ci.appveyor.com/project/clown/cube-images)
[![Codecov](https://codecov.io/gh/cube-soft/Cube.Images/branch/master/graph/badge.svg)](https://codecov.io/gh/cube-soft/Cube.Images)

Cube.Images is an image library available for .NET Framework 3.5, 4.5, .NET Standard 2.0, or later.

## Installation

You can install the library through the NuGet package.
Add a dependency in your project file using the following syntax:

    <ItemGroup>
        <PackageReference Include="Cube.Images" Version="4.0.1" />
    </ItemGroup>

Or select it from the NuGet packages UI on Visual Studio.

## Contributing

1. Fork [Cube.Images](https://github.com/cube-soft/Cube.Images/fork) repository.
2. Create a feature branch from the master, net45, or net50 branch (e.g. git checkout -b my-new-feature origin/master). Note that the master branch may refer some pre-release NuGet packages. Try the [rake clobber](https://github.com/cube-soft/Cube.Images/blob/master/Rakefile) command when build errors occur.
3. Commit your changes.
4. Rebase your local changes to the corresponding branch.
5. Run the dotnet test command or the Visual Studio and confirm that it passes.
6. Create a new Pull Request.

## License

Copyright © 2010 [CubeSoft, Inc.](https://www.cube-soft.jp/)
The project is licensed under the [Apache 2.0](https://github.com/cube-soft/Cube.Images/blob/master/License.txt).