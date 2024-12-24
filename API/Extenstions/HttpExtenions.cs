using System;
using System.Text.Json;
using API.Helpers;

namespace API.Extenstions;

public static class HttpExtenions
{   //create a new class to add response to header. not returning anything
    public static void AddPaginationHeader<T>(this HttpResponse response, PagedList<T> data)
    {
        var paginationHeader = new PaginationHeader(data.CurrentPage, data.PageSize,
                                                    data.TotalCount, data.TotalPages);

        var jsonOptions = new JsonSerializerOptions{PropertyNamingPolicy=JsonNamingPolicy.CamelCase};
        response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationHeader, jsonOptions));
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");                                                    
    }

}
