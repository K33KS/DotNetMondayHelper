using Newtonsoft.Json;
using GraphQL.Client.Abstractions;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

public class MondayHelper
{
    private readonly IGraphQLClient _client;
    private readonly HttpClient _httpClient;

    public MondayHelper(string apiToken)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiToken);
        _httpClient.DefaultRequestHeaders.Add("API-Version", "2024-01");
        GraphQLHttpClientOptions options = new GraphQLHttpClientOptions
        {
            EndPoint = new Uri("https://api.monday.com/v2")
        };
        _client = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer(), _httpClient);
    }

    public async Task<dynamic> LookupItemAsync(string lookupItemID)
    {
        var query = new GraphQLRequest
        {
            //Those mirror column and board relation values are a bit wacky to handle
            //Need to find a better way to trigger the case
            Query = @"query {
                items (ids: " + lookupItemID + @") 
                {
                    name 
                    state
                    id
                    parent_item {id}
                    group {id title}
                    column_values 
                    {
                        column{title id}
                        ... on MirrorValue {display_value text}
                        ... on BoardRelationValue {display_value}
                        value text
                    }
                }
            }",
        };
        try
        {
            var response = await _client.SendQueryAsync<dynamic>(query);
            return response;
        }
        catch (GraphQLHttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return "Unauthorized";
            else
                return null;
        }
    }

    public async Task<string> CreateItemAsync(string itemName, string boardID, Dictionary<string, string> columnValues)
    {
        var graphQLQuery = new GraphQLRequest
        {
            Query = $@"mutation {{
    create_item (board_id: 1756504507
    item_name: ""{itemName}""
    column_values: ""{JsonConvert.SerializeObject(columnValues).Replace("\"", "\\\"")}"")
    {{ id }}
}}"
        };

        var response = await _client.SendQueryAsync<dynamic>(graphQLQuery);
        string revReportItemID = response.Data.create_item.id;
        return revReportItemID;
    }
    public async Task<string> ChangeMultipleColumnValuesAsync(string itemID, string boardID, Dictionary<string, string> columnValues)
    {
        var graphQLQuery = new GraphQLRequest
        {
            Query = $@"mutation {{
    change_multiple_column_values (item_id: {itemID}
    board_id: {boardID}
    column_values: ""{JsonConvert.SerializeObject(columnValues).Replace("\"", "\\\"")}"")
    {{ id }}
}}"
        };

        var response = await _client.SendQueryAsync<dynamic>(graphQLQuery);
        string revReportItemID = response.Data.change_multiple_column_values.id;
        return revReportItemID;
    }

    public async Task<string> ChangeSimpleColumnValueAsync(string itemID, string boardID, string columnID, string strValue)
    {

        var graphQLQuery = new GraphQLRequest
        {
            Query = $@"mutation {{
    change_simple_column_value (item_id: {itemID}, board_id: {boardID}, column_id: ""{columnID}"", value: ""{strValue}"")
    {{ id }}
}}"
        };

        var response = await _client.SendQueryAsync<dynamic>(graphQLQuery);
        string revReportItemID = response.Data.change_multiple_column_values.id;
        return revReportItemID;
    }
}