# DotNetMondayHelper
 A C# library for Monday.com's GraphQL API

## Introduction
`MondayHelper` is a C# library designed to facilitate interactions with the Monday.com API. It leverages Monday.com's GraphQL API to enable efficient querying and manipulation of data on Monday.com boards.

## Requirements
- .NET Core 3.1 or later
- Newtonsoft.Json
- GraphQL.Client.Abstractions
- GraphQL
- GraphQL.Client.Http
- GraphQL.Client.Serializer.Newtonsoft

## Installation
Include `MondayHelper` in your project by adding it as a dependency in your project file.

## Usage
### Initializing the Client
By default, MondayHelper is using https://api.monday.com/v2 as an endpoint and API version 2024-01.
```csharp
var apiToken = "your_api_token";
var mondayHelper = new MondayHelper(apiToken);
```

### Initializing the Client w/ Specific API Version
You can choose different versions or endpoints on initialization.
```csharp
var apiToken = "your_api_token";
var endpoint = "https://api.monday.com/v2";
var apiVersion = "2023-10";
var mondayHelper = new MondayHelper(apiToken, endpoint, apiVersion);
```

### Lookup Item
```csharp
var lookupItemID = "123456";
var item = await mondayHelper.LookupItemAsync(lookupItemID);
```

### Create Item
```csharp
var itemName = "New Item";
var boardID = "123456";
var columnValues = new Dictionary<string, string>
{
    {"column_key", "value"}
};
var newItemID = await mondayHelper.CreateItemAsync(itemName, boardID, columnValues);
```

### Change Multiple Column Values
```csharp
var itemID = "123456";
var boardID = "654321";
var columnValues = new Dictionary<string, string>
{
    {"column_key", "new_value"}
};
var updatedItemID = await mondayHelper.ChangeMultipleColumnValuesAsync(itemID, boardID, columnValues);
```

### Change Simple Column Value
```csharp
var itemID = "123456";
var boardID = "654321";
var columnID = "column_key";
var strValue = "new_value";
var updatedItemID = await mondayHelper.ChangeSimpleColumnValueAsync(itemID, boardID, columnID, strValue);
```

## Contributing
Contributions to improve `MondayHelper` are always welcome. Please feel free to fork the repository and submit pull requests.

## License
This project is licensed under the MIT License - see the LICENSE file for details.

## Contact
<a href="https://github.com/K33KS">Keenan DeAngelis</a><br/>
<a href="https://deegeetek.com">DEEGEETEK LLC</a><br/>
![Visitor Count](https://profile-counter.glitch.me/{YOUR USER}/count.svg)

