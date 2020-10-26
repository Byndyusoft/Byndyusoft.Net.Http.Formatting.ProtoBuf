# Byndyusoft.Net.Http.Formatting.ProtoBuf
This package adds support for Protocol Buffers formatting and content negotiation to System.Net.Http

[![(License)](https://img.shields.io/github/license/Byndyusoft/Byndyusoft.Net.Http.Formatting.ProtoBuf.svg)](LICENSE.txt)
[![Nuget](http://img.shields.io/nuget/v/Byndyusoft.Net.Http.Formatting.ProtoBuf.svg?maxAge=10800)](https://www.nuget.org/packages/Byndyusoft.Net.Http.Formatting.ProtoBuf/) [![NuGet downloads](https://img.shields.io/nuget/dt/Byndyusoft.Net.Http.Formatting.ProtoBuf.svg)](https://www.nuget.org/packages/Byndyusoft.Net.Http.Formatting.ProtoBuf/) 

[Protocol Buffers](https://developers.google.com/protocol-buffers) are Google's language-neutral, platform-neutral, extensible mechanism for serializing structured data – think XML, but smaller, faster, and simpler. 

```Byndyusoft.Net.Http.Formatting.ProtoBuf``` actually depends on ```Microsoft.Net.Http```, and extends the ```HttpClient``` with ```Protocol Buffers```
features that you would likely need to talk to a RESTful service such as ASP.NET Web API.

Package operates in the ```System.Net.Http``` namespace and adds some handy extension methods to ```HttpClient``` and ```HttpContent```.

So for example:

```
using (var client = new HttpClient())
{
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/protobuf"));
    var response = await client.GetAsync("http://localhost/api/products/1");
    response.EnsureSuccessStatusCode();
    var product = await response.Content.ReadAsAsync<ProductInfo>(new[]
            {
                new ProtoBufMediaTypeFormatter()
            });
}
```

If you tried to just use ```Microsoft.Net.Http```, the ```ReadAsAsync``` method wouldn't be available to you, and you'd only be able to read the content 
as raw data such as bytes or string, and have to do the serializing / de-serializing yourself.

You also get extension methods to PUT / POST back to the service in ```ProtoBuf``` format without having to do that yourself:

```
// Save the ProductInfo model back to the API service
await client.PutAsProtoBufAsync("http://localhost/api/products/1", product);
await client.PostAsProtoBufAsync("http://localhost/api/products/1", product);
```

## Installing

```shell
dotnet add package Byndyusoft.Net.Http.Formatting.ProtoBuf
```

# Contributing

To contribute, you will need to setup your local environment, see [prerequisites](#prerequisites). For the contribution and workflow guide, see [package development lifecycle](#package-development-lifecycle).

A detailed overview on how to contribute can be found in the [contributing guide](CONTRIBUTING.md).

## Prerequisites

Make sure you have installed all of the following prerequisites on your development machine:

- Git - [Download & Install Git](https://git-scm.com/downloads). OSX and Linux machines typically have this already installed.
- .NET Core (version 3.0 or higher) - [Download & Install .NET Core](https://dotnet.microsoft.com/download/dotnet-core/3.0).

## General folders layout

### src
- source code

### tests

- unit-tests

### example

- example console application

## Package development lifecycle

- Implement package logic in `src`
- Add or addapt unit-tests (prefer before and simultaneously with coding) in `tests`
- Add or change the documentation as needed
- Open pull request in the correct branch. Target the project's `master` branch

# Maintainers

[github.maintain@byndyusoft.com](mailto:github.maintain@byndyusoft.com)