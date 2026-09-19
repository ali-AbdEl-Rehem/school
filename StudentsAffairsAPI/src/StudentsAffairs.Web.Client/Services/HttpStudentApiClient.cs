using System.Net;
using System.Net.Http.Json;
using StudentsAffairs.Shared.Abstractions;
using StudentsAffairs.Shared.DTOs;

namespace StudentsAffairs.Web.Client.Services;

/// <summary>Browser (WebAssembly) implementation of <see cref="IStudentApi"/> over the REST endpoints.</summary>
public sealed class HttpStudentApiClient : IStudentApi
{
    private const string Root = "api/students";
    private readonly HttpClient _http;

    public HttpStudentApiClient(HttpClient http) => _http = http;

    public async Task<PagedResult<StudentDto>> GetPagedAsync(StudentQuery query, CancellationToken cancellationToken = default)
        => await _http.GetFromJsonAsync<PagedResult<StudentDto>>($"{Root}?{query.ToQueryString()}", cancellationToken)
           ?? new PagedResult<StudentDto>();

    public async Task<IReadOnlyList<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _http.GetFromJsonAsync<List<StudentDto>>($"{Root}/all", cancellationToken) ?? [];

    public async Task<StudentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _http.GetAsync($"{Root}/{id}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<StudentDto>(cancellationToken);
    }

    public async Task<StudentDto> CreateAsync(StudentCreateDto dto, CancellationToken cancellationToken = default)
    {
        var response = await _http.PostAsJsonAsync(Root, dto, cancellationToken);
        await EnsureSuccessOrThrowAsync(response, cancellationToken);
        return (await response.Content.ReadFromJsonAsync<StudentDto>(cancellationToken))!;
    }

    public async Task<StudentDto?> UpdateAsync(int id, StudentUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var response = await _http.PutAsJsonAsync($"{Root}/{id}", dto, cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        await EnsureSuccessOrThrowAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<StudentDto>(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _http.DeleteAsync($"{Root}/{id}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        await EnsureSuccessOrThrowAsync(response, cancellationToken);
        return true;
    }

    public async Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
        => await _http.GetFromJsonAsync<List<string>>($"{Root}/departments", cancellationToken) ?? [];

    private static async Task EnsureSuccessOrThrowAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        string? detail = null;
        try
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsLite>(cancellationToken);
            detail = problem?.Detail ?? problem?.Title;
        }
        catch
        {
            // response body was not ProblemDetails JSON
        }

        throw new HttpRequestException(detail ?? $"Request failed with status {(int)response.StatusCode}.");
    }

    private sealed record ProblemDetailsLite(string? Title, string? Detail, int? Status);
}
