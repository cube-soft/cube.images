Cube.Images
====

[![NuGet](https://img.shields.io/nuget/v/Cube.Images.svg)](https://www.nuget.org/packages/Cube.Images/)
[![AppVeyor](https://ci.appveyor.com/api/projects/status/ffsk5mc9i8o9iy72?svg=true)](https://ci.appveyor.com/project/clown/cube-images)
[![Codecov](https://codecov.io/gh/cube-soft/Cube.Images/branch/master/graph/badge.svg)](https://codecov.io/gh/cube-soft/Cube.Images)
[![Codacy](https://api.codacy.com/project/badge/Grade/edc1223d4e614d2fbbea422b26ed020e)](https://www.codacy.com/app/clown/Cube.Images)

Cube.Images is an image library for CubeSoft applications.

## Installation

You can install using NuGet like this:

    PM> Install-Package Cube.Images

Or select it from the NuGet packages UI on Visual Studio.

## Dependencies

* [Cube.Core](https://github.com/cube-soft/Cube.Core)
* [Cube.FileSystem](https://github.com/cube-soft/Cube.FileSystem)

## Contributing

1. Fork [Cube.Images](https://github.com/cube-soft/Cube.Images/fork) repository.
2. Create a feature branch from the [stable](https://github.com/cube-soft/Cube.Images/tree/stable) branch (git checkout -b my-new-feature origin/stable). The [master](https://github.com/cube-soft/Cube.Images/tree/master) branch may refer some pre-released NuGet packages. See [AppVeyor.yml](https://github.com/cube-soft/Cube.Images/blob/master/AppVeyor.yml) if you want to build and commit in the master branch.
3. Commit your changes.
4. Rebase your local changes against the stable (or master) branch.
5. Run test suite with the [NUnit](http://nunit.org/) console or the Visual Studio (NUnit 3 test adapter) and confirm that it passes.
6. Create new Pull Request.

## License

Copyright (c) 2010 [CubeSoft, Inc.](http://www.cube-soft.jp/)
The project is licensed under the [Apache 2.0](https://github.com/cube-soft/Cube.Images/blob/master/License.txt).