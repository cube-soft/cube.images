Cube.Images
====

[![Package](https://badgen.net/nuget/v/cube.images)](https://www.nuget.org/packages/cube.images/)
[![AppVeyor](https://badgen.net/appveyor/ci/clown/cube-images)](https://ci.appveyor.com/project/clown/cube-images)
[![Codecov](https://badgen.net/codecov/c/github/cube-soft/cube.images)](https://codecov.io/gh/cube-soft/cube.images)

Cube.Images is an image library available for .NET Framework 3.5, 4.5, .NET Standard 2.0, or later.

## Installation

You can install the library through the NuGet package.
Add a dependency in your project file using the following syntax:

    <PackageReference Include="Cube.Images" Version="5.0.0" />

Or select it from the NuGet packages UI on Visual Studio.

## Contributing

1. Fork [Cube.Images](https://github.com/cube-soft/Cube.Images/fork) repository.
2. Create a feature branch from the master, net45, or net50 branch (e.g. git checkout -b my-new-feature origin/master). Note that the master branch may refer some pre-release NuGet packages. Try the [rake clobber](https://github.com/cube-soft/Cube.Images/blob/master/Rakefile) command when build errors occur.
3. Commit your changes.
4. Rebase your local changes to the corresponding branch.
5. Run the dotnet test command or the Visual Studio (NUnit 3 test adapter) and confirm that it passes.
6. Create a new Pull Request.

## License

Copyright © 2010 [CubeSoft, Inc.](https://www.cube-soft.jp/)
The project is licensed under the [Apache 2.0](https://github.com/cube-soft/Cube.Images/blob/master/License.txt).